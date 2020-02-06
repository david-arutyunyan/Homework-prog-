using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        /// <summary>
        /// Метод считает ответ каждого примера, обрезает его и добавляет в массив ответов
        /// </summary>
        /// <param name="allLines">Массив примеров</param>
        /// <param name="answers">Массив ответов(с обрезкой)</param>
        static void Answers(string[] allLines, string[] answers)
        {
            for (int i = 0; i < allLines.Length; i++)
            {
                double x = Calculator.Calculate(allLines[i]);
                string s = $"{x:f3}";
                answers[i] = s;
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
                double a = Convert.ToDouble(answers[i]);
                double b = Convert.ToDouble(answers_checkers[i]);
                if (a == b)
                {
                    results[i] = "OK";
                }
                else
                {
                    k++;
                    results[i] = "Error";
                    Console.WriteLine(i + " " + answers[i] + " " + answers_checkers[i]);
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

                Answers(allLines, answers);

                //Записываем ответы в файл
                File.WriteAllLines(answer_txt, answers);

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
