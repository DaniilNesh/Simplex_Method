using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex_Method
{
    class Double_SMethod : SMethod
    {
        List<string> znak;
        List<double> B;


        public List<double> SvB
        {
            get
            {
                return B;
            }
            set
            {
                B = value;
            }
        }

        public Double_SMethod(List<List<double>> perem, List<double> function, List<string> symbols) : base(perem, function, symbols)
        {
            znak = new List<string>();
            B = new List<double>();
            foreach (var i in symbols)
                znak.Add(i);

            for(int i = 0; i < perem.Count-1; i++)
            {
                if (znak[i].Equals(">="))
                {
                    for (int j = 0; j < perem[i].Count; j++)
                        perem[i][j] *= -1;
                }
            }

            foreach (var i in perem)
            {
                if (i == perem.Last())
                    break;
                B.Add(i.Last());
            }
        }
        /// <summary>
        /// Двойственный симплекс метод
        /// </summary>
        public void Double_Action()
        {
            for (; ; )
            {
                var tuple = Proverka();

                if (!tuple.Item1)
                {
                    Console.WriteLine("\n" + tuple.Item2 + "\n");
                    if (tuple.Item2.Equals("Все коэффициенты положительны. Решаем Симплекс методом"))
                        base.Action();
                    //To_Table();
                    return;
                }

                Find_Row();
                Find_Column();
                double buffer = Variables[Row][Column];
                for (int i = 0; i < Variables[Row].Count; i++)
                {
                    Variables[Row][i] /= buffer;
                }

                for (int j = 0; j < Variables.Count; j++)
                {
                    if (Variables[j] == Variables[Row])
                        continue;
                    buffer = Variables[j][Column];

                    for (int k = 0; k < Variables[j].Count; k++)
                    {
                        Variables[j][k] += (Variables[Row][k] * (-1 * buffer));
                    }
                }
                for(int i = 0; i < Variables.Count-1; i++)
                {
                    B[i] = Variables[i][Variables[i].Count - 1];
                }
                To_Table();
            }
            

        }

        private (bool, string) Proverka()
        {
            var min = B.Min();
            if(min >= 0)
                return (false, "Все коэффициенты положительны. Решаем Симплекс методом");

            for(int i = 0; i < B.Count; i++)
            {
                if(B[i] < 0)
                {
                    var min_per = Variables[i].Min();
                    if (min_per >= 0)
                        return (false, "Задача неразрешима в силу несовместимости системы ограничений");
                }
            }

            return (true, string.Empty);
        }

        private void Find_Row()
        {
            Row = B.FindIndex( x => x == B.Min());
            return;
        }

        private void Find_Column()
        {
            List<(int, double)> relationship = new List<(int, double)>();
            for(int i = 0; i < Variables[Row].Count-1; i++)
            {
                if (Variables[Row][i] < 0)
                {
                    var item = (i, Math.Abs(Func[i] / Variables[Row][i]));
                    relationship.Add(item);
                }
            }
            Column = relationship[0].Item1;
            for(int i = 1; i < relationship.Count; i++)
            {
                if (relationship[i - 1].Item2 > relationship[i].Item2)
                    Column = relationship[i].Item1;
            }
            return;
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
