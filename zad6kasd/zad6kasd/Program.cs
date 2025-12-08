using System;
using System.Collections;
using System.Collections.Generic;

public delegate int PriorityQueueComparer<T>(T x, T y);

public class MyPriorityQueue<T>
{
    private T[] queue;
    private int size;
    private PriorityQueueComparer<T> comparator;

    public MyPriorityQueue()
    {
        queue = new T[11 + 1];
        size = 0;
        comparator = null;
    }
    public MyPriorityQueue(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");
        queue = new T[a.Length + 1 + 11];
        size = a.Length;
        comparator = null;
        for (int i = 0; i < a.Length; i++)
        {
            queue[i + 1] = a[i];
        }
        MakeHeap();
    }
    public MyPriorityQueue(int initialCapacity)
    {
        if (initialCapacity < 1) throw new ArgumentException("Емкость должна быть больше 0");
        queue = new T[initialCapacity + 1];
        size = 0;
        comparator = null;
    }
    public MyPriorityQueue(int initialCapacity, PriorityQueueComparer<T> comparator)
    {
        if (initialCapacity < 1) throw new ArgumentException("Емкость должна быть больше 0");
        queue = new T[initialCapacity + 1];
        size = 0;
        this.comparator = comparator;
    }

    public MyPriorityQueue(MyPriorityQueue<T> other)
    {
        if (other == null) throw new ArgumentNullException("Очередь не может быть null");
        this.size = other.size;
        this.comparator = other.comparator;
        this.queue = new T[other.queue.Length];
        for (int i = 0; i < other.queue.Length; i++)
        {
            this.queue[i] = other.queue[i];
        }
    }
    public void Add(T e)
    {
        if (e == null) throw new ArgumentNullException("Элемент не может быть null");
        if (size + 1 >= queue.Length)
        {
            int newSize;
            if (queue.Length < 64)
            {
                newSize = queue.Length * 2;
            }
            else
            {
                newSize = queue.Length + (queue.Length / 2);
            }
            T[] newQueue = new T[newSize];
            for (int i = 0; i < queue.Length; i++)
            {
                newQueue[i] = queue[i];
            }
            queue = newQueue;
        }
        size++;
        queue[size] = e;
        Swim(size);
    }
    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        foreach (T item in a)
        {
            Add(item);
        }
    }
    public void Clear()
    {
        for (int i = 1; i <= size; i++)
        {
            queue[i] = default(T);
        }
        size = 0;
    }
    public bool Contains(object o)
    {
        if (o == null) return false;

        for (int i = 1; i <= size; i++)
        {
            if (queue[i] != null && queue[i].Equals(o))
            {
                return true;
            }
        }
        return false;
    }
    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        foreach (T item in a)
        {
            if (!Contains(item))
            {
                return false;
            }
        }
        return true;
    }
    public bool IsEmpty()
    {
        return size == 0;
    }
    public bool Remove(object o)
    {
        if (o == null) return false;
        int index = -1;
        for (int i = 1; i <= size; i++)
        {
            if (queue[i] != null && queue[i].Equals(o))
            {
                index = i;
                break;
            }
        }
        if (index == -1) return false;
        queue[index] = queue[size];
        queue[size] = default(T);
        size--;
        if (index <= size)
        {
            Sink(index);
            Swim(index);
        }
        return true;
    }
    public void RemoveAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");

        foreach (T item in a)
        {
            Remove(item);
        }
    }
    public void RetainAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException("Массив не может быть null");
        MyPriorityQueue<T> temp = new MyPriorityQueue<T>(size, comparator);
        for (int i = 1; i <= size; i++)
        {
            bool found = false;
            foreach (T item in a)
            {
                if (queue[i] != null && queue[i].Equals(item))
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                temp.Add(queue[i]);
            }
        }
        this.size = temp.size;
        for (int i = 0; i < temp.queue.Length; i++)
        {
            this.queue[i] = temp.queue[i];
        }
    }
    public int Size()
    {
        return size;
    }
    public T[] ToArray()
    {
        T[] result = new T[size];
        for (int i = 0; i < size; i++)
        {
            result[i] = queue[i + 1];
        }
        return result;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null)
        {
            return ToArray();
        }
        if (a.Length < size)
        {
            return ToArray();
        }
        for (int i = 0; i < size; i++)
        {
            a[i] = queue[i + 1];
        }
        if (a.Length > size)
        {
            a[size] = default(T);
        }
        return a;
    }
    public T Element()
    {
        if (size == 0) throw new InvalidOperationException("Очередь пуста");
        return queue[1];
    }
    public bool Offer(T obj)
    {
        try
        {
            Add(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public T Peek()
    {
        if (size == 0) return default(T);
        return queue[1];
    }
    public T Poll()
    {
        if (size == 0) return default(T);
        T result = queue[1];
        queue[1] = queue[size];
        queue[size] = default(T);
        size--;
        if (size > 0)
        {
            Sink(1);
        }

        return result;
    }
    private void MakeHeap()
    {
        for (int i = size / 2; i >= 1; i--)
        {
            Sink(i);
        }
    }

    private void Swim(int k)
    {
        while (k > 1 && Compare(k / 2, k) < 0)
        {
            Swap(k, k / 2);
            k = k / 2;
        }
    }

    private void Sink(int k)
    {
        while (2 * k <= size)
        {
            int j = 2 * k;
            if (j < size && Compare(j, j + 1) < 0) j++;
            if (Compare(k, j) >= 0) break;
            Swap(k, j);
            k = j;
        }
    }

    private int Compare(int i, int j)
    {
        if (comparator != null)
        {
            return comparator(queue[i], queue[j]);
        }
        IComparable<T> comp1 = queue[i] as IComparable<T>;
        if (comp1 != null)
        {
            return comp1.CompareTo(queue[j]);
        }
        IComparable comp2 = queue[i] as IComparable;
        if (comp2 != null)
        {
            return comp2.CompareTo(queue[j]);
        }
        throw new InvalidOperationException("Невозможно сравнить элементы");
    }
    private void Swap(int i, int j)
    {
        T temp = queue[i];
        queue[i] = queue[j];
        queue[j] = temp;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Тестирование MyPriorityQueue ===");
        Console.WriteLine("\n1. Тест с целыми числами:");
        MyPriorityQueue<int> pq1 = new MyPriorityQueue<int>();
        pq1.Add(5);
        pq1.Add(1);
        pq1.Add(10);
        pq1.Add(3);

        Console.WriteLine("Размер: " + pq1.Size());
        Console.WriteLine("Голова: " + pq1.Peek());

        Console.WriteLine("Извлекаем все:");
        while (!pq1.IsEmpty())
        {
            Console.Write(pq1.Poll() + " ");
        }

        Console.WriteLine("\n\n2. Создание из массива:");
        int[] arr = { 7, 2, 9, 4, 1 };
        MyPriorityQueue<int> pq2 = new MyPriorityQueue<int>(arr);
        Console.WriteLine("Массив из очереди: " + string.Join(", ", pq2.ToArray()));

        Console.WriteLine("\n3. С кастомным компаратором:");
        PriorityQueueComparer<int> reverse = (x, y) => y.CompareTo(x);
        MyPriorityQueue<int> pq3 = new MyPriorityQueue<int>(5, reverse);
        pq3.Add(5);
        pq3.Add(1);
        pq3.Add(10);

        Console.WriteLine("В обратном порядке:");
        while (!pq3.IsEmpty())
        {
            Console.Write(pq3.Poll() + " ");
        }

        Console.WriteLine("\n\n4. Проверка Contains и Remove:");
        MyPriorityQueue<string> pq4 = new MyPriorityQueue<string>();
        pq4.Add("яблоко");
        pq4.Add("банан");
        pq4.Add("апельсин");

        Console.WriteLine("Есть 'банан'? " + pq4.Contains("банан"));
        Console.WriteLine("Удаляем 'банан': " + pq4.Remove("банан"));
        Console.WriteLine("Есть 'банан' теперь? " + pq4.Contains("банан"));

        Console.WriteLine("\n5. Проверка ошибок:");
        try
        {
            MyPriorityQueue<int> pq5 = new MyPriorityQueue<int>(-5);
        }
        catch (Exception e)
        {
            Console.WriteLine("Поймали ошибку: " + e.Message);
        }
        Console.WriteLine("\n=== Тестирование завершено ===");
    }
}