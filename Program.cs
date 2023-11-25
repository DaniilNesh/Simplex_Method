using System;
using System.Collections.Generic;
using System.IO;

namespace Simplex_Method
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            Output output = new Output();
            List<string> top = new List<string>();
            List<string> left = new List<string>();

            top.Add("xi");


            #endregion
            Console.SetBufferSize(Console.WindowWidth + 250, Console.WindowHeight + 250);
            List<double> function;
            string keyWord;
            List<string> znak;
            // TestFile - для Симплекс метода и М-Метода; TestFile2 - для двойственного симплекс метода
            List<List<string>> strs = Input.Scan(new StreamReader("TestFile2.txt"));
            var variable = Input.Pass(strs, out znak, out keyWord, out function);

            #region
            //Double Simplex
            //Double_SMethod dm = new Double_SMethod(variable, function, znak);
            //dm.Double_Action();
            #endregion

            #region
            //M Method
            //M_Method mm = new M_Method(variable, function, keyWord, znak);

            //mm.M_Action();
            #endregion

            #region
            //Simplex Method
            //SMethod method = new SMethod(variable, function, znak);

            //method.Action();
            #endregion

            Gomory.Method_Action(new StreamReader("TestFile3.txt"));

            Console.Read();
        }
    }
}
