using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtras
{
    public static List<T> Resize<T>(this List<T> list, int size, T element = default(T)) // добавление метода Resize для контейнера List, является подобием одноименной функции в c++
    {
        list = new List<T>();
        int count = 0;
        if (list == null)
            count = 0;
        if (list != null)
            count = list.Count;
        if (size < count)
        {
            list.RemoveRange(size, count - size);
            return list;
        }
        else if (size > count)
        {
            if (list != null)
            {
                if (size > list.Capacity)
                    list.Capacity = size;
            }
            list.AddRange(Enumerable.Repeat(element, size - count));
            return list;
        }
        return list;
    }
}

namespace testerSharp
{
    class Program
    {
        public void startGame() // функция, в которой начинается игра
        {
            Game theGame = new Game();
            char number;
            int playerNumber;
            Console.WriteLine("Введите число игроков (от 2 до 5): ");
            number = Convert.ToChar(Console.ReadLine());
            if (Char.IsDigit(number))
            {
                playerNumber = (int)number - 48;
                if (2 > playerNumber || playerNumber > 5)
                {
                    Console.WriteLine("Введено некорректное значение, по умолчанию выставлено значение 2!");
                    playerNumber = 2;
                }
            }
            else
            {
                Console.WriteLine("Введено некорректное значение, по умолчанию выставлено значение 2!");
                playerNumber = 2;
            }
            char people;
            int peoplePlayers;
            Console.WriteLine("Введите число игроков-людей (1-4): ");
            people = Convert.ToChar(Console.ReadLine());
            if (char.IsDigit(people))
            {
                peoplePlayers = (int)people - 48;
                if (1 > peoplePlayers || peoplePlayers > 4)
                {
                    Console.WriteLine("Введено некорректное число, по умолчанию выставлено значение 1!");
                    peoplePlayers = 1;
                }
            }
            else
            {
                Console.WriteLine("Введено некорректное число, по умолчанию выставлено значение 1!");
                peoplePlayers = 1;
            }
            theGame.createDesk(playerNumber, peoplePlayers);
            theGame.addThinkingPlayer();
            theGame.AddRandomPlayer(playerNumber);
            Console.WriteLine("Игра начинается!");
            while (!(theGame.Winner))
            {
                theGame.MakeMove();
            }
            while (true)
            {
                char choose;
                Console.WriteLine("Выберите пункт:\n1-Сыграть снова\n2-Начать новую игру\n3-Удалить игрока\n4-Выход");
                choose = Convert.ToChar(Console.ReadLine());
                if (char.IsDigit(choose))
                {
                    if (choose == '1')
                    {
                        theGame.newGame();
                        while (!theGame.Winner)
                            theGame.MakeMove();
                    }
                    if (choose == '2')
                    {
                        theGame = null;
                        theGame = new Game();
                        startGame();
                    }
                    if (choose == '3')
                    {
                        string name;
                        theGame.printNames();
                        Console.WriteLine("Введите имя игрока: ");
                        name = Convert.ToString(Console.ReadLine());
                        theGame.deletePlayerByName(name);
                    }
                    if (choose == '4')
                    {
                        System.Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("Введено неправильное значение!");
                }
            }
        }
        public static void Main(string[] args) // главная функция
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            Program p = new Program();
            try
            {
                p.startGame();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                System.Threading.Thread.Sleep(10000);
                System.Environment.Exit(0);
            }
        }
    }
}
