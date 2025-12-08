using System;

struct Complex
{
    public double re;
    public double im;
}

class Program
{
    static Complex Add(Complex a, Complex b)
    {
        Complex res;
        res.re = a.re + b.re;
        res.im = a.im + b.im;
        return res;
    }
    static Complex Sub(Complex a, Complex b)
    {
        Complex res;
        res.re = a.re - b.re;
        res.im = a.im - b.im;
        return res;
    }
    static Complex Mul(Complex a, Complex b)
    {
        Complex res;
        res.re = a.re * b.re - a.im * b.im;
        res.im = a.re * b.im + a.im * b.re;
        return res;
    }
    static Complex Div(Complex a, Complex b)
    {
        Complex res;
        double z = b.re * b.re + b.im * b.im;
        if (Math.Abs(z) < 0.000001)
        {
            Console.WriteLine("Ошибка: деление на ноль!");
            return a;
        }
        res.re = (a.re * b.re + a.im * b.im) / z;
        res.im = (a.im * b.re - a.re * b.im) / z;
        return res;
    }
    static void Main()
    {
        Complex x = new Complex();
        x.re = 0;
        x.im = 0;
        while (true)
        {
            Console.WriteLine("\nТекущее число:");
            if (x.im >= 0)
                Console.WriteLine($"{x.re} + {x.im}i");
            else
                Console.WriteLine($"{x.re} - {-x.im}i");
            Console.WriteLine("Меню:");
            Console.WriteLine("n - ввести новое число");
            Console.WriteLine("+ - сложить");
            Console.WriteLine("- - вычесть");
            Console.WriteLine("* - умножить");
            Console.WriteLine("/ - разделить");
            Console.WriteLine("m - модуль");
            Console.WriteLine("a - аргумент");
            Console.WriteLine("p - показать число");
            Console.WriteLine("q - выход");
            Console.Write("> ");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Неизвестная команда");
                continue;
            }
            char cmd = input[0];
            switch (cmd)
            {
                case 'n':
                    Console.Write("Действительная часть: ");
                    x.re = double.Parse(Console.ReadLine());
                    Console.Write("Мнимая часть: ");
                    x.im = double.Parse(Console.ReadLine());
                    break;
                case '+':
                    {
                        Console.Write("re второго: ");
                        double r2 = double.Parse(Console.ReadLine());
                        Console.Write("im второго: ");
                        double i2 = double.Parse(Console.ReadLine());
                        Complex t = new Complex();
                        t.re = r2;
                        t.im = i2;
                        x = Add(x, t);
                    }
                    break;
                case '-':
                    {
                        Console.Write("re второго: ");
                        double r2 = double.Parse(Console.ReadLine());
                        Console.Write("im второго: ");
                        double i2 = double.Parse(Console.ReadLine());
                        Complex t = new Complex();
                        t.re = r2;
                        t.im = i2;
                        x = Sub(x, t);
                    }
                    break;

                case '*':
                    {
                        Console.Write("re второго: ");
                        double r2 = double.Parse(Console.ReadLine());
                        Console.Write("im второго: ");
                        double i2 = double.Parse(Console.ReadLine());
                        Complex t = new Complex();
                        t.re = r2;
                        t.im = i2;
                        x = Mul(x, t);
                    }
                    break;

                case '/':
                    {
                        Console.Write("re второго: ");
                        double r2 = double.Parse(Console.ReadLine());
                        Console.Write("im второго: ");
                        double i2 = double.Parse(Console.ReadLine());
                        Complex t = new Complex();
                        t.re = r2;
                        t.im = i2;
                        x = Div(x, t);
                    }
                    break;
                case 'm':
                    {
                        double mod = Math.Sqrt(x.re * x.re + x.im * x.im);
                        Console.WriteLine($"Модуль = {mod}");
                    }
                    break;
                case 'a':
                    {
                        double arg = Math.Atan2(x.im, x.re);
                        Console.WriteLine($"Аргумент = {arg} рад");
                    }
                    break;
                case 'p':
                    break;

                case 'q':
                case 'Q':
                    Console.WriteLine("Выход");
                    return;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }
    }
}