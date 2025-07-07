#include "HashTag.h"
#include "GameSystem.h"

using namespace std;


HashTag::HashTag() :Tool('#',-1)
{
	_color = -1;
	_type = '#';
}
void HashTag::move(string in, string out, char* board,Tool** Tools)
{

}
bool HashTag::isAlive()
{
	return false;
}
HashTag::~HashTag()
{

}
char HashTag::getType()
{
	return _type;
}
int HashTag::getColor()
{
	return _color;
}