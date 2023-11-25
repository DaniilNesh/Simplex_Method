using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplex_Method
{
    class M_Method : SMethod
    {
        List<string> znak;
        string word;
        List<double> coefficients;
        List<double> delts;
        double M;

        public M_Method(List<List<double>> perem, List<double> function, string label, List<string> symbols) : base(perem, function, symbols)
        {
            perem[perem.Count - 1].Remove(perem[perem.Count - 1].Last());
            int length = perem[0].Count;
            word = label;
            M = 9999;
            znak = symbols;
            coefficients = new List<double>();
            for (var i = 0; i < perem.Count - 1; i++)
                coefficients.Add(0);

            for (int i = 0; i < Func.Count; i++)
                Func[i] *= -1;

            int count = symbols.FindAll(x => x == ">=").Count();
            double[,] matrix = new double[perem.Count - 1, count];
            int index = 0;
            for (int i = 0; i < znak.Count; i++)
            {
                if (znak[i].Equals(">="))
                {
                    matrix[i, index++] = 1;
                    coefficients[i] = M * (-1);
                    perem[perem.Count - 1].Add(-M);
                }
            }

            for (int i = 0; i < perem.Count - 1; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    perem[i].Add(matrix[i, j]);
                    Swap(perem[i]);
                }
            }
            delts = new List<double>();
            foreach (var i in perem[0])
                delts.Add(0);
            GetDelts();
            Cap_Left.Add("Delts");
            for(int i = 0; i < count; i++)
            {
                Cap_Top.Add($"x{length + i}");
                Swap(Cap_Top);
            }
            To_Table();
        }
        /// <summary>
        /// Выполнение М Метода
        /// </summary>
        public void M_Action()
        {            
            if(znak.FindIndex(x => x.Equals(">=")) == -1)
            {
                for(int i = 0; i < Variables[Variables.Count-1].Count; i++)
                {
                    Variables[Variables.Count - 1][i] *= -1;
                }
                base.Action();
                return;
            }
            int count = 0;
            for (int i = 0; i < Variables.Count; i++)
            {
                if (Variables[i][Variables[i].Count - 1] < 0)
                    count++;
            }
            if (count == Variables.Count - 1)
            {
                Console.WriteLine("Нет решений");
                return;
            }
            while (Find_Column())
            {
                string str = Find_Row();
                if (!str.Equals(string.Empty))
                {
                    Console.WriteLine(str);
                    return;
                }

                double buffer = Variables[Row][Column];
                for (int i = 0; i < Variables[Row].Count; i++)
                {
                    Variables[Row][i] /= buffer;
                }

                for (int j = 0; j < Variables.Count-1; j++)
                {
                    if (Variables[j] == Variables[Row])
                        continue;
                    buffer = Variables[j][Column];

                    for (int k = 0; k < Variables[j].Count; k++)
                    {
                        Variables[j][k] += (Variables[Row][k] * (-1 * buffer));
                    }
                }

                coefficients[Row] = Variables[Variables.Count-1][Column];
                GetDelts();

                To_Table();
            }


            foreach(var i in coefficients)
            {
                if(i / M >= 1)
                    Console.WriteLine("Нет оптимального решения задачи. Искусственные переменные остались в базисе");
            }
            return;
        }
        /// <summary>
        /// Поиск разрешающего столбца
        /// </summary>
        /// <returns>1 - есть столбец; 0 - нет такого; index - номер столбца</returns>
        bool Find_Column()
        {
            List<double> function = new List<double>();
            for (int i = 0; i < delts.Count - 1; i++)
                function.Add(delts[i]);
            //function.RemoveAt(delts.Count - 1);
            int index = function.IndexOf(function.Min());
            if (function[index] >= 0)
            {
                return false;
            }
            int count = 0;
            for (int i = 0; i < Variables.Count - 1; i++)
            {
                if (Variables[i][index] <= 0)
                    count++;
            }
            if (count == Variables.Count - 1)
            {
                Console.WriteLine("Нет решений");
                return false;
            }

            Column = index;
            return true;
        }
        /// <summary>
        /// Находит разрешающую строку
        /// </summary>
        /// <returns></returns>
        string Find_Row()
        {
            List<double> relationship = new List<double>();
            List<List<double>> buffer = new List<List<double>>();
            int res = 0;
            int count = 0;
            for(int i = 0; i < Variables.Count; i++)
            {
                if (Variables[i][Variables[i].Count - 1] < 0)
                    count++;
            }
            if (count == Variables.Count)
                return "Все коэффициенты В отрицательные";
            for (int i = 0; i < Variables.Count - 1; i++)
            {
                if (Variables[i][Column] > 0)
                {
                    double variable = Variables[i][Variables[i].Count - 1] / Variables[i][Column];
                    relationship.Add(variable);
                }
                else if (i != Variables.Count - 1)
                {
                    relationship.Add(8888);
                }
            }
            count = 0;
            for (int i = 0; i < relationship.Count; i++)
            {
                if (relationship[i] <= 0)
                    count++;
            }
            if (count == relationship.Count)
                return "Все симплексные отношения отрицательные";
            double min = relationship.Min();
            res = relationship.FindAll(
                delegate (double x)
                {
                    return x == min;
                }).Count;

            if (res != 1)
                return Find_Row_2();
            Row = relationship.IndexOf(relationship.Min());
            return string.Empty;
        }
        /// <summary>
        /// Если нашлось несколько наименьших значений, то используем правило Креко
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        string Find_Row_2()
        {
            int res = 0;
            int j = 1;
            List<double> relationship = new List<double>();
            while (res != 1)
            {
                relationship.Clear();
                for (int i = 0; i < Variables.Count - 1; i++)
                {
                    double variable = Variables[i][j] / Variables[i][Column];
                    relationship.Add(variable);
                }
                double min = relationship.Min();
                res = relationship.FindAll(
                    delegate (double x)
                    {
                        return x == min;
                    }).Count;
                j++;
            }

            Row = relationship.IndexOf(relationship.Min());
            return  string.Empty;
        }
        /// <summary>
        /// Пересчет для значения Дельта
        /// </summary>
        private void GetDelts()
        {
            for (int i = 0; i < delts.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < Variables.Count - 1; j++)
                {
                    sum += coefficients[j] * Variables[j][i];
                }
                if (i != delts.Count - 1)
                    delts[i] = sum - Func[i];
                else
                    delts[i] = sum;
            }
        }
        /// <summary>
        /// Выводит результаты в виде таблицы
        /// </summary>
        private void To_Table()
        {
            Output outp = new Output();
            outp.Draw_Row(Cap_Top.ToArray());
            Cap_Left[Row] = Cap_Top[Column + 1];
            for (int i = 0; i < Variables.Count + 1; i++)
            {
                if (i == Variables.Count)
                {
                    Ceil(delts);
                    double[] a = delts.ToArray();                    
                    outp.Draw_Row(Cap_Left[i], a);
                    break;
                }
                Ceil(Variables[i]);
                double[] arr = Variables[i].ToArray();                
                outp.Draw_Row(Cap_Left[i], arr);
                //if (i == Variables.Count - 1)
                //{
                //    Console.WriteLine("\t \t");

                //    outp.Draw_line();
                //}
            }
            outp.Draw_line();
            Console.WriteLine("\n");

        }
    }
}