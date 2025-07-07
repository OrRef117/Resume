#include "Tool.h"

using namespace std;

Tool::Tool(char type, int color)
{
	_type = type;
	_color = color;
	_hasEat = false;
	_trheat = false;
}
Tool::~Tool()
{

}

bool Tool::getEat()
{
	return _hasEat;
}

void Tool::SetEat(bool b)
{
	_hasEat = b;
}

void Tool::SetThreat(bool b)
{
	_trheat = b;
}

bool Tool::getThreat()
{
	return _trheat;
}