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
    document.getElementById("messanger" + GameID).appendChild(li);
});

connection.on("ReceiveMessage", (user, message, GameID) => {
    const encodedMsg = user + " says " + message;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messanger" + GameID).appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    const GameID = document.getElementById("messagerID").value;
    connection.invoke("SendMessage", user, message, GameID).catch(err => console.error);
    event.preventDefault();
});

connection.on("ReceiveData", (data) => {
    enemyBest = new Node(data, 3);
});

connection.on("ReceiveFinale", (best, challenge) => {

    enemyBest = new Node(best, 3);
    enemyChallenge = new PreSet(challenge, 2);
    $(".main").attr("style", "display: none;");
    $(".finale").attr("style", "display: inline-block;");
    gameState = new GameState();
    population.Score();
    if (calledFinale == false) {
        
        const user = document.getElementById("userInput").value;
        const GameId = document.getElementById("messagerID").value;
        const UserId = document.getElementById("userID").value;
        connection.invoke("SendFinale", user, UserId, population.nodes[0].genes, preSet.moveSet, GameId).catch(err => console.error);
        event.preventDefault();
    }
    selfBest = new Node(population.nodes[0].genes, 3);
    gameState.PlayAIMatch(enemyChallenge, selfBest, true);
    var enemyGameState = new enemyBoard();
    enemyGameState.PlayAIMatch(preSet, enemyBest, true);
    if (enemyBest.score > selfBest.score) {
        console.log("blah1");
        $(".winner").text("You Lose!");
    }
    else if (enemyBest.score < selfBest.score) {
        console.log("blah2");
        $(".winner").text("You Win!");
    }
    else {
        console.log("blah3");
        $(".winner").text("Tie!")
    }
});
connection.start().catch(err => console.error);