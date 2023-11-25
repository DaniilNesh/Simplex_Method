using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplex_Method
{
    class SMethod
    {
        List<List<double>> variables;
        List<double> zFunction;
        int ras_column;
        int ras_row;
        List<string> cap_top;
        List<string> cap_left;

        /// <summary>
        /// Свойства
        /// </summary>
        #region
        public List<List<double>> Variables
        {
            get
            {
                return variables;
            }
            set
            {
                variables = value;
            }
        }
        public List<double> Func
        {
            get
            {
                return zFunction;
            }
            set
            {
                zFunction = value;
            }
        }
        public int Column
        {
            get
            {
                return ras_column;
            }
            set
            {
                ras_column = value;
            }
        }
        public int Row
        {
            get
            {
                return ras_row;
            }
            set
            {
                ras_row = value;
            }
        }
        public List<string> Cap_Top
        {
            get
            {
                return cap_top;
            }
            set
            {
                cap_top = value;
            }
        }
        public List<string> Cap_Left
        {
            get
            {
                return cap_left;
            }
            set
            {
                cap_left = value;
            }
        }
        #endregion
        public SMethod(List<List<double>> perem, List<double> function, List<string> znak)
        {
            cap_top = new List<string>();
            cap_left = new List<string>();
            variables = perem;
            int left_length = 0;
            for (int i = 0; i < variables.Count; i++)
            {
                for (int j = 0; j < variables.Count; j++)
                {
                    if (i == j)
                    {
                        if (znak[i].Equals("<="))
                        {
                            variables[i].Add(1);
                            Swap(variables[i]);
                        }else if (znak[i].Equals(">="))
                        {
                            variables[i].Add(-1);
                            Swap(variables[i]);
                        }                        
                        continue;
                    }
                    if (!znak[i].Equals("="))
                    {
                        variables[i].Add(0);
                        Swap(variables[i]);
                    }
                    
                }
                left_length++;
            }

            zFunction = function;

            for (int i = 0; i < zFunction.Count; i++)
                zFunction[i] *= -1;
            for (int j = 0; j < variables.Count + 1; j++)
                zFunction.Add(0);
            variables.Add(zFunction);

            cap_top.Add("xi");
            for (int i = 0; i < variables[0].Count; i++)
            {
                if (i == variables[0].Count - 1)
                {
                    cap_top.Add("B");
                    break;
                }
                cap_top.Add($"x{i + 1}");
            }

            for (int i = variables[0].Count - left_length - 1; i < variables[0].Count; i++)
            {
                if (i == variables[0].Count - 1)
                {
                    cap_left.Add("Z");
                    break;
                }
                cap_left.Add($"x{i + 1}");
            }
            //To_Table();
            Output outp = new Output();
            outp.Draw_Row(Cap_Top.ToArray());
            //Cap_Left[Row] = Cap_Top[Column + 1];
            for (int i = 0; i < Variables.Count; i++)
            {
                Ceil(Variables[i]);
                double[] arr = Variables[i].ToArray();
                outp.Draw_Row(Cap_Left[i], arr);
            }
            outp.Draw_line();
            Console.WriteLine("\n");
        }

        public List<List<double>> Action()
        {
            Output outp = new Output();
            int count = 0;
            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i][variables[i].Count - 1] < 0)
                    count++;
            }
            if (count == variables.Count - 1)
            {
                Console.WriteLine("Нет решений");
                return null;
            }

            while (Find_Column(out ras_column, variables))
            {
                ras_row = Find_Row();
                if (ras_row == -1)
                {
                    Console.WriteLine("Отрицателтный коэффициент в столбце В");
                    return null;
                }

                double buffer = variables[ras_row][ras_column];
                for (int i = 0; i < variables[ras_row].Count; i++)
                {
                    variables[ras_row][i] /= buffer;
                }

                for (int j = 0; j < variables.Count; j++)
                {
                    if (variables[j] == variables[ras_row])
                        continue;
                    buffer = variables[j][ras_column];

                    for (int k = 0; k < variables[j].Count; k++)
                    {
                        variables[j][k] += (variables[ras_row][k] * (-1 * buffer));
                    }
                }
                outp.Draw_Row(cap_top.ToArray());
                cap_left[ras_row] = cap_top[ras_column + 1];
                for (int i = 0; i < variables.Count; i++)
                {
                    Ceil(variables[i]);
                    double[] arr = variables[i].ToArray();                    
                    outp.Draw_Row(cap_left[i], arr);
                }
                outp.Draw_line();
                Console.WriteLine("\n");
            }

            return null;
        }

        /// <summary>
        /// Смена последнего и предпоследнего элементов
        /// </summary>
        /// <param name="list"></param>
        protected void Swap<T>(List<T> list)
        {
            T tmp = list[list.Count - 1];
            list[list.Count - 1] = list[list.Count - 2];
            list[list.Count - 2] = tmp;
        }
        /// <summary>
        /// Поиск разрешающего столбца
        /// </summary>
        /// <returns>1 - есть столбец; 0 - нет такого; index - номер столбца</returns>
        bool Find_Column(out int index, List<List<double>> variables)
        {
            index = zFunction.IndexOf(zFunction.Min());
            if (zFunction[index] >= 0)
            {
                return false;
            }
            int count = 0;
            for (int i = 0; i < variables.Count - 1; i++)
            {
                if (variables[i][index] <= 0)
                    count++;
            }
            if (count == variables.Count - 1)
            {
                Console.WriteLine("Нет решений");
                return false;
            }

            return true;
        }
        /// <summary>
        /// Находит разрешающую строку
        /// </summary>
        /// <returns></returns>
        int Find_Row()
        {
            List<double> relationship = new List<double>();
            List<List<double>> buffer = new List<List<double>>();
            int res = 0;
            for (int i = 0; i < variables.Count - 1; i++)
            {
                if (variables[i][ras_column] > 0)
                {
                    double variable = variables[i][variables[i].Count - 1] / variables[i][ras_column];
                    relationship.Add(variable);
                }
                else if (i != variables.Count - 1)
                {
                    relationship.Add(8888);
                }
            }
            int count = 0;
            for (int i = 0; i < relationship.Count; i++)
            {
                if (relationship[i] <= 0)
                    count++;
            }
            if (count == relationship.Count)
                return -1;
            double min = relationship.Min();
            res = relationship.FindAll(
                delegate (double x)
                {
                    return x == min;
                }).Count;

            if (res != 1)
                return Find_Row_2(variables);
            return relationship.IndexOf(relationship.Min());
        }
        /// <summary>
        /// Если нашлось несколько наименьших значений, то используем правило Креко
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        int Find_Row_2(List<List<double>> items)
        {
            int res = 0;
            int j = 1;
            List<double> relationship = new List<double>();
            while (res != 1)
            {
                relationship.Clear();
                for (int i = 0; i < variables.Count - 1; i++)
                {
                    double variable = variables[i][j] / variables[i][ras_column];
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


            return relationship.IndexOf(relationship.Min()); ;
        }
        /// <summary>
        /// Округление чисел
        /// </summary>
        /// <param name="list">массив для округления</param>
        protected void Ceil(List<double> list)
        {
            for (int i = 0; i < list.Count; i++)
                list[i] = Math.Round(list[i], 2);
        }

        private void To_Table()
        {
            Output outp = new Output();
            outp.Draw_Row(Cap_Top.ToArray());
            Cap_Left[Row] = Cap_Top[Column + 1];
            for (int i = 0; i < Variables.Count; i++)
            {
                Ceil(Variables[i]);
                double[] arr = Variables[i].ToArray();                
                outp.Draw_Row(Cap_Left[i], arr);
            }
            outp.Draw_line();
            Console.WriteLine("\n");
        }
    }
}