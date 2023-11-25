using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Simplex_Method
{
    class Gomory
    {
        public static void Method_Action(StreamReader stream)
        {
            List<List<string>> strs = Input.Scan(stream);
            
            List<double> function;
            string keyWord;
            List<string> znak;
            var variable = Input.Pass(strs, out znak, out keyWord, out function);
            List<List<double>> buffer = new List<List<double>>();
            List<double> fraction = new List<double>();
            bool done = false;
            Double_SMethod double_S = new Double_SMethod(variable, function, znak);

            do
            {
                
                if (done)
                {
                    double_S.Cap_Left.Insert(double_S.Cap_Left.Count - 1, $"x{variable[0].Count}");
                    double_S.Cap_Top.Insert(double_S.Cap_Top.Count - 1, $"x{variable[0].Count}");
                    variable.Insert(variable.Count - 1, Clipping(buffer, znak, fraction));
                    for(int i = 0; i < variable.Count; i++)
                    {
                        if(i == variable.Count - 2)
                        {
                            variable[i].Insert(variable[i].Count - 1, 1);
                            continue;
                        }
                        variable[i].Insert(variable[i].Count - 1, 0);
                    }
                    double_S.Variables = variable;
                    double_S.SvB.Clear();
                    for(int i = 0; i < variable.Count; i++)
                    {
                        double_S.SvB.Add(variable[i][variable[i].Count - 1]);
                    }
                    Output outp = new Output();
                    outp.Draw_Row(double_S.Cap_Top.ToArray());
                    //Cap_Left[Row] = Cap_Top[Column + 1];
                    for (int i = 0; i < double_S.Variables.Count; i++)
                    {
                        for (int j = 0; j < double_S.Variables.Count; j++)
                            double_S.Variables[i][j] = Math.Round(double_S.Variables[i][j], 2);
                        double[] arr = double_S.Variables[i].ToArray();
                        outp.Draw_Row(double_S.Cap_Left[i], arr);
                    }
                    outp.Draw_line();
                    Console.WriteLine("\n");
                }
                //double_S.Cap_Left.Insert(double_S.Cap_Left.Count-1,$"x{variable[0].Count}");
                double_S.Double_Action();
                buffer = double_S.Variables;
                done = true;
            } while (!Proverka(buffer, out fraction));

        }

        private static bool Proverka(List<List<double>> items, out List<double> frac)
        {
            frac = new List<double>();
            List<double> buffer = new List<double>();
            for (int i = 0; i < items.Count-1; i++)
                buffer.Add(items[i].Last());

            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer[i] != Math.Round(buffer[i]))
                {
                    for (int j = 0; j < buffer.Count; j++)
                    {
                        frac.Add(buffer[j] - Math.Truncate(buffer[j]));
                    }
                    return false;
                }                    
            }            
            return true;
        }

        private static List<double> Clipping(List<List<double>> items, List<string> znak, List<double> fraction)
        {
            var index = fraction.FindIndex(x => x == fraction.Max());

            List<double> newRow = new List<double>();

            foreach (var i in items[index])
                newRow.Add(-1 * (i - Math.Truncate(i)));
            //for (int i = 0; i < znak.Count; i++)
            //    znak[i] = "=";
            //znak.Add(">=");
            return newRow;
        }
    }
}
