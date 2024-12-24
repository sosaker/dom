using System;

class Program
{
    static void Main()
    {
        double a, b, c;

        // Запрашиваем коэффициент a для уравнения
        Console.Write("Введите коэффициент a: ");
        a = double.Parse(Console.ReadLine()); // Читаем и конвертируем введенное значение в число типа double

        // Если a равно нулю, то у нас линейное уравнение
        if (a == 0)
        {
            // Запрашиваем коэффициент b
            Console.Write("Введите коэффициент b: ");
            b = double.Parse(Console.ReadLine()); // Читаем коэффициент b

            // Если b также равно нулю, то у нас константное уравнение
            if (b == 0)
            {
                // Запрашиваем коэффициент c
                Console.Write("Введите коэффициент c: ");
                c = double.Parse(Console.ReadLine()); // Читаем коэффициент c

                // Если все коэффициенты равны нулю, то x - любое число
                if (c == 0)
                {
                    Console.WriteLine("Решение: x - любое число");
                }
                else
                {
                    // Если c не равно нулю, то решений нет
                    Console.WriteLine("Нет решений");
                }
            }
            else
            {
                // Если a равно 0, но b не равно 0, решаем линейное уравнение
                Console.Write("Введите коэффициент c: ");
                c = double.Parse(Console.ReadLine()); // Читаем коэффициент c

                SolveLinearEquation(b, c); // Вызываем метод для решения линейного уравнения
            }
        }
        else
        {
            // Если a не равно 0, решаем квадратное уравнение
            Console.Write("Введите коэффициент b: ");
            b = double.Parse(Console.ReadLine()); // Читаем коэффициент b

            Console.Write("Введите коэффициент c: ");
            c = double.Parse(Console.ReadLine()); // Читаем коэффициент c

            SolveQuadraticEquation(a, b, c); // Вызываем метод для решения квадратного уравнения
        }
    }

    // Метод для решения линейного уравнения вида bx + c = 0
    static void SolveLinearEquation(double b, double c)
    {
        // Решение линейного уравнения x = -c / b
        double solution = -c / b;
        Console.WriteLine($"Решение: x = {solution}"); // Выводим решение
    }

    // Метод для решения квадратного уравнения ax^2 + bx + c = 0
    static void SolveQuadraticEquation(double a, double b, double c)
    {
        // Вычисляем дискриминант
        double discriminant = b * b - 4 * a * c;

        // Если дискриминант положительный, то два решения
        if (discriminant > 0)
        {
            double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a); // Первое решение
            double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a); // Второе решение
            Console.WriteLine($"Два решения: x1 = {root1}, x2 = {root2}"); // Выводим оба решения
        }
        // Если дискриминант равен нулю, то одно решение
        else if (discriminant == 0)
        {
            double root = -b / (2 * a); // Единственное решение
            Console.WriteLine($"Одно решение: x = {root}"); // Выводим решение
        }
        // Если дискриминант отрицательный, то решений нет
        else
        {
            Console.WriteLine("Нет решений"); // Сообщаем, что решений нет
        }
    }
}

