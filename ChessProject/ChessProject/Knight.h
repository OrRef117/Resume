#pragma once
#include "Tool.h"
#include "GameSystem.h"

//class King;
class Knight : public Tool
{
public:
	Knight(char type, int color);
	~Knight();
	virtual char getType();
	virtual int getColor();
	virtual bool isAlive();
	virtual void move(string in, string out, char* board, Tool** Tools);

protected:
	char _type;
	int _color;
	bool _hasMoved;
	bool _hasEat;
};