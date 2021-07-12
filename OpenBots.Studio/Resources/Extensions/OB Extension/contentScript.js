//convert dom element Web Element Object
function domToWebElementObject(element) {
	//Create CSS Selectors for Selenium Commands
	var selectorArray = [];
	for (var att, i = 0, atts = element.attributes, n = atts.length; i < n; i++) {
		att = atts[i];
		if (att.nodeValue && att.nodeName != "obhighlightedelement") {
			var doubleQuoteExists = att.nodeValue.toString().includes('"');
			if(!doubleQuoteExists){
				selectorArray.push(element.tagName + "[" + att.nodeName + "='" + att.nodeValue + "']");
			}
		}
	}
	var relativeXPath;
	try {
		relativeXPath = buildRelXpath(document, element);
	}
	catch (err) { }
	var selectorArrStr = selectorArray.join(",,");
	var obj = { id: element.id, name: element.name, outerText: element.outerText, tagName: element.tagName, className: element.className, type: element.getAttribute("type"), cssSelector: UTILS.cssPath(element), value: element.value, linkText: element.href, xpath: Xpath.getElementXPath(element), relXPath: relativeXPath, cssSelectors: selectorArrStr, selectionRules: "" };
	return obj;
}

//Inspect Element OnClickEvent
function onClickEvent(event) {
	event.stopPropagation();
	event.preventDefault();
	let target = event.target;
	stopListerners();

	target.style.background = '';

	var elementStr = JSON.stringify(domToWebElementObject(target));
	chrome.runtime.sendMessage({ response: elementStr });
}

//Inspect Element MouseOverEvent
function mouseOverEvent(event) {
	let target = event.target;
	target.setAttribute('obhighlightedelement', '1');
	target.style.background = 'lightblue';
}

//Inspect Element MouseOutEvent
function mouseOutEvent(event) {
	let target = event.target;
	target.style.background = '';
	target.removeAttribute('obhighlightedelement');
}
function stopListerners(){
	document.removeEventListener("click", onClickEvent);
	document.removeEventListener("mouseover", mouseOverEvent);
	document.removeEventListener("mouseout", mouseOutEvent);
	document.addEventListener('keydown', RegisterKeys);

	var element = document.querySelector('[obhighlightedelement="1"]');
	if(element){
		element.removeAttribute('obhighlightedelement');
		element.style.background = '';
	}
}

function RegisterKeys(keyPressed) {
	//Esc pressed
	if (keyPressed.keyCode == 27) {
		stopListerners();
		chrome.runtime.sendMessage({ response: "stopped"});
	}
	//F2 pressed
	if (keyPressed.keyCode == 113) {
		stopListerners();
		chrome.runtime.sendMessage({ response: "delay" });
	}
}


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
		//Find element by CssSelectors
		var domElementByAllCssSelectorsList;
		if (webElementJson["cssSelectors"] != "") {
			try {
				domElementByAllCssSelectorsList = document.querySelectorAll(webElementJson["cssSelectors"]);
			}
			catch (err) {
				return false;
			}
			if (domElementByAllCssSelectorsList.length > 0) {
				arrOfArrays.push(Array.from(domElementByAllCssSelectorsList));
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







