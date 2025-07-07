#include "Knight.h"
#include "GameSystem.h"

using namespace std;

Knight::Knight(char type, int color) :Tool(type, color), _hasMoved(false), _hasEat(false)
{
	_type = type;
	_color = color;
}
Knight::~Knight()
{

}
char Knight::getType()
{
	return _type;
}
int Knight::getColor()
{
	return _color;
}
bool Knight::isAlive()
{
	return true;
}
void Knight::move(string in, string out, char* board, Tool** Tools)
{
	int place1 = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, place2 = (((int)out.at(0) - 97)) + (8 - ((int)out.at(1) - 48)) * 8;
	char temp;
	if (Tools[place1]->getEat())
	{
		board[place2] = board[place1];
		board[place1] = '#';

	}
	else
	{
		temp = board[place1];
		board[place1] = board[place2];
		board[place2] = temp;
	}
	_hasEat = false;
	_hasMoved = true;
}

