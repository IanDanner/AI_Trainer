// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

const connection = new signalR.HubConnection(
    "/gamehub", { logger: signalR.LogLevel.Information });

connection.on("SendAction", function (sender, action) {
    const encodedMsg = sender + " " + action;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessage", (user, message) => {
    const encodedMsg = user + " says " + message;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(err => console.error);
    event.preventDefault();
});

document.getElementById("joinButton").addEventListener("click", event => {
    const PlayerName = document.getElementById("PlayerName").value;
    const AIName = document.getElementById("AIName").value;
    const GameId = document.getElementById("GameId").value;
    connection.invoke("JoinGame", PlayerName, AIName, GameId).catch(err => console.error);
    event.preventDefault();

});

connection.start().catch(err => console.error);