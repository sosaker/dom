using System;
using System.Collections.Generic;
class MazeGame
{
    // Размеры лабиринта
    private const int Width = 20;  // Ширина лабиринта
    private const int Height = 10; // Высота лабиринта

    // Символы для отображения разных элементов
    private static readonly char WallChar = '#'; // Стена
    private static readonly char PathChar = '.'; // Путь
    private static readonly char PlayerChar = '@'; // Игрок
    private static readonly char ExitChar = 'E'; // Выход

    // Структура для хранения координат игрока или ячеек лабиринта
    struct Point // хранит информацию о стенках и т.д и т.п
    {
        public int X { get; set; } // Координата по оси X
        public int Y { get; set; } // Координата по оси Y

        // Конструктор для инициализации координат
        public Point(int x, int y)
        {
            this.X = x; // Устанавливаем координату X
            this.Y = y; // Устанавливаем координату Y
        }
    }

    // Направления, в которых можно двигаться: вверх, вниз, влево, вправо
    private static readonly List<Point> Directions = new List<Point>
    {
        new Point(-1, 0), // Вверх
        new Point(1, 0),  // Вниз
        new Point(0, -1), // Влево
        new Point(0, 1)   // Вправо
    };

    // Массив для хранения лабиринта
    private char[,] maze;

    // Позиция игрока
    private Point playerPosition;

    // Максимальное количество ходов
    private int maxMoves;

    // Текущий счетчик ходов
    private int currentMove;

    // Генератор случайных чисел (создаем один раз для всего класса)
    private static readonly Random random = new Random();

    // Конструктор, который инициализирует игру
    public MazeGame()
    {
        GenerateMaze(); // Генерация лабиринта
        PlacePlayerAtStart(); // Помещаем игрока на стартовую позицию
        PlaceExitAtEnd(); // Помещаем выход в конечную точку
        maxMoves = Width * Height / 2; // Максимальное количество ходов — половина всех клеток
        currentMove = 0; // Изначально ходов 0
    }

    // Генерация случайного лабиринта с использованием алгоритма поиска в глубину
    private void GenerateMaze()
    {
        maze = new char[Width, Height]; // Создаем массив для лабиринта
        for (int i = 0; i < Width; ++i) // Проходим по всем строкам
        {
            for (int j = 0; j < Height; ++j) // Проходим по всем столбцам
            {
                maze[i, j] = WallChar; // Заполняем лабиринт стенами
            }
        }

        // Используем стек для реализации алгоритма поиска в глубину
        var stack = new Stack<Point>();
        var startPoint = new Point(1, 1); // Начальная точка лабиринта (второй ряд и второй столбец)
        stack.Push(startPoint); // Добавляем начальную точку в стек

        // Алгоритм поиска в глубину
        while (stack.Count > 0) // Пока в стеке есть клетки
        {
            var currentCell = stack.Peek(); // Берем верхнюю клетку из стека
            maze[currentCell.X, currentCell.Y] = PathChar; // Отмечаем эту клетку как путь

            // Получаем все непосещенные соседние клетки
            var unvisitedNeighbors = GetUnvisitedNeighbors(currentCell);
            if (unvisitedNeighbors.Count == 0) // Если соседей нет, возвращаемся назад
            {
                stack.Pop(); // Убираем текущую клетку из стека
            }
            else
            {
                // Выбираем случайного соседа и вырезаем проход
                var nextCell = unvisitedNeighbors[random.Next(unvisitedNeighbors.Count)];
                CarvePassage(currentCell, nextCell); // Создаем проход между клетками
                stack.Push(nextCell); // Добавляем выбранную клетку в стек
            }
        }
    }

    // Получаем список непосещенных соседей для текущей клетки
    private List<Point> GetUnvisitedNeighbors(Point cell)
    {
        var neighbors = new List<Point>(); // Список соседей
        foreach (var direction in Directions) // Для каждого из четырех направлений
        {
            var neighbor = new Point(cell.X + direction.X * 2, cell.Y + direction.Y * 2); // Находим соседа на два шага дальше
            // Если сосед в пределах лабиринта и является стеной, то добавляем его
            if (IsInBounds(neighbor) && maze[neighbor.X, neighbor.Y] == WallChar)
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors; // Возвращаем список непосещенных соседей
    }

    // Проверяем, находится ли точка в пределах лабиринта
    private bool IsInBounds(Point point)
    {
        return point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height; // Проверка на границы лабиринта
    }

    // Вырезаем проход между двумя клетками
    private void CarvePassage(Point from, Point to)
    {
        var passage = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2); // Находим клетку между двумя клетками
        maze[passage.X, passage.Y] = PathChar; // Отмечаем эту клетку как путь
    }

    // Размещение игрока на стартовой позиции
    private void PlacePlayerAtStart()
    {
        playerPosition = new Point(1, 1); // Игрок начинает с позиции (1, 1)
    }

    // Размещение выхода на конечной позиции
    private void PlaceExitAtEnd()
    {
        var exitX = Width - 2;  // Сохраняем координату X выхода
        var exitY = Height - 2; // Сохраняем координату Y выхода
        maze[exitX, exitY] = ExitChar; // Выход размещаем в правом нижнем углу лабиринта
    }

    // Рисуем лабиринт на экране
    private void DrawMaze()
    {
        Console.Clear(); // Очищаем консоль перед рисованием нового состояния лабиринта
        for (int i = 0; i < Width; ++i) // Проходим по строкам лабиринта
        {
            for (int j = 0; j < Height; ++j) // Проходим по столбцам лабиринта
            {
                if (playerPosition.X == i && playerPosition.Y == j) // Если это клетка с игроком
                {
                    Console.Write(PlayerChar); // Выводим символ игрока
                }
                else
                {
                    Console.Write(maze[i, j]); // Выводим символ текущей клетки
                }
            }
            Console.WriteLine(); // Перевод строки после каждой строки лабиринта
        }
        Console.WriteLine($"Ходов осталось: {maxMoves - currentMove}"); // Выводим оставшееся количество ходов
    }

    // Обрабатываем нажатие клавиш для перемещения игрока
    private void ProcessInput()
    {
        ConsoleKeyInfo key = Console.ReadKey(true); // Считываем нажатую клавишу
        switch (key.Key) // В зависимости от нажатой клавиши
        {
            case ConsoleKey.LeftArrow:
                MovePlayer(new Point(0, -1)); // Двигаем влево
                break;
            case ConsoleKey.RightArrow:
                MovePlayer(new Point(0, 1)); // Двигаем вправо
                break;
            case ConsoleKey.UpArrow:
                MovePlayer(new Point(-1, 0)); // Двигаем вверх
                break;
            case ConsoleKey.DownArrow:
                MovePlayer(new Point(1, 0)); // Двигаем вниз
                break;
        }
    }

    // Перемещаем игрока в указанном направлении
    private void MovePlayer(Point direction)
    {
        var newPosition = new Point(playerPosition.X + direction.X, playerPosition.Y + direction.Y); // Рассчитываем новую позицию
        // Если новая позиция в пределах лабиринта и не является стеной
        if (IsInBounds(newPosition) && maze[newPosition.X, newPosition.Y] != WallChar)
        {
            playerPosition = newPosition; // Обновляем позицию игрока
            currentMove++; // Увеличиваем счетчик ходов
        }
    }

    // Проверяем, дошел ли игрок до выхода
    private bool HasFoundExit()
    {
        return maze[playerPosition.X, playerPosition.Y] == ExitChar; // Проверка на то, находится ли игрок на выходе
    }

    // Основной игровой цикл
    public void Run()
    {
        while (!HasFoundExit() && currentMove < maxMoves) // Пока игрок не найдет выход или не исчерпает максимальное количество ходов
        {
            DrawMaze(); // Рисуем лабиринт
            ProcessInput(); // Обрабатываем ввод с клавиатуры
        }

        DrawMaze(); // Рисуем финальное состояние лабиринта
        if (HasFoundExit()) // Если игрок достиг выхода
        {
            Console.WriteLine("Вы нашли выход!"); // Сообщаем, что победили
        }
        else // Если игрок исчерпал количество ходов
        {
            Console.WriteLine("Время вышло. Вы проиграли."); // Сообщаем, что время вышло
        }
    }

    // Точка входа в программу
    static void Main(string[] args)
    {
        var game = new MazeGame(); // Создаем объект игры
        game.Run(); // Запускаем игру
    }
}
