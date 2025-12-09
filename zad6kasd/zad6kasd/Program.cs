using System;
using System.Collections.Generic;

public class MyPriorityQueue<T>
{
    private T[] items;
    private int count;
    private IComparer<T> comparer;
    public MyPriorityQueue()
    {
        items = new T[11];
        comparer = Comparer<T>.Default;
    }
    public MyPriorityQueue(T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        items = new T[array.Length];
        Array.Copy(array, items, array.Length);
        count = array.Length;
        comparer = Comparer<T>.Default;
        Heapify();
    }
    public MyPriorityQueue(int nach_emk)
    {
        if (nach_emk < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(nach_emk));
        }
        items = new T[nach_emk];
        comparer = Comparer<T>.Default;
    }
    public MyPriorityQueue(int initialCapacity, IComparer<T> comparer)
    {
        if (initialCapacity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(initialCapacity));
        }
        items = new T[initialCapacity];
        if (comparer == null)
        {
            this.comparer = Comparer<T>.Default;
        }
        else
        {
            this.comparer = comparer;
        }
    }
    public MyPriorityQueue(MyPriorityQueue<T> queue)
    {
        if (queue == null)
        {
            throw new ArgumentNullException(nameof(queue));
        }
        items = new T[queue.items.Length];
        Array.Copy(queue.items, items, queue.count);
        count = queue.count;
        comparer = queue.comparer;
    }
    public void Add(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        EnsureCapacity(count + 1);
        items[count] = item;
        SiftUp(count);
        count++;
    }
    public void AddAll(T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        EnsureCapacity(count + array.Length);
        foreach (var element in array)
        {
            if (element == null)
            {
                throw new ArgumentException("Array contains null");
            }
            items[count] = element;
            SiftUp(count);
            count++;
        }
    }
    public void Clear()
    {
        Array.Clear(items, 0, count);
        count = 0;
    }
    public bool Contains(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
        for (int i = 0; i < count; i++)
        {
            if (EqualityComparer<T>.Default.Equals((T)obj, items[i]))
            {
                return true;
            }
        }
        return false;
    }
    public bool ContainsAll(T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        foreach (var item in array)
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
        return count == 0;
    }
    public bool Remove(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
        for (int i = 0; i < count; i++)
        {
            if (EqualityComparer<T>.Default.Equals((T)obj, items[i]))
            {
                RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public void RemoveAll(T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        foreach (var item in array)
        {
            while (Remove(item)) { }
        }
    }
    public void RetainAll(T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        var temp = new List<T>();
        for (int i = 0; i < count; i++)
        {
            bool found = false;
            foreach (var element in array)
            {
                if (EqualityComparer<T>.Default.Equals(element, items[i]))
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                temp.Add(items[i]);
            }
        }
        Clear();
        foreach (var item in temp)
        {
            Add(item);
        }
    }
    public int Size()
    {
        return count;
    }
    public T[] ToArray()
    {
        var result = new T[count];
        Array.Copy(items, result, count);
        return result;
    }
    public T[] ToArray(T[] array)
    {
        if (array == null)
        {
            return ToArray();
        }
        if (array.Length < count)
        {
            return ToArray();
        }
        Array.Copy(items, array, count);
        if (array.Length > count)
        {
            array[count] = default(T);
        }
        return array;
    }
    public T Element()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        return items[0];
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
        if (count == 0)
        {
            return default(T);
        }
        else
        {
            return items[0];
        }
    }
    public T Poll()
    {
        if (count == 0)
        {
            return default(T);
        }
        T result = items[0];
        RemoveAt(0);
        return result;
    }
    private void Heapify()
    {
        for (int i = (count - 1) / 2; i >= 0; i--)
        {
            SiftDown(i);
        }
    }
    private void SiftUp(int idx)
    {
        T x = items[idx];
        while (idx > 0)
        {
            int parent = (idx - 1) / 2;
            if (comparer.Compare(x, items[parent]) >= 0)
            {
                break;
            }
            items[idx] = items[parent];
            idx = parent;
        }
        items[idx] = x;
    }
    private void SiftDown(int idx)
    {
        T x = items[idx];
        while (idx * 2 + 1 < count)
        {
            int child = idx * 2 + 1;
            if (child + 1 < count && comparer.Compare(items[child], items[child + 1]) > 0)
            {
                child++;
            }
            if (comparer.Compare(x, items[child]) <= 0)
            {
                break;
            }
            items[idx] = items[child];
            idx = child;
        }
        items[idx] = x;
    }
    private void RemoveAt(int idx)
    {
        count--;
        if (idx == count)
        {
            items[idx] = default(T);
            return;
        }
        items[idx] = items[count];
        items[count] = default(T);
        SiftDown(idx);
        if (idx < count && comparer.Compare(items[idx], items[count]) == 0)
        {
            SiftUp(idx);
        }
    }
    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity <= items.Length)
        {
            return;
        }

        int newCapacity;
        if (items.Length < 64)
        {
            newCapacity = items.Length * 2;
        }
        else
        {
            newCapacity = items.Length + items.Length / 2;
        }

        Array.Resize(ref items, newCapacity);
    }
}
class Program
{
    static void Main(string[] args)
    {
        var queue = new MyPriorityQueue<int>();
        queue.Add(5);
        queue.Add(2);
        queue.Add(8);
        queue.Offer(1);
        Console.WriteLine("Размер: " + queue.Size());
        Console.WriteLine("Первый элемент: " + queue.Peek());
        Console.WriteLine("Элемент (с исключением): " + queue.Element());

        Console.WriteLine("Содержит 2: " + queue.Contains(2));
        Console.WriteLine("Содержит все [1,2]: " + queue.ContainsAll(new[] { 1, 2 }));

        var array = queue.ToArray();
        Console.Write("Массив: ");
        for (int i = 0; i < array.Length; i++)
        {
            if (i > 0) Console.Write(", ");
            Console.Write(array[i]);
        }
        Console.WriteLine();
        queue.Remove(2);
        Console.WriteLine("После удаления 2, размер: " + queue.Size());
        Console.WriteLine("Извлечение элементов:");
        while (!queue.IsEmpty())
        {
            Console.WriteLine(queue.Poll());
        }
    }
}