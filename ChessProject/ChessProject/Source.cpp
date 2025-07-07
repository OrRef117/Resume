#include "Pipe.h"
#include <iostream>
#include <thread>
#include "GameSystem.h"
#include <string>
#include "Rook.h"
#include "King.h"
#include "HashTag.h"
#include "Bishop.h"
#include "Queen.h"
#include "Knight.h"
#include "Pawn.h"

using namespace std;
void main()
{
	srand(time_t(NULL));


	Pipe p;
	bool isConnect = p.connect();

	string ans;
	while (!isConnect)
	{
		cout << "cant connect to graphics" << endl;
		cout << "Do you try to connect again or exit? (0-try again, 1-exit)" << endl;
		cin >> ans;

		if (ans == "0")
		{
			cout << "trying connect again.." << endl;
			//Sleep(5000);
			isConnect = p.connect();
		}
		else
		{
			p.close();
			return;
		}
	}
	char board[] = "rnbkqbnrpppppppp################################PPPPPPPPRNBKQBNR0";
	Game* Chess = new Game(board,1);
	Tool* ToolsPointer[64];
	Tool* t;
	int color, place, j = 0, y = 0;
	string s1 = "12", s2 = "12";
	string  msgToGraphics;
	char* msg;
	for (int i = 0; i < 64; i++)
	{
		if (i >= 32)
		{
			color = 0;
		}
		else
		{
			color = 1;
		}
		switch (board[i])
		{
			case'r':
			case 'R':
				 t = new Rook(board[i], color);
				ToolsPointer[i] = t;
				break;
			case 'k':
			case 'K':
				t = new King(board[i], color);
				ToolsPointer[i] = t;
				break;
			case 'B':
			case 'b':
				t = new Bishop(board[i], color);
				ToolsPointer[i] = t;
				break;
			case 'Q':
			case 'q':
				t = new Queen(board[i], color);
				ToolsPointer[i] = t;
				break;
			case 'N':
			case 'n':
				t = new Queen(board[i], color);
				ToolsPointer[i] = t;
				break;
			case 'P':
			case 'p':
				t = new Pawn(board[i], color);
				ToolsPointer[i] = t;
				break;
			default:
				t = new HashTag;
				ToolsPointer[i] = t;
				break;
		}
	}
	
	// msgToGraphics should contain the board string accord the protocol
	// YOUR CODE
	msgToGraphics.assign(board);
	msg = _strdup(msgToGraphics.c_str());
	p.sendMessageToGraphics(msg);   // send the board string
	Chess->printBoard();
	// get message from graphics
	string msgFromGraphics = p.getMessageFromGraphics();
	while (msgFromGraphics != "quit")
	{//a1a2
		s1.at(0) = msgFromGraphics[0];//a
		s1.at(1) = msgFromGraphics[1];//1
		s2.at(0) = msgFromGraphics[2];//a
		s2.at(1) = msgFromGraphics[3];//2
		place = ( ((int)s1.at(0) - 97)) + (8 - ((int)s1.at(1) - 48)) * 8;
		// t == tool in s1
		//system("cls");
		if(!(Chess->isValid(s1, s2, ToolsPointer[place],ToolsPointer)))
		{
			msgToGraphics.assign(Chess->getErrorType());
		}
		else
		{
			if (!Chess->inCheck(s1, s2, ToolsPointer[place],ToolsPointer))
			{
				msgToGraphics.assign(Chess->getErrorType());
			}
			else
			{
				msgToGraphics.assign(Chess->getErrorType());
				ToolsPointer[place]->move(s1, s2, board, ToolsPointer);
				Chess->setBoard(board);
				Chess->nextTurn();
				Chess->printBoard();
				Chess->setTools(s1, s2, ToolsPointer);
			}
		}
		// should handle the string the sent from graphics
		// according the protocol. Ex: e2e4           (move e2 to e4)

		// YOUR CODE
		//strcpy_s(msgToGraphics, "YOUR CODE"); // msgToGraphics should contain the result of the operation


		/******* JUST FOR EREZ DEBUGGING *****
		int r = rand() % 10; // just for debugging......
		msgToGraphics[0] = (char)(1 + '0');
		msgToGraphics[1] = 0;
		****** JUST FOR EREZ DEBUGGING ******/
	


		// return result to graphics		
		msg = _strdup(msgToGraphics.c_str());
		p.sendMessageToGraphics(msg);

		// get message from graphics
		msgFromGraphics = p.getMessageFromGraphics();
	}

	p.close();
}