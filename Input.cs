using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Simplex_Method
{
    class Input
    {
        public static List<List<double>> Pass(List<List<string>> strs, out List<string> znak, out string keyWord, out List<double> func)
        {
            List<List<double>> variables = new List<List<double>>();
            int length;

            int.TryParse(strs[0][1].Substring(1), out length);
            strs.RemoveAt(0);
            for (int i = 0; i < strs.Count - 1; i++)
            {
                variables.Add(new List<double>());
                int j = 0;
                double buffer;
                while (variables[i].Count != length)
                {
                    if (strs[i][j + 1] == $"x{variables[i].Count + 1}")
                    {
                        Double.TryParse(strs[i][j], out buffer);
                        variables[i].Add(buffer);
                    }
                    else if (strs[i][j] == $"x{variables[i].Count + 1}")
                    {
                        variables[i].Add(1);
                        j--;
                    }
                    else
                    {
                        variables[i].Add(0);
                        j -= 2;
                    }
                    j += 2;
                }
                double.TryParse(strs[i][strs[i].Count-1], out buffer);
                variables[i].Add(buffer);
            }
            func = ParseFunc(strs[strs.Count - 1], length);
            keyWord = ParseKeyWors(strs[strs.Count - 1]);
            znak = ParseZnak(strs);

            return variables;
        }

        private static List<double> ParseFunc(List<string> item, int length)
        {
            List<double> func = new List<double>();
            int j = 0;
            while (func.Count != length)
            {
                if (item[j] == "F")
                {
                    j++;
                    continue;
                }
                if (item[j + 1] == $"x{func.Count + 1}")
                {
                    double buffer;
                    if(!Double.TryParse(item[j], out buffer))
                    {
                        double.TryParse(item[j], NumberStyles.Float, CultureInfo.InvariantCulture, out buffer);
                    }
                    func.Add(buffer);
                }
                else
                {
                    func.Add(0);
                    j -= 2;
                }
                j += 2;
            }
            return func;
        }

        private static string ParseKeyWors(List<string> item)
        {
            return item[item.Count - 3] + item[item.Count - 2] + item[item.Count - 1];
        }

        private static List<string> ParseZnak(List<List<string>> item)
        {
            List<string> variable = new List<string>();

            for (int i = 0; i < item.Count - 1; i++)
                variable.Add(item[i][item[i].Count - 2]);

            return variable;
        }
        /// <summary>
        /// Читает запись из файла
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static List<List<string>> Scan(StreamReader stream)
        {
            string word = string.Empty;
            List<List<string>> wordBuffer = new List<List<string>>() { new List<string>() };
            int index = 0;
            while (!stream.EndOfStream)
            {
                var symbol = (char)stream.Read();
                if (char.IsDigit(symbol))
                {
                    word += symbol;
                }
                else if (char.IsLetter(symbol))
                {
                    if (word != "")
                        wordBuffer[index].Add(word);
                    word = symbol.ToString();
                    if (word == "F")
                    {
                        wordBuffer[index].Add(word);
                        word = "";
                    }
                }
                else if (symbol == '+')
                {
                    if (word != "")
                        wordBuffer[index].Add(word);
                    word = "";
                }
                else if (symbol == '-')
                {
                    if (word != "")
                        wordBuffer[index].Add(word);
                    word = "-";
                }
                else if (symbol == '.')
                {
                    if(word.Length == 1)
                    {
                        var value = word.Substring(word.Length - 1).ToCharArray();
                        if (char.IsDigit(value[0]))
                            word += symbol;
                    }
                }
                else if (symbol == '<' || symbol == '>')
                {
                    if (word != "")
                        wordBuffer[index].Add(word);
                    word = symbol.ToString();
                    symbol = (char)stream.Read();
                    word += symbol.ToString();
                    wordBuffer[index].Add(word);
                    word = "";
                }
                else if (symbol == '\n')
                {
                    if (word != "")
                        wordBuffer[index].Add(word);
                    word = "";
                    wordBuffer.Add(new List<string>());
                    index++;
                }
            }
            wordBuffer.RemoveAt(index);
            return wordBuffer;
        }
    }
}
