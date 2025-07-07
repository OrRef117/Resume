#include "GameSystem.h"
#include "Tool.h"
#include <iostream>
#include "HashTag.h"
#include "King.h"
#include "Queen.h"

using namespace std;

Game::Game(char* board, int turn)
{
	int i = 0;
	if (turn != 0 && turn != 1)
	{
		_turn = 1;
	}
	else
	{
		_turn = turn;
	}
	while (board[i] != NULL)
	{
		i++;
	}
	if (i > 65)
	{
		cout << "Error the board isnt big enough" << endl;
	}
	else
	{
		for (i = 0; i < 66; i++)
		{
			_board[i] = board[i];
		}
	}

	hasEnded = false;
	char* _errorType = new char[10];
}

Game::~Game()
{

}

char* Game::getBoard()
{
	return _board;
}

void Game::setBoard(char* board)
{
	int i = 0;
	while (board[i] != NULL)
	{
		i++;
	}
	if (i > 65)
	{
		cout << "Error the board isnt big enough please RESET IT!!" << endl;
	}
	else
	{
		for (i = 0; i < 65; i++)
		{
			_board[i] = board[i];
		}
	}
}

int Game::getTurn()
{
	return _turn;
}

void Game::nextTurn()
{
	if (!getTurn())
	{
		_turn = 1;
	}
	else
	{
		_turn = 0;
	}
}

bool Game::isValid(string in, string out, Tool* t,Tool** Tools)
{
	char a = t->getType();
	switch (a)
	{
	case 'q':
		if (_turn != 0)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveQ(in, out, t, Tools));
			break;
		}
	case 'Q':
		if (_turn != 1)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveQ(in, out, t, Tools));
			break;
		}
	case 'k':
		if (_turn != 0)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveK(in, out, t, Tools));
			break;
		}
	case 'K':
		if (_turn != 1)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveK(in, out, t, Tools));
			break;
		}
	case 'r':
		if (_turn != 0)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveR(in, out, t, Tools));
			break;
		}
	case 'R':
		if (_turn != 1)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveR(in, out, t, Tools));
			break;
		}
	case 'p':
		if (_turn != 0)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveP(in, out, t, Tools));
			break;
		}
	case 'P':
		if (_turn != 1)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveP(in, out, t, Tools));
			break;
		}
	case 'b':
		if (_turn != 0)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveB(in, out, t, Tools));
			break;
		}
	case 'B':
		if (_turn != 1)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveB(in, out, t, Tools));
			break;
		}
	case 'n':
		if (_turn != 0)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveN(in, out, t, Tools));
			break;
		}
	case 'N':
		if (_turn != 1)
		{
			_errorType = "2";
			return false;
			break;
		}
		else
		{
			return(checkMoveN(in, out, t, Tools));
			break;
		}
	case '#':
		_errorType = "2";
		return false;
		break;
	default:
		cout << "Error" << endl;
	}
	return false;
}

bool Game::checkMoveK(string in, string out, Tool* t, Tool** Tools)//done
{
	int Index = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, Outdex = (((int)out.at(0) - 97)) + (8 - ((int)out.at(1) - 48)) * 8;
	int flag = 0;
	string s1 = "12";
	if (in.at(0) == out.at(0) && in.at(1) == out.at(1))
	{
		_errorType = "7";
		return false;
	}
	if (Tools[Index]->getType()== '#')
	{
		_errorType = "2";
		return false;
	}
	for (int i = 97; i <= 104; i++)
	{
		if (out.at(0) == (char)i || in.at(0) == (char)i)             //wrong index
		{
			flag = 1;
		}
	}
	if (flag == 0)
	{
		this->_errorType = "5";
		return false;
	}
	flag = 0;
	for (int i = 49; i <= 56; i++)
	{
		if (out.at(1) == (char)i || in.at(1) == char(i))         //wrong index
		{
			flag = 1;
		}
	}
	if (flag == 0)
	{
		this->_errorType = "5";
		return false;
	}


	if (abs(in.at(0) - out.at(0)) > 1)
	{
		_errorType = "6";
		return false;
	}
	if (abs(in.at(1) - out.at(1)) > 1)
	{
		_errorType = "6";
		return false;
	}
	if (Tools[Outdex]->getType() != '#')
	{
		if (Tools[Outdex]->getColor() == Tools[Index]->getColor())
		{
			this->_errorType = "3";
			return false;
		}
		else
		{
			t->SetEat(true);
		}
	}
	_errorType = "0";
	return true;
}

bool Game::checkMoveR(string in, string out, Tool* t,Tool** Tools) //done
{
	int counter = 0, flag = 0, Index = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8,Outdex = ((int)out.at(0) - 97) + (8 - ((int)out.at(1) - 48)) * 8;
	t->SetEat(false);
	int in1 = (int)in.at(1) - 48, in2 = (int)out.at(1) - 48;
	int out1 = (int)in.at(0) - 97, out2 = (int)out.at(0) - 97;
	if (Tools[Index]->getType() == '#')
	{
		this->_errorType = "2";
		return false;
	}
	if (in.at(0) == out.at(0) && in.at(1) == out.at(1))       //same cordinates
	{
		this->_errorType = "7";
		return false;
	}
	if (in.at(0) != out.at(0))           //not rook movement
	{
		if (in.at(1) != out.at(1))
		{
			this->_errorType = "6";
			return false;
		}
		else
		{
			counter = 1;
		}
	}
	for (int i = 97; i <= 104; i++) 
	{
		if (out.at(0) == (char)i || in.at(0) == (char)i)             //wrong index
		{
			flag = 1;
		}
	}
	if (flag == 0)
	{
		this->_errorType = "5";
		return false;
	}
	flag = 0;
	for (int i = 49; i <= 56; i++)
	{
		if (out.at(1) == (char)i || in.at(1) == char(i))         //wrong index
		{
			flag = 1;
		}
	}
	if (flag == 0)
	{
		this->_errorType = "5";
		return false;
	}
	if (Tools[Outdex]->getType() != '#')
	{
		if (Tools[Outdex]->getColor() == Tools[Index]->getColor())
		{
			this->_errorType = "3";
			return false;
		}
		else
		{
			t->SetEat(true);
		}
		
	}
	if (in1 < in2)
	{
		for (int j = in1 + 1; j < in2; j++)
		{
			if (Tools[(((8-j)*8)+ out1)]->getType() != '#')
			{
				t->SetEat(false);
				_errorType = "6";
				return false;
			}
		}
	}
	else if (in1>in2)
	{
		for (int j = in2+1; j < in1;j++)
		{
			if (Tools[(((8-j)*8) + out1 )]->getType() != '#')
			{
				t->SetEat(false);
				_errorType = "6";
				return false;
			}
		}
	}
	else
	{
		if (out1 < out2)
		{
			for (int j = out1 + 1; j < out2; j++)
			{
				if (Tools[(j+ (8*(8 - in1)))]->getType() != '#')
				{
					t->SetEat(false);
					_errorType = "6";
					return false;
				}
			}
		}
		else if (out2 < out1)
		{
			for (int j = out2+1; j < out1; j++)
			{
				if (Tools[(j+ (8* (8 - in2)) )]->getType() != '#')
				{
					t->SetEat(false);
					_errorType = "6";
					return false;
				}
			}
		}
	}
	
	_errorType = "0";
	return true;
}

bool Game::checkMoveB(string in, string out, Tool* t, Tool** Tools)//done
{
	int counter = 0, flag = 0, Index = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, Outdex = ((int)out.at(0) - 97) + (8 - ((int)out.at(1) - 48)) * 8;
	t->SetEat(false);
	int in1 = (int)in.at(1) - 48, in2 = (int)out.at(1) - 48;
	int out1 = (int)in.at(0) - 97, out2 = (int)out.at(0) - 97;
	if (Tools[Index]->getType() == '#')
	{
		this->_errorType = "2";
		return false;
	}
	if (in.at(0) == out.at(0) && in.at(1) == out.at(1))       //same cordinates
	{
		this->_errorType = "7";
		return false;
	}
	if (!(abs(in1 - in2) >= 1 && abs(out1 - out2) >= 1))
	{
		_errorType = "6";
		return false;
	}
	else
	{
		if (abs(in1 - in2) != abs(out1 - out2))
		{
			_errorType = "6";
			return false;
		}
	}
	if (in1 < in2) //b1c2
	{
		for (int i = 1; i <in2-in1 ; i++)
		{
			if (out1 < out2)//right top
			{
				if (Tools[(8 - (in1 + i)) * 8 + (out1 + i)]->getType() != '#')
				{
					_errorType = "6";
					return false;
				}
			}
			else//left top
			{
				if (Tools[(8 - (in1 + i)) * 8 + (out1 - i)]->getType() != '#')
				{
					_errorType = "6";
					return false;
				}
			}
		}
	}
	else
	{
		for (int i = 1; i <in1-in2; i++)
		{
			if (out1 > out2) //right bot
			{
				if (Tools[(8 - (in2 + i)) * 8 + (out2 + i)]->getType() != '#')
				{
					_errorType = "6";
					return false;
				}
			}
			else//left bot
			{
				if (Tools[(8 - (in2 + i)) * 8 + (out2 - i)]->getType() != '#')
				{
					_errorType = "6";
					return false;
				}
			}
		}

	}



	if (Tools[Outdex]->getType() != '#')
	{
		if (Tools[Outdex]->getColor() == Tools[Index]->getColor())
		{
			this->_errorType = "3";
			return false;
		}
		else
		{
			t->SetEat(true);
		}

	}
	_errorType = "0";
	return true;

}

bool Game::checkMoveQ(string in, string out, Tool* t, Tool** Tools)
{
	int counter = 0, flag = 0, Index = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, Outdex = ((int)out.at(0) - 97) + (8 - ((int)out.at(1) - 48)) * 8;
	t->SetEat(false);
	int in1 = (int)in.at(1) - 48, in2 = (int)out.at(1) - 48;
	int out1 = (int)in.at(0) - 97, out2 = (int)out.at(0) - 97;
	if (Tools[Index]->getType() == '#')
	{
		this->_errorType = "2";
		return false;
	}
	if (in.at(0) == out.at(0) && in.at(1) == out.at(1))       //same cordinates
	{
		this->_errorType = "7";
		return false;
	}
	if (in.at(0) != out.at(0))           //not rook movement
	{
		if (in.at(1) != out.at(1))
		{
			if (!(abs(in1 - in2) >= 1 && abs(out1 - out2) >= 1))
			{
				_errorType = "6";
				return false;
			}
			else
			{
				if (abs(in1 - in2) != abs(out1 - out2))
				{
					_errorType = "6";
					return false;
				}
			}
		}
		else
		{
			flag = 1;
		}
	}
	else
	{
		flag = 1;
	}

	if (flag == 1)
	{
		if (in1 < in2)
		{
			for (int j = in1 + 1; j < in2; j++)
			{
				if (Tools[(((8 - j) * 8) + out1)]->getType() != '#')
				{
					_errorType = "6";
					return false;
				}
			}
		}
		else if (in1>in2)
		{
			for (int j = in2 + 1; j < in1; j++)
			{
				if (Tools[(((8 - j) * 8) + out1)]->getType() != '#')
				{
					_errorType = "6";
					return false;
				}
			}
		}
		else
		{
			if (out1 < out2)
			{
				for (int j = out1 + 1; j < out2; j++)
				{
					if (Tools[(j + (8 * (8 - in1)))]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
			}
			else if (out2 < out1)
			{
				for (int j = out2 + 1; j < out1; j++)
				{
					if (Tools[(j + (8 * (8 - in2)))]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
			}
		}
	}
	else
	{
		if (in1 < in2) //b1c2
		{
			for (int i = 1; i <in2 - in1; i++)
			{
				if (out1 < out2)//right top
				{
					if (Tools[(8 - (in1 + i)) * 8 + (out1 + i)]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
				else//left top
				{
					if (Tools[(8 - (in1 + i)) * 8 + (out1 - i)]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
			}
		}
		else
		{
			for (int i = 1; i <in1 - in2; i++)
			{
				if (out1 > out2) //right bot
				{
					if (Tools[(8 - (in2 + i)) * 8 + (out2 + i)]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
				else//left bot
				{
					if (Tools[(8 - (in2 + i)) * 8 + (out2 - i)]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
			}

		}
	}
	if (Tools[Outdex]->getType() != '#')
	{
		if (Tools[Outdex]->getColor() == Tools[Index]->getColor())
		{
			this->_errorType = "3";
			return false;
		}
		else
		{
			t->SetEat(true);
		}

	}
	_errorType = "0";
	return true;
}

bool Game::checkMoveN(string in, string out, Tool* t, Tool** Tools)//done
{
	int counter = 0, flag = 0, Index = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, Outdex = ((int)out.at(0) - 97) + (8 - ((int)out.at(1) - 48)) * 8;
	t->SetEat(false);
	int in1 = (int)in.at(1) - 48, in2 = (int)out.at(1) - 48;
	int out1 = (int)in.at(0) - 97, out2 = (int)out.at(0) - 97;
	if (Tools[Index]->getType() == '#')
	{
		this->_errorType = "2";
		return false;
	}
	if (in.at(0) == out.at(0) && in.at(1) == out.at(1))       
	{
		this->_errorType = "7";
		return false;
	}
	if (abs(in1 - in2) != 2 || abs(out1 - out2) != 1)
	{
		if (abs(in1 - in2) != 1 || abs(out1 - out2) != 2)
		{
			_errorType = "6";
			return false;
		}
	}
	if (Tools[Outdex]->getType() != '#')
	{
		if (Tools[Outdex]->getColor() == Tools[Index]->getColor())
		{
			this->_errorType = "3";
			return false;
		}
		else
		{
			t->SetEat(true);
		}

	}
	_errorType = "0";
	return true;
}

bool Game::checkMoveP(string in, string out, Tool* t, Tool** Tools)//done
{
	int counter = 0, flag = 0, Index = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, Outdex = ((int)out.at(0) - 97) + (8 - ((int)out.at(1) - 48)) * 8;
	t->SetEat(false);
	int in1 = (int)in.at(1) - 48, in2 = (int)out.at(1) - 48;
	int out1 = (int)in.at(0) - 97, out2 = (int)out.at(0) - 97;
	if (Tools[Index]->getType() == '#')
	{
		this->_errorType = "2";
		return false;
	}
	if (in.at(0) == out.at(0) && in.at(1) == out.at(1))
	{
		this->_errorType = "7";
		return false;
	}
	if (t->getColor() == 0)//white
	{
		if (in2 - in1 != 1 || out2 != out1)
		{
			if (in1 == 2) //go 2
			{
				if (in2 - in1 == 2 && out2 == out1)
				{
					if (Tools[Outdex]->getType() != '#')
					{
						_errorType = "3";
						return false;
					}
					else if (Tools[(8-(in2-1))*8 + out2]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
				else if (!(in2 - in1 == 1 && out2 == out1))
				{
					if (in2 - in1 == 1 && abs(out1 - out2) == 1)
					{
						if (Tools[Outdex]->getColor() != 1)
						{
							_errorType = "3";
							return false;
						}
						t->SetEat(true);
					}
					else
					{
						_errorType = "6";
						return false;
					}
				}
				else if (Tools[Outdex]->getType() != '#')
				{
					_errorType = "3";
					return false;
				}
			}
			else
			{
				if (in2 - in1 == 1)
				{
					if (out1 == out2)
					{
						if (Tools[Outdex]->getType() != '#')
						{
							_errorType = "3";
							return false;
						}
					}
					else
					{
						if (abs(out2 - out1) == 1)
						{
							if (Tools[Outdex]->getColor() != 1)
							{
								_errorType = "3";
								return false;
							}
							t->SetEat(true);
						}
						else
						{
							_errorType = "6";
							return false;
						}
					}
				}
				else
				{
					_errorType = "6";
					return false;
				}
			}
		}
		else
		{
			if (Tools[Outdex]->getType() != '#')
			{
				_errorType = "3";
				return false;
			}
		}
	}
	else
	{
		if (in1 - in2 != 1 || out2 != out1)
		{
			if (in1 == 7) //go 2
			{
				if (in1 - in2 == 2 && out2 == out1)
				{
					if (Tools[Outdex]->getType() != '#')
					{
						_errorType = "3";
						return false;
					}
					else if (Tools[(8 - (in1-1)) * 8 + out2]->getType() != '#')
					{
						_errorType = "6";
						return false;
					}
				}
				else if (!(in1 - in2 == 1 && out2 == out1))
				{
					if (in1 - in2 == 1 && abs(out1 - out2) == 1)
					{
						if (Tools[Outdex]->getColor() != 1)
						{
							_errorType = "3";
							return false;
						}
						t->SetEat(true);
					}
					else
					{
						_errorType = "6";
						return false;
					}
				}
				else if (Tools[Outdex]->getType() != '#')
				{
					_errorType = "3";
					return false;
				}
			}
			else
			{
				if (in1 - in2 == 1)
				{
					if (out1 == out2)
					{
						if (Tools[Outdex]->getType() != '#')
						{
							_errorType = "3";
							return false;
						}
					}
					else
					{
						if (abs(out2 - out1) == 1)
						{
							if (Tools[Outdex]->getColor() != 0)
							{
								_errorType = "3";
								return false;
							}
							t->SetEat(true);
						}
						else
						{
							_errorType = "6";
							return false;
						}
					}
				}
				else
				{
					_errorType = "6";
					return false;
				}
			}
		}
		else
		{
			if (Tools[Outdex]->getType() != '#')
			{
				_errorType = "3";
				return false;
			}
		}
	}
	_errorType = "0";
	return true;
}

bool Game::isEnded()
{
	return hasEnded;
}

void Game::printBoard()
{
	int i = 0, j = 0;

	while (_board[i+1])
	{
		for (j = 0; j < 8; j++)
		{
			cout <<_board[i + j];
			cout <<" ";

		}
		cout << "" << endl;
		i += j;
	}
}

string Game::getErrorType()
{
	return _errorType;
}

void Game::setTools(string in,string out,Tool** Tools)
{

	int place1 = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, place2 = (((int)out.at(0) - 97)) + (8 - ((int)out.at(1) - 48)) * 8;
	Tool* temp;
	Tool* t = new HashTag;
	if (Tools[place1]->getEat())
	{
		*(Tools + place2) = *(Tools + place1);
		*(Tools + place1) = t;

	}
	else
	{
		temp = *(Tools + place1);
		*(Tools + place1) = *(Tools + place2);
		 *(Tools + place2) = temp;
	}
	//delete temp;
	//delete t;
	
}

bool Game::inCheck(string in,string out,Tool* t,Tool** Tools)
{
	int Index = (((int)in.at(0) - 97)) + (8 - ((int)in.at(1) - 48)) * 8, Outdex = ((int)out.at(0) - 97) + (8 - ((int)out.at(1) - 48)) * 8, i = 0;
	Tool* Wking = nullptr;
	Tool* Bking = nullptr;
	Tool** temp = Tools;
	char* board = _board;
	bool b;
	int white, black, j = 0, x = 0;
	while (i < 64)
	{
		if (Tools[i]->getType() == 'K')
		{
			white = i;
			Wking = new King(Tools[i]); //white king class
		}
		else if (Tools[i]->getType() == 'k')
		{
			black = i;
			Bking = new King(Tools[i]); // black king class
		}
		i++;
	}
	if (t->getColor() == Wking->getColor()) //white turn
	{ 
		b = t->getEat();
		t->move(in,out,_board,Tools);
		t->SetEat(b);
		this->setBoard(_board);
		this->setTools(in, out, temp);
		if (t->getType() == 'K')
		{
			i = 0;
			while (i < 64)
			{
				if (temp[i]->getType() == 'K')
				{
					white = i;
				}
				else if (temp[i]->getType() == 'k')
				{
					black = i;
				}
				i++;
			}
		}
		nextTurn();
		j = 0;
		while (j < 64)
		{
			if (temp[j]->getColor() == Bking->getColor())
			{
				b = t->getEat();
				if (isValid(getLoc(j), getLoc(white), temp[j], temp))
				{
					delete Wking;
					delete Bking;
					nextTurn();
					t->SetEat(b);
					_errorType = "4";
					this->setTools(in, out, Tools);
					this->setBoard(board);
					return false;
				}
				t->SetEat(b);
			}
			j++;
		}
		nextTurn();
		b = t->getEat();
		if (isValid(out, getLoc(black), t, temp))//return if next turn i can eat king
		{
		    delete Wking;
			delete Bking;
			t->SetEat(b);
			_errorType = "1";//i made chesss
			this->setTools(in, out, Tools);
			this->setBoard(board);
			return true;
		}
		else
		{
			delete Wking;
			delete Bking;
			t->SetEat(b);
			_errorType = "0";
			this->setTools(in, out, Tools);
			this->setBoard(board);
			return true;
		}
	}
	else//black turn
	{
		b = t->getEat();
		t->move(in, out, _board, Tools);
		t->SetEat(b);
		this->setBoard(_board);
		this->setTools(in, out, temp);
		nextTurn();
		if (t->getType() == 'k')
		{
			i = 0;
			while (i < 64)
			{
				if (temp[i]->getType() == 'K')
				{
					white = i;
				}
				else if (temp[i]->getType() == 'k')
				{
					black = i;
				}
				i++;
			}
		}
		while (j < 64)
		{
			if (temp[j]->getColor() == Wking->getColor())
			{
				b = t->getEat();
				if (isValid(getLoc(j), getLoc(black), temp[j], temp))
				{
					delete Wking;
					delete Bking;
					t->SetEat(b);
					_errorType = "4";//i made chesss
					nextTurn();
					this->setTools(in, out, Tools);
					this->setBoard(board);
					return false;
				}
				t->SetEat(b);
			}
			j++;
		}
		nextTurn();
		b = t->getEat();
		if (isValid(out, getLoc(white), t, temp))//return if next turn i can eat king
		{
			delete Wking;
			delete Bking;
			_errorType = "1";//i made chesss
			t->SetEat(b);
			this->setTools(in, out, Tools);
			this->setBoard(board);
			return true;
		}
		else
		{
			delete Wking;
			delete Bking;
			_errorType = "0";
			t->SetEat(b);
			this->setTools(in, out, Tools);
			this->setBoard(board);
			return true;
		}
	}
	return true;
}

string Game::getLoc(int num) //24 a5
{
	int number = 0 , letter = 0;
	string s = "12";
	number = num / 8;
	number = 8-number;
	letter = num % 8;
	s.at(1) = char(number + 48);
	s.at(0) = char(letter + 97);
	return s;
}