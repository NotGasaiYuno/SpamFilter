﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MailService.SpamFiltering
{
    public class DataProcessing
    { 
        private readonly static string[] _stopWords;
        private readonly static char[] _separators;
        private readonly static string[] _endings;
        private readonly string _text;

        static DataProcessing()
        {
            _stopWords = new string[]
            {
                "a", "the", "am", "is", "are", "he", "she", "it", "they", "them", "this", "that", "those", "will",
                "have", "has", "would", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
            };
            _separators = new char[] { ' ', ',', '-', '.', '!', '?', '/', '\"', '\t', '\n', '\r' };
            _endings = new string[] { "s", "es", "ies" };
        }

        public DataProcessing(string text) => _text = AllToLower(text);

        private string AllToLower(string text) => text.ToLower();

        private string[] Tokenization() => _text.Split(_separators);

        private void StopWordsRemoval(ref string[] textVector) 
            => textVector = textVector.Where(element => !_stopWords.Contains(element)).ToArray();

        private void Stemming(string[] textVector)
        {
            for (int i = 0; i < textVector.Length; i++)
            {
                ChangeEnding(ref textVector[i]);
                AllAfterApostropheRemoval(ref textVector[i]);
            }
        }

        private void ChangeEnding(ref string word) 
        {
            foreach (var ending in _endings.OrderBy(ending => ending.Count()))
            {

                var regex = new Regex(ending + "$");

                //if ()
                //{

                //}
            }
            word = word.TrimEnd('s', 'e', 'i');
        }

        private void AllAfterApostropheRemoval(ref string word)
        {
            string apostrophe = "\'";

            if (word.Contains(apostrophe) && word.IndexOf(apostrophe) is int index && index == word.LastIndexOf(apostrophe))
            {
                word.Remove(index);
            }
            else
            {
                word.Replace(apostrophe, string.Empty);
            }
        }

        public IEnumerable<string> Start()
        {
            string[] textVector = Tokenization();

            StopWordsRemoval(ref textVector);
            Stemming(textVector);

            Array.Sort(textVector);

            return textVector;
        }
    }
}