#pragma once
#include "Tool.h"
#include "GameSystem.h"

class King : public Tool
{

public:
	King(char type, int color);
	King(Tool* other);
	~King();
	virtual char getType();
	virtual int getColor();
	virtual bool isAlive();
	virtual void move(string in, string out, char* board,Tool** Tools);
	bool isCheck();

protected:
	char _type;
	int _color;
	bool _hasMoved;
	bool _hasEat;
};