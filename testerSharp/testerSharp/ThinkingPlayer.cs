using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testerSharp
{
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
}
