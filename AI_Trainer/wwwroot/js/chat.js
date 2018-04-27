// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

const connection = new signalR.HubConnection(
    "/gamehub", { logger: signalR.LogLevel.Information });

connection.on("SendAction", function (sender, action, GameID) {
    const encodedMsg = sender + " " + action;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messanger"+GameID).appendChild(li);
});

connection.on("ReceiveMessage", (user, message, GameID) => {
    const encodedMsg = user + " says " + message;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messanger"+GameID).appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    const GameID = document.getElementById("messagerID").value;
    connection.invoke("SendMessage", user, message, GameID).catch(err => console.error);
    event.preventDefault();
});

document.getElementById("joinButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const GameId = document.getElementById("GameId").value;
    connection.invoke("JoinGame", user, GameId).catch(err => console.error);
    event.preventDefault();
    return window.location.href = "/game/"+GameId;
});

connection.start().catch(err => console.error);