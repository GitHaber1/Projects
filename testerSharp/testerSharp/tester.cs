using System;
using System.Collections.Generic;
using System.Linq;

public class Desk
{
    private int size; // размер игрового поля
    private List<List<char>> square; // игровое поле
    private char emptySign = '_'; // значение пустого символа, предназначенного для проверки
    public Desk() // конструктор по умлочанию
    {

    }
    public Desk(int numberOfPlayers) // конструктор с параметрами 
    {
        size = numberOfPlayers + 1;
        square = new List<List<char>>();
        square = square.Resize(size);
        for (int i = 0; i < size; i++)
        {
            square[i] = square[i].Resize(size);
            for (int j = 0; j < size; j++)
                square[i][j] = emptySign;
        }
    }
    public int Size // свойства поля size
    {
        get
        {
            return size;
        }
        set
        {
            if (size > 0)
                size = value;
        }
    }
    public char Emptysign // свойства поля emptySign
    {
        get
        {
            return emptySign;
        }
    }
    ~Desk() // деструктор
    {
        square.Clear();
    }
    public char defineSign(Tuple<int, int> xy) // функция, определяющая символ, расположенный в ячейке игрового поля по координатам
    {
        int x = xy.Item1;
        int y = xy.Item2;
        if (x > square.Count || y > square[x].Count || x < 0 || y < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        else
            return square[x][y];

    }
    public void SetSign(Tuple<int, int> xy, char sym) // функция, устанавливающая символ в ячейку игрового поля
    {
        int x = xy.Item1;
        int y = xy.Item2;
        square[x][y] = sym;
    }
    public bool isNoEmptySign() // функция, определяющая, есть ли свободные ячейки на игровом поле
    {
        int flag = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
                if (square[i][j] != emptySign)
                    flag++;
        }
        if (flag == size * size)
            return true;
        else
            return false;
    }

    public void printDesk() // функция, печатающая игровое поле
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(square[i][j]);
            }
            Console.WriteLine();
        }
    }
}
public class Game
{
    private Desk gameDesk; // игровое поле
    private int robotsNumber; // количество ИИ
    private int playerNumber; // количество игроков-людей
    public List<Player> players; // список всех игроков
    private int moveNumber; // кол-во ходов
    private bool winner = false; // переменная, определяющая, есть ли победитель
    private List<char> SIGNS = new List<char>() { 'x', 'o', '#', '$', '%' }; // список всех символов
    public Desk GameDesk // свойства для gameDesk
    {
        get
        {
            return gameDesk;
        }
    }
    public bool Winner // свойства поля winner 
    {
        get
        {
            return winner;
        }
        set
        {
            if (value == true || value == false)
                winner = value;
        }
    }
    ~Game() // деструктор
    {
        gameDesk = null;
        for (int i = 0; i < playerNumber; i++)
        {
            deletePlayerByName(players[i].playername);
        }
    }
    void delSymbol(char ch) // функция, удаляющая из списка всех символов выбранный символ
    {
        for (int i = 0; i < 5; i++)
        {
            if (SIGNS[i] == ch)
            {
                SIGNS.RemoveAt(i);
                break;
            }
        }
    }

    public bool isValidSymbol(char symbol) // функция, определяющая, является ли введенный символ корректным 
    {
        for (int i = 0; i < SIGNS.Count(); i++)
        {
            if (symbol == SIGNS[i])
            {
                delSymbol(SIGNS[i]);
                return true;
            }

        }
        throw new ArgumentException("Неправильный символ!");
    }

    public void createDesk(int playerNum, int peoples) // создание игровой среды
    {
        playerNumber = playerNum;
        gameDesk = new Desk(playerNumber);
        robotsNumber = playerNumber - peoples;
    }
    public void printNames() // вывод списка игроков 
    {
        Console.WriteLine(" ");
        Console.WriteLine("Список игроков: ");
        for (int i = 0; i < playerNumber; i++)
        {
            Console.WriteLine(players[i].playername);
        }
        Console.WriteLine(" ");
    }
    public void deletePlayerByName(string name) // удаление игрока по имени
    {
        int checker = 0;
        for (int i = 0; i < playerNumber; i++)
        {
            if (players[i].playername == name)
            {
                players[i] = null;
                playerNumber--;
                checker = 1;
                for (; i < playerNumber; i++)
                    players[i] = players[i + 1];
            }
        }
        if (checker == 1) Console.WriteLine("Удаление прошло успешно!");
        else Console.WriteLine("Игрока с таким именем нет!");
    }
    public void addThinkingPlayer() // добавление игроков-людей в список всех игроков 
    {
        Player play = new Player();
        int PeopleNumber = playerNumber - robotsNumber;
        for (int i = 0; i < PeopleNumber; i++)
        {
            char symbol;
            string name = "";
            Console.WriteLine("Введите ваше имя: ");
            do
            {
                name = Console.ReadLine();                
            } while (!play.isValidName(name));
            while (true)
            {
                Console.WriteLine("Выберите один из символов: ");
                for (int j = 0; j < SIGNS.Count(); j++)
                {
                    Console.WriteLine(SIGNS[j] + " ");
                }
                symbol = Convert.ToChar(Console.ReadLine());
                if (!isValidSymbol(symbol)) Console.WriteLine("Введен неправильный символ!");
                if (players == null) players = new List<Player>();
                players.Insert(i, new ThinkingPlayer());
                players[i].playername = name;
                players[i].playersign = symbol;
                break;
            }
        }

    }
    public void AddRandomPlayer(int playerNum) // добавление информации об ИИ 
    {
        for (int i = playerNum - robotsNumber; i < playerNum; i++)
        {
            players.Add(new RandomPLayer());
            players[i].playername = "Player" + Convert.ToString(i);
            players[i].playersign = SIGNS[0];
            delSymbol(SIGNS[0]);
        }
    }
    public bool defineWinner(char currentsymbol) // функция, определяющая победителя 
    {
        int size = gameDesk.Size;
        Tuple<int, int> tup = new Tuple<int, int>(1, 1);
        List<int> scores = new List<int> { 0, 0, 0, 0 };
        for (int i = 0; i < size; i++)
        {
            if (gameDesk.defineSign(tup = new Tuple<int, int>(i, i)) == currentsymbol)
                scores[2]++;
            if (gameDesk.defineSign(tup = new Tuple<int, int>(i, size - 1 - i)) == currentsymbol)
                scores[3]++;
            for (int j = 0; j < size; j++)
            {
                if (gameDesk.defineSign(tup = new Tuple<int, int>(i, j)) == currentsymbol)
                    scores[0]++;
                if (gameDesk.defineSign(tup = new Tuple<int, int>(j, i)) == currentsymbol)
                    scores[1]++;
            }
            for (int a = 0; a < 2; a++)
            {
                if (scores[a] < size) scores[a] = 0;
            }
            continue;
        }
        for (int i = 0; i < 4; i++)
        {
            if (scores[i] == size)
            {
                winner = true;
                break;
            }
        }
        return winner;
    }
    public void MakeMove() // функция, определяющая действия игрока во время игры
    {
        bool win = winner;
        moveNumber = 0;
        bool employDetector = false;
        for (int i = 0; i < playerNumber; i++)
        {
            Console.WriteLine("Ход делает " + players[i].playername);
            gameDesk.printDesk();
            employDetector = gameDesk.isNoEmptySign();
            if (employDetector) break;
            Tuple<int, int> xy = players[i].makeMove(gameDesk.Size, ref gameDesk);
            gameDesk.SetSign(xy, players[i].playersign);
            win = defineWinner(players[i].playersign);
            if (win) break;
            moveNumber++;
        }
        if (employDetector)
        {
            gameDesk.printDesk();
            Console.WriteLine("No winner!");
            win = true;
            winner = true;
        }
        if (win)
        {
            if (!employDetector)
            {
                gameDesk.printDesk();
                string name;
                name = players[moveNumber].playername;
                Console.WriteLine("Winner is " + name);
            }
        }
    }
    public void newGame() // создание поля для новой игры
    {
        winner = false;
        gameDesk = null;
        gameDesk = new Desk(playerNumber);
    }

}
public class Player
{
    protected string player; // имя игрока
    protected char sign; // знак, который игрок рисует при своем ходе
    public string playername // свойства поля player
    {
        get
        {
            return player;
        }
        set
        {
            if (isValidName(value))
                player = value;
        }
    }
    public char playersign // свойства поля sign
    {
        get
        {
            return sign;
        }
        set
        {
            if (value == 'x' || value == 'o' || value == '#' || value == '$' || value == '%')
                sign = value;
        }
    }
    public Player() // конструктор по умолчанию
    {

    }
    public bool isValidName(string name) // проверка имени игрока
    {
        char ch;
        bool flag;
        int i = 0;
        while (name != "\0" && i < name.Length)
        {
            ch = name[i];
            flag = char.IsLetter(ch) || char.IsWhiteSpace(ch) || char.IsDigit(ch);
            if (!flag) throw new ArgumentException("Неправильное имя!");
            i++;
        }
        return true;
    }
    virtual public Tuple<int, int> makeMove(int a, ref Desk b) // виртуальная функция, отвечающая за ход игрока
    {
        var ret = new Tuple<int, int>(0, 0);
        return ret;
    }
    public Player(ref Player orig) // конструктор с параметрами 
    {
        player = orig.playername;
        sign = orig.playersign;
    }


}
public class RandomPLayer : Player
{
    override public Tuple<int, int> makeMove(int border, ref Desk desk) // алгоритм игры для ИИ 
    {
        var rand = new Random();
        int x = 0;
        int y = 0;
        var xy = new Tuple<int, int>(x, y);
        while (true)
        {
            x = rand.Next() % border;
            y = rand.Next() % border;
            var newEntry = new Tuple<int, int>(x, y);
            xy = newEntry;
            if (desk.defineSign(xy) == desk.Emptysign)
            {
                break;
            }
        }
        return xy;
    }

}
public class ThinkingPlayer : Player
{
    override public Tuple<int, int> makeMove(int border, ref Desk desk) // алгоритм игры для игрока-человека
    {
        Console.WriteLine("Выберите вид хода: 1 - Доверить свой ход алгоритму,  2 - Ходить самому");
        int x = 0;
        int y = 0;
        var en = new Tuple<int, int>(x, y);
        var coord = new Tuple<int, int>(x, y);
        while (true)
        {

            string turn = Console.ReadLine();
            if (turn == "1")
            {
                for (int i = 0; i < desk.Size; i++)
                    for (int j = 0; j < desk.Size; j++)
                    {
                        x = i;
                        y = j;
                        coord = new Tuple<int, int>(x, y);
                        if (desk.defineSign(coord) == desk.Emptysign)
                        {
                            return coord;
                        }
                    }
            }
            if (turn == "2")
            {
                while (true)
                {
                    Console.WriteLine("\nВведте координат (начиная с 1): ");
                    x = Convert.ToInt32(Console.ReadLine());
                    y = Convert.ToInt32(Console.ReadLine());
                    x--;
                    y--;
                    en = new Tuple<int, int>(x, y);
                    if (((x >= border) || (x < 0) || (y >= border) || (y < 0)))
                    {
                        Console.WriteLine("\nНекорректные данные!");
                    }
                    else if (desk.defineSign(en) != desk.Emptysign)
                    {
                        Console.WriteLine("Ячейка занята!");
                    }
                    else break;
                }
                return en;
            }
        }
    }
}
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

namespace Laba
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
