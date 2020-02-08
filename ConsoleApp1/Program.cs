using System;
using System.IO;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {

        static void ConsoleErrorHandler(string message)
        {
            Console.WriteLine("Возникла исключительная ситуация: " + message + ". Время: " + DateTime.Now.ToLongTimeString());
        }

        static void ResultErrorHandler(string message)
        {
            File.AppendAllText(answer_txt, message + Environment.NewLine);
        }

        /// <summary>
        /// Метод принимает строку с примером, разделяет её на числа и операцию(знак)
        /// и возвращает результат(при этом выявляются ошибки по ходу рассчитывания ответа примера)
        /// </summary>
        /// <param name="expr">Строка с примерчиком</param>
        /// <param name="g">Событие</param>
        /// <param name="bul">Переменная проверки успешности решения примера</param>
        /// <returns>Ответ на пример</returns>
        public static double Calculate(string expr, Calculator g, ref bool bul)
        {
            string[] a = expr.Split(' ');
            string v = "0";
            try
            {
                v = Calculator.operations[a[1].ToCharArray()[0]](checked(Convert.ToDouble(a[0])), checked(Convert.ToDouble(a[2]))).ToString();

                //Если произошла попытка деления на 0
                if (a[1].ToCharArray()[0] == '/' && a[2] == "0")
                {
                    bul = true;
                    g.Invoke("bruh");
                }

                //Если v - не число
                if (Convert.ToDouble(v) is Double.NaN)
                {
                    bul = true;
                    g.Invoke(Double.NaN.ToString());
                }
            }
            catch (KeyNotFoundException)
            {
                bul = true;
                g.Invoke("неверный оператор");
            }
            catch (OverflowException)
            {
                bul = true;
                g.Invoke("∞");
            }
            catch (FormatException)
            {
                bul = true;
                g.Invoke("∞");
            }
            return Convert.ToDouble(v);
        }

        /// <summary>
        /// Метод считает ответ каждого примера, обрезает его и добавляет в массив ответов
        /// </summary>
        /// <param name="allLines">Массив примеров</param>
        /// <param name="answers">Массив ответов(с обрезкой)</param>
        static void Answers(string[] allLines, string answer_txt)
        {
            //Подписываем методы на события
            Calculator g = new Calculator();
            g.ErrorNotification += ConsoleErrorHandler;
            g.ErrorNotification += ResultErrorHandler;

            for (int i = 0; i < allLines.Length; i++)
            {
                bool bul = false;
                double x = Calculate(allLines[i], g, ref bul);

                //Если не словили никакую из ошибок(то есть примерчик посчитался нормально), то записываем результат в файл
                if (!bul)
                {
                    string s = $"{x:f3}";
                    File.AppendAllText(answer_txt, s + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Метод сравнивает ответы примерчиков и считает количество несовпадений
        /// </summary>
        /// <param name="answers_checkers">Массив ответов(с округлением)</param>
        /// <param name="answers">Массив ответов(с обрезкой)</param>
        /// <param name="results">Массив результатов сравнений</param>
        /// <returns>Возвращает количество несовпадающих ответов</returns>
        static int isEquals(string[] answers_checkers, string[] answers, string[] results)
        {
            //Счётчик количества несовпадений ответов
            int k = 0;

            //Сверяем ответы из двух файлов, считаем кол-во несовпадений и заполняем results
            for (int i = 0; i < answers_checkers.Length; i++)
            {
                string a = answers[i];
                string b = answers_checkers[i];
                if (a == b)
                {
                    results[i] = "OK";
                }
                else
                {
                    k++;
                    results[i] = "Error";
                    //Console.WriteLine(answers[i] + " " + answers_checkers[i]);
                }
            }
            return k;
        }

        static string path = "expressions.txt";
        static string path_checker = "expressions_checker.txt";
        static string answer_txt = "answers.txt";
        static string results_txt = "results.txt";

        static void Main(string[] args)
        {
            try
            {
                //Считываем все примерчики из файла и заполняем массив
                string[] allLines = File.ReadAllLines(path);

                //Создаём массив ответов на примерчики из allLines(отбрасываем лишние разряды)
                string[] answers = new string[allLines.Length];

                File.Delete(answer_txt);

                Answers(allLines, answer_txt);

                //Читаем ответы, которые только что записали
                answers = File.ReadAllLines(answer_txt);

                //Создаём массив ответов на примерчики(ответы с округлением)
                string[] answers_checkers = File.ReadAllLines(path_checker);

                //Создаём массив, в который будет записывать информацию о проверке ответов из двух файлов
                string[] results = new string[answers_checkers.Length + 1];

                int k = isEquals(answers_checkers, answers, results);

                //Добавляем в конец массива число несовпадений ответов
                results[answers_checkers.Length] = k.ToString();

                //Записываем результаты проверки в файл
                File.WriteAllLines(results_txt, results);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Исключение! Файл не найден.");
            }
            catch (IOException)
            {
                Console.WriteLine("Исклюление! Ошибка ввода-вывода.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Исключение! Недопустимый формат аргумента. " +
                    "Скорее всего, где-то используется разделитель между целой " +
                    "и дробной частью числа <точка> вместо <запятая>");
            }
        }
    }
}
