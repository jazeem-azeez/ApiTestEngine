using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace ApiTestEngine.Shared.Services
{
    /// <summary></summary>
    public class HandleBarParser
    {
        //TODO: Leverage Concurrent Dictionary
        /// <summary> The handelbar pattern </summary>
        private Regex _handelbarPattern = new Regex(@"{{([^{}]+)}}");

        /// <summary> Initializes the <see cref="HandleBarParser" /> class. </summary>
        static HandleBarParser()
        {
            GlobalVariablePool = new Dictionary<string, string>();
        }

        /// <summary> Initializes a new instance of the <see cref="HandleBarParser" /> class. </summary>
        /// <param name="dictionary"> The dictionary. </param>
        public HandleBarParser(Dictionary<string, string> dictionary)
        {
            _variablePool = dictionary;
        }

        /// <summary> Gets or sets the global variable pool. </summary>
        /// <value> The global variable pool. </value>
        public static Dictionary<string, string> GlobalVariablePool { get; set; }


        /// <summary> Gets or sets the variable pool. </summary>
        /// <value> The variable pool. </value>
        public Dictionary<string, string> _variablePool { get; set; }

        /// <summary> Upserts the global variable pool. </summary>
        /// <param name="globalVariablesCollection"> The global variables collection. </param>
        public static void UpsertGlobalVariablePool(Dictionary<string, string> globalVariablesCollection)
        {
            foreach (var item in globalVariablesCollection)
            {
                UpsertGlobalVariablePool(item.Key, item.Value);
            }
        }

        /// <summary> Upserts the global variable pool. </summary>
        /// <param name="key">   The key. </param>
        /// <param name="value"> The value. </param>
        public static void UpsertGlobalVariablePool(string key, string value)
        {
            UpsertDictionary("{{" + key + "}}", value, GlobalVariablePool);
        }

        /// <summary> Processes the input string. </summary>
        /// <param name="value"> The value. </param>
        /// <returns></returns>
        public string ProcessInputString(string value)
        {
            string result = value;
            if (!string.IsNullOrEmpty(result))
            {
                var matches = _handelbarPattern.Matches(result);
                do
                {
                    foreach (var item in matches)
                    {
                        string replaceVar = string.Empty;
                        string key = item.ToString();

                        if (key.Replace(" ", "").StartsWith("{{$"))
                        {
                            replaceVar = GetPreDefValues(key: key.Replace(" ", ""));
                        }
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                        {
                            replaceVar = ConfigurationManager.AppSettings[key];
                        }
                        if (GlobalVariablePool.ContainsKey(key))
                        {
                            replaceVar = GlobalVariablePool[key];
                        }
                        if (_variablePool.ContainsKey(key))
                            replaceVar = _variablePool[key];

                        result = result.Replace(key, replaceVar);
                    }
                    matches = _handelbarPattern.Matches(result);
                } while (matches.Count > 0);
            }
            return result;
        }

        /// <summary> Processes the input string. </summary>
        /// <param name="headers"> The headers. </param>
        /// <returns></returns>
        public Dictionary<string, string> ProcessInputString(Dictionary<string, string> headers)
        {
            var tempDictionary = new Dictionary<string, string>();
            foreach (var item in headers)
            {
                tempDictionary.Add(item.Key, ProcessInputString(item.Value));
            }
            return tempDictionary;
        }

        /// <summary> Processes the input string. </summary>
        /// <param name="expectedResponse"> The expected response. </param>
        /// <returns></returns>
        public string[] ProcessInputString(string[] expectedResponse)
        {
            var tempStrList = new List<string>();
            foreach (var item in expectedResponse)
            {
                tempStrList.Add(ProcessInputString(item));
            }
            return tempStrList.ToArray();
        }

        /// <summary> Upserts the local variable. </summary>
        /// <param name="key">   The key. </param>
        /// <param name="value"> The value. </param>
        internal void UpsertLocalVariable(string key, string value)
        {
            //TODO: Handle CircularReference if value is as handler
            UpsertDictionary("{{" + key + "}}", value, _variablePool);
        }

        /// <summary> Upserts the dictionary. </summary>
        /// <param name="key">        The key. </param>
        /// <param name="value">      The value. </param>
        /// <param name="dictionary"> The dictionary. </param>
        private static void UpsertDictionary(string key, string value, Dictionary<string, string> dictionary)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary> Gets the pre definition values. </summary>
        /// <param name="key"> The key. </param>
        /// <returns></returns>
        private string GetPreDefValues(string key)
        {
            switch (key)
            {
                case "{{$guid}}": return Guid.NewGuid().ToString();
                case "{{$time}}": return DateTime.Now.ToShortTimeString();
                case "{{$date}}": return DateTime.Now.ToShortDateString();
                case "{{$rand}}": return new Random().NextDouble().ToString();
                default: return string.Empty;
            }
        }
    }
}