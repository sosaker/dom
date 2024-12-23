﻿using System;

class Program
{
    // Метод для подсчета шагов до 1 по правилам гипотезы Коллатца
    static int CountSteps(int n)
    {
        int count = 0; // Инициализация счетчика шагов
        // Цикл продолжается, пока число n не станет равным 1
        while (n != 1)
        {
            // Если число четное
            if (n % 2 == 0)
                n /= 2; // Делим на 2
            else
                n = 3 * n + 1; // Если нечетное, умножаем на 3 и прибавляем 1

            count++; // Увеличиваем счетчик шагов
        }
        return count; // Возвращаем количество шагов
    }

    // Главный метод программы
    static void Main()
    {
        Console.Write("число n: "); // Запрашиваем у пользователя ввод числа n
        int n;

        // Проверяем, что введенное значение является целым положительным числом
        if (int.TryParse(Console.ReadLine(), out n) && n > 0)
        {
            // Вызываем метод CountSteps для подсчета количества шагов
            int steps = CountSteps(n);
            // Выводим результат: количество шагов до 1
            Console.WriteLine($"требуется {steps} замен для числа {n}  для достижения 1");
        }
        else
        {
            // Если введено нецелое число или отрицательное, выводим сообщение об ошибке
            Console.WriteLine("Пожалуйста, введите положительное целое число.");
        }
    }
}
