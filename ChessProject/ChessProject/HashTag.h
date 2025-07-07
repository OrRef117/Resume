#pragma once
#include "GameSystem.h"
#include "Tool.h"

using namespace std;

class HashTag:public Tool
{
public:
	HashTag();
	~HashTag();
	virtual char getType();
	virtual int getColor();
	virtual bool isAlive();
	virtual void move(string in, string out, char* board, Tool** Tools);

protected:
	char _type;
	int _color;

};