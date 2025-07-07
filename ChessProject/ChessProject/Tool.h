#pragma once
#include "GameSystem.h"

using namespace std;

class Tool
{
public:
	Tool(char type,int color);
	~Tool();
	virtual char getType() = 0;
	virtual int getColor() = 0;
	virtual bool isAlive() = 0;
	virtual bool getEat();
	virtual void SetEat(bool b);
	virtual void SetThreat(bool b);
	virtual bool getThreat();
	virtual void move(string in, string out, char* board,Tool** Tools) = 0;

protected:
	char _type;
	int _color;
	bool _hasEat;
	bool _trheat;

};