using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testerSharp
{
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
}
