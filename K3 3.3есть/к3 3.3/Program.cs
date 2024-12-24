using System;

class Program
{
    static void Main(string[] args)
    {
        // Ввод действительной и мнимой части для первого комплексного числа
        Console.Write("действительная часть для c1: ");
        double realC1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("мнимая часть для c1: ");
        double imagC1 = Convert.ToDouble(Console.ReadLine());

        var c1 = new ComplexNumber(realC1, imagC1);  // Создание первого комплексного числа

        // Ввод действительной и мнимой части для второго комплексного числа
        Console.Write("действительная часть для c2: ");
        double realC2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("мнимая часть для c2: ");
        double imagC2 = Convert.ToDouble(Console.ReadLine());

        var c2 = new ComplexNumber(realC2, imagC2);  // Создание второго комплексного числа

        // Вывод комплексных чисел
        Console.WriteLine($"c1 = {c1}");
        Console.WriteLine($"c2 = {c2}");

        // Операции с комплексными числами
        var sum = c1 + c2;  // Сложение
        var difference = c1 - c2;  // Вычитание
        var product = c1 * c2;  // Умножение
        var quotient = c1 / c2;  // Деление

        // Вывод результатов операций
        Console.WriteLine($"сумма = {sum}");
        Console.WriteLine($"разность = {difference}");
        Console.WriteLine($"произведение = {product}");
        Console.WriteLine($"частное = {quotient}");

        // Математические операции с комплексным числом
        Console.WriteLine($"модуль c1 = {c1.Magnitude()}");
        Console.WriteLine($"угол c1 = {c1.Angle()} радиан");
        Console.WriteLine($"квадратный корень из c1 = {c1.Sqrt()}");
        Console.WriteLine($"возведение c1 в куб = {c1.Pow(3)}");
    }
}

// Класс для представления комплексных чисел
public class ComplexNumber
{
    public double Real { get; set; }  // Действительная часть комплексного числа
    public double Imaginary { get; set; }  // Мнимая часть комплексного числа

    // Конструктор без параметров (по умолчанию 0 + 0i)
    public ComplexNumber()
    {
        this.Real = 0;
        this.Imaginary = 0;
    }

    // Конструктор с параметрами для задания действительной и мнимой частей
    public ComplexNumber(double real, double imaginary)
    {
        this.Real = real;
        this.Imaginary = imaginary;
    }

    // Оператор сложения двух комплексных чисел
    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
    }

    // Оператор вычитания двух комплексных чисел
    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.Real - b.Real, a.Imaginary - b.Imaginary);
    }

    // Оператор умножения двух комплексных чисел
    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
    {
        var real = a.Real * b.Real - a.Imaginary * b.Imaginary;  // Реальная часть
        var imag = a.Real * b.Imaginary + a.Imaginary * b.Real;  // Мнимая часть
        return new ComplexNumber(real, imag);
    }

    // Оператор деления двух комплексных чисел
    public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
    {
        // Проверка на деление на ноль
        if (b.Real == 0 && b.Imaginary == 0)
            throw new DivideByZeroException("Деление на ноль");

        // Вычисление знаменателя (модуль b^2)
        var denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        var real = (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator;  // Реальная часть
        var imag = (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator;  // Мнимая часть
        return new ComplexNumber(real, imag);
    }

    // Возведение комплексного числа в степень
    public ComplexNumber Pow(int exponent)
    {
        if (exponent == 0)
            return new ComplexNumber(1, 0);  // Число в степени 0 всегда 1

        if (exponent == 1)
            return this;  // Число в первой степени остается неизменным

        if (exponent < 0)
            return new ComplexNumber(1, 0) / this.Pow(-exponent);  // Для отрицательной степени инвертируем

        var result = this;
        for (int i = 1; i < exponent; i++)
            result *= this;  // Умножаем на себя несколько раз для степени больше 1
        return result;
    }

    // Извлечение квадратного корня из комплексного числа
    public ComplexNumber Sqrt()
    {
        var r = Math.Sqrt(this.Magnitude());  // Модуль комплексного числа
        var theta = this.Angle() / 2;  // Половина угла
        return new ComplexNumber(r * Math.Cos(theta), r * Math.Sin(theta));  // Корень
    }

    // Модуль комплексного числа
    public double Magnitude()
    {
        return Math.Sqrt(this.Real * this.Real + this.Imaginary * this.Imaginary);  // Расстояние от начала координат
    }

    // Угол (аргумент) комплексного числа
    public double Angle()
    {
        return Math.Atan2(this.Imaginary, this.Real);  // Угол относительно действительной оси
    }

    // Переопределение метода ToString для красивого вывода комплексного числа
    public override string ToString()
    {
        return $"{this.Real} + {this.Imaginary}i";  // Форматируем комплексное число как "a + bi"
    }
}
