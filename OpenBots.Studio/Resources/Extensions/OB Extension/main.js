var port = null;
var command;
var commandObject;
var responeSent = false;
const sleep = (delay) => new Promise((resolve) => setTimeout(resolve, delay));

//Content Script Message
chrome.runtime.onMessage.addListener(function(message, sender, sendResponse) {
	onMessageFromContentScript(message);
});

 async function onMessageFromContentScript(message){
	if(message.response == "delay"){
		await sleep(3000);
		chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
			chrome.tabs.sendMessage(tabs[0].id, {responseType: commandObject["Method"], responseData: commandObject["Body"]}, function(response) {
				console.log(response.targetHTML);
			});
		});
		return;
	}
    SendNativeMessage(message.response);
	return;
}

connect();
function SendNativeMessage(message) 
{	
	chrome.tabs.onUpdated.removeListener(PageLoaded);
	if(!responeSent){
		responeSent = true;
		port.postMessage({"text": message});
	}
}

function onNativeMessage(message) 
{	
	responeSent = false;
	command = message["message"];
	if(command=="stoplisteners"){
		chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
			chrome.tabs.sendMessage(tabs[0].id, {responseType: "stoplisteners", responseData: ""}, function(response) {
				console.log(response.targetHTML);
				
			});
		});
		return;
	}
	commandObject = JSON.parse( command );
	chrome.tabs.onUpdated.addListener(PageLoaded);
	if(!command)
	{
		//There's no text
		SendNativeMessage("Empty message received");
		return;
	}
		if(commandObject["Method"]=="activatetab"){
			var responseObj;
			var webElement = JSON.parse(commandObject["Body"]);
			chrome.tabs.query({currentWindow: true}, function(tabs) {
				for (var i = 0; i < tabs.length; i++) {
					if(tabs[i].title == webElement["value"])
					{
						chrome.tabs.update(tabs[i].id, {active: true});
						responseObj = {result: "",status:"Success"};
						SendNativeMessage(JSON.stringify(responseObj));
						return;
					}                        
					}
				});
		}
		//Check if active tab is new tab or extensions tab
		chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
			var responseObj;
			if((tabs[0].url == "chrome://newtab/") || (tabs[0].url == "chrome://extensions/")){
				responseObj = {result: "Incorrect URL to get element!",status: "Failed"};
				SendNativeMessage(JSON.stringify(responseObj));
				return;
			}
		});

		if(commandObject["Method"]=="injectjsscript"){
			var responseObj;
			var webElement = JSON.parse(commandObject["Body"]);
				// for the current tab, inject the "inject.js" file & execute its
				chrome.tabs.executeScript(null, {
					code: webElement["value"]
				});
			responseObj = {result: "",status: "Success"};
			SendNativeMessage(JSON.stringify(responseObj));
			return;
		}
		
		chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
			chrome.tabs.sendMessage(tabs[0].id, {responseType: commandObject["Method"], responseData: commandObject["Body"]}, function(response) {
				console.log(response.targetHTML);
				
			});
		});
		
		return;

}


function onDisconnected() {
port = null;
}

function connect() {
var hostName = "com.openbots.chromeserver.message";
port = chrome.runtime.connectNative(hostName);
port.onMessage.addListener(onNativeMessage);
port.onDisconnect.addListener(onDisconnected);
}


function PageLoaded(tabId, changeInfo, tab) {
		chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
			if(tabs[0].id == tabId){
				if (changeInfo.status == 'complete') {
					chrome.tabs.sendMessage(tabs[0].id, {responseType: commandObject["Method"], responseData: commandObject["Body"]}, function(response) {
						console.log(response.targetHTML);
						
					});
				}
			}
	});
}
