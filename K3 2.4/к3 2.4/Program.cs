using System;

class Program
{
    static void Main()
    {
        // Ввод размера массива
        Console.Write("размер массива: ");
        int size = Convert.ToInt32(Console.ReadLine());

        // Создание массива и заполнение его случайными числами в диапазоне от -200 до 200
        Random random = new Random();
        int[] array = new int[size];

        // Заполнение массива случайными числами
        for (int i = 0; i < size; i++)
        {
            array[i] = random.Next(-200, 200);  // генерируем случайные числа в диапазоне от -200 до 200
        }

        // Вывод изначального массива
        Console.WriteLine("\nИзначальный массив:");
        PrintArray(array);

        // Пузырьковая сортировка массива
        BubbleSort(array);

        // Вывод отсортированного массива
        Console.WriteLine("\nМассив после пузырьковой сортировки:");
        PrintArray(array);
    }

    // Метод для вывода элементов массива
    static void PrintArray(int[] arr)
    {
        foreach (var item in arr)
        {
            Console.Write(item + " ");  // вывод каждого элемента массива
        }
        Console.WriteLine();  // переход на новую строку после вывода массива
    }

    // Метод для выполнения пузырьковой сортировки
    static void BubbleSort(int[] arr)
    {
        int temp;

        // Внешний цикл — определяет количество проходов по массиву
        for (int i = 0; i < arr.Length - 1; i++)
        {
            // Внутренний цикл — выполняет сравнение соседних элементов
            for (int j = 0; j < arr.Length - 1 - i; j++)
            {
                // Если текущий элемент больше следующего, меняем их местами
                if (arr[j] > arr[j + 1])
                {
                    // Обмен элементов местами с использованием временной переменной
                    temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }
}
