/*
 * Copyright (C) 2015 Pavel Savshenko
 * Copyright (C) 2011 Google Inc.  All rights reserved.
 * Copyright (C) 2007, 2008 Apple Inc.  All rights reserved.
 * Copyright (C) 2008 Matt Lilek <webkit@mattlilek.com>
 * Copyright (C) 2009 Joseph Pecoraro
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1.  Redistributions of source code must retain the above copyright
 *     notice, this list of conditions and the following disclaimer.
 * 2.  Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 * 3.  Neither the name of Apple Computer, Inc. ("Apple") nor the names of
 *     its contributors may be used to endorse or promote products derived
 *     from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY APPLE AND ITS CONTRIBUTORS "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL APPLE OR ITS CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

var UTILS = {};
UTILS.cssPath = function(node, optimized)
{
    if (node.nodeType !== Node.ELEMENT_NODE)
        return "";
    var steps = [];
    var contextNode = node;
    while (contextNode) {
        var step = UTILS._cssPathStep(contextNode, !!optimized, contextNode === node);
        if (!step)
            break; // Error - bail out early.
        steps.push(step);
        if (step.optimized)
            break;
        contextNode = contextNode.parentNode;
    }
    steps.reverse();
    return steps.join(" > ");
}
UTILS._cssPathStep = function(node, optimized, isTargetNode)
{
    if (node.nodeType !== Node.ELEMENT_NODE)
        return null;
 
    var id = node.getAttribute("id");
    if (optimized) {
        if (id)
            return new UTILS.DOMNodePathStep(idSelector(id), true);
        var nodeNameLower = node.nodeName.toLowerCase();
        if (nodeNameLower === "body" || nodeNameLower === "head" || nodeNameLower === "html")
            return new UTILS.DOMNodePathStep(node.nodeName.toLowerCase(), true);
 	}
    var nodeName = node.nodeName.toLowerCase();
 
    if (id)
        return new UTILS.DOMNodePathStep(nodeName.toLowerCase() + idSelector(id), true);
    var parent = node.parentNode;
    if (!parent || parent.nodeType === Node.DOCUMENT_NODE)
        return new UTILS.DOMNodePathStep(nodeName.toLowerCase(), true);

    /**
     * @param {UTILS.DOMNode} node
     * @return {Array.<string>}
     */
    function prefixedElementClassNames(node)
    {
        var classAttribute = node.getAttribute("class");
        if (!classAttribute)
            return [];

        return classAttribute.split(/\s+/g).filter(Boolean).map(function(name) {
            // The prefix is required to store "__proto__" in a object-based map.
            return "$" + name;
        });
     }
 
    /**
     * @param {string} id
     * @return {string}
     */
    function idSelector(id)
    {
        return "#" + escapeIdentifierIfNeeded(id);
    }

    /**
     * @param {string} ident
     * @return {string}
     */
    function escapeIdentifierIfNeeded(ident)
    {
        if (isCSSIdentifier(ident))
            return ident;
        var shouldEscapeFirst = /^(?:[0-9]|-[0-9-]?)/.test(ident);
        var lastIndex = ident.length - 1;
        return ident.replace(/./g, function(c, i) {
            return ((shouldEscapeFirst && i === 0) || !isCSSIdentChar(c)) ? escapeAsciiChar(c, i === lastIndex) : c;
        });
    }

    /**
     * @param {string} c
     * @param {boolean} isLast
     * @return {string}
     */
    function escapeAsciiChar(c, isLast)
    {
        return "\\" + toHexByte(c) + (isLast ? "" : " ");
    }

    /**
     * @param {string} c
     */
    function toHexByte(c)
    {
        var hexByte = c.charCodeAt(0).toString(16);
        if (hexByte.length === 1)
          hexByte = "0" + hexByte;
        return hexByte;
    }

    /**
     * @param {string} c
     * @return {boolean}
     */
    function isCSSIdentChar(c)
    {
        if (/[a-zA-Z0-9_-]/.test(c))
            return true;
        return c.charCodeAt(0) >= 0xA0;
    }

    /**
     * @param {string} value
     * @return {boolean}
     */
    function isCSSIdentifier(value)
    {
        return /^-?[a-zA-Z_][a-zA-Z0-9_-]*$/.test(value);
    }

    var prefixedOwnClassNamesArray = prefixedElementClassNames(node);
    var needsClassNames = false;
    var needsNthChild = false;
    var ownIndex = -1;
    var siblings = parent.children;
    for (var i = 0; (ownIndex === -1 || !needsNthChild) && i < siblings.length; ++i) {
        var sibling = siblings[i];
        if (sibling === node) {
            ownIndex = i;
            continue;
        }
        if (needsNthChild)
            continue;
        if (sibling.nodeName.toLowerCase() !== nodeName.toLowerCase())
            continue;

        needsClassNames = true;
        var ownClassNames = prefixedOwnClassNamesArray;
        var ownClassNameCount = 0;
        for (var name in ownClassNames)
            ++ownClassNameCount;
        if (ownClassNameCount === 0) {
            needsNthChild = true;
            continue;
        }
        var siblingClassNamesArray = prefixedElementClassNames(sibling);
        for (var j = 0; j < siblingClassNamesArray.length; ++j) {
            var siblingClass = siblingClassNamesArray[j];
            if (ownClassNames.indexOf(siblingClass))
                continue;
            delete ownClassNames[siblingClass];
            if (!--ownClassNameCount) {
                needsNthChild = true;
                break;
            }
        }
    }
 
    var result = nodeName.toLowerCase();
    if (isTargetNode && nodeName.toLowerCase() === "input" && node.getAttribute("type") && !node.getAttribute("id") && !node.getAttribute("class"))
        result += "[type=\"" + node.getAttribute("type") + "\"]";
    if (needsNthChild) {
        result += ":nth-child(" + (ownIndex + 1) + ")";
    } else if (needsClassNames) {
        for (var prefixedName in prefixedOwnClassNamesArray)
        // for (var prefixedName in prefixedOwnClassNamesArray.keySet())
            result += "." + escapeIdentifierIfNeeded(prefixedOwnClassNamesArray[prefixedName].substr(1));
    }

    return new UTILS.DOMNodePathStep(result, false);
}

/**
 * @constructor
 * @param {string} value
 * @param {boolean} optimized
 */
UTILS.DOMNodePathStep = function(value, optimized)
{
    this.value = value;
    this.optimized = optimized || false;
}

UTILS.DOMNodePathStep.prototype = {
    /**
     * @return {string}
     */
    toString: function()
    {
        return this.value;
    }
}


// XPATH
/* See license.txt for terms of usage */

// define([
    // "firebug/lib/string"
// ],
// function(Str) {

// "use strict";

// // ********************************************************************************************* //
// // Constants

var Xpath = {};

// ********************************************************************************************* //
// XPATH

/**
 * Gets an XPath for an element which describes its hierarchical location.
 */
Xpath.getElementXPath = function(element)
{
    // if (element && element.id)
        // return '//*[@id="' + element.id + '"]';
    // else
        return Xpath.getElementTreeXPath(element);
};

Xpath.getElementTreeXPath = function(element)
{
    var paths = [];

    // Use nodeName (instead of localName) so namespace prefix is included (if any).
    for (; element && element.nodeType == Node.ELEMENT_NODE; element = element.parentNode)
    {
        var index = 0;
        var hasFollowingSiblings = false;
        for (var sibling = element.previousSibling; sibling; sibling = sibling.previousSibling)
        {
            // Ignore document type declaration.
            if (sibling.nodeType == Node.DOCUMENT_TYPE_NODE)
                continue;

            if (sibling.nodeName == element.nodeName)
                ++index;
        }

        for (var sibling = element.nextSibling; sibling && !hasFollowingSiblings;
            sibling = sibling.nextSibling)
        {
            if (sibling.nodeName == element.nodeName)
                hasFollowingSiblings = true;
        }

        var tagName = (element.prefix ? element.prefix + ":" : "") + element.localName;
        var pathIndex = (index || hasFollowingSiblings ? "[" + (index + 1) + "]" : "");
        paths.splice(0, 0, tagName + pathIndex);
    }

    return paths.length ? "/" + paths.join("/") : null;
};

Xpath.cssToXPath = function(rule)
{
    var regElement = /^([#.]?)([a-z0-9\\*_-]*)((\|)([a-z0-9\\*_-]*))?/i;
    var regAttr1 = /^\[([^\]]*)\]/i;
    var regAttr2 = /^\[\s*([^~=\s]+)\s*(~?=)\s*"([^"]+)"\s*\]/i;
    var regPseudo = /^:([a-z_-])+/i;
    var regCombinator = /^(\s*[>+\s])?/i;
    var regComma = /^\s*,/i;

    var index = 1;
    var parts = ["//", "*"];
    var lastRule = null;

    while (rule.length && rule != lastRule)
    {
        lastRule = rule;

        // Trim leading whitespace
        rule = Str.trim(rule);
        if (!rule.length)
            break;

        // Match the element identifier
        var m = regElement.exec(rule);
        if (m)
        {
            if (!m[1])
            {
                // XXXjoe Namespace ignored for now
                if (m[5])
                    parts[index] = m[5];
                else
                    parts[index] = m[2];
            }
            else if (m[1] == '#')
                parts.push("[@id='" + m[2] + "']");
            else if (m[1] == '.')
                parts.push("[contains(concat(' ',normalize-space(@class),' '), ' " + m[2] + " ')]");

            rule = rule.substr(m[0].length);
        }

        // Match attribute selectors
        m = regAttr2.exec(rule);
        if (m)
        {
            if (m[2] == "~=")
                parts.push("[contains(@" + m[1] + ", '" + m[3] + "')]");
            else
                parts.push("[@" + m[1] + "='" + m[3] + "']");

            rule = rule.substr(m[0].length);
        }
        else
        {
            m = regAttr1.exec(rule);
            if (m)
            {
                parts.push("[@" + m[1] + "]");
                rule = rule.substr(m[0].length);
            }
        }

        // Skip over pseudo-classes and pseudo-elements, which are of no use to us
        m = regPseudo.exec(rule);
        while (m)
        {
            rule = rule.substr(m[0].length);
            m = regPseudo.exec(rule);
        }

        // Match combinators
        m = regCombinator.exec(rule);
        if (m && m[0].length)
        {
            if (m[0].indexOf(">") != -1)
                parts.push("/");
            else if (m[0].indexOf("+") != -1)
                parts.push("/following-sibling::");
            else
                parts.push("//");

            index = parts.length;
            parts.push("*");
            rule = rule.substr(m[0].length);
        }

        m = regComma.exec(rule);
        if (m)
        {
            parts.push(" | ", "//", "*");
            index = parts.length-1;
            rule = rule.substr(m[0].length);
        }
    }

    var xpath = parts.join("");
    return xpath;
};

Xpath.getElementsBySelector = function(doc, css)
{
    var xpath = Xpath.cssToXPath(css);
    return Xpath.getElementsByXPath(doc, xpath);
};

Xpath.getElementsByXPath = function(doc, xpath)
{
    try
    {
        return Xpath.evaluateXPath(doc, xpath);
    }
    catch(ex)
    {
        return [];
    }
};

/**
 * Evaluates an XPath expression.
 *
 * @param {Document} doc
 * @param {String} xpath The XPath expression.
 * @param {Node} contextNode The context node.
 * @param {int} resultType
 *
 * @returns {*} The result of the XPath expression, depending on resultType :<br> <ul>
 *          <li>if it is XPathResult.NUMBER_TYPE, then it returns a Number</li>
 *          <li>if it is XPathResult.STRING_TYPE, then it returns a String</li>
 *          <li>if it is XPathResult.BOOLEAN_TYPE, then it returns a boolean</li>
 *          <li>if it is XPathResult.UNORDERED_NODE_ITERATOR_TYPE
 *              or XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, then it returns an array of nodes</li>
 *          <li>if it is XPathResult.ORDERED_NODE_SNAPSHOT_TYPE
 *              or XPathResult.UNORDERED_NODE_SNAPSHOT_TYPE, then it returns an array of nodes</li>
 *          <li>if it is XPathResult.ANY_UNORDERED_NODE_TYPE
 *              or XPathResult.FIRST_ORDERED_NODE_TYPE, then it returns a single node</li>
 *          </ul>
 */
Xpath.evaluateXPath = function(doc, xpath, contextNode, resultType)
{
    if (contextNode === undefined)
        contextNode = doc;

    if (resultType === undefined)
        resultType = XPathResult.ANY_TYPE;

    var result = doc.evaluate(xpath, contextNode, null, resultType, null);

    switch (result.resultType)
    {
        case XPathResult.NUMBER_TYPE:
            return result.numberValue;

        case XPathResult.STRING_TYPE:
            return result.stringValue;

        case XPathResult.BOOLEAN_TYPE:
            return result.booleanValue;

        case XPathResult.UNORDERED_NODE_ITERATOR_TYPE:
        case XPathResult.ORDERED_NODE_ITERATOR_TYPE:
            var nodes = [];
            for (var item = result.iterateNext(); item; item = result.iterateNext())
                nodes.push(item);
            return nodes;

        case XPathResult.UNORDERED_NODE_SNAPSHOT_TYPE:
        case XPathResult.ORDERED_NODE_SNAPSHOT_TYPE:
            var nodes = [];
            for (var i = 0; i < result.snapshotLength; ++i)
                nodes.push(result.snapshotItem(i));
            return nodes;

        case XPathResult.ANY_UNORDERED_NODE_TYPE:
        case XPathResult.FIRST_ORDERED_NODE_TYPE:
            return result.singleNodeValue;
    }
};

Xpath.getRuleMatchingElements = function(rule, doc)
{
    var css = rule.selectorText;
    var xpath = Xpath.cssToXPath(css);
    return Xpath.getElementsByXPath(doc, xpath);
};

// ********************************************************************************************* //
// Registration

// return Xpath;

// // ********************************************************************************************* //
// });

function rightClick(element){

  // var evt = element.ownerDocument.createEvent('MouseEvents');

 

  // var RIGHT_CLICK_BUTTON_CODE = 2; // the same for FF and IE

 

  // evt.initMouseEvent('click', true, true,

      // element.ownerDocument.defaultView, 1, 0, 0, 0, 0, false,

      // false, false, false, RIGHT_CLICK_BUTTON_CODE, null);

 

  // if (document.createEventObject){

    // // dispatch for IE

    // return element.fireEvent('onclick', evt)

  // }

  // else{

    // // dispatch for firefox + others

    // return !element.dispatchEvent(evt);

  // }
  var e = element.ownerDocument.createEvent('MouseEvents');

e.initMouseEvent('contextmenu', true, true,
     element.ownerDocument.defaultView, 1, 0, 0, 0, 0, false,
     false, false, false,2, null);


return !element.dispatchEvent(e);

}

// Relative XPath
var isFirefox = "undefined" !== typeof InstallTrigger,
    isSafari = /^((?!chrome|android).)*safari/i.test(navigator.userAgent),
    inputText = "",
    recorderActive = !1,
    attrArr = ",withid,withclass,withname,withplaceholder,withtext",
    _document = "",
    frameOriframe = "";
var chooseAttrsForXpath = ["withid", "withclass", "withname", "withplaceholder", "withtext"];
var inspectedElement = "";
var shadowDOMOpenOrClosed = "open";
var elementInShadowDom = "";
var relXpathChecked = "relXpathOn";

function buildRelXpath(a, b) {
	_document = a;
    var c = chooseAttrsForXpath[0],
        d = !1,
        e = [].reduce
            .call(
                b.childNodes,
                function (v, u) {
                    return v + (3 === u.nodeType ? u.textContent : "");
                },
                ""
            )
            .trim();
    if (e.includes("  ") || e.includes("\n") || e.match(/[^\u0000-\u00ff]/)) d = !0;
    var f = b.nodeName.toLowerCase();
    f.includes("svg") && (f = "*");
    e.includes("'")
        ? ((e = e.split("  ")[0]),
          (dotText = '[contains(.,"' + e + '")]'),
          (equalsText = '[normalize-space()="' + e + '"]'),
          (containsText = 50 < e.length || d ? '[contains(text(),"' + e.slice(0, 50) + '")]' : '[normalize-space()="' + e + '"]'))
        : ((e = e.split("  ")[0]),
          (dotText = "[contains(.,'" + e + "')]"),
          (equalsText = "[normalize-space()='" + e + "']"),
          (containsText = 50 < e.length || d ? "[contains(text(),'" + e.slice(0, 50) + "')]" : "[normalize-space()='" + e + "']"));
    equalsText = containsText;
    if (chooseAttrsForXpath.includes("withouttext") || generateCssSelectorFlag) e = "";
    if (e && ("a" == f || "button" == f || "label" == f || "h" == f.charAt(0))) {
        var h = "//" + f + equalsText,
            g = a.evaluate(h, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
        if (1 === g) {
            g = "//" + f + dotText;
            g = a.evaluate(g, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
            if (1 === g) return h;
            g = b.firstChild.textContent;
            g = deleteGarbageFromInnerText(g);
            var k = "//" + f + ("[normalize-space()='" + g + "']");
            g = a.evaluate(k, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
            if (1 === g) return k;
        }
    }
    if (f.includes("html") || f.includes("body")) return "//" + f + tempXpath;
    var l = (g = "");
    var m = {};
    if (!l)
        for (var n = 0; n < b.attributes.length; n++)
            (d = b.attributes[n].name),
                (g = b.attributes[n].nodeValue),
                null == g || "" == g || ("style" === d && "style" !== c) || "id" === d || "xpath" === d || "xpathtest" === d || (chooseAttrsForXpath.includes("without" + d) && c != d) || (m[d] = g);
    n = Object.keys(m).length;
    if ((b.getAttribute(c) && "id" !== c.toLowerCase()) || "" === b.id || (!chooseAttrsForXpath.includes("withid") && "id" !== c.toLowerCase()))
        if (0 != n) {
            var q = (k = "");
            for (d = 0; d < n; d++) {
                k = tempXpath;
                c in m
                    ? ((g = c), (l = m[g]))
                    : "placeholder" in m
                    ? ((g = "placeholder"), (l = m[g]))
                    : "title" in m
                    ? ((g = "title"), (l = m[g]))
                    : "name" in m
                    ? ((g = "name"), (l = m[g]))
                    : "value" in m
                    ? ((g = "value"), (l = m[g]))
                    : "aria-label" in m
                    ? ((g = "aria-label"), (l = m[g]))
                    : "alt" in m
                    ? ((g = "alt"), (l = m[g]))
                    : "for" in m
                    ? ((g = "for"), (l = m[g]))
                    : "data-label" in m
                    ? ((g = "data-label"), (l = m[g]))
                    : "date-fieldlabel" in m
                    ? ((g = "date-fieldlabel"), (l = m[g]))
                    : "data-displaylabel" in m
                    ? ((g = "data-displaylabel"), (l = m[g]))
                    : "role" in m
                    ? ((g = "role"), (l = m[g]))
                    : "type" in m
                    ? ((g = "type"), (l = m[g]))
                    : "class" in m
                    ? ((g = "class"), (l = m[g]))
                    : ((g = Object.keys(m)[0]), (l = m[g]), isAlphaNumeric(l) && (l = g = ""));
                l = deleteLineGap(l);
                delete m[g];
                if (null != l && "" != l && "xpath" !== g) {
                    var t = "//" + f,
                        r = (h = "");
                    l.includes("  ") && ((l = l.split("  ")[l.split("  ").length - 1]), (containsFlag = !0));
                    h = l.includes("'")
                        ? (" " !== l.charAt(0) && " " !== l.charAt(l.length - 1) && !containsFlag) || xpathForCss
                            ? "//" + f + "[@" + g + '="' + l + '"]'
                            : "//" + f + "[contains(@" + g + ',"' + l.trim() + '")]'
                        : (" " !== l.charAt(0) && " " !== l.charAt(l.length - 1) && !containsFlag) || xpathForCss
                        ? "//" + f + "[@" + g + "='" + l + "']"
                        : "//" + f + "[contains(@" + g + ",'" + l.trim() + "')]";
                    r = h + k;
                    g = a.evaluate(r, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                    if (1 === g) {
                        if ((r.includes("@href") && !c.includes("href")) || (r.includes("@src") && !c.includes("src") && e))
                            if (((p = "//" + f + containsText + k), (g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 0 === g)) {
                                if (((h = "//" + f + equalsText + k), (g = a.evaluate(h, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === g)) return h;
                            } else if (1 === g) return p;
                        return r;
                    }
                    if (e)
                        if (((p = "//" + f + containsText + k), (g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 0 === g)) {
                            h = "//" + f + equalsText + k;
                            g = a.evaluate(h, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                            if (1 === g) return h;
                            1 < g ? (k = h) : 0 === g && (k = r);
                        } else if (1 === g) {
                            g = "//" + f + dotText + k;
                            g = a.evaluate(g, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                            if (1 === g) return p;
                            g = b.firstChild.textContent;
                            g = deleteGarbageFromInnerText(g);
                            l = "[normalize-space()='" + g + "']";
                            r = "//" + f + l + k;
                            g = a.evaluate(r, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                            if (1 === g) return r;
                            k = h + l + k;
                            g = a.evaluate(k, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                            if (1 === g) return k;
                        } else {
                            p = "//" + f + "[contains(text(),'" + e.slice(0, 50) + "')]";
                            g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                            if (1 === g) return p;
                            p = h + containsText + k;
                            g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                            if (0 === g) {
                                if (((h = h + equalsText + k), (g = a.evaluate(h, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === g)) return h;
                            } else {
                                if (1 === g) return p;
                                if (l.includes("/") || e.includes("/")) l.includes("/") && (p = t + containsText + k), e.includes("/") && (p = p.replace(containsText, ""));
                                k = p;
                            }
                        }
                    else (k = r), l.includes("/") && (k = "//" + f);
                } else if (e) {
                    p = "//" + f + containsText + k;
                    g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                    if (0 === g) {
                        if (((h = "//" + f + equalsText + k), (g = a.evaluate(h, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === g)) return h;
                    } else {
                        if (1 === g) return p;
                        p = "//" + f + "[contains(text(),'" + e.slice(0, 50) + "')]";
                        g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                        if (1 === g) return p;
                    }
                    k = p;
                } else if (null == l || "" == l || g.includes("xpath")) k = "//" + f + k;
                0 == d && (q = k);
            }
            tempXpath = q;
        } else if ("" == l && e && !f.includes("script"))
            if (((p = "//" + f + containsText + tempXpath), (g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 0 === g)) {
                if (((tempXpath = "//" + f + equalsText + tempXpath), (g = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === g)) return tempXpath;
            } else {
                if (1 === g) return p;
                p = "//" + f + "[contains(text(),'" + e.slice(0, 50) + "')]";
                g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                if (1 === g) return p;
                tempXpath = p;
            }
        else tempXpath = "//" + f + tempXpath;
    else {
        d = b.id;
        d = deleteLineGap(d);
        tempXpath = "//" + f + "[@id='" + d + "']" + tempXpath;
        g = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
        if (1 === g) return tempXpath;
        if (e && 0 === b.getElementsByTagName("*").length) {
            var p = "//" + f + containsText + tempXpath;
            g = a.evaluate(p, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
            if (0 === g) {
                if (((h = "//" + f + equalsText + tempXpath), (g = a.evaluate(h, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === g)) return h;
            } else if (1 === g) return p;
        }
    }
    e = 0;
    f = b.parentNode.childNodes;
    for (n = 0; n < f.length; n++) {
        c = f[n];
        if (c === b)
            if ((indexes.push(e + 1), (tempXpath = buildRelXpath(a, b.parentNode)), tempXpath.includes("/"))) {
                g = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                if (1 === g) return tempXpath;
                tempXpath = "/" + tempXpath.replace(/\/\/+/g, "/");
                d = /\/+/g;
                for (m = /[^[\]]+(?=])/g; null != (match = d.exec(tempXpath)); ) matchIndex.push(match.index);
                for (d = 0; d < indexes.length; d++)
                    if (
                        (0 === d
                            ? ((g = tempXpath.slice(matchIndex[matchIndex.length - 1])),
                              null != (match = m.exec(g)) ? ((g = g.replace(m, indexes[d]).split("]")[0] + "]"), (tempXpath = tempXpath.slice(0, matchIndex[matchIndex.length - 1]) + g)) : (tempXpath = tempXpath + "[" + indexes[d] + "]"))
                            : ((g = tempXpath.slice(matchIndex[matchIndex.length - (d + 1)], matchIndex[matchIndex.length - d])),
                              null != (match = m.exec(g))
                                  ? ((g = g.replace(m, indexes[d])), (tempXpath = tempXpath.slice(0, matchIndex[matchIndex.length - (d + 1)]) + g + tempXpath.slice(matchIndex[matchIndex.length - d])))
                                  : (tempXpath = tempXpath.slice(0, matchIndex[matchIndex.length - d]) + "[" + indexes[d] + "]" + tempXpath.slice(matchIndex[matchIndex.length - d]))),
                        (g = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength),
                        1 === g)
                    ) {
                        e = /([a-zA-Z])([^/]*)/g;
                        f = tempXpath.match(e).length;
                        for (d += 1; d < f - 1; d++) {
                            g = tempXpath.match(/\/([^\/]+)\/?$/)[1];
                            m = tempXpath.match(e);
                            m.splice(f - d, 1, "/");
                            c = "";
                            for (n = 0; n < m.length - 1; n++) c = m[n] ? c + "/" + m[n] : c + "//" + m[n];
                            c = (c + "/" + g).replace(/\/\/+/g, "//");
                            c = c.replace(/\/\/+/g, "/");
                            c = c.replace(/\/+/g, "//");
                            g = a.evaluate(c, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                            1 === g && (tempXpath = c);
                        }
                        return tempXpath.replace("//html", "");
                    }
            } else return tempXpath;
        1 === c.nodeType && c.nodeName.toLowerCase() === b.nodeName.toLowerCase() && e++;
    }
}

var xpathForCss = !1,
    generateCssSelectorFlag = !1;
function createCssSelector(a, b) {
    xpathForCss = generateCssSelectorFlag = !0;
    inspectedElement = a;
    b = b.toString().replace("withtext", "withouttext");
    var c = [],
        d = 0,
        e = "",
        f = b[0];
    try {
        a || ((e = "element is inside iframe & it is not supported by SelectorsHub currently. Please write CSS manually."), (d = 0));
        if (shadowDOMOpenOrClosed.includes("closed")) return (e = "element is inside iframe & it is not supported by SelectorsHub currently. Please write CSS manually."), (d = 0), c.push(e), c.push(d), c;
        var h = a.nodeName.toLowerCase();
        if (h.includes("style") || h.includes("script")) (e = "This is " + h + " tag. For " + h + " tag, no need to write selector. :P"), (d = 0);
        if ("" !== a.id) e = "#" + a.id;
        else if ("body" == h || "head" == h || "html" == h) e = h;
        else if (elementInShadowDom || hostShadowDom) {
            if (a.className && !h.includes("svg") && !isSVGChild(a) && ((e = "." + replaceAll(a.className.trim(), " ", ".")), (d = cssSelectorMatchingNode(a, e)), 1 === d)) return c.push(e), c.push(d), c;
            if (h.includes("svg") || isSVGChild(a)) return (e = h), (d = cssSelectorMatchingNode(a, e)), c.push(e), c.push(d), c;
            var g = "",
                k = "",
                l = {};
            e = h;
            for (var m = 0; m < a.attributes.length; m++)
                (g = a.attributes[m].name),
                    (k = a.attributes[m].nodeValue),
                    null == k || "" == k || ("style" === g && "style" !== f) || "id" === g || "xpath" === g || "xpathtest" === g || "cssselectortest" === g || (chooseAttrsForXpath.includes("without" + g) && f != g) || (l[g] = k);
            for (
                m = 0;
                m < Object.keys(l).length &&
                ((g =
                    f in l
                        ? f
                        : "placeholder" in l
                        ? "placeholder"
                        : "title" in l
                        ? "title"
                        : "value" in l
                        ? "value"
                        : "name" in l
                        ? "name"
                        : "aria-label" in l
                        ? "aria-label"
                        : "alt" in l
                        ? "alt"
                        : "for" in l
                        ? "for"
                        : "data-label" in l
                        ? "data-label"
                        : "date-fieldlabel" in l
                        ? "date-fieldlabel"
                        : "data-displaylabel" in l
                        ? "data-displaylabel"
                        : "role" in l
                        ? "role"
                        : "type" in l
                        ? "type"
                        : "class" in l
                        ? "class"
                        : Object.keys(l)[0]),
                (k = l[g]),
                (k = deleteLineGap(k)),
                delete l[g],
                null == k || "" == k || "css" === g || "xpath" === g || ((e = k.includes("'") ? e + "[" + g + '="' + k.trim() + '"]' : e + "[" + g + "='" + k + "']"), (d = cssSelectorMatchingNode(a, e)), 1 !== d));
                m++
            );
        } else if (!absXpath.includes("/*[local-name")) {
            f = e = "";
            f = suggestedFlag || globalRelXpath.includes("normalize-space()") || globalRelXpath.includes("text()") || "relXpathOff" == relXpathChecked ? createRelXpath(a, b)[0] : globalRelXpath;
            xpathForCss = !1;
            try {
                if (f.includes("@href") || f.includes("@src")) e = f.replace("//", "").replace(/\[@/g, "[");
                else
                    for (g = f.replace(/\/+/g, "//"), g = g.split("//"), f = /[^[\]]+(?=])/g, m = 1; m < g.length; m++)
                        (k = g[m]), k.includes("[") && ((l = g[m].match(f)[0]), (k = 4 > l.length ? g[m].split("[")[0] + ":nth-child(" + l + ")" : g[m].replace(/\[@/g, "["))), (e = e + " " + k);
            } catch (r) {}
            d = cssSelectorMatchingNode(a, e);
        }
        d = cssSelectorMatchingNode(a, e);
        if (1 != d || absXpath.includes("/*[local-name"))
            if (((e = buildAbsCssSelector(a)), (d = cssSelectorMatchingNode(a, e)), 1 != d)) {
                var n = parents(a, []);
                n = n.reverse();
                var q = n.slice(n.length - 1, n.length),
                    t = n.slice(0, n.length - 1);
                e = "";
                e = 0 != t.length ? t.join(" ") + " > " + q : q;
            }
        d = cssSelectorMatchingNode(a, e);
    } catch (r) {
        (e = buildAbsCssSelector(a)), (d = cssSelectorMatchingNode(a, e));
    }
    c.push(e);
    c.push(d);
    generateCssSelectorFlag = !1;
    return c;
}

var tempXpath = "",
    indexes = [],
    matchIndex = [],
    containsFlag = !1;
function deleteLineGap(a) {
    a && (a = 0 < a.split("\n")[0].length ? a.split("\n")[0] : a.split("\n")[1]);
    return a;
}
var containsText = "",
    equalsText = "";

function deleteGarbageFromInnerText(a) {
    a = deleteLineGap(a);
    a = a
        .split(/[^\u0000-\u00ff]/)
        .reduce(function (b, c) {
            return b.length > c.length ? b : c;
        }, "")
        .trim();
    return (a = a.split("/")[0].trim());
}
function isAlphaNumeric(a) {
    var b = /\d/;
    return (/[a-zA-Z]/.test(a) && b.test(a)) || b.test(a) ? !0 : !1;
}

var globalRelXpath = "";

var hostShadowDom = !1;
function getAllShadowHost(a, b) {
    for (var c = (a && a.parentNode).getRootNode().host, d = []; c; ) (hostShadowDom = !0), (elementInShadowDom = isInShadow(c)), d.push(createCssSelector(c, b)[0]), (c = (c && c.parentNode).getRootNode().host);
    inspectedElement = a;
    return d;
}

function isSVGChild(a) {
    for (var b = 0; 4 > b; b++)
        if (a.parentNode) {
            if ("svg" === a.parentNode.nodeName.toLowerCase()) return !0;
            a = a.parentNode;
        }
}
function getPrecedingSiblings(a) {
    for (var b = []; (a = a.previousSibling); ) 3 !== a.nodeType && 8 !== a.nodeType && b.push(a.nodeName.toLowerCase());
    return b;
}
function getFollowingSiblings(a) {
    for (var b = []; (a = a.nextSibling); ) 3 !== a.nodeType && 8 !== a.nodeType && b.push(a.nodeName.toLowerCase());
    return b;
}
function getAllSiblings(a) {
    var b = [];
    a = a.parentNode.firstChild;
    do 3 !== a.nodeType && 8 !== a.nodeType && b.push(a.nodeName.toLowerCase());
    while ((a = a.nextSibling));
    return b;
}
function getAllAncestors(a) {
    for (var b = []; a.parentNode && "#document" != a.parentNode.nodeName.toLowerCase() && "html" != a.parentNode.nodeName.toLowerCase(); ) 3 !== a.parentNode && 8 !== a.parentNode && ((a = a.parentNode), b.push(a.nodeName.toLowerCase()));
    return b;
}
function getAllDescendants(a) {
    a = a.getElementsByTagName("*");
    for (var b = [], c = 0; c < a.length; c++) 3 != a[c].nodeType && 8 != a[c].nodeType && b.push(a[c].nodeName.toLowerCase());
    return b;
}
function getAllChildren(a) {
    a = a.children;
    for (var b = [], c = 0; c < a.length; c++) 3 != a[c].nodeType && 8 != a[c].nodeType && b.push(a[c].nodeName.toLowerCase());
    return b;
}
function containsNewLineBlankSpaceStartEnd(a) {
    return /\r|\n/.exec(a) || " " == a.charAt(0) || " " == a.charAt(a.length - 1) ? !0 : !1;
}
function placeZerosAtEnd(a) {
    return a.filter(isntZero).concat(a.filter(isZero));
}
function isntZero(a) {
    return 0 < a.charAt(0);
}
function isZero(a) {
    return 0 == a.charAt(0);
}

function cssSelectorMatchingNode(a, b) {
    var c = 0;
    if (elementInShadowDom)
        try {
            c = a.getRootNode().host.shadowRoot.querySelectorAll(b).length;
        } catch (d) {
            c = 0;
        }
    else
        try {
            c = _document.querySelectorAll(b).length;
        } catch (d) {
            c = 0;
        }
    return c;
}
function createSelectorName(a) {
    _document = a.ownerDocument;
    var b = _document !== document ? "(inside iframe)" : "",
        c = "",
        d = {};
    var e = "";
    var f = a.nodeName.toLowerCase();
    if (f.includes("path") || "g" == f) for (; !a.nodeName.toLowerCase().includes("svg") && ((a = a.parentElement), (f = a.nodeName.toLowerCase()), !f.includes("svg")); );
    f.includes("svg") && (a = a.parentElement);
    if (f.includes("pseudo")) return (f = a.parentElement.className), (e = f.includes("icon") ? f.replace(/-/g, "") : getComputedStyle(a, "").getPropertyValue("content")), e.trim() + b;
    if ("button input meter output progress select textarea".split(" ").includes(f) && a.id)
        try {
            return (e = _document.querySelector("label[for='" + a.id + "']").textContent), e.trim() + b;
        } catch (l) {}
    else if (f.includes("style") || f.includes("script") || f.includes("body") || f.includes("html") || f.includes("head") || f.includes("link") || f.includes("meta") || f.includes("title") || f.includes("comment"))
        try {
            return (e = a.className ? a.className.split(" ")[0] : a.id ? a.id : f + "Tag"), e.trim() + b;
        } catch (l) {}
    var h = a.textContent.trim();
    h && 2 > h.length && (h = a.parentNode.textContent.trim());
    if (f.includes("label")) e = h;
    else if (0 != a.attributes.length) {
        if (!c) for (var g = 0; g < a.attributes.length; g++) (e = a.attributes[g].name), (c = a.attributes[g].nodeValue), null == c || "" == c || e.includes("style") || e.includes("id") || e.includes("xpath") || (d[e] = c);
        c = "";
        "placeholder" in d
            ? (c = d.placeholder)
            : h.trim()
            ? (c = h)
            : "aria-label" in d
            ? (c = d["aria-label"])
            : "name" in d
            ? (c = d.name)
            : "value" in d
            ? (c = d.value)
            : "title" in d
            ? (c = d.title)
            : "alt" in d
            ? (c = d.alt)
            : "for" in d
            ? (c = d["for"])
            : "data-label" in d
            ? (c = d["data-label"])
            : "date-fieldlabel" in d
            ? (c = d["date-fieldlabel"])
            : "data-displaylabel" in d
            ? (c = d["data-displaylabel"])
            : "role" in d && (c = d.role);
        e = c;
        if (!e && ((c = "search remove delete close cancel plus add subtract minus cart home logo notification globe".split(" ")), (d = a.className.toLowerCase()))) for (g = 0; g < c.length; g++) d.includes(c[g]) && (e = c[g] + " icon");
    }
    if (!e || 3 > e.length) {
        e = (e = h) ? e : a.textContent.trim();
        if (!e) {
            h = new WebPage(document);
            try {
                e = h.getLabel(a);
            } catch (l) {}
            h = !1;
            if (e && e.text && e.text.length) {
                h = e && e.text && Math.round(e.text.length / 2);
                var k = e.text.slice(0, h);
                h = k == e.text.slice(h);
            }
            h && ((e.text = k), (e = e.text));
            e = e ? e.text : "";
        }
        !e && a.parentElement ? (e = a.parentElement.textContent.trim()) : e || (e = f + "tag");
    }
    e || ((a = a.parentElement), (e = createSelectorName(a)));
    e = e ? e : "";
    e = e.includes("inside iframe") ? e : e + b;
    return e.trim().substr(0, 100);
}
function createOptimizedRelXpath(a, b) {
    var c = "";
    try {
        c = buildRelXpath(a, b);
    } catch (f) {
        (tempXpath = ""), (chooseAttrsForXpath = ["withoutid", "withoutclass"]), (c = buildRelXpath(a, b));
    }
    var d = /\/\/+/g,
        e = 0;
    try {
        e = c.match(d).length;
    } catch (f) {}
    1 < e && c.includes("[") && !c.includes("@href") && !c.includes("@src") && (c = optimizeXpath(a, c));
    void 0 === c && (c = "It might be pseudo element/comment/inside iframe from different src. XPath doesn't support for them.");
    return c;
}
function buildRelXpathForSVG(a, b) {
    var c = chooseAttrsForXpath[0],
        d = b.nodeName.toLowerCase();
    tagFormat = d.includes("svg") ? "//*[local-name()='" + d : "//*[name()='" + d;
    if (d.includes("svg")) {
        try {
            (d = tempXpath), (tempXpath = ""), (indexes = []), (tempXpath = createOptimizedRelXpath(a, b.parentNode) + "//*[local-name()='svg']" + d), (indexes = []);
        } catch (m) {}
        return tempXpath;
    }
    var e = "",
        f = {};
    d = "";
    try {
        b.attributes.removeNamedItem("xpath"), (d = b.attributes.length);
    } catch (m) {
        d = b.attributes.length;
    }
    if ((b.getAttribute(c) && "id" !== c.toLowerCase()) || "" === b.id || !chooseAttrsForXpath.includes("withid"))
        if (0 < d) {
            if (!e)
                for (d = 0; d < b.attributes.length; d++) {
                    var h = b.attributes[d].name;
                    e = b.attributes[d].nodeValue;
                    null == e || "" == e || ("style" === h && "style" !== c) || "id" === h || "xpath" === h || (f[h] = e.trim().slice(0, 10));
                }
            if (0 < Object.keys(f).length)
                if (
                    ((h = c in f ? c : "placeholder" in f ? "placeholder" : "title" in f ? "title" : "value" in f ? "value" : "name" in f ? "name" : "type" in f ? "type" : "class" in f ? "class" : Object.keys(f)[0]),
                    (e = f[h]),
                    (e = deleteLineGap(e)),
                    null != e && "" != e && "xpath" !== h)
                ) {
                    if (
                        (e.includes("  ") && ((e = e.split("  ")[e.split("  ").length - 1]), (containsFlag = !0)),
                        (tempXpath = e.includes("'") ? tagFormat + '" and contains(@' + h + ',"' + e + '")]' + tempXpath : tagFormat + "' and contains(@" + h + ",'" + e + "')]" + tempXpath),
                        (c = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength),
                        1 === c)
                    )
                        return tempXpath;
                } else if (null == e || "" == e || h.includes("xpath"))
                    if (((tempXpath = tagFormat + "'" + tempXpath), (c = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === c)) return tempXpath;
        } else {
            if (((tempXpath = tagFormat + "']" + tempXpath), (c = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === c)) return tempXpath;
        }
    else if (((d = b.id), (d = deleteLineGap(d)), (tempXpath = tagFormat + "' and @id='" + d + "']" + tempXpath), (c = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === c)) return tempXpath;
    e = 0;
    f = b.parentNode.childNodes;
    for (d = 0; d < f.length; d++) {
        h = f[d];
        if (h === b)
            if ((indexes.push(e + 1), (tempXpath = buildRelXpathForSVG(a, b.parentNode)), tempXpath.includes("/"))) {
                c = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
                if (1 === c) return tempXpath;
                for (var g = absXpath.split("/*"), k = tempXpath.split("/*"), l = g.length - 1; 0 < l; l--)
                    if (((tempXpath = tempXpath.replace(k[l], g[l])), (c = a.evaluate(tempXpath, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === c)) return tempXpath;
            } else return tempXpath;
        1 === h.nodeType && h.nodeName.toLowerCase() === b.nodeName.toLowerCase() && e++;
    }
}

function buildAbsXpath(a) {
    if ("html" === a.nodeName.toLowerCase()) return "/html[1]";
    if ("body" === a.nodeName.toLowerCase()) return "/html[1]/body[1]";
    for (var b = 0, c = a.parentNode.childNodes, d = 0; d < c.length; d++) {
        var e = c[d];
        if (e === a) {
            var f = buildAbsXpath(a.parentNode) + "/" + a.nodeName.toLowerCase() + "[" + (b + 1) + "]";
            break;
        }
        1 === e.nodeType && e.nodeName.toLowerCase() === a.nodeName.toLowerCase() && b++;
    }
    return f;
}
var absXpath = "";
function createAbsXpath(a) {
    var b = a.nodeName.toLowerCase();
    b.includes("#comment")
        ? (absXpath = "This is a comment and selectors can't be generated for comment.")
        : b.includes("<pseudo:")
        ? (absXpath = "This is a pseudo element and selectors can't be generated for pseudo element.")
        : b.includes("style") || b.includes("script")
        ? (absXpath = "This is a " + b + " tag. For " + b + " tag, no need to write xpath. :P")
        : b.includes("#document-fragment")
        ? (absXpath = "This is a shadow-root and xpath can't be written for it. Please inspect an element.")
        : elementInShadowDom
        ? ((absXpath = "This element is inside Shadow DOM & for such elements XPath won't support."),
          (absXpath = shadowDOMOpenOrClosed.includes("closed") ? "This element is inside closed Shadow DOM which is inaccessible so for such elements we can't verify/write selectors." : absXpath))
        : (absXpath = buildAbsXpath(a));
    a = 0;
    if (absXpath.includes("svg")) {
        b = absXpath.split("svg")[0];
        for (var c = absXpath.split("svg")[1].split("/"), d = "", e = 0; e < c.length; e++) d = 0 === e ? "*[local-name()='svg']" + c[e] : d + "/*[name()='" + c[e].split("[")[0] + "'][" + c[e].split("[")[1];
        absXpath = b + d;
    }
    try {
        a = _document.evaluate(absXpath, _document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
    } catch (f) {
        a = 0;
    }
    b = [];
    b.push(absXpath);
    b.push(a);
    return b;
}

function createRelXpath(a, b) {
    inspectedElement = a;
    chooseAttrsForXpath = b.toString().split(",");
    var c = "",
        d = [];
    try {
        _document = a.ownerDocument;
    } catch (h) {
        return d.push("0 element matching."), d.push("0"), d;
    }
    var e = a.nodeName.toLowerCase();
    if (e.includes("#comment")) c = "This is a comment and selectors can't be generated for comment.";
    else if (e.includes("::")) c = "This is a pseudo element and selectors can't be generated for pseudo element.";
    else if (e.includes("style") || e.includes("script")) c = "This is a " + e + " tag. For " + e + " tag, no need to write xpath. :P";
    else if (elementInShadowDom)
        (c = "This element is inside Shadow DOM and for such elements XPath won't support."),
            (c = shadowDOMOpenOrClosed.includes("closed") ? "This element is inside closed Shadow DOM which is inaccessible so for such elements we can't verify/write selectors." : c);
    else
        try {
            c = absXpath.includes("/*[local-name") ? buildRelXpathForSVG(_document, a) : createOptimizedRelXpath(_document, a);
        } catch (h) {
            frameOriframe && createAbsXpath(a), (c = absXpath);
        }
    tempXpath = "";
    try {
        var f = _document.evaluate(c, _document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
    } catch (h) {
        f = 0;
    }
    d.push(c);
    d.push(f);
    (c.includes("@id=") || c.includes("contains(@id")) && isAttributeDynamic(c, "id") && d.push("id");
    (c.includes("@class=") || c.includes("contains(@class")) && isAttributeDynamic(c, "class") && d.push("class");
    return d;
}
function optimizeXpath(a, b) {
    for (var c = b.split("//"), d = c.length, e = b.match(/[^[\]]+(?=])/g), f = 1, h = e.length - 1; 0 < h && !(f++, 3 < e[h].length); h--);
    h = b.split("//" + c[d - f])[1];
    e = 0;
    try {
        e = a.evaluate(h, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
    } catch (g) {
        return b;
    }
    if (1 === e) return h;
    for (d -= f; 0 < d; d--) {
        f = b.replace("//" + c[d], "");
        try {
            (e = a.evaluate(f, a, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength), 1 === e && (b = f);
        } catch (g) {
            break;
        }
    }
    return b;
}
function getElementNodename(a) {
    var b = "";
    a.classList.length && ((b = [a.nodeName.toLowerCase()]), (a = a.attributes["class"].value.trim()), (a = a.replace(/  +/g, " ")), b.push(a.split(" ").join(".")), (b = b.join(".")));
    return b;
}
function getElementChildNumber(a) {
    var b = {},
        c,
        d = a.parentNode;
    var e = d.children.length;
    for (c = 0; c < e; c++)
        if (d.children[c].classList.length) {
            var f = d.children[c].classList[0];
            b[f] ? b[f].push(d.children[c]) : (b[f] = [d.children[c]]);
        }
    c = Object.keys(b).length || -1;
    f = { childIndex: -1, childLen: e };
    b[Object.keys(b)[0]] === e
        ? ((f.childIndex = Array.prototype.indexOf.call(b[a.classList[0]], a)), (f.childLen = b[Object.keys(b)[0]].length))
        : c && -1 !== c && c !== e
        ? ((f.childIndex = Array.prototype.indexOf.call(d.children, a)), (f.childLen = b[Object.keys(b)[0]].length))
        : -1 === c && ((f.childIndex = Array.prototype.indexOf.call(d.children, a)), (f.childLen = e));
    return f;
}
function parents(a, b) {
    var c;
    if (void 0 === b) b = [];
    else {
        var d = getElementChildNumber(a);
        (c = getElementNodename(a))
            ? (1 <= d.childLen && -1 !== d.childIndex && (c += ":nth-child(" + (d.childIndex + 1) + ")"), b.push(c))
            : 5 > b.length && ((c = a.nodeName.toLowerCase()), -1 !== d.childIndex && (c += ":nth-child(" + (d.childIndex + 1) + ")"), b.push(c));
    }
    return "BODY" !== a.nodeName ? parents(a.parentNode, b) : b;
}
function buildAbsCssSelector(a) {
    if ("html" === a.nodeName.toLowerCase()) return "html";
    if ("body" === a.nodeName.toLowerCase()) return "body";
    if ("#document-fragment" === a.nodeName.toLowerCase()) return "";
    for (var b = a.parentNode.children, c = 0; c < b.length; c++)
        if (b[c] === a) {
            var d = buildAbsCssSelector(a.parentNode) + " > " + a.nodeName.toLowerCase() + ":nth-child(" + (c + 1) + ")";
            d = ">" == d.trim().charAt(0) ? d.trim().slice(1) : d;
            break;
        }
    return d;
}

//Right Click Event
// function simulatedRightClick(target, options) {

//   var event = target.ownerDocument.createEvent('MouseEvents'),
//       options = options || {},
//       opts = { // These are the default values, set up for un-modified right clicks
//         type: 'click',
//         canBubble: true,
//         cancelable: true,
//         view: target.ownerDocument.defaultView,
//         detail: 1,
//         screenX: 0, //The coordinates within the entire page
//         screenY: 0,
//         clientX: 0, //The coordinates within the viewport
//         clientY: 0,
//         ctrlKey: false,
//         altKey: false,
//         shiftKey: false,
//         metaKey: false, //I *think* 'meta' is 'Cmd/Apple' on Mac, and 'Windows key' on Win. Not sure, though!
//         button: 2, //0 = left, 1 = middle, 2 = right
//         relatedTarget: null,
//       };

//   //Merge the options with the defaults
//   for (var key in options) {
//     if (options.hasOwnProperty(key)) {
//       opts[key] = options[key];
//     }
//   }

//   //Pass in the options
//   event.initMouseEvent(
//       opts.type,
//       opts.canBubble,
//       opts.cancelable,
//       opts.view,
//       opts.detail,
//       opts.screenX,
//       opts.screenY,
//       opts.clientX,
//       opts.clientY,
//       opts.ctrlKey,
//       opts.altKey,
//       opts.shiftKey,
//       opts.metaKey,
//       opts.button,
//       opts.relatedTarget
//   );

//   //Fire the event
//   target.dispatchEvent(event);
// }

//Get offset of an element
function getOffset(el) {
    const rect = el.getBoundingClientRect();
    return {
      left: rect.left,
      top: rect.top
    };
  }

//   function getOffset(el) {
//     const rect = el.getBoundingClientRect();
//     return {
//       left: rect.left + window.scrollX,
//       top: rect.top + window.scrollY
//     };
//   }

  function getElementByXpath(path) {
	const nodes = [];
	try {
		var xPathIterator = document.evaluate(path, document, null, XPathResult.ORDERED_NODE_ITERATOR_TYPE, null);
		if (xPathIterator) {
			let node = xPathIterator.iterateNext();
			while (node) {
				nodes.push(node);
				node = xPathIterator.iterateNext();
			}
		}
	}
	catch (err) {

	}
	return nodes;
}

// function getSnapshotLengthByXpath(path) {
// 	return document.evaluate(path, document, null, XPathResult.UNORDERED_NODE_SNAPSHOT_TYPE, null).snapshotLength;
// }
function getMatchingElements(arrOfArrays) {
	if (arrOfArrays.length > 0) {
		//If there is only one array
		if (arrOfArrays.length == 1) {
			return arrOfArrays[0];
		}
		else {
			var i = 0;
			var arr1 = arrOfArrays[i];
			while (i + 1 < arrOfArrays.length) {
				//alert("ArrOfArrLenth: "+ arrOfArrays.length);
				var matchingElements = compareDomElements(arr1, arrOfArrays[i + 1]);
				i = i + 1;
				if (matchingElements.length > 0) {
					arr1 = matchingElements;
				}
				else {
					return false;
				}
			}
			return arr1;
		}
	}
	else {
		return false;
	}

}
function compareDomElements(arr1, arr2) {
	var matchingElements = [];
	for (var i = 0; i < arr1.length; i++) {
		for (var j = 0; j < arr2.length; j++) {
			//alert("FirstElement:"+arr1[i].tagName+" Second Element:"+arr2[j].tagName);
			if (arr1[i].isEqualNode(arr2[j])) {
				matchingElements.push(arr2[j])
			}
		}
	}
	return matchingElements;
}


  
 
