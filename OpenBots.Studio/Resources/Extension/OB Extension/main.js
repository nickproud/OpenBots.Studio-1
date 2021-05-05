
var port = null;
var command;
var commandObject;
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
}

connect();
function SendNativeMessage(message) 
{	
	port.postMessage({"text": message});
}

function onNativeMessage(message) 
{	
	command = message["message"];
	commandObject = JSON.parse( command );
	
	if(!command)
	{
		//There's no text
		SendNativeMessage("Empty message received");
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
