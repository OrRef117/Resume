#include "King.h"

using namespace std;


King::King(char type, int color) :Tool(type, color), _hasMoved(false), _hasEat(false)
{
	_type = type;
	_color = color;
}
King::King(Tool* other) : Tool(other->getType(), other->getColor())
{
	_type = other->getType();
	_color = other->getColor();
	_hasEat = other->getEat();
	_trheat = other->getThreat();
}
King::~King()
{

}
char King::getType()
{
	return _type;
}

int King::getColor()
{
	return _color;
}
bool King::isAlive()
{
	return true;
}
void King::move(string in, string out, char* board,Tool** Tools)
{
	char temp;
	if (_hasEat == true)
	{
		board[(((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8] = board[(((int)out.at(0) - 97)) + (8 - ((int)out.at(1) - 48)) * 8];
	}
	else
	{
		temp = board[(((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8];
		board[(((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8] = board[(((int)out.at(0) - 97)) + (8 - ((int)out.at(1) - 48)) * 8];
		board[(((int)out.at(0) - 97)) + (8 - ((int)out.at(1) - 48)) * 8] = temp;
	}
	_hasEat = false;
	_hasMoved = true;

}
