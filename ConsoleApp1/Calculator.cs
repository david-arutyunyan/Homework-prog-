using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Calculator
    {
        public delegate double MathOperation(double a, double b);

        static Dictionary<char, MathOperation> operations;

        static Calculator()
        {
            operations = new Dictionary<char, MathOperation>()
            {
                ['+'] = (x, y) => x + y,
                ['-'] = (x, y) => x - y,
                ['*'] = (x, y) => x * y,
                ['/'] = (x, y) => x / y,
                ['^'] = Math.Pow,
            };
        }

        /// <summary>
        /// Метод принимает строку с примером, разделяет её на числа и операцию(знак)
        /// и возвращает результат
        /// </summary>
        /// <param name="expr">Строка с прмерчиком</param>
        /// <returns>Ответ примера</returns>
        public static double Calculate(string expr)
        {
            string[] a = expr.Split(' ');
            return operations[a[1].ToCharArray()[0]](Convert.ToDouble(a[0]), Convert.ToDouble(a[2]));
        }
    }
}
