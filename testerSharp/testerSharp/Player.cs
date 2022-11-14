using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testerSharp
{
    public class Player
    {
        protected string player; // имя игрока
        protected char sign; // знак, который игрок рисует при своем ходе
        protected bool isRobot; // индикатор, который показывает, является ли игрок ИИ
        public bool IsRobot // свойства для isRobot (проверка не требуется)
        {
            get
            {
                return isRobot;
            }
            set
            {
                isRobot = value;
            }
        }
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
}
