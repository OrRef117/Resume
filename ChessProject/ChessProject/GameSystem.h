#pragma once

#include <string>
using namespace std;

class Tool;
class Rook;
class King;
class Game
{
public:
	Game(char* board,int turn);
	~Game();
	char* getBoard();
	bool isValid(string in, string out,Tool* t,Tool** Tools);
	void setBoard(char* board);
	bool isEnded();
	void printBoard();
	string getErrorType();
	bool inCheck(string in,string out,Tool* t,Tool** Tools);
	string getLoc(int num);
	void setTools(string in, string out, Tool** Tools);
	bool checkMoveQ(string in, string out,Tool* t,Tool** Tools);
	bool checkMoveK(string in, string out, Tool* t, Tool** Tools);
	bool checkMoveR(string in, string out, Tool* t, Tool** Tools);
	bool checkMoveN(string in, string out,Tool* t,Tool** Tools);
	bool checkMoveB(string in, string out,Tool* t,Tool** Tools);
	bool checkMoveP(string in, string out,Tool* t,Tool** Tools);
	int getTurn();
	void nextTurn();

protected:
	char _board[66];
	int _turn;
	bool hasEnded;
	string _errorType;
	Tool* Tools[64];

};