'use strict';
class GameState
{
	constructor()
	{
		this.board = new Array(mapLength);
		for(var iii = 0; iii < this.board.length; iii++)
		{
			this.board[iii] = new Array(mapLength);
			for(var zzz = 0; zzz < this.board[iii].length; zzz++)
			{
				this.board[iii][zzz] = 0;
			}
		}
		this.board[0][0] = 1;
		this.board[9][9] = 1;
		this.player1 = {x:0, y:0, direction:3};
		this.player2 = {x:9, y:9, direction:1};
		$(".victory").text("");
	}
	Render()
	{
		var board = "";
		for(var iii = 0; iii < this.board.length; iii++)
		{
			board += "<div class='gridRow'>";
			for(var zzz = 0; zzz < this.board[iii].length; zzz++)
			{
				if(this.board[iii][zzz] === 0)
					board += "<div class='gridPart empty'></div>";
				if(this.board[iii][zzz] === 1)
					board += "<div class='gridPart player'></div>";
				if(this.board[iii][zzz] === 2)
					board += "<div class='gridPart takenSpot1'></div>";
				if(this.board[iii][zzz] === 3)
					board += "<div class='gridPart takenSpot2'></div>";
			}
			board += "</div>"
		}
		$(".board").html(board);
	}
	PlayAIMatch(Player1, Player2, render)
	{
		var won = false;
		var random = 0;
		while(random !== Player1.moveSet.length)
		{
			var status = Player1.nextMove(this.player1, this.player2, this.board)
			if(status === 1)
			{
				$(".victory").text("Player 1 Wins!");
				Player2.score += 1000;
				break;
			}
			if(status === 2)
			{
				$(".victory").text("Player 2 Wins!");
				break;
			}
			status = Player2.nextMove(this.player2, this.player1, this.board)
			if(status === 1)
			{
				$(".victory").text("Player 2 Wins");
				break;
			}
			if(status === 2)
			{
				$(".victory").text("Player 1 Wins!");
				Player2.score += 1000;
				break;
			}
			random++;
		}
		if(render === true)
			this.Render();
	}
}

class enemyBoard extends GameState
{
	constructor()
	{
		super();
	}
	Render()
	{
		var board = "";
		for (var iii = 0; iii < this.board.length; iii++)
		{
			board += "<div class='gridRow'>";
			for (var zzz = 0; zzz < this.board[iii].length; zzz++)
			{
				if (this.board[iii][zzz] === 0)
					board += "<div class='gridPart empty'></div>";
				if (this.board[iii][zzz] === 1)
					board += "<div class='gridPart player'></div>";
				if (this.board[iii][zzz] === 2)
					board += "<div class='gridPart takenSpot1'></div>";
				if (this.board[iii][zzz] === 3)
					board += "<div class='gridPart takenSpot2'></div>";
			}
			board += "</div>"
		}
		$(".enemyBoard").html(board);
	}
}
var gameState;
var population;
var count = 0;
var setting = false;
var preSet;
var enemyBest;
$(document).ready(function()
{
	population = new Population(3);
});

$(document).keypress(function(e)
{
	if(setting)
	{
		if(e.keyCode === 119)
		{
			preSet.moveSet.push(1);
		}
		else if (e.keyCode === 100)
		{
			preSet.moveSet.push(0);
		}
		else if (e.keyCode === 115)
		{
			preSet.moveSet.push(3);
		}
		else if (e.keyCode === 97)
		{
			preSet.moveSet.push(2)
		}
		preSet.nextMove(gameState.player1, gameState.player2, gameState.board);
		console.log(preSet.moveSet, preSet.moveCount);
		gameState.Render();
	}
});

function runGen()
{
	var random = 0;
	while(random < 25)
	{
		population.Score();
		population.NewSetSingle();
		count++;
		console.log(random);
	}
	random++;
	if (random == 10)
	{
		Finale();
    }
    //jklhjklhjkl
    gameState = new GameState();
	population.nodes[0].score = 0;
	gameState.PlayAIMatch(preSet, population.nodes[0], true);
	console.log(population.nodes[0].score)
}

function setPreSet()
{
	setting = true;
	gameState = new GameState();
	preSet = new PreSet([], 2);
}

function runEnemy()
{
	if (enemyBest === null)
	{
		alert("Your opponent has not run a generation yet")
		return;
	}
	var enemyGameState = new enemyBoard();
	enemyGameState.PlayAIMatch(preSet, enemyBest);
}

function Finale()
{
	//send best node and current preSet to server
	window.location.href = window.location.href + "/Finale";
}