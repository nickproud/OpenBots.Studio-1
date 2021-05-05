//convert dom element Web Element Object
function domToWebElementObject(element) {
	//Create CSS Selectors for Selenium Commands
	var selectorArray = [];
	for (var att, i = 0, atts = element.attributes, n = atts.length; i < n; i++) {
		att = atts[i];
		if (att.nodeValue) {
			selectorArray.push(element.tagName + "[" + att.nodeName + "='" + att.nodeValue + "']");
		}
	}
	var relativeXPath;
	try {
		relativeXPath = buildRelXpath(document, element);
	}
	catch (err) { }
	var selectorArrStr = selectorArray.toString();
	var obj = { id: element.id, name: element.name, outerText: element.outerText, tagName: element.tagName, className: element.className, type: element.getAttribute("type"), cssSelector: UTILS.cssPath(element), value: element.value, linkText: element.href, xpath: Xpath.getElementXPath(element), relXPath: relativeXPath, cssSelectors: selectorArrStr, selectionRules: "" };
	return obj;
}

//Inspect Element OnClickEvent
function onClickEvent(event) {
	event.stopPropagation();
	event.preventDefault();
	let target = event.target;
	document.removeEventListener("click", onClickEvent);
	document.removeEventListener("mouseover", mouseOverEvent);
	document.removeEventListener("mouseout", mouseOutEvent);

	target.style.background = '';

	var elementStr = JSON.stringify(domToWebElementObject(target));
	chrome.runtime.sendMessage({ response: elementStr });
}

//Inspect Element MouseOverEvent
function mouseOverEvent(event) {
	let target = event.target;
	target.style.background = 'lightblue';

	// text.value += `over -> ${target.tagname}\n`;
	// text.scrolltop = text.scrollheight;
}

//Inspect Element MouseOutEvent
function mouseOutEvent(event) {
	let target = event.target;
	target.style.background = '';

	// text.value += `out <- ${target.tagName}\n`;
	// text.scrollTop = text.scrollHeight;
}


$(document).keydown(function (keyPressed) {
	//Esc pressed
	if (keyPressed.keyCode == 27) {
		document.removeEventListener("click", onClickEvent);
		document.removeEventListener("mouseover", mouseOverEvent);
		document.removeEventListener("mouseout", mouseOutEvent);
		chrome.runtime.sendMessage({ response: ""});
	}
	//F2 pressed
	if (keyPressed.keyCode == 113) {
		document.removeEventListener("click", onClickEvent);
		document.removeEventListener("mouseover", mouseOverEvent);
		document.removeEventListener("mouseout", mouseOutEvent);
		chrome.runtime.sendMessage({ response: "delay" });
	}
});


function getElementByAttributes(webElementJson) {
	var arrOfArrays = [];
	var elementFilter = "";

	//Find element by attributes
	if (webElementJson["tagName"] != "") {
		elementFilter += webElementJson["tagName"];
	}
	if (webElementJson["id"] != "") {
		elementFilter += "[id='" + webElementJson["id"] + "']";
	}
	if (webElementJson["name"] != "") {
		elementFilter += "[name='" + webElementJson["name"] + "']";
	}
	if (webElementJson["className"] != "") {
		elementFilter += "[className='" + webElementJson["className"] + "']";
	}
	if (webElementJson["linkText"] != "") {
		elementFilter += "[href='" + webElementJson["linkText"] + "']";
	}
	if (elementFilter != "") {
		var domElementByAttributesList;
		try {
			domElementByAttributesList = document.querySelectorAll(elementFilter);
		}
		catch (err) {
			return false;
		}
		if (domElementByAttributesList.length > 0) {
			arrOfArrays.push(Array.from(domElementByAttributesList));
		}
		else {
			return false;
		}
	}

	//Find element by CssSelector
	var domElementByCssSelectorsList, domElementByXPath, domElementByRelXPath;
	if (webElementJson["cssSelector"] != "") {
		try {
			domElementByCssSelectorsList = document.querySelectorAll(webElementJson["cssSelector"]);
		}
		catch (err) {
			return false;
		}
		if (domElementByCssSelectorsList.length > 0) {
			arrOfArrays.push(Array.from(domElementByCssSelectorsList));
		}
		else {
			return false;
		}
	}
	//Find element by XPath
	if (webElementJson["xpath"] != "") {
		domElementByXPathArr = getElementByXpath(webElementJson["xpath"]);
		if (domElementByXPathArr.length > 0) {
			arrOfArrays.push(domElementByXPathArr);
		}
		else {
			return false;
		}
	}
	//Find element by Relative XPath
	if (webElementJson["relXPath"] != "") {
		domElementByRelXPathArr = getElementByXpath(webElementJson["relXPath"]);
		if (domElementByRelXPathArr.length > 0) {
			arrOfArrays.push(domElementByRelXPathArr);
		}
		else {
			return false;
		}
	}
	var matchingElements = getMatchingElements(arrOfArrays);
	if (matchingElements) {
		if (matchingElements.length == 1) {
			return matchingElements[0];
		}
	}
	return false;
}







