const rotationCount = 16;
const lengthCount = 8;
const mapLength = 10;
const popSize = 100;
const savePercentage = .5;

'use strict';
class MoveHandler
{
	constructor(_playerColor)
	{
		this.next;
		this.prev = [];
		this.playerColor = _playerColor;
		this.direction;
		this.score = 0;
	}
	nextMove(self, enemy, board)
	{
		board[self.y][self.x] = this.playerColor;
		var tryX = self.x;
		var tryY = self.y;
		if(this.next === 0)
			tryX++;
		if(this.next === 1)
			tryY--;
		if(this.next === 2)
			tryX--;
		if(this.next === 3)
			tryY++;
		if(tryX < 0 || tryX > 9)
			return -1;
		if(tryY < 0 || tryY > 9)
			return -1;
		if(board[tryY][tryX] === this.playerColor || board[tryY][tryX] === 1)
			return -1;
		self.y = tryY;
		self.x = tryX;
		self.direction = this.next;
		board[self.y][self.x] = 1;
		this.score += Math.sqrt(Math.pow(enemy.x - self.x, 2) + Math.pow(enemy.y - self.y, 2));
		if((self.x === enemy.x) && ((self.y-1 === enemy.y && enemy.direction === 3) || (self.y+1 === enemy.y && enemy.direction === 1)))
		{
			return 1;
		}
		if((self.y === enemy.y) && ((self.x-1 === enemy.x && enemy.direction === 0) || (self.x+1 === enemy.x && enemy.direction === 2)))
			return 1;
		return 0;
	}
}

class Node extends MoveHandler
{
	constructor(_genes, _playerColor)
	{
		super(_playerColor);
		this.genes = _genes;
	}
	nextMove(self, enemy, board)
	{
		var distance = Math.sqrt(Math.pow(enemy.x - self.x, 2) + Math.pow(enemy.y - self.y, 2));
		var angle = Math.atan((enemy.y-self.y)/(enemy.x-self.x));
		var angleChoice = 0;
		for(var iii = 1; iii <= rotationCount; iii++)
		{
			if(angle >= ((2*Math.PI)/(rotationCount*2))*(iii - rotationCount/2 - 1) && angle < ((2*Math.PI)/(rotationCount*2))*(iii - rotationCount/2))
			{
				angleChoice = iii-1;
				break;
			}
		}
		var maxDistance = Math.sqrt(2*Math.pow(mapLength,2));
		for(iii = 1; iii <= lengthCount; iii++)
		{
			if(distance > (maxDistance/lengthCount)*(iii-1) && distance < (maxDistance/lengthCount)*iii)
			{
				var offset = 0;
				var status = 0;
				do 
				{
					if(offset > 24)
					{
						return 2;
					}
					if(this.genes[((self.direction*rotationCount*lengthCount)+((iii*rotationCount)+angleChoice+offset))%this.genes.length] === undefined)
						throw console.error(this.genes.length);
					this.next = this.genes[((self.direction*rotationCount*lengthCount)+((iii*rotationCount)+angleChoice+offset))%this.genes.length];
					offset++;
					status = super.nextMove(self, enemy, board)
				} while(status === -1)
				return status;
			}
		}
	}
}

class PreSet extends MoveHandler
{
	constructor(_moveSet, _playerColor)
	{
		super(_playerColor);
		this.moveSet = _moveSet;
		this.moveCount = -1;
	}
	nextMove(self, enemy, board)
	{
		var offset = 0;
		var status;
		do {
			offset++;
			if (offset > this.moveSet.length) {
				return 2;
			}
			var temp = this.moveSet[this.moveCount];
			this.moveCount++;
			if (this.moveCount >= this.moveSet.length) {
				this.moveCount = 0;
			}
			this.next = this.moveSet[this.moveCount];
			status = super.nextMove(self, enemy, board);
		} while (status === -1);
		return status;
	}
}

class Population
{
	constructor(_playerColor)
	{
		this.nodes = new Array(popSize);
		var tempByteString;
		this.playerColor = _playerColor;
		for(var iii = 0; iii < this.nodes.length; iii++)
		{
			tempByteString = new Array(rotationCount * lengthCount * 4);
			for(var zzz = 0; zzz < tempByteString.length; zzz++)
			{
				tempByteString[zzz] = Math.floor(Math.random() * 4);
			}
			this.nodes[iii] = new Node(tempByteString, _playerColor);
		}
	}
	Reproduce(node1, node2)
	{
		var newNodes = new Array((1/savePercentage)*2);
		var tempByteString = new Array(rotationCount*lengthCount*4);
		for(var iii = 0; iii < newNodes.length; iii++)
		{
			for(var zzz = 0; zzz < tempByteString.length; zzz++)
			{
				if(Math.floor(Math.random()*40) === 0)
				{
					tempByteString[zzz] = Math.floor(Math.random()*4);
				}
				else
				{
					if(Math.floor(Math.random()*2) === 0)
					{
						tempByteString[zzz] = node1.genes[zzz];
					}
					else
					{
						tempByteString[zzz] = node2.genes[zzz];
					}
				}
			}
			newNodes[iii] = new Node(tempByteString, this.playerColor);
		}
		return newNodes;
	}
	ReproduceSingle(node1)
	{
		var newNodes = new Array((1/savePercentage));
		var tempByteString = new Array(rotationCount*lengthCount*4);
		for(var iii = 0; iii < newNodes.length; iii++)
		{
			for(var zzz = 0; zzz < tempByteString.length; zzz++)
			{
				if(Math.floor(Math.random()*40) === 0)
				{
					tempByteString[zzz] = Math.floor(Math.random()*4);
				}
				else
				{
					tempByteString[zzz] = node1.genes[zzz];
				}
			}
			newNodes[iii] = new Node(tempByteString, this.playerColor);
		}
		return newNodes;
	}
	Score()
	{
		var gameState;
		for(var iii = 0; iii < this.nodes.length; iii++)
		{
			gameState = new GameState();
			gameState.PlayAIMatch(preSet, this.nodes[iii], false);
		}
		this.nodes.sort(function(a,b){return a.score - b.score});
	}
	NewSet()
	{
		var savedNodes = new Array(popSize*savePercentage);
		var temp = 0;
		for(var iii = 0; iii < savedNodes.length; iii++)
		{
			if(Math.floor(Math.random()*5) === 0)
			{
				temp++;
			}
			savedNodes[iii] = this.nodes[iii+temp];
		}
		this.nodes = new Array(popSize);
		var tempNodes;
		var bbb = 0;
		shuffle(savedNodes);
		for(iii = 0; iii < savedNodes.length; iii+=2)
		{
			tempNodes = this.Reproduce(savedNodes[iii], savedNodes[iii+1]);
			for(var zzz = 0; zzz < tempNodes.length; zzz++)
			{
				this.nodes[bbb] = tempNodes[zzz];
				bbb++;
			}
		}
	}
	NewSetSingle()
	{
		var savedNodes = new Array(popSize*savePercentage);
		var temp = 0;
		for(var iii = 0; iii < savedNodes.length; iii++)
		{
			if(Math.floor(Math.random()*5) === 0)
			{
				temp++;
			}
			savedNodes[iii] = this.nodes[iii+temp];
		}
		this.nodes = new Array(popSize);
		var tempNodes;
		var bbb = 0;
		for(iii = 0; iii < savedNodes.length; iii++)
		{
			tempNodes = this.ReproduceSingle(savedNodes[iii]);
			for(var zzz = 0; zzz < tempNodes.length; zzz++)
			{
				this.nodes[bbb] = tempNodes[zzz];
				bbb++;
			}
		}
	}
}
function shuffle(a) 
{
    var j, x, i;
	for (i = a.length - 1; i > 0; i--)
	{
        j = Math.floor(Math.random() * (i + 1));
        x = a[i];
        a[i] = a[j];
        a[j] = x;
    }
    return a;
}