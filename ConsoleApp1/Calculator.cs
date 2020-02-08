using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public delegate void ErrorNotificationType(string message);

    class Calculator
    {
        public delegate double MathOperation(double a, double b);

        public event ErrorNotificationType ErrorNotification;

        public static Dictionary<char, MathOperation> operations;

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

        public void Invoke(string message)
        {
            ErrorNotification?.Invoke(message);
        }
    }
}
