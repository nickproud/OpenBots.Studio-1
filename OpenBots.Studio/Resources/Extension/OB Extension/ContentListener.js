chrome.runtime.onMessage.addListener(
	function (request, sender, sendResponse) {
		var responseObj;
		var elementValue = "";
		console.log(sender.tab ?
			"from a content script:" + sender.tab.url :
			"from the extension");
		if (request.responseType == "getelement") {
			document.addEventListener("click", onClickEvent);
			document.addEventListener("mouseover", mouseOverEvent);
			document.addEventListener("mouseout", mouseOutEvent);
			//sendResponse({targetHTML: clickPoints});
		}
		else{
			if (request.responseType == "leftclick") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					foundElement.click();
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "settext") {
	
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					if (webElement["selectionRules"] = "No") {
						foundElement.value = foundElement.value + webElement["value"];
					}
					else {
						foundElement.value = webElement["value"];
					}
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "readtext") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					responseObj = {result: foundElement.outerText,status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "cleartext") {
	
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					foundElement.value = "";
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "elementexists") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "rightclick") {
				var rightClick = new MouseEvent("click", { "button": 2, "which": 3 });
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					foundElement.dispatchEvent(rightClick);
					simulatedRightClick(foundElement);
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "middleclick") {
				var middleClick = new MouseEvent("click", { "button": 1 });
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					foundElement.dispatchEvent(middleClick);
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "doubleclick") {
				var doubleClick = new MouseEvent("dblclick", { "button": 0 });
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					foundElement.dispatchEvent(doubleClick);
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "getattribute") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					var attributeValue = foundElement.getAttribute(webElement["selectionRules"]);
					responseObj = {result: attributeValue,status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "setoption") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					if (webElement["selectionRules"] == "Select By Index") {
						foundElement.selectedIndex = parseInt(webElement["value"]);
					}
					else if (webElement["selectionRules"] == "Select By Value") {
						foundElement.value = webElement["value"];
					}
					else if (webElement["selectionRules"] == "Select By Text") {
						var textToFind = webElement["value"];
	
						for (var i = 0; i < foundElement.options.length; i++) {
							if (foundElement.options[i].text === textToFind) {
								foundElement.selectedIndex = i;
								break;
							}
						}
					}
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			//return all dropdown options as comma separated string
			else if (request.responseType == "getoptions") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					var optionsArray= new Array();
					for (i = 0; i < foundElement.options.length; i++) {
					optionsArray[i] = foundElement.options[i].value;
					}
					elementValue = optionsArray.join();
					responseObj = {result: elementValue,status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "gettable") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					responseObj = {result: foundElement.innerHTML,status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
            else if (request.responseType == "scrolltoelement") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					foundElement.scrollIntoView();
					responseObj = {result: "",status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "getxypoints") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					var elementOffset = getOffset(foundElement);
					responseObj = {result: parseInt(elementOffset.left,10)+","+parseInt(elementOffset.top,10),status:"Success"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "stoplisteners") {
				stopListerners();
			}
			var responseJson = JSON.stringify(responseObj);
			chrome.runtime.sendMessage({ response: responseJson });
		}
		
	}
);