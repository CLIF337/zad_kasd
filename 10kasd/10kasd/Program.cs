using System;
using System.Collections.Generic;

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
class Program
{
    static void Main()
    {
        MyVector<int> vector = new MyVector<int>();
        vector.Add(10);
        vector.Add(20);
        vector.Add(30);
        vector.Add(1, 15);
        Console.WriteLine($"Первый элемент: {vector.FirstElement()}");
        Console.WriteLine($"Последний элемент: {vector.LastElement()}");
        Console.WriteLine($"Элемент по индексу 2: {vector.Get(2)}");
        Console.WriteLine($"Содержит 20: {vector.Contains(20)}");
        vector.Remove(20);
        Console.WriteLine($"После удаления 20, содержит 20: {vector.Contains(20)}");
        int[] array = vector.ToArray();
        Console.WriteLine($"Массив: [{string.Join(", ", array)}]");
        var subVector = vector.SubList(1, 3);
        Console.WriteLine($"Подсписок: [{string.Join(", ", subVector.ToArray())}]");
    }
}