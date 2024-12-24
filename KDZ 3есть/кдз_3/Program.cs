using System;                // Директива для использования стандартных классов и методов .NET Framework, таких как Console, String и т.д.
using System.Collections.Generic;  // Директива для работы с коллекциями, такими как List<T>, Dictionary<TKey, TValue>, и другими коллекциями.
using System.Linq;           // Директива для использования LINQ (Language Integrated Query) — языка запросов для работы с коллекциями данных.
using System.Text;           // Директива для работы с текстовыми кодировками, например, для изменения кодировки вывода в консоль.


public abstract class GameObject
{
    public char Symbol { get; protected set; } // Символ объекта (например, 'C' для игрока)
    public int X { get; set; } // Координата по оси X
    public int Y { get; set; } // Координата по оси Y
    public ConsoleColor Color { get; protected set; } // Цвет объекта

    // Активировать объект (по умолчанию ничего не делает)
    public virtual void Activate() { }

    // Деактивировать объект (по умолчанию ничего не делает)
    public virtual void Deactivate() { }
}

public class Player : GameObject
{
    public Player(int x, int y)
    {
        X = x;
        Y = y;
        Symbol = 'C'; // Символ игрока
        Color = ConsoleColor.White; // Начальный цвет игрока
    }

    // Метод активации игрока (изменяет символ и цвет)
    public override void Activate()
    {
        Symbol = 'Ⓒ'; // Символ активированного игрока
        Color = ConsoleColor.Blue; // Синий цвет при активации
    }

    // Метод деактивации игрока (восстанавливает исходный символ и цвет)
    public override void Deactivate()
    {
        Symbol = 'C'; // Исходный символ
        Color = ConsoleColor.White; // Исходный цвет
    }

    public override string ToString() => $"Player at ({X}, {Y})"; // Строковое представление игрока
}

public class Stone : GameObject
{
    public Stone(int x, int y)
    {
        X = x;
        Y = y;
        Symbol = 'R'; // Символ камня
        Color = ConsoleColor.Magenta; // Цвет камня
    }

    // Метод активации камня (изменяет символ и цвет)
    public override void Activate()
    {
        Symbol = 'Ⓡ'; // Символ активированного камня
        Color = ConsoleColor.Blue; // Синий цвет при активации
    }

    // Метод деактивации камня (восстанавливает исходный символ и цвет)
    public override void Deactivate()
    {
        Symbol = 'R'; // Исходный символ
        Color = ConsoleColor.Magenta; // Исходный цвет
    }

    public override string ToString() => $"Stone at ({X}, {Y})"; // Строковое представление камня
}

public class Tree : GameObject
{
    public Tree(int x, int y)
    {
        X = x;
        Y = y;
        Symbol = 'T'; // Символ дерева
        Color = ConsoleColor.Gray; // Цвет дерева
    }

    public override string ToString() => $"Tree at ({X}, {Y})"; // Строковое представление дерева
}

public class Tile : GameObject
{
    public bool IsActive { get; private set; } // Флаг активации плитки

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
        Symbol = 'O'; // Символ плитки
        Color = ConsoleColor.Yellow; // Цвет плитки
    }

    // Метод активации плитки (делает плитку активной)
    public override void Activate()
    {
        if (!IsActive) // Проверка, если плитка уже активна
        {
            IsActive = true; // Активируем плитку
            Color = ConsoleColor.Blue; // Изменяем цвет на синий
        }
    }

    // Метод деактивации плитки (делает плитку неактивной)
    public override void Deactivate()
    {
        if (IsActive) // Проверка, если плитка активна
        {
            IsActive = false; // Деактивируем плитку
            Symbol = 'O'; // Восстанавливаем исходный символ
            Color = ConsoleColor.Yellow; // Восстанавливаем исходный цвет
        }
    }

    public override string ToString() => $"Tile at ({X}, {Y})"; // Строковое представление плитки
}

public class Level
{
    private readonly List<GameObject> _objects; // Список объектов на уровне
    public Player Player { get; private set; } // Игрок на уровне
    public bool IsCompleted { get; private set; } // Флаг завершения уровня

    public Level(List<GameObject> objects, Player player)
    {
        _objects = objects; // Сохраняем объекты уровня
        Player = player; // Сохраняем игрока
    }

    // Получить список объектов на уровне
    public List<GameObject> GetObjects() => _objects.ToList();

    // Проверка, завершен ли уровень (все плитки активированы)
    public void CheckCompletion()
    {
        var tiles = _objects.OfType<Tile>().ToList(); // Все плитки на уровне
        var stones = _objects.OfType<Stone>().ToList(); // Все камни на уровне

        foreach (var tile in tiles)
        {
            // Проверка, если плитка не активирована и нет камня на её месте
            if (!tile.IsActive && !stones.Any(stone => stone.X == tile.X && stone.Y == tile.Y))
                return; // Уровень не завершён

        }

        IsCompleted = true; // Уровень завершён
    }

    // Обновление состояния плиток (активируем или деактивируем плитки в зависимости от положения объектов)
    public void UpdateTiles()
    {
        foreach (var tile in _objects.OfType<Tile>())
        {
            var onTile = _objects.FirstOrDefault(obj => obj.X == tile.X && obj.Y == tile.Y); // На плитке ли объект?
            if (onTile != null && (onTile is Player || onTile is Stone)) // Если на плитке игрок или камень
            {
                tile.Activate(); // Активируем плитку
            }
            else
            {
                tile.Deactivate(); // Деактивируем плитку
            }
        }
    }
}

public class GameEngine
{
    private const int Width = 10; // Ширина уровня
    private const int Height = 10; // Высота уровня
    private readonly List<Level> _levels; // Список уровней
    private int _currentLevelIndex; // Индекс текущего уровня

    public GameEngine(List<Level> levels)
    {
        _levels = levels; // Сохраняем уровни
        _currentLevelIndex = 0; // Начинаем с первого уровня
    }

    // Запуск игры (проходим все уровни)
    public void Start()
    {
        while (_currentLevelIndex < _levels.Count)
        {
            PlayCurrentLevel(); // Играть на текущем уровне
            _currentLevelIndex++; // Переходим к следующему уровню
        }

        Console.WriteLine("Поздравляем! Вы прошли все уровни!"); // Сообщение по завершению игры
    }

    // Игрок проходит текущий уровень
    private void PlayCurrentLevel()
    {
        var level = _levels[_currentLevelIndex]; // Получаем текущий уровень
        DrawLevel(level.GetObjects()); // Отображаем уровень

        while (!level.IsCompleted) // Пока уровень не завершён
        {
            Console.WriteLine($"Уровень {_currentLevelIndex + 1}"); // Выводим номер уровня

            MovePlayer(level); // Перемещаем игрока
            level.CheckCompletion(); // Проверяем, завершён ли уровень
            level.UpdateTiles(); // Обновляем состояние плиток
            DrawLevel(level.GetObjects()); // Отображаем уровень
        }
    }

    // Отображение уровня (рисуем объекты на экране)
    private static void DrawLevel(IEnumerable<GameObject> objects)
    {
        Console.Clear(); // Очищаем консоль

        var objectList = objects.ToList(); // Преобразуем объекты в список для упрощения работы

        for (int y = 0; y < Height; y++) // Проходим по всем строкам
        {
            for (int x = 0; x < Width; x++) // Проходим по всем столбцам
            {
                var obj = objectList.FirstOrDefault(o => o.X == x && o.Y == y); // Ищем объект на данной позиции
                if (obj != null)
                {
                    Console.ForegroundColor = obj.Color; // Устанавливаем цвет объекта
                    Console.Write(obj.Symbol); // Выводим символ объекта
                    Console.ResetColor(); // Сбрасываем цвет
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen; // Цвет фона (зелёный)
                    Console.Write('#'); // Рисуем стенку
                    Console.ResetColor(); // Сбрасываем цвет
                }
            }
            Console.WriteLine(); // Переходим на новую строку
        }
    }

    // Перемещение игрока по уровню
    private void MovePlayer(Level level)
    {
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key; // Чтение клавиши
        } while (key != ConsoleKey.UpArrow &&
                 key != ConsoleKey.DownArrow &&
                 key != ConsoleKey.LeftArrow &&
                 key != ConsoleKey.RightArrow); // Ограничиваем только стрелки

        var player = level.Player; // Получаем игрока
        var newX = player.X;
        var newY = player.Y;

        // Обновляем координаты игрока в зависимости от нажатой клавиши
        switch (key)
        {
            case ConsoleKey.UpArrow: newY--; break;
            case ConsoleKey.DownArrow: newY++; break;
            case ConsoleKey.LeftArrow: newX--; break;
            case ConsoleKey.RightArrow: newX++; break;
        }

        // Если новые координаты в пределах поля
        if (newX >= 0 && newX < Width && newY >= 0 && newY < Height)
        {
            var targetObj = level.GetObjects().FirstOrDefault(obj => obj.X == newX && obj.Y == newY); // Ищем объект на новой позиции

            if (targetObj is Stone stone) // Если это камень
            {
                MoveStone(stone, key, level); // Перемещаем камень
            }
            else if (targetObj is Tile tile) // Если это плитка
            {
                player.X = newX;
                player.Y = newY;
                player.Activate(); // Активируем плитку
            }
            else if (targetObj == null) // Если на позиции ничего нет
            {
                player.X = newX;
                player.Y = newY;
                player.Deactivate(); // Деактивируем плитку
            }
        }
    }

    // Перемещение камня
    private void MoveStone(Stone stone, ConsoleKey key, Level level)
    {
        var nextX = stone.X;
        var nextY = stone.Y;

        // Обновляем координаты камня в зависимости от нажатой клавиши
        switch (key)
        {
            case ConsoleKey.UpArrow: nextY--; break;
            case ConsoleKey.DownArrow: nextY++; break;
            case ConsoleKey.LeftArrow: nextX--; break;
            case ConsoleKey.RightArrow: nextX++; break;
        }

        var nextTarget = level.GetObjects().FirstOrDefault(obj => obj.X == nextX && obj.Y == nextY); // Ищем объект на новой позиции
        if (nextTarget is Tile) // Если это плитка, камень может стоять на ней
        {
            stone.X = nextX;
            stone.Y = nextY;
            stone.Activate(); // Активируем плитку
        }
        else
        {
            stone.X = nextX;
            stone.Y = nextY;
            stone.Deactivate(); // Деактивируем плитку
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8; // Устанавливаем кодировку
        var levels = CreateLevels(); // Создаем уровни игры

        new GameEngine(levels).Start(); // Запускаем игру
    }

    // Метод для создания уровней
    private static List<Level> CreateLevels()
    {
        var player1 = new Player(0, 0); // Игрок на первом уровне
        var stone11 = new Stone(8, 6); // Камни на первом уровне
        var stone12 = new Stone(3, 5);
        var tree11 = new Tree(5, 6); // Деревья на первом уровне
        var tree12 = new Tree(7, 4);
        var tile11 = new Tile(2, 3); // Плитки на первом уровне
        var tile12 = new Tile(7, 7);
        var tile13 = new Tile(8, 9);
        var objects1 = new List<GameObject>
        {
            player1,
            stone11,
            stone12,
            tree11,
            tree12,
            tile11,
            tile12,
            tile13
        };
        var level1 = new Level(objects1, player1);

        // Второй уровень
        var player2 = new Player(4, 4);
        var stone21 = new Stone(6, 7);
        var stone22 = new Stone(8, 7);
        var stone23 = new Stone(3, 6);
        var tree21 = new Tree(1, 5);
        var tree22 = new Tree(3, 7);
        var tree23 = new Tree(6, 2);
        var tile21 = new Tile(2, 2);
        var tile22 = new Tile(0, 5);
        var tile23 = new Tile(8, 5);
        var tile24 = new Tile(1, 8);
        var objects2 = new List<GameObject>
        {
            player2,
            stone21,
            stone22,
            stone23,
            tree21,
            tree22,
            tree23,
            tile21,
            tile22,
            tile23,
            tile24
        };
        var level2 = new Level(objects2, player2);

        return new List<Level> { level1, level2 }; // Возвращаем все уровни
    }
}
