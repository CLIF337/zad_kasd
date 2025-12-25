using System;
using System.Collections.Generic;
using System.Globalization;

namespace CalcApp
{
    public class MyVector<T>
    {
        private T[] elementData;
        private int elementCount;
        private int capacityIncrement;

        public MyVector(int initialCapacity, int capacityIncrement)
        {
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Начальная емкость не может быть отрицательной");
            this.elementData = new T[initialCapacity];
            this.capacityIncrement = capacityIncrement;
            this.elementCount = 0;
        }

        public MyVector(int initialCapacity)
        {
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Начальная емкость не может быть отрицательной");
            this.elementData = new T[initialCapacity];
            this.capacityIncrement = 0;
            this.elementCount = 0;
        }

        public MyVector()
        {
            this.elementData = new T[10];
            this.capacityIncrement = 0;
            this.elementCount = 0;
        }

        public MyVector(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            this.elementData = new T[a.Length];
            Array.Copy(a, elementData, a.Length);
            this.elementCount = a.Length;
            this.capacityIncrement = 0;
        }

        public int Count => elementCount;
        public int Capacity => elementData.Length;

        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > elementData.Length)
            {
                int newCapacity;
                if (capacityIncrement > 0)
                {
                    newCapacity = elementData.Length + capacityIncrement;
                    if (newCapacity < minCapacity)
                        newCapacity = minCapacity;
                }
                else
                {
                    newCapacity = elementData.Length * 2;
                    if (newCapacity < minCapacity)
                        newCapacity = minCapacity;
                }
                T[] newArray = new T[newCapacity];
                Array.Copy(elementData, 0, newArray, 0, elementCount);
                elementData = newArray;
            }
        }

        private void ShiftElementsRight(int index, int count)
        {
            if (elementCount + count > elementData.Length)
                EnsureCapacity(elementCount + count);
            Array.Copy(elementData, index, elementData, index + count, elementCount - index);
            elementCount += count;
        }

        private void ShiftElementsLeft(int index, int count)
        {
            Array.Copy(elementData, index + count, elementData, index, elementCount - index - count);
            elementCount -= count;
        }

        public void Add(T e)
        {
            EnsureCapacity(elementCount + 1);
            elementData[elementCount] = e;
            elementCount++;
        }

        public void AddAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            EnsureCapacity(elementCount + a.Length);
            Array.Copy(a, 0, elementData, elementCount, a.Length);
            elementCount += a.Length;
        }

        public void Clear()
        {
            Array.Clear(elementData, 0, elementCount);
            elementCount = 0;
        }

        public bool Contains(object o)
        {
            if (o == null)
            {
                for (int i = 0; i < elementCount; i++)
                    if (elementData[i] == null)
                        return true;
            }
            else
            {
                for (int i = 0; i < elementCount; i++)
                    if (o.Equals(elementData[i]))
                        return true;
            }
            return false;
        }

        public bool ContainsAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            foreach (var item in a)
                if (!Contains(item))
                    return false;
            return true;
        }

        public bool IsEmpty() => elementCount == 0;

        public bool Remove(object o)
        {
            int index = IndexOf(o);
            if (index >= 0)
            {
                ShiftElementsLeft(index, 1);
                return true;
            }
            return false;
        }

        public void RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            foreach (var item in a)
                Remove(item);
        }

        public void RetainAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            var set = new HashSet<T>(a);
            int writeIndex = 0;
            for (int readIndex = 0; readIndex < elementCount; readIndex++)
            {
                if (set.Contains(elementData[readIndex]))
                {
                    elementData[writeIndex] = elementData[readIndex];
                    writeIndex++;
                }
            }
            Array.Clear(elementData, writeIndex, elementCount - writeIndex);
            elementCount = writeIndex;
        }

        public void Add(int index, T e)
        {
            if (index < 0 || index > elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            ShiftElementsRight(index, 1);
            elementData[index] = e;
        }

        public void AddAll(int index, T[] a)
        {
            if (index < 0 || index > elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            ShiftElementsRight(index, a.Length);
            Array.Copy(a, 0, elementData, index, a.Length);
        }

        public T Get(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            return elementData[index];
        }

        public int IndexOf(object o)
        {
            if (o == null)
            {
                for (int i = 0; i < elementCount; i++)
                    if (elementData[i] == null)
                        return i;
            }
            else
            {
                for (int i = 0; i < elementCount; i++)
                    if (o.Equals(elementData[i]))
                        return i;
            }
            return -1;
        }

        public int LastIndexOf(object o)
        {
            if (o == null)
            {
                for (int i = elementCount - 1; i >= 0; i--)
                    if (elementData[i] == null)
                        return i;
            }
            else
            {
                for (int i = elementCount - 1; i >= 0; i--)
                    if (o.Equals(elementData[i]))
                        return i;
            }
            return -1;
        }

        public T RemoveAt(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            T removed = elementData[index];
            ShiftElementsLeft(index, 1);
            return removed;
        }

        public T Set(int index, T e)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            T old = elementData[index];
            elementData[index] = e;
            return old;
        }

        public MyVector<T> SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex > elementCount)
                throw new ArgumentOutOfRangeException(nameof(fromIndex));
            if (toIndex < 0 || toIndex > elementCount)
                throw new ArgumentOutOfRangeException(nameof(toIndex));
            if (fromIndex > toIndex)
                throw new ArgumentException("fromIndex > toIndex");
            int length = toIndex - fromIndex;
            T[] subArray = new T[length];
            Array.Copy(elementData, fromIndex, subArray, 0, length);

            return new MyVector<T>(subArray);
        }

        public T FirstElement()
        {
            if (elementCount == 0)
                throw new InvalidOperationException("Вектор пуст");
            return elementData[0];
        }

        public T LastElement()
        {
            if (elementCount == 0)
                throw new InvalidOperationException("Вектор пуст");
            return elementData[elementCount - 1];
        }

        public void RemoveElementAt(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            ShiftElementsLeft(index, 1);
        }

        public void RemoveRange(int begin, int end)
        {
            if (begin < 0 || begin > elementCount)
                throw new ArgumentOutOfRangeException(nameof(begin));
            if (end < 0 || end > elementCount)
                throw new ArgumentOutOfRangeException(nameof(end));
            if (begin > end)
                throw new ArgumentException("begin > end");
            int length = end - begin;
            ShiftElementsLeft(begin, length);
        }

        public T[] ToArray()
        {
            T[] array = new T[elementCount];
            Array.Copy(elementData, 0, array, 0, elementCount);
            return array;
        }

        public T[] ToArray(T[] a)
        {
            if (a == null)
                return ToArray();
            if (a.Length < elementCount)
            {
                a = new T[elementCount];
            }
            Array.Copy(elementData, 0, a, 0, elementCount);
            if (a.Length > elementCount)
                a[elementCount] = default(T);
            return a;
        }
    }

    public class MyStack<T> : MyVector<T>
    {
        public MyStack() : base() { }
        public MyStack(int initialCapacity) : base(initialCapacity) { }
        public MyStack(int initialCapacity, int capacityIncrement)
            : base(initialCapacity, capacityIncrement) { }

        public MyStack(T[] a) : base(a) { }

        public void Push(T item)
        {
            Add(item);
        }

        public T Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Стек пуст. Нельзя извлечь элемент.");
            T item = LastElement();
            RemoveAt(Count - 1);
            return item;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Стек пуст. Нельзя получить элемент.");
            return LastElement();
        }

        public bool Empty()
        {
            return IsEmpty();
        }

        public int Search(T item)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                T current = Get(i);
                if (item == null)
                {
                    if (current == null)
                        return Count - i;
                }
                else if (item.Equals(current))
                {
                    return Count - i;
                }
            }
            return -1;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Использование: Task13.exe \"выражение\" [a=5 b=10 ...]");
                Console.WriteLine("Пример: Task13.exe \"3 + 4 * 2 / (1 - 5) ^ 2\"");
                Console.WriteLine("Пример с переменными: Task13.exe \"a + b * c\" a=3 b=4 c=2");
                return;
            }

            string expression = args[0];
            Dictionary<string, double> variables = new Dictionary<string, double>();

            for (int i = 1; i < args.Length; i++)
            {
                string[] parts = args[i].Split('=');
                if (parts.Length == 2 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                {
                    variables[parts[0]] = val;
                }
                else
                {
                    Console.WriteLine($"Ошибка: неверный параметр {args[i]}");
                    return;
                }
            }

            try
            {
                List<string> tokens = Tokenize(expression);
                List<string> rpn = ToRPN(tokens);
                double result = EvaluateRPN(rpn, variables);
                Console.WriteLine(result.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        static List<string> Tokenize(string expr)
        {
            List<string> tokens = new List<string>();
            string number = "";

            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (char.IsWhiteSpace(c)) continue;

                if (char.IsDigit(c) || c == '.')
                {
                    number += c;
                }
                else
                {
                    if (number.Length > 0)
                    {
                        tokens.Add(number);
                        number = "";
                    }

                    // Проверка на оператор "//" (целочисленное деление)
                    if (c == '/' && i + 1 < expr.Length && expr[i + 1] == '/')
                    {
                        tokens.Add("//");
                        i++; // Пропускаем второй символ
                        continue;
                    }

                    if (char.IsLetter(c))
                    {
                        string func = c.ToString();
                        while (i + 1 < expr.Length && char.IsLetter(expr[i + 1]))
                        {
                            func += expr[++i];
                        }
                        tokens.Add(func);
                    }
                    else if ("()+-*/^%,".IndexOf(c) >= 0) // ← ДОБАВЛЕНА ЗАПЯТАЯ
                    {
                        tokens.Add(c.ToString());
                    }
                    else
                    {
                        throw new Exception($"Неизвестный символ: {c}");
                    }
                }
            }

            if (number.Length > 0)
            {
                tokens.Add(number);
            }

            return tokens;
        }

        static int Priority(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                case "%":
                case "//":
                    return 2;
                case "^":
                    return 3;
                default:
                    if (IsFunction(op)) return 4;
                    return 0;
            }
        }

        static bool IsFunction(string token)
        {
            string[] functions = {
                "sqrt", "abs", "sign", "sin", "cos", "tan",
                "ln", "log", "min", "max", "exp", "trunc"
            };

            for (int i = 0; i < functions.Length; i++)
            {
                if (token == functions[i]) return true;
            }
            return false;
        }

        static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" ||
                   token == "^" || token == "%" || token == "//";
        }

        static int GetFunctionArgumentCount(string func)
        {
            switch (func)
            {
                case "min":
                case "max":
                    return 2;
                default:
                    return 1;
            }
        }

        static List<string> ToRPN(List<string> tokens)
        {
            List<string> output = new List<string>();
            MyStack<string> ops = new MyStack<string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _)
                    || (char.IsLetter(token[0]) && !IsFunction(token)))
                {
                    // Число или переменная
                    output.Add(token);
                }
                else if (IsFunction(token))
                {
                    // Функция
                    ops.Push(token);
                }
                else if (token == ",")
                {
                    // Разделитель аргументов функции
                    while (!ops.Empty() && ops.Peek() != "(")
                    {
                        output.Add(ops.Pop());
                    }
                    if (ops.Empty() || ops.Peek() != "(")
                        throw new Exception("Запятая не внутри функции");
                }
                else if (token == "(")
                {
                    // Открывающая скобка
                    ops.Push(token);
                }
                else if (token == ")")
                {
                    // Закрывающая скобка
                    while (!ops.Empty() && ops.Peek() != "(")
                    {
                        output.Add(ops.Pop());
                    }

                    if (ops.Empty())
                        throw new Exception("Несогласованные скобки");

                    ops.Pop(); // Удаляем "("

                    if (!ops.Empty() && IsFunction(ops.Peek()))
                    {
                        output.Add(ops.Pop()); // Добавляем функцию
                    }
                }
                else if (IsOperator(token))
                {
                    // Оператор
                    while (!ops.Empty() && IsOperator(ops.Peek()) &&
                           Priority(ops.Peek()) >= Priority(token))
                    {
                        output.Add(ops.Pop());
                    }
                    ops.Push(token);
                }
                else
                {
                    throw new Exception($"Неизвестный токен: {token}");
                }
            }

            while (!ops.Empty())
            {
                if (ops.Peek() == "(" || ops.Peek() == ")")
                    throw new Exception("Несогласованные скобки");
                output.Add(ops.Pop());
            }

            return output;
        }

        static double EvaluateRPN(List<string> rpn, Dictionary<string, double> vars)
        {
            MyStack<double> stack = new MyStack<double>();

            for (int i = 0; i < rpn.Count; i++)
            {
                string token = rpn[i];

                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                {
                    stack.Push(num);
                }
                else if (vars.ContainsKey(token))
                {
                    stack.Push(vars[token]);
                }
                else if (IsFunction(token))
                {
                    int argCount = GetFunctionArgumentCount(token);
                    if (stack.Count < argCount)
                        throw new Exception($"Недостаточно аргументов для функции {token}");

                    double result = ApplyFunction(token, stack, argCount);
                    stack.Push(result);
                }
                else if (IsOperator(token))
                {
                    if (stack.Count < 2)
                        throw new Exception($"Недостаточно операндов для операции {token}");

                    double b = stack.Pop();
                    double a = stack.Pop();
                    double result = ApplyOperator(token, a, b);
                    stack.Push(result);
                }
                else
                {
                    throw new Exception($"Неизвестный токен: {token}");
                }
            }

            if (stack.Count != 1)
                throw new Exception("Некорректное выражение");

            return stack.Pop();
        }

        static double ApplyFunction(string func, MyStack<double> stack, int argCount)
        {
            switch (func)
            {
                case "sqrt":
                    double x = stack.Pop();
                    if (x < 0) throw new Exception("Квадратный корень из отрицательного числа");
                    return Math.Sqrt(x);

                case "abs":
                    return Math.Abs(stack.Pop());

                case "sign":
                    return Math.Sign(stack.Pop());

                case "sin":
                    return Math.Sin(stack.Pop());

                case "cos":
                    return Math.Cos(stack.Pop());

                case "tan":
                    return Math.Tan(stack.Pop());

                case "ln":
                    x = stack.Pop();
                    if (x <= 0) throw new Exception("Логарифм неположительного числа");
                    return Math.Log(x);

                case "log":
                    x = stack.Pop();
                    if (x <= 0) throw new Exception("Логарифм неположительного числа");
                    return Math.Log10(x);

                case "min":
                    double b = stack.Pop();
                    double a = stack.Pop();
                    return Math.Min(a, b);

                case "max":
                    b = stack.Pop();
                    a = stack.Pop();
                    return Math.Max(a, b);

                case "exp":
                    return Math.Exp(stack.Pop());

                case "trunc":
                    return Math.Truncate(stack.Pop());

                default:
                    throw new Exception($"Неизвестная функция: {func}");
            }
        }

        static double ApplyOperator(string op, double a, double b)
        {
            switch (op)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    if (b == 0) throw new DivideByZeroException("Деление на ноль");
                    return a / b;
                case "^":
                    return Math.Pow(a, b);
                case "%":
                    return a % b;
                case "//":
                    if (b == 0) throw new DivideByZeroException("Целочисленное деление на ноль");
                    return Math.Truncate(a / b);
                default:
                    throw new Exception($"Неизвестная операция: {op}");
            }
        }
    }
}