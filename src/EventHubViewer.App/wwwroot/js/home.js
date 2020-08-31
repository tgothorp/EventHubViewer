let connection = new signalR.HubConnectionBuilder().withUrl("/eventhubmessages").build();
let pauseMessages = false;
connection.on("MessageReceived", function (message) {
    if (pauseMessages)
        return;
    incrementCounter();
    if (message.messageFormat == 0)
        processRawMessage(message);
    if (message.messageFormat == 1)
        processJsonMessage(message);
    let messageCount = document.querySelectorAll(".message-container").length;
    if (messageCount > 100) {
        let jsonMessageList = document.getElementById("eventhub-messages");
        jsonMessageList.removeChild(jsonMessageList.lastChild);
    }
});
function togglePause() {
    let pauseButton = document.getElementById("message-pause");
    pauseMessages = !pauseMessages;
    if (pauseMessages) {
        pauseButton.innerText = "Resume";
    }
    else {
        pauseButton.innerText = "Pause";
    }
}
function clearMessages() {
    document.querySelectorAll(".message-container").forEach(el => el.remove());
}
function processJsonMessage(message) {
    let jsonMessageList = document.getElementById("eventhub-messages");
    let messageContainer = document.createElement("div");
    messageContainer.classList.add("message-container", "animate__fadeIn");
    let collapseData = generateSummary(messageContainer, message, "JSON");
    let propertyTable = document.createElement("table");
    propertyTable.classList.add("w-100");
    collapseData.append(propertyTable);
    let jsonKeys = Object.keys(message.jsonValues);
    let jsonValues = Object.values(message.jsonValues);
    let keyRow = propertyTable.insertRow(0);
    for (let i = 0; i < jsonKeys.length; i++) {
        let cell = keyRow.insertCell(i);
        cell.innerText = jsonKeys[i];
    }
    let valueRow = propertyTable.insertRow(1);
    for (let i = 0; i < jsonValues.length; i++) {
        let cell = valueRow.insertCell(i);
        cell.innerText = jsonValues[i];
    }
    generateRaw(collapseData, message);
    jsonMessageList.insertBefore(messageContainer, jsonMessageList.firstChild);
}
function processRawMessage(message) {
    let messageList = document.getElementById("eventhub-messages");
    let messageContainer = document.createElement("div");
    messageContainer.classList.add("message-container", "animate__fadeIn");
    let collapseData = generateSummary(messageContainer, message, "Unknown");
    generateRaw(collapseData, message);
    messageList.insertBefore(messageContainer, messageList.firstChild);
}
function generateSummary(messageContainer, message, messageFormat) {
    let summaryContainer = document.createElement("div");
    summaryContainer.classList.add("messages-summary");
    summaryContainer.classList.add("row");
    messageContainer.append(summaryContainer);
    let dateEnqueued = document.createElement("div");
    dateEnqueued.classList.add("3", "col");
    dateEnqueued.innerHTML = "<strong>Enqueued:</strong> " + message.enqueued;
    summaryContainer.append(dateEnqueued);
    let dateReceived = document.createElement("div");
    dateReceived.classList.add("3", "col");
    dateReceived.innerHTML = "<strong>Received:</strong> " + message.received;
    summaryContainer.append(dateReceived);
    let format = document.createElement("div");
    format.classList.add("3", "col");
    format.innerHTML = "<strong>Format:</strong> " + messageFormat;
    summaryContainer.append(format);
    let length = document.createElement("div");
    length.classList.add("3", "col");
    length.innerHTML = "<strong>Length:</strong> " + message.messageLength + " bytes";
    summaryContainer.append(length);
    let collapseButton = document.createElement("button");
    collapseButton.classList.add("w-100", "messages-collapse-button");
    collapseButton.innerHTML = "<img src='/img/arrow-down.svg'>";
    collapseButton.addEventListener("click", function () {
        this.classList.toggle("active");
        let content = this.nextElementSibling;
        if (content.style.display === "block") {
            content.style.display = "none";
        }
        else {
            content.style.display = "block";
        }
    });
    messageContainer.append(collapseButton);
    let collapseData = document.createElement("div");
    collapseData.classList.add("messages-collapse");
    messageContainer.append(collapseData);
    return collapseData;
}
function generateRaw(messageContainer, message) {
    let rawTextArea = document.createElement("textarea");
    rawTextArea.value = message.rawMessage;
    rawTextArea.classList.add("message-raw");
    messageContainer.append(rawTextArea);
}
function incrementCounter() {
    let counter = document.getElementById("message-counter");
    let total = parseInt(counter.innerText) + 1;
    counter.innerText = total.toString();
}
connection.start();
