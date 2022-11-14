using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//tested by danyatk
namespace testerSharp
{
    public class Game
    {
        private Desk gameDesk; // игровое поле
        private int robotsNumber; // количество ИИ
        public int playerNumber; // количество игроков-людей
        private List<Player> players { get; set; } // список всех игроков
        private int moveNumber; // кол-во ходов
        private bool winner = false; // переменная, определяющая, есть ли победитель
        private List<char> SIGNS = new List<char>() { 'x', 'o', '#', '$', '%' }; // список всех символов
        public List<Player> Players // свойства для players (проверка не требуется)
        {
            get
            {
                return players;
            }
            set
            {
                if (value.Count > 0)
                    players = value;
            }
        }
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
                    playerNumber--;
                    checker = 1;
                    players.RemoveAt(i);
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
                    players[i].IsRobot = false;
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
                players[i].IsRobot = true;
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
}
