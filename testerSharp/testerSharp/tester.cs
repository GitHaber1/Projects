using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        public void startGame(bool firstTime) // функция, в которой начинается игра
        {
            int choice = 0;
            string fileName = "Players.json";
            Game theGame = new Game();
            char number;
            int playerNumber;
            if (firstTime)
            {
                Console.WriteLine("Загрузить сохраненных игроков?\n1.Да\n2.Нет");
                choice = Convert.ToInt32(Console.ReadLine());
            }
            if (choice == 1 && firstTime)
            {
                string jsonInfo = File.ReadAllText(fileName);
                theGame.Players = JsonSerializer.Deserialize<List<Player>>(jsonInfo); // десериализация
                theGame.playerNumber = theGame.Players.Count;
                for (int i = 0; i < theGame.playerNumber; i++)
                {
                    if (theGame.Players[i].IsRobot == false)
                    {
                        theGame.Players[i] = new ThinkingPlayer(theGame.Players[i].playername, theGame.Players[i].playersign, theGame.Players[i].IsRobot);
                    }
                    else
                    {
                        theGame.Players[i] = new RandomPLayer(theGame.Players[i].playername, theGame.Players[i].playersign, theGame.Players[i].IsRobot);
                    }
                }
                if (theGame.playerNumber <= 1)
                {
                    Console.WriteLine("Слишком мало игроков для игры!");
                    firstTime = false;
                    startGame(firstTime);
                }
                else
                {
                    theGame.newGame();
                    while (!theGame.Winner)
                        theGame.MakeMove();
                }
                firstTime = false;                
            }
            else
            {
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
                firstTime = false;
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
                        startGame(firstTime);
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
                        choice = 0;
                        Console.WriteLine("Сохранить игроков текущей сессии?\n1.Да\n2.Нет");
                        choice = Convert.ToInt32(Console.ReadLine());
                        if (choice == 1)
                        {
                            string jsonInfo = JsonSerializer.Serialize(theGame.Players); // сериализация
                            File.WriteAllText(fileName, jsonInfo); // сохранение списка игроков
                            Console.WriteLine("Данные успешно сохранены!");
                            System.Threading.Thread.Sleep(6000);
                        }
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
            bool firstTime = true;
            try
            {
                p.startGame(firstTime);
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
