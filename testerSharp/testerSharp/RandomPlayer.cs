using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testerSharp
{
    public class RandomPLayer : Player
    {
        public RandomPLayer() // конструктор по умолчанию
        {

        }
        public RandomPLayer(string name, char symbol, bool check) // конструктор с параметрами 
        {
            playername = name;
            playersign = symbol;
            IsRobot = check;
        }
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
}
