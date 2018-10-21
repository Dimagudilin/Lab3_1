﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{

    class Program
    {




        public class SimpleListItem<T>
        {
            /// <summary>
            /// Данные
            /// </summary>
            public T data { get; set; }
            /// <summary>
            /// Следующий элемент
            /// </summary>
            public SimpleListItem<T> next { get; set; }
            ///конструктор
            public SimpleListItem(T param)
            {
                this.data = param;
            }
        }

        public class SimpleList<T> : IEnumerable<T>
        where T : IComparable
        {
            /// <summary>
            /// Первый элемент списка
            /// </summary>
            protected SimpleListItem<T> first = null;
            /// <summary>
            /// Последний элемент списка
            /// </summary>
            protected SimpleListItem<T> last = null;
            /// <summary>
            /// Количество элементов
            /// </summary>
            public int Count
            {
                get { return _count; }
                protected set { _count = value; }
            }
            int _count;
            /// <summary>
            /// Добавление элемента
            /// </summary>
            public void Add(T element)
            {
                SimpleListItem<T> newItem =
                new SimpleListItem<T>(element);
                this.Count++;
                //Добавление первого элемента
                if (last == null)
                {
                    this.first = newItem;
                    this.last = newItem;
                }
                //Добавление следующих элементов
                else
                {
                    //Присоединение элемента к цепочке
                    this.last.next = newItem;
                    //Присоединенный элемент считается последним
                    this.last = newItem;
                }
            }
            /// <summary>
            /// Чтение контейнера с заданным номером
            /// </summary>
            public SimpleListItem<T> GetItem(int number)
            {
                if ((number < 0) || (number >= this.Count))
                {
                    //Можно создать собственный класс исключения
                    throw new Exception("Выход за границу индекса");
                }
                SimpleListItem<T> current = this.first;
                int i = 0;
                //Пропускаем нужное количество элементов
                while (i < number)
                {
                    //Переход к следующему элементу
                    current = current.next;
                    //Увеличение счетчика
                    i++;
                }
                return current;
            }
            /// <summary>
            /// Чтение элемента с заданным номером
            /// </summary>
            public T Get(int number)
            {
                return GetItem(number).data;
            }


            public IEnumerator<T> GetEnumerator()
            {
                SimpleListItem<T> current = this.first;
                //Перебор элементов
                while (current != null)
                {
                    //Возврат текущего значения
                    yield return current.data;
                    //Переход к следующему элементу
                    current = current.next;
                }
            }
            System.Collections.IEnumerator
            System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            /// <summary>
            /// Cортировка
            /// </summary>
            public void Sort()
            {
                Sort(0, this.Count - 1);
            }
            /// <summary>
            /// Алгоритм быстрой сортировки
            /// </summary>
            private void Sort(int low, int high)
            {
                int i = low;
                int j = high;
                T x = Get((low + high) / 2);
                do
                {
                    while (Get(i).CompareTo(x) < 0) ++i;
                    while (Get(j).CompareTo(x) > 0) --j;
                    if (i <= j)
                    {
                        Swap(i, j);
                        i++; j--;
                    }
                } while (i <= j);

                if (low < j) Sort(low, j);
                if (i < high) Sort(i, high);
            }
            /// <summary>
            private void Swap(int i, int j)
            {
                SimpleListItem<T> ci = GetItem(i);
                SimpleListItem<T> cj = GetItem(j);
                T temp = ci.data;
                ci.data = cj.data;
                cj.data = temp;
            }
        }

        class SimpleStack<T> : SimpleList<T> where T : IComparable
        {
            /// <summary>
            /// Добавление в стек
            /// </summary>
            public void Push(T element)
            {
                //Добавление в конец списка уже реализовано
                Add(element);
            }
            /// <summary>
            /// Удаление и чтение из стека
            /// </summary>
            public T Pop()
            {
                //default(T) - значение для типа T по умолчанию
                T Result = default(T);
                //Если стек пуст, возвращается значение по умолчанию для типа
                if (this.Count == 0) return Result;
                //Если элемент единственный
                if (this.Count == 1)
                {
                    //то из него читаются данные

                    Result = this.first.data;
                    //обнуляются указатели начала и конца списка
                    this.first = null;
                    this.last = null;
                }
                //В списке более одного элемента
                else
                {
                    //Поиск предпоследнего элемента
                    SimpleListItem<T> newLast = this.GetItem(this.Count - 2);
                    //Чтение значения из последнего элемента
                    Result = newLast.next.data;
                    //предпоследний элемент считается последним
                    this.last = newLast;
                    //последний элемент удаляется из списка
                    newLast.next = null;
                }
                //Уменьшение количества элементов в списке
                this.Count--;
                //Возврат результата
                return Result;
            }
        }






        //Разряженная матрица
        public class Matrix<T>
        {
            Dictionary<string, T> _matrix = new Dictionary<string, T>();
            int maxX;
            int maxY;
            int maxZ;
            IMatrixCheckEmpty<T> сheckEmpty;
            /// Конструктор
            public Matrix(int px, int py, int pz,
            IMatrixCheckEmpty<T> сheckEmptyParam)
            {
                this.maxX = px;
                this.maxY = py;
                this.maxZ = pz;
                this.сheckEmpty = сheckEmptyParam;
            }
            public T this[int x, int y, int z]
            {
                set
                {
                    CheckBounds(x, y, z);
                    string key = DictKey(x, y, z);
                    this._matrix.Add(key, value);
                }
                get
                {
                    CheckBounds(x, y, z);
                    string key = DictKey(x, y, z);
                    if (this._matrix.ContainsKey(key))
                    {
                        return this._matrix[key];
                    }
                    else
                    {
                        return this.сheckEmpty.getEmptyElement();
                    }
                }
            }
            void CheckBounds(int x, int y, int z)
            {

                if (x < 0 || x >= this.maxX)
                {
                    throw new ArgumentOutOfRangeException("x",
                    "x=" + x + " выходит за границы");
                }
                if (y < 0 || y >= this.maxY)
                {
                    throw new ArgumentOutOfRangeException("y",
                    "y=" + y + " выходит за границы");
                }
                if (z < 0 || z >= this.maxZ)
                {
                    throw new ArgumentOutOfRangeException("y",
                    "z=" + z + " выходит за границы");
                }
            }
            string DictKey(int x, int y, int z)
            {
                return x.ToString() + "_" + y.ToString() + "_" + z.ToString();
            }


            public override string ToString()
            {
                StringBuilder b = new StringBuilder();
                for (int k = 0; k < this.maxZ; k++)
                {
                    b.Append("пространство " + k);
                    b.Append("\n");
                    for (int j = 0; j < this.maxY; j++)
                    {
                        b.Append("[");
                        for (int i = 0; i < this.maxX; i++)
                        {
                            //Добавление разделителя-табуляции
                            if (i > 0)
                            {
                                b.Append("\t");
                            }
                            //Если текущий элемент не пустой
                            if (!this.сheckEmpty.checkEmptyElement(this[i, j, k]))
                            {
                                //Добавить приведенный к строке текущий элемент
                                b.Append(this[i, j, k].ToString());
                            }
                            else
                            {
                                //Иначе добавить признак пустого значения
                                b.Append(" - ");
                            }
                        }
                        b.Append("]\n");
                    }

                    b.Append("\n");
                    b.Append("\n");
                }
                return b.ToString();
            }






        }


        public interface IMatrixCheckEmpty<T>
        {
            /// <summary>
            /// Возвращает пустой элемент
            /// </summary>
            T getEmptyElement();
            /// <summary>
            /// Проверка что элемент является пустым
            /// </summary>
            bool checkEmptyElement(T element);
        }


        class FigureMatrixCheckEmpty : IMatrixCheckEmpty<Figura>
        {
            /// <summary>
            /// В качестве пустого элемента возвращается null
            /// </summary>
            public Figura getEmptyElement()
            {
                return null;
            }
            /// <summary>
            /// Проверка что переданный параметр равен null
            /// </summary>
            public bool checkEmptyElement(Figura element)
            {
                bool Result = false;
                if (element == null)
                {
                    Result = true;
                }
                return Result;
            }
        }

        interface IPrint
        {
            void Print();
        }

        //Фигуры
        abstract class Figura : IComparable
        {
            public abstract double Area();
            public string Type
            {
                get
                {
                    return this._Type;
                }
                protected set
                {
                    this._Type = value;
                }
            }
            string _Type;
            public int CompareTo(object obj)
            {
                Figura p = (Figura)obj;
                if (this.Area() < p.Area()) return -1;
                else if (this.Area() == p.Area()) return 0;
                else return 1;
            }
        }





        class Rectangle : Figura, IPrint
        {
            double a, b;

            public Rectangle(double a, double b)
            {
                this.a = a;
                this.b = b;
                this.Type = "Прямоугольник";
            }
            public Rectangle(double a)
            {
                this.a = a;
                this.b = b;
            }
            public override string ToString()
            {
                return this.Type + " Площадью " + Area();
            }
            public void Print()
            {
                Console.WriteLine(ToString());
            }
            public override double Area() { return this.a * this.b; }
        }
        class Square : Rectangle, IPrint
        {
            double a; public Square(double a) : base(a)
            {
                this.a = a;
                this.Type = "Квадрат";
            }
            public override string ToString()
            {
                return this.Type + " Площадью " + Area();
            }
            public void Print()
            {
                Console.WriteLine(ToString());
            }
            public override double Area()
            {
                return this.a * this.a;
            }
        }
        class Circle : Figura, IPrint
        {
            double a; public Circle(double a)
            {
                this.a = a;
                this.Type = "Круг";
            }
            public override string ToString()
            {
                return this.Type + " Площадью " + Area();
            }
            public void Print()
            {
                Console.WriteLine(ToString());
            }
            public override double Area()
            {
                return Math.PI * this.a * this.a;
            }
        }
        static void Main(string[] args)
        {
            Circle circle = new Circle(5);
            Square square = new Square(5);
            Rectangle rectangle = new Rectangle(5, 4);
            circle.Print();
            square.Print();
            rectangle.Print();

            List<Figura> li1 = new List<Figura>
                {circle, square, rectangle};
            Console.WriteLine();
            Console.WriteLine("Сортировка коллекции класса List<Figure>");
            li1.Sort();
            foreach (var x in li1) Console.WriteLine(x);

            ArrayList li2 = new ArrayList();
            li2.Add(circle);
            li2.Add(square);
            li2.Add(rectangle);
            li2.Sort();
            Console.WriteLine();
            Console.WriteLine("Сортировка коллекции класса ArrayList");
            foreach (var x in li2) Console.WriteLine(x);

            Console.WriteLine();
            Console.WriteLine("\n Разреженная матрица");
            Matrix<Figura> matrix = new Matrix<Figura>(3, 3, 3,
                 new FigureMatrixCheckEmpty());
            matrix[0, 0, 0] = rectangle;
            matrix[1, 1, 1] = square;
            matrix[2, 2, 2] = circle;
            Console.WriteLine(matrix.ToString());

            Console.WriteLine();
            Console.WriteLine("\n Стэээк");
            SimpleStack<Figura> stack = new SimpleStack<Figura>();
            //добавление данных в стек
            stack.Push(rectangle);
            stack.Push(square);
            stack.Push(circle);
            //чтение данных из стека
            while (stack.Count > 0)
            {
                Figura f = stack.Pop();
                Console.WriteLine(f);
            }



            Console.ReadLine();
        }
    }
}

