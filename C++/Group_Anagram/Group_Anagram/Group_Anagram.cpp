// Author : Nirmal Gopinath Janardhanan
// Date : June, 2019 
/*############################################################################
Problem 2: Anagram Groups.
Write a C++, (and C#) program that accepts a block of text (English words), and outputs the groups of words that are anagrams of each other.  For example, “apt”, “tap” and “pat” are anagrams of each other.

Assumptions:
1. Duplicate words are allowed for Anagram. This can be easily changed in the implementation if needed to be restricted.
2. Characters are treated as case-sensitive. This can be easily changed in the implementation if required.

Solution proposal
1. This is an iterative problem and can be solved by recursion by breaking down the system to subsystems.
2. For memory efficiency, computing of anagram is solved with in-place re-arrangement of words by swapping rather than creating intermediate additional memory.

Possible improvements
1. Additional exception handling can be added for specific exceptions. Currently, internal APIs catches the exception, traces it and rethrows. The main program catches exception and traces it and doesn't re-throw.
2. Additional test cases can be added including negative test cases for error input and runtime exceptions.
3. Extension of test cases with possible UT frameworks like CppUnit
4. Currently, the number of test cases is pre-configured to 4. This can be moved to an external file configuration for easy extensibility
############################################################################*/

#include "stdafx.h"
#include <vector>
#include <string>
#include <iostream>
#include <exception>

//predfined number of test cases
const unsigned int numOfTestCases = 4;

//Simply display the current container of the words
void displayWords(const std::vector<std::string> words_in, std::string comments_in = "")
{
	std::cout << "\n" << comments_in;
	for (unsigned int i = 0; i < words_in.size(); ++i)
		std::cout << words_in[i] << " ";
}

// Swapping two characters in the word based on the indices
void swap(std::string& word_in_out, unsigned int id_1_in, unsigned int id_2_in)
{
	char aTemp = word_in_out[id_1_in];
	word_in_out[id_1_in] = word_in_out[id_2_in];
	word_in_out[id_2_in] = aTemp;
}

// Main API for recursive invocation of anagram operation
void GetAnagrams(std::string& word_in, unsigned int start_index_in, unsigned int end_index_in, std::vector<std::string>& anagrams_out)
{
	try
	{
		if (start_index_in > end_index_in)
			return;
		if (start_index_in == end_index_in)
			anagrams_out.push_back(word_in);

		//step 1: For the current index fixed, do Recursive call with the subsequent indices
		GetAnagrams(word_in, start_index_in + 1, end_index_in, anagrams_out);

		//Step 2: Iterate the current index value by swapping with the remaining indices and perform the same recursion  
		for (unsigned int i = start_index_in + 1; i <= end_index_in; ++i)
		{
			swap(word_in, start_index_in, i);
			GetAnagrams(word_in, start_index_in + 1, end_index_in, anagrams_out);
			swap(word_in, i, start_index_in);
		}
	}
	catch (const std::exception& ex)
	{
		std::cout << "Exception caught in GetAnagrams() : " << ex.what();
		throw;
	}
}

// Group the anagram words.
// Use the above anagram API to get the anagram for a specific word
// Iterate through the words to group the anagrams
// For computational efficiency, once a word is grouped into an anagram group, remove it from the searching words
void GroupAnagramWords(std::vector<std::string>& words_in, std::vector<std::vector<std::string>>& anagramGroups_out)
{
	try
	{
		anagramGroups_out.resize(0);
		size_t wordsCount = words_in.size();
		size_t word_id = 0;

		std::vector<std::string> validAnagram;
		std::vector<std::string> allAnagrams;

		//Pick a word
		while (word_id < wordsCount)
		{
			//add the word to the new anagram group
			validAnagram.resize(0);
			validAnagram.push_back(words_in[word_id]);

			//identify all the possible anagrams
			allAnagrams.resize(0);
			GetAnagrams(words_in[word_id], 0, words_in[word_id].length() - 1, allAnagrams);

			//check and add if a following word is a possible anagram
			size_t next_word_id = word_id + 1;
			while (next_word_id < wordsCount)
			{
				if (std::find(allAnagrams.begin(), allAnagrams.end(), words_in[next_word_id]) != allAnagrams.end())
				{
					validAnagram.push_back(words_in[next_word_id]);
					words_in.erase(words_in.begin() + next_word_id);
					--wordsCount;
				}
				else
					++next_word_id;
			}

			//store the newly added anagram group
			anagramGroups_out.push_back(validAnagram);
			++word_id;
		}
	}
	catch (const std::exception& ex)
	{
		std::cout << "Exception caught in GroupAnagramWords() : " << ex.what();
		throw;
	}
}

//Initialise the input for specific test cases
void initializeTestInput(std::vector<std::string>& words_in_out, int TestCase_num_in)
{
	try
	{
		words_in_out.resize(0);
		switch (TestCase_num_in)
		{
			//Simple words as input
		case 1:
		{
			words_in_out.push_back("Let");
			words_in_out.push_back("etL");
			words_in_out.push_back("the");
			words_in_out.push_back("eht");			
			words_in_out.push_back("everyone");
			words_in_out.push_back("happy");
			words_in_out.push_back("eth");			
			break;
		}
			//Words with duplicates as input
		case 2:
		{
			words_in_out.push_back("Let");
			words_in_out.push_back("etL");
			words_in_out.push_back("the");
			words_in_out.push_back("the"); // duplicate
			words_in_out.push_back("eht");
			words_in_out.push_back("everyone");
			words_in_out.push_back("happy");
			words_in_out.push_back("eth");
			break;
		}
		//characters with duplicate as input
		case 3:
		{
			words_in_out.push_back("A");
			words_in_out.push_back("A"); // duplicate
			words_in_out.push_back("B");
			words_in_out.push_back("C");
			break;
		}
		//Words with duplicates and case-sensitive as input
		case 4:
		{
			words_in_out.push_back("Let");
			words_in_out.push_back("etL");
			words_in_out.push_back("the");
			words_in_out.push_back("the"); // duplicate
			words_in_out.push_back("eht");
			words_in_out.push_back("everyone");
			words_in_out.push_back("happy");
			words_in_out.push_back("etH"); //case-sensitive
			break;
		}
		
		case 0:
		default: throw new std::invalid_argument("Invalid test case number");
		};
	}
	catch (const std::exception& ex)
	{
		std::cout << "Exception caught in initializeTestInput() : " << ex.what();
		throw;
	}
}

//Main calling program for Anagram
int _tmain(int argc, _TCHAR* argv[])
{
	std::vector<std::string> aWords;

	unsigned int aTestCase = 1;
	while (aTestCase <= numOfTestCases)
	{
		try
		{
			std::cout << "\nPerform the anagram grouping for Test Case : " << aTestCase;

			//Initialize input with specific test case
			initializeTestInput(aWords, aTestCase);

			//display the input for the selected test case
			displayWords(aWords, "Input : ");

			//perform the anagram
			std::cout << "\nOutput :";
			std::vector<std::vector<std::string>> aWordsOutput;
			GroupAnagramWords(aWords, aWordsOutput);
			for (int i = 0; i < aWordsOutput.size(); ++i)
				displayWords(aWordsOutput[i], "Group : ");
			std::cout << "\n";
		}
		catch (const std::exception& ex)
		{
			std::cout << "Exception caught while performing Anagram : " << ex.what();
		}
		++aTestCase;
	}
	return 0;
}

