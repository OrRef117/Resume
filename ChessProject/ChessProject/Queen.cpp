#include "Queen.h"
#include "GameSystem.h"

using namespace std;

Queen::Queen(char type, int color) :Tool(type, color), _hasMoved(false), _hasEat(false)
{
	_type = type;
	_color = color;
}
Queen::~Queen()
{

}
char Queen::getType()
{
	return _type;
}
int Queen::getColor()
{
	return _color;
}
bool Queen::isAlive()
{
	return true;
}
void Queen::move(string in, string out, char* board, Tool** Tools)
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

