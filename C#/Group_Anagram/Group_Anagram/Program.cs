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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group_Anagram
{
    class Anagram_Generator
    {
        //Simply display the current container of the words
        public void displayWords(IList<string> words_in, string comments_in = "")
        {
	        System.Console.Write(comments_in);
	        for (int i = 0; i < words_in.Count(); ++i)
                System.Console.Write(words_in[i] + " ");
        }

        // Swapping two characters in the word based on the indices
        private void swap(ref string word_in_out, int id_1_in, int id_2_in)
        {
            StringBuilder aStr = new StringBuilder(word_in_out);

            char aTemp = aStr[id_1_in];
            aStr[id_1_in] = aStr[id_2_in];
            aStr[id_2_in] = aTemp;

            word_in_out = aStr.ToString();
        }

        // Main API for recursive invocation of anagram operation
        private void GetAnagrams(string word_in, int start_index_in, int end_index_in, IList<string> anagrams_out)
        {
	        try
	        {
		        if (start_index_in > end_index_in)
			        return;
		        if (start_index_in == end_index_in)
                    anagrams_out.Add(word_in);
			       
		        //step 1: For the current index fixed, do Recursive call with the subsequent indices
		        GetAnagrams(word_in, start_index_in + 1, end_index_in, anagrams_out);

		        //Step 2: Iterate the current index value by swapping with the remaining indices and perform the same recursion  
		        for (int i = start_index_in + 1; i <= end_index_in; ++i)
		        {
			        swap(ref word_in, start_index_in, i);
			        GetAnagrams(word_in, start_index_in + 1, end_index_in, anagrams_out);
			        swap(ref word_in, i, start_index_in);
		        }
	        }
	        catch (System.Exception ex)
	        {
		        System.Console.WriteLine("Exception caught in GetAnagrams() : " + ex.ToString());
		        throw;
	        }
        }

        // Group the anagram words.
        // Use the above anagram API to get the anagram for a specific word
        // Iterate through the words to group the anagrams
        // For computational efficiency, once a word is grouped into an anagram group, remove it from the searching words
        public void GroupAnagramWords(IList<string> words_in, IList<IList<string>> anagramGroups_out)
        {
            try
            {
                anagramGroups_out.Clear();
	            int wordsCount = words_in.Count();
	            int word_id = 0;

	            
                IList<string> allAnagrams = new List<string>();
	
	            //Pick a word
	            while (word_id < wordsCount)
	            {
		            //add the word to the new anagram group
                    IList<string> validAnagram = new List<string> { words_in[word_id] };

		            //identify all the possible anagrams
		            allAnagrams.Clear();
		            GetAnagrams(words_in[word_id], 0, words_in[word_id].Count() - 1, allAnagrams);	
		
		            //check and add if a following word is a possible anagram
		            int next_word_id = word_id + 1;
		            while (next_word_id < wordsCount)
		            {
                        if (allAnagrams.Contains(words_in[next_word_id]))			  
			            {		
				            validAnagram.Add(words_in[next_word_id]);
                            words_in.RemoveAt(next_word_id);				            
				            --wordsCount;
			            }
			            else
				            ++next_word_id;
		            } 

		            //store the newly added anagram group
		            anagramGroups_out.Add(validAnagram);
		            ++word_id;
	            }
            }
            catch (System.Exception ex)
	        {
		        System.Console.WriteLine("Exception caught in GetAnagrams() : " + ex.ToString());
		        throw;
	        }
        }

        //Initialise the input for specific test cases
        public void initializeTestInput(IList<string> words_in_out, int TestCase_num_in)
        {
	        try
	        {
		        words_in_out.Clear();
		        switch (TestCase_num_in)
		        {
			        //Simple words as input
		        case 1:
		        {
			        words_in_out.Add("Let");
			        words_in_out.Add("etL");
			        words_in_out.Add("the");
			        words_in_out.Add("eht");			
			        words_in_out.Add("everyone");
			        words_in_out.Add("happy");
			        words_in_out.Add("eth");			
			        break;
		        }
			        //Words with duplicates as input
		        case 2:
		        {
			        words_in_out.Add("Let");
			        words_in_out.Add("etL");
			        words_in_out.Add("the");
			        words_in_out.Add("the"); // duplicate
			        words_in_out.Add("eht");
			        words_in_out.Add("everyone");
			        words_in_out.Add("happy");
			        words_in_out.Add("eth");
			        break;
		        }
		        //characters with duplicate as input
		        case 3:
		        {
			        words_in_out.Add("A");
			        words_in_out.Add("A"); // duplicate
			        words_in_out.Add("B");
			        words_in_out.Add("C");
			        break;
		        }
		        //Words with duplicates and case-sensitive as input
		        case 4:
		        {
			        words_in_out.Add("Let");
			        words_in_out.Add("etL");
			        words_in_out.Add("the");
			        words_in_out.Add("the"); // duplicate
			        words_in_out.Add("eht");
			        words_in_out.Add("everyone");
			        words_in_out.Add("happy");
			        words_in_out.Add("etH"); //case-sensitive
			        break;
		        }
		
		        case 0:
		        default: throw new ArgumentException("Invalid test case number");
		        };
	        }
            catch (System.Exception ex)
	        {
		        System.Console.WriteLine("Exception caught in initializeTestInput() : " + ex.ToString());
		        throw;
	        }	        
        }
    }

    class Program
    {
        //predfined number of test cases
        private const int numOfTestCases = 4;

        static void Main(string[] args)
        {
            Anagram_Generator aAnagram_Generator = new Anagram_Generator();
            IList<string> aWords = new List<string>();

	        int aTestCase = 1;
	        while (aTestCase <= numOfTestCases)
	        {
		        try
		        {
			        System.Console.WriteLine("\n\nPerform the anagram grouping for Test Case : " + aTestCase);

			        //Initialize input with specific test case
                    aAnagram_Generator.initializeTestInput(aWords, aTestCase);

			        //display the input for the selected test case
                    aAnagram_Generator.displayWords(aWords, "Input : ");

			        //perform the anagram
                    System.Console.Write("\nOutput :");                    
			        IList<IList<string>> aWordsOutput = new List<IList<string>>();
                    aAnagram_Generator.GroupAnagramWords(aWords, aWordsOutput);
			        for (int i = 0; i < aWordsOutput.Count(); ++i)
                        aAnagram_Generator.displayWords(aWordsOutput[i], "\nGroup : ");                    
		        }

                catch (System.Exception ex)
	            {
		            System.Console.WriteLine("Exception caught while performing Anagram : " + ex.ToString());
		        }
		        ++aTestCase;
	        }
        }
    }
}
