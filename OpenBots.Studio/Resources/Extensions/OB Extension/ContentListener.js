chrome.runtime.onMessage.addListener(
	function (request, sender, sendResponse) {
		var responseObj;
		var elementValue = "";

		if (request.responseType == "getelement") {
			document.addEventListener("click", onClickEvent);
			document.addEventListener("mouseover", mouseOverEvent);
			document.addEventListener("mouseout", mouseOutEvent);
			document.addEventListener('keydown', RegisterKeys);
		}
		else{
			if (request.responseType == "leftclick") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				var linkURL = "";
				if (foundElement) {
					if(webElement["value"]=="Same Tab"){
						foundElement.click();
					}
					else if(webElement["value"]=="New Tab"){
						window.open(foundElement.href);
					}
					else{
						linkURL = foundElement.href;
					}
					responseObj = {result: linkURL,status:"CommandSuccess"};
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
					responseObj = {result: "",status:"CommandSuccess"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "readtext") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					responseObj = {result: foundElement.outerText,status:"CommandSuccess"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "hoveroverelement") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement); 
				if (foundElement) {
				foundElement.scrollIntoView({
					behavior: "auto",
					block: "center",
					inline: "center"
				  });
				  
				foundElement.addEventListener('mouseover', function() {
					console.log('Event triggered');
				  });

				  var eventOver = new MouseEvent('mouseover', {
					'view': window,
					'bubbles': true,
					'cancelable': true
				  });

				  foundElement.dispatchEvent(eventOver);
				 
					responseObj = {result: "",status:"CommandSuccess"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "getcoordinates") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				var chromeToolBar = window.outerHeight - window.innerHeight;
				  
				if (foundElement) {
					foundElement.scrollIntoView({
						behavior: "auto",
						block: "center",
						inline: "center"
					  });
				var rect = foundElement.getBoundingClientRect();
					responseObj = {result: rect.left+","+rect.top+","+rect.width+","+rect.height+","+chromeToolBar,status:"CommandSuccess"};
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
					responseObj = {result: "",status:"CommandSuccess"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "elementexists") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					responseObj = {result: "",status:"CommandSuccess"};
				}
				else {
					responseObj = {result: "Element not found!",status:"CommandSuccess"};
				}
			}
			else if (request.responseType == "rightclick") {
				var rightClick = new MouseEvent("click", { "button": 2, "which": 3 });
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					foundElement.dispatchEvent(rightClick);
					simulatedRightClick(foundElement);
					responseObj = {result: "",status:"CommandSuccess"};
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
					responseObj = {result: "",status:"CommandSuccess"};
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
					responseObj = {result: "",status:"CommandSuccess"};
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
					responseObj = {result: attributeValue,status:"CommandSuccess"};
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
					responseObj = {result: "",status:"CommandSuccess"};
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
					responseObj = {result: elementValue,status:"CommandSuccess"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "gettable") {
				var webElement = JSON.parse(request.responseData);
				var foundElement = getElementByAttributes(webElement);
				if (foundElement) {
					responseObj = {result: foundElement.innerHTML,status:"CommandSuccess"};
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
					responseObj = {result: "",status:"CommandSuccess"};
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
					responseObj = {result: parseInt(elementOffset.left,10)+","+parseInt(elementOffset.top,10),status:"CommandSuccess"};
				}
				else {
					responseObj = {result: "Element not found!",status:"Failed"};
				}
			}
			else if (request.responseType == "navigatetourl") {
				var webElement = JSON.parse(request.responseData);
				window.location.href = webElement["value"];
				responseObj = {result: "",status:"CommandSuccess"};
			}
			else if (request.responseType == "opennewtab") {
				var webElement = JSON.parse(request.responseData);
				window.open(webElement["value"], '_blank');
				responseObj = {result: "",status:"CommandSuccess"};
			}
			else if (request.responseType == "closetab") {
				window.close();
				responseObj = {result: "",status:"CommandSuccess"};
			}
			else if (request.responseType == "refreshtab") {
				location.reload();
				responseObj = {result: "",status:"CommandSuccess"};
			}
			else if (request.responseType == "navigateback") {
				window.history.back();
				responseObj = {result: "",status:"CommandSuccess"};
			}
			else if (request.responseType == "navigateforward") {
				window.history.forward();
				responseObj = {result: "",status:"CommandSuccess"};
			}
			else if (request.responseType == "stoplisteners") {
				stopListerners();
			}
			var responseJson = JSON.stringify(responseObj);
			chrome.runtime.sendMessage({ response: responseJson });
		}
	}
);