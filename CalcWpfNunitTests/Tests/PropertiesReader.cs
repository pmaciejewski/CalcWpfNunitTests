using System;
using System.Collections.Generic;
using System.IO;

namespace CalcWpfNunitTests.Tests
{
    public class PropertiesReader
    {
        public PropertiesReader(string fileName)
        {
            ReadDictionaryFile(fileName);
        }

        private Dictionary<string, string> _dictionary = null;

        private void ReadDictionaryFile(string fileName)
        {
            _dictionary = new Dictionary<string, string>();

            /* 
             * property name should start with letter, number or _
             * property name can contain letters, numbers and _
             * values can be in format: ="value", ='value', or =value
             * spaces can be used only in value, do not use space before =
             * */
            string sPattern = "^[a-zA-Z1-9_]*=.*";

            foreach (string line in File.ReadAllLines(fileName))
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line, sPattern))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    // take value, remove "" or ''
                    if ((value.StartsWith("\"") && value.EndsWith("\"")) || (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    _dictionary.Add(key, value);
                }
            }
        }

        public string GetProperty(string key, string defaultValue)
        {
            // If key dosent exist, return default value
            return String.IsNullOrEmpty(_dictionary[key]) ? defaultValue : _dictionary[key];
        }
    }
}
