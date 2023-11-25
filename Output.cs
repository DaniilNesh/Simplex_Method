using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex_Method
{
    class Output
    {
        string column;
        string line;
        int col_column;

        public Output()
        {
            column = "|";
            line = "";
            for (int i = 0; i < 16; i++)
            {
                line += "-";
            }
            col_column = 0;
        }

        public void Draw_Row(string name, params double[] list)
        {
            string newLine = "";
            col_column = list.Length;
            for (int i = 0; i < col_column + 1; i++)
            {
                newLine += line;
            }

            Console.WriteLine(newLine);

            newLine = column + "\t" + name + "\t" + column;
            for (int i = 0; i < col_column; i++)
            {
                newLine += "\t" + list[i] + "\t" + column;
            }

            Console.WriteLine(newLine);
        }

        public void Draw_Row(params string[] list)
        {
            string newLine = "";
            col_column = list.Length;
            for (int i = 0; i < col_column ; i++)
            {
                newLine += line;
            }

            Console.WriteLine(newLine);

            newLine = column;
            for (int i = 0; i < col_column; i++)
            {
                newLine += "\t" + list[i] + "\t" + column;
            }

            Console.WriteLine(newLine);
        }

        public void Draw_line()
        {
            string newLine = "";
            for (int i = 0; i < col_column+1; i++)
            {
                newLine += line;
            }
            Console.WriteLine(newLine);
        }
    }
}
