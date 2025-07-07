#pragma warning(disable : 4996) // Disable warning 4996
#include<stdio.h>
#include <stdlib.h>
#include<string.h>
#include <time.h>

#define MAX_BOOKS 4
#define MAX_MEMBERS 300
#define MAX_SIZE 100
#define MAX_ID 10

typedef struct {

	int year;
	int month;
	int day;

}Date;
typedef struct  {
	char* BookName;
	char* AuthorName;
	Date  ReturnDate;
}Book;
typedef struct {
	char* Name;
	char Id[10];
	Date DateOfBirth;
	int nBooks;
	Book LoanBooks[4];
}LibMember;


int checkId(char* str);
int insertBirthDate(LibMember* libmember);
int insertName(LibMember* libmember);
int checkName(char* Name);
void convertToLower(char* str);
void initBooksArr(LibMember* libmember);
int insertBookDate(LibMember* libmember);
void removeBook(int index, LibMember* libmember);
int checkIfHaveBook(char* bookName, LibMember libmember);
void deleteMember(LibMember* libmember, int* nMems, char* id);
int isDatePast(Date date);
int search_id(LibMember libmember[], int nMems, char* id);
void RuppinSort(LibMember libmember[], int nMems);
void printMemeber(LibMember libmember);
void printDate(Date date);
void printBooks(Book book[]);
void printBook(Book book);


void add_member(LibMember* libmember, int *nMems);
void loan_books(LibMember* libmember, int nMems);
void return_book(LibMember* libmember, int nMems);
void check_books_overdue(LibMember* libmember, int nMems);
void delete_member(LibMember* libmember, int *nMems);
void print_members(LibMember* libmember, int nMems);
void quit(LibMember* libmember, int nMems);


void main()
{
	int choice = 0;
	LibMember libmembers[MAX_MEMBERS];
	int nMmes = 0;//fillArr(libmembers); // fill array with info

	do 
	{
		printf("\nDatabase System Menu:\n1. Add member\n2. Loan books\n3. Return books\n4. Check books overdue\n5. Delete member\n6. Display all members\n7. Quit\n");

		printf("Enter your choice: ");
		scanf("%d", &choice);
		printf("\n");
		while (getchar() != '\n');
		
		switch (choice)
		{
		case 1:
			add_member(libmembers, &nMmes);
			break;
		case 2:
			loan_books(libmembers, nMmes);
			break;
		case 3:
			return_book(libmembers, nMmes);
			break;
		case 4:
			check_books_overdue(libmembers, nMmes);
			break;
		case 5:
			delete_member(libmembers, &nMmes);
			break;
		case 6:
			print_members(libmembers, nMmes);
			break;
		case 7:
			printf("Exiting program.\n");
			break;
		default:
			printf("Invalid choice. Please enter a number from 1 to 7.\n");
		}
	} while (choice != 7);
	quit(libmembers,nMmes);
	return;
}

void add_member(LibMember *libmember, int *nMems)
{
	char tempId[MAX_ID];

	if (*nMems >= MAX_MEMBERS)
	{
		printf("Max amount of member cannot add new one. \n");
		return;
	}

	LibMember libTemp;
	printf("Adding member fucntion pressed please enter the asked info: \n");
	printf("Name: ");
	if (!insertName(&libTemp))
	{
		printf("Name inserted has disallowed characters\n");
		return;
	}

	printf("\nID: ");
	fgets(tempId, MAX_ID, stdin);
	if (checkId(tempId)==-1)
	{
		free(libTemp.Name);
		return;
	}
	if (search_id(libmember, *nMems, tempId) != -1)
	{
		printf("Id entered is already present in database\n");
		free(libTemp.Name);
		return;
	}

	strcpy(libTemp.Id, tempId);

	printf("\nDate of birth: \n");
	if(!insertBirthDate(&libTemp))
    {
		free(libTemp.Name);
		printf("Date inserted has disallowed characters\n");
		return;
	}
	libTemp.nBooks = 0;
	initBooksArr(&libTemp);

	
	libmember[*nMems] = libTemp; // enter the new library member struct into the array to the first availavle space
	printf("Member %s added succesfully\n", libmember[*nMems].Name);
	(*nMems)++;// increase number of members
	RuppinSort(libmember,*nMems); // sort array

	
	//return to main
}

void loan_books(LibMember* libmember, int nMems)
{
	char bookName[MAX_SIZE], authorName[MAX_SIZE];
	char idTemp[MAX_ID];
	int index = 0, c = 0;
	fflush(stdin);
	printf("Loan books fucntion pressed please enter the asked info: \n");
	printf("Please enter Id: ");
	fgets(idTemp, MAX_ID, stdin);
	index = search_id(libmember, nMems, idTemp);
	if (index==-1)
	{
		printf("Please enter a vailed id the current one caanot be found in the database\n");
		return;
	}
	else
		printMemeber(libmember[index]);
	
	if (libmember[index].nBooks >= MAX_BOOKS)
	{
		printf("The member already has 4 books at his possesion\n");
		return;
	}

	while ((c = getchar()) != '\n' && c != EOF);// לנקות את באפר

	printf("please enter the name of the book you wish to borrow\n");
	fgets(bookName, MAX_SIZE, stdin);
	bookName[strcspn(bookName, "\n")] = 0;


	printf("\nplease enter the name of the author who wrote the book you wish to borrow\n");
	fgets(authorName, MAX_SIZE, stdin);
	authorName[strcspn(authorName, "\n")] = 0;

	libmember[index].LoanBooks[libmember[index].nBooks].BookName = malloc(strlen(bookName) + 1);
	libmember[index].LoanBooks[libmember[index].nBooks].AuthorName = malloc(strlen(authorName) + 1);

	if (libmember[index].LoanBooks[libmember[index].nBooks].BookName == NULL || libmember[index].LoanBooks[libmember[index].nBooks].AuthorName == NULL) 
	{
		printf("\nMemory allocation failed. back to main menu\n");
		free(libmember[index].LoanBooks[libmember[index].nBooks].BookName);
		free(libmember[index].LoanBooks[libmember[index].nBooks].AuthorName);
		return;
	}

	strcpy(libmember[index].LoanBooks[libmember[index].nBooks].BookName, bookName);
	strcpy(libmember[index].LoanBooks[libmember[index].nBooks].AuthorName, authorName);

	

	if (!insertBookDate(&libmember[index]))
	{
		printf("something went wrong 1\n");
		return ;
	}
	
	libmember[index].nBooks++;

	printf("loaning oppertion finished succesfully!\n");

	//return to main

}

void return_book(LibMember* libmember, int nMems)
{
	char bookName[MAX_SIZE];
	char idTemp[MAX_ID];
	int index = 0, bookIndex = 0, c = 0;

	
	printf("Return books fucntion pressed please enter the asked info: \n");

	printf("Please enter Id: ");
	fgets(idTemp, MAX_ID, stdin);

	index = search_id(libmember, nMems, idTemp);
	if (index == -1)
	{
		printf("Please enter a vailed id the current one cannot be found in the database\n");
		return;
	}
	
	printMemeber(libmember[index]);


	if (libmember[index].nBooks <= 0)
	{
		printf("The member has 0 books at his possesion\n");
		return;
	}

	while ((c = getchar()) != '\n' && c != EOF);

	printf("please enter the name of the book u wish to return: ");
	fgets(bookName, 100, stdin);
	bookName[strcspn(bookName, "\n")] = 0;
	if ((bookIndex = checkIfHaveBook(bookName,libmember[index])) == -1)
	{
		printf("Book name does not match member's books names\n");
		return;//return to main
	}

	removeBook(bookIndex, libmember);
	libmember[index].nBooks--;
	printf("Book has been succesfully returned!\n");
	return 0;
}

void print_members(LibMember* libmember, int nMems)
{
	for (int i = 0; i < nMems; i++)
	{
		printMemeber(libmember[i]);
	}
} // works

void delete_member(LibMember* libmember, int *nMems)
{
	char idTemp[MAX_ID];
	int index = 0;
	fflush(stdin);//clean buffer after scanf
	printf("Delete member fucntion pressed please enter the asked info: \n");
	printf("Please enter Id: ");
	fgets(idTemp, MAX_ID, stdin);
	index = search_id(libmember, nMems, idTemp);
	if (index == -1)
	{
		printf("Please enter a vailed id the current one caanot be found in the database\n");
		return;
	}
	
	printMemeber(libmember[index]);



	deleteMember(libmember, nMems, idTemp);
	
} //works

void check_books_overdue(LibMember* libmember, int nMems)
{
	int flag = 0;
	int hasOverdueBooks = 0;
	printf("Members with Overdue Books:\n");
	for (int i = 0; i < nMems; i++)
	{
		flag = 0;
		for (int j = 0; j < libmember[i].nBooks; j++) 
		{		
			if (isDatePast(libmember[i].LoanBooks[j].ReturnDate)) 
			{
				if(!flag)
					printf("\nName: %s  ID: %s Books overdue: \n", libmember[i].Name, libmember[i].Id);
				flag = 1;
				printBook(libmember[i].LoanBooks[j]);
				hasOverdueBooks = 1;
			}			
		}


	}

	if (!hasOverdueBooks)
		printf("NONE\n");

	return 0; // tomain
}

void quit(LibMember* libmember, int nMems)
{
	for (int i = 0; i < nMems; ++i) {
		free(libmember[i].Name);

		for (int j = 0; j < libmember[i].nBooks; ++j) {
			free(libmember[i].LoanBooks[j].BookName);
			free(libmember[i].LoanBooks[j].AuthorName);
		}
	}
}



//add member helping functions
int checkId(char* str)
{

	for (int i = 0; i < 9; i++)
	{
		if (str[i] < '0' || str[i] > '9')
		{
			printf("Invalid input. Please enter only numbers.\n");
			return -1;
		}
	}

	return 0;
}
int insertBirthDate(LibMember* libmember)
{
	int year = 0, month = 0, day = 0;


	printf("please enter the year you were born: ");
	scanf("%d", &year);
	if (year < 1900 || year > 2024) //מעל 1900 מתחת ל2024 
	{
		printf("\nyear entered out of range please insert a year between 1900 - 2024\n");
		return 0;
	}


	printf("\nplease choose the number of the month you were born: ");
	scanf("%d", &month);
	if (month < 0 || month > 12) //מעל 0 מתחת ל12 
	{
		printf("\nmonth entered out of range please insert a month between 1 - 12\n");
		return 0;
	}

	printf("\nplease choose the number of the day you were born: ");
	scanf("%d", &day);
	if (day < 0 || day > 30) //מעל 0 מתחת 30 
	{
		printf("\nday entered out of range please insert a day between 1 - 30\n");
		return 0;
	}

	libmember->DateOfBirth.day = day;
	libmember->DateOfBirth.month = month;
	libmember->DateOfBirth.year = year;

	return 1;
}
int insertName(LibMember* libmember)
{
	char temp[MAX_SIZE];

	fgets(temp, MAX_SIZE, stdin);

	if (!checkName(temp))
	{
		printf("string entered has not allowed characters please make sure to use only letters and whitespace.\n");
		return 0;
	}

	convertToLower(temp);

	if (strlen(temp) > 0 && temp[strlen(temp) - 1] == '\n')    //מחליף את ההורדת שורה בסימן שמראה לקומפיילר שזה מחרוזת ולא מערך תווים
		temp[strlen(temp) - 1] = '\0';


	libmember->Name = malloc((strlen(temp) + 1) * sizeof(char)); //הגדרת זכרון בזמן ריצה


	if (libmember->Name == NULL)
	{
		printf("Memory allocation failed!\n");
		free(libmember->Name);
		return 0;
	}
	strcpy(libmember->Name, temp); // העברה של הערך

	return 1; // פעולת שמירת השם עברה בהצלחה
}
int checkName(char* Name)
{
	for (int i = 0; i < strlen(Name) - 1; i++)
	{
		if (!((Name[i] >= 'a' && Name[i] <= 'z') || (Name[i] >= 'A' && Name[i] <= 'Z') || Name[i] == ' '))
		{
			return 0;
		}
	}
	return 1;
}
void convertToLower(char* str)
{
	int i = 1;//מתחיל מאחד כי האות הראשונה תמיד תהפוך לגדולה

	if (str[0] >= 'a' && str[0] <= 'z') //אות ראשונה תמיד תהיה אות גדולה
		str[0] = str[0] - 32;
	while (str[i] != '\0')
	{
		if (str[i] >= 'A' && str[i] <= 'Z')
			str[i] = str[i] + 32; // העברה לאות קטנה
		else if (str[i] == ' ')// כל אות שהיא לא קטנה תהפוך לקטנה ובמידה ויש רווח האות שאחרי הרווח תמיד תהיה אות גדולה
		{
			str[i + 1] = str[i + 1] - 32;//העברה לאות גדולה
			i++;
		}

		i++;
	}
}
void initBooksArr(LibMember* libmember)
{
	for (int i = 0; i < 4; i++)
	{
		libmember->LoanBooks[i].AuthorName = NULL;
		libmember->LoanBooks[i].BookName = NULL;
		libmember->LoanBooks[i].ReturnDate.day = 0;
		libmember->LoanBooks[i].ReturnDate.month = 0;
		libmember->LoanBooks[i].ReturnDate.year = 0;
	}


}

//loan book helping functions
int insertBookDate(LibMember* libmember)
{

	time_t currentTime;
	struct tm* localTime;

	currentTime = time(NULL);
	localTime = localtime(&currentTime);

	int year = localTime->tm_year + 1900;
	int month = localTime->tm_mon + 1;
	int day = localTime->tm_mday;

	if (month == 12)
	{
		year++;
		month = 1;
	}
	else
		month++;

	libmember->LoanBooks[libmember->nBooks].ReturnDate.year = year;
	libmember->LoanBooks[libmember->nBooks].ReturnDate.month = month;      // Adjust month to 1-based index
	libmember->LoanBooks[libmember->nBooks].ReturnDate.day = day;

	return 1;

}

//delete book helping functions
void removeBook(int index, LibMember* libmember)
{
	if (index < 0 || index >= libmember->nBooks) {
		printf("Invalid index.\n");
		return;
	}
	free(libmember->LoanBooks[index].BookName);
	libmember->LoanBooks[index].BookName = NULL;
	free(libmember->LoanBooks[index].AuthorName);
	libmember->LoanBooks[index].AuthorName = NULL;

	libmember->LoanBooks[index].ReturnDate.day = 0;
	libmember->LoanBooks[index].ReturnDate.month = 0;
	libmember->LoanBooks[index].ReturnDate.year = 0;


	if (index != libmember->nBooks - 1) {
		libmember->LoanBooks[index] = libmember->LoanBooks[libmember->nBooks - 1];
	}
}
int checkIfHaveBook(char* bookName, LibMember libmember)
{
	int i = 0;
	for (int i = 0; i < libmember.nBooks; i++)
	{

		if (strcmp(libmember.LoanBooks[i].BookName, bookName) == 0)
			return i;
	}
	return -1;
}

//delete member helping functions
void deleteMember(LibMember* libmember, int* nMems, char* id)
{
	for (int i = 0; i < *nMems; i++)
	{
		if (strcmp(libmember[i].Id, id) == 0)
		{

			free(libmember[i].Name); // לשחרר זכרון
			for (int j = 0; j < libmember[i].nBooks; j++)
			{
				if (libmember[i].LoanBooks->BookName != NULL)
				{
					free(libmember[i].LoanBooks[j].BookName);
					free(libmember[i].LoanBooks[j].AuthorName);
				}
			}


			if (i != *nMems - 1)
				libmember[i] = libmember[*nMems - 1]; // להזיז את האחרון למקום שהורדנו



			(*nMems)--; // להוריד כמות מנויים באחד


			printf("Member with ID '%s' removed.\n", id);
			return;//
		}
	}
}

//check overdue helping functions
int isDatePast(Date date)
{
	time_t current_time;
	struct tm* local_time;


	time(&current_time);
	local_time = localtime(&current_time);


	if (date.year < local_time->tm_year + 1900)
		return 1;
	if (date.year == local_time->tm_year + 1900 && date.month < local_time->tm_mon + 1)
		return 1;
	if (date.year == local_time->tm_year + 1900 && date.month == local_time->tm_mon + 1 && date.day < local_time->tm_mday)
		return 1;

	return 0;
}

//print members helping functions
void printMemeber(LibMember libmember)
{
	int i = 0;
	printf("****************************************************************\n");
	printf("Showing member - %s Info:\n", libmember.Name);
	printf("Name: %s\n", libmember.Name);
	printf("ID: %s", libmember.Id);
	printf("\nDate of Birth: ");
	printDate(libmember.DateOfBirth);
	printf("Number of books currently booked: %d \n", libmember.nBooks);
	printf("Books info: \n");
	printBooks(libmember.LoanBooks);

}
void printDate(Date date)
{
	printf("%d.%d.%d\n", date.day, date.month, date.year);
}
void printBooks(Book book[])
{
	int i = 0;
	while (i < MAX_BOOKS)
	{
		if (book[i].BookName != NULL)
		{
			printBook(book[i]);
		}

		i++;
	}
}
void printBook(Book book)
{
	if (book.BookName == NULL)
		return;
	printf("Book Name: %s\n", book.BookName);
	printf("Author Name: %s\n", book.AuthorName);
	printf("Return Date: ");
	printDate(book.ReturnDate);
}


//search the desired id
int search_id(LibMember libmember[], int nMems, char* id)
{
	int i = 0;
	for (i = 0; i < nMems; i++)
	{
		if (strcmp(id, libmember[i].Id) == 0)
		{
			return i;
		}

	}
	return -1;
}

//sorts the array according to id field
void RuppinSort(LibMember libmember[], int nMems)
{
	int i, j;
	LibMember temp;
	for (i = 0; i < nMems; i++)
	{
		for (j = 0; j < nMems - i - 1; j++)
		{
			if (strcmp(libmember[j].Id, libmember[j + 1].Id) > 0)
			{
				temp = libmember[j];
				libmember[j] = libmember[j + 1];
				libmember[j + 1] = temp;
			}
		}
	}
}



int fillArr( LibMember libmember[])
{

	 LibMember person1 = {
	   .Name = "John Johnson",
	   .Id = "123456789",
	   .DateOfBirth = {1990, 10, 15},
	   .nBooks = 2,
	   .LoanBooks = {
		   {.BookName = "Book1", .AuthorName = "Author1", .ReturnDate = {2024, 4, 5}},
		   {.BookName = "Book2", .AuthorName = "Author2", .ReturnDate = {2024, 3, 30}}
	   }
	};

	 LibMember person2 = {
	   .Name = "Alice Lab",
	   .Id = "987654321",
	   .DateOfBirth = {1985, 5, 20},
	   .nBooks = 1,
	   .LoanBooks = {
		   {.BookName = "Book3", .AuthorName = "Author3", .ReturnDate = {2020, 1, 1}}
	   }
	};


	 LibMember person3 = {
		.Name = "Bobi Botten",
		.Id = "123789456",
		.DateOfBirth = {1988, 8, 10},
		.nBooks = 3,
		.LoanBooks = {
			{.BookName = "Book4", .AuthorName = "Author4", .ReturnDate = {2022, 9, 25}},
			{.BookName = "Book5", .AuthorName = "Author5", .ReturnDate = {2023, 4, 30}},
			{.BookName = "Book6", .AuthorName = "Author6", .ReturnDate = {2022, 11, 12}}
		}
	};
	libmember[0] = person1;
	libmember[1] = person2;
	libmember[2] = person3;

	printf("array filled\n");

	return 3;
}






