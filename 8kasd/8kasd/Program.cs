using System;
using System.Collections.Generic;

public class MyArrayList<T>
{
    private T[] elementData;
    private int size;
    private const int DefaultCapacity = 10;

    //пустой
    public MyArrayList()
    {
        elementData = new T[DefaultCapacity];
        size = 0;
    }

    //из массива
    public MyArrayList(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        elementData = new T[array.Length];
        Array.Copy(array, elementData, array.Length);
        size = array.Length;
    }

    //с определенной емкостью
    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Ёмкость не может быть отрицательной");
        elementData = new T[capacity];
        size = 0;
    }

    //добав в конец
    public void Add(T element)
    {
        EnsureCapacity(size + 1);
        elementData[size] = element;
        size++;
    }

    //всех из массива
    public void AddAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        EnsureCapacity(size + array.Length);
        Array.Copy(array, 0, elementData, size, array.Length);
        size += array.Length;
    }

    public void Clear()
    {
        for (int i = 0; i < size; i++)
        {
            elementData[i] = default(T);
        }
        size = 0;
    }

    //есть ли элемент
    public bool Contains(object obj)
    {
        if (obj is T item)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], item))
                    return true;
            }
        }
        return false;
    }

    //есть ли все из массива
    public bool ContainsAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        foreach (T item in array)
        {
            if (!Contains(item))
                return false;
        }
        return true;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }

    //удаление по значению
    public bool Remove(object obj)
    {
        if (obj is T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
        }
        return false;
    }

    // удаление всех указанных
    public bool RemoveAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        bool modified = false;
        foreach (T item in array)
        {
            while (Contains(item))
            {
                Remove(item);
                modified = true;
            }
        }
        return modified;
    }

    //оставить из массива
    public bool RetainAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        bool modified = false;
        MyArrayList<T> tempList = new MyArrayList<T>();
        for (int i = 0; i < size; i++)
        {
            if (Array.IndexOf(array, elementData[i]) >= 0)
            {
                tempList.Add(elementData[i]);
            }
            else
            {
                modified = true;
            }
        }
        Clear();
        for (int i = 0; i < tempList.size; i++)
        {
            Add(tempList.elementData[i]);
        }
        return modified;
    }

    public int Size()
    {
        return size;
    }

    // преобраз в массив
    public T[] ToArray()
    {
        T[] result = new T[size];
        Array.Copy(elementData, result, size);
        return result;
    }

    // преобразование в массив
    public T[] ToArray(T[] array)
    {
        if (array == null)
            return ToArray();
        if (array.Length < size)
            return ToArray();
        Array.Copy(elementData, array, size);
        if (array.Length > size)
        {
            array[size] = default(T);
        }
        return array;
    }

    // добавление по индексу
    public void Add(int index, T element)
    {
        CheckIndexForAdd(index);
        EnsureCapacity(size + 1);
        for (int i = size; i > index; i--)
        {
            elementData[i] = elementData[i - 1];
        }

        elementData[index] = element;
        size++;
    }

    // добавление массива по индексу
    public void AddAll(int index, T[] array)
    {
        CheckIndexForAdd(index);
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        EnsureCapacity(size + array.Length);
        for (int i = size - 1; i >= index; i--)
        {
            elementData[i + array.Length] = elementData[i];
        }
        Array.Copy(array, 0, elementData, index, array.Length);
        size += array.Length;
    }

    // вывод элемента
    public T Get(int index)
    {
        CheckIndex(index);
        return elementData[index];
    }

    // поиск вхождения 1
    public int IndexOf(object obj)
    {
        if (obj is T item)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], item))
                    return i;
            }
        }
        return -1;
    }

    // поиск вхождения last
    public int LastIndexOf(object obj)
    {
        if (obj is T item)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], item))
                    return i;
            }
        }
        return -1;
    }

    // удаление по индексу
    public T RemoveAt(int index)
    {
        CheckIndex(index);
        T removed = elementData[index];
        for (int i = index; i < size - 1; i++)
        {
            elementData[i] = elementData[i + 1];
        }
        elementData[size - 1] = default(T);
        size--;

        return removed;
    }

    // замена по индексу
    public T Set(int index, T element)
    {
        CheckIndex(index);
        T oldElement = elementData[index];
        elementData[index] = element;
        return oldElement;
    }

    // подсписок по индексам
    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex > size)
            throw new ArgumentOutOfRangeException(nameof(fromIndex));
        if (toIndex < fromIndex || toIndex > size)
            throw new ArgumentOutOfRangeException(nameof(toIndex));
        MyArrayList<T> subList = new MyArrayList<T>(toIndex - fromIndex);
        for (int i = fromIndex; i < toIndex; i++)
        {
            subList.Add(elementData[i]);
        }
        return subList;
    }

    // увеличение емкости
    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > elementData.Length)
        {
            int newCapacity = elementData.Length * 3 / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, size);
            elementData = newArray;
        }
    }

    // проверка индекса
    private void CheckIndex(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне границ массива");
    }

    // проверка индекса для добавления
    private void CheckIndexForAdd(int index)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне границ массива");
    }

    public override string ToString()
    {
        if (size == 0)
            return "[]";
        string result = "[";
        for (int i = 0; i < size; i++)
        {
            result += elementData[i];
            if (i < size - 1)
                result += ", ";
        }
        result += "]";
        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Тестирование MyArrayList ===");

        MyArrayList<int> list1 = new MyArrayList<int>();
        Console.WriteLine($"Пустой список: {list1}");
        Console.WriteLine($"Размер: {list1.Size()}, Пустой: {list1.IsEmpty()}");

        list1.Add(10);
        list1.Add(20);
        list1.Add(30);
        Console.WriteLine($"\nПосле добавления 10, 20, 30: {list1}");

        list1.Add(1, 15);
        Console.WriteLine($"После добавления 15 на позицию 1: {list1}");

        Console.WriteLine($"Элемент на позиции 2: {list1.Get(2)}");

        Console.WriteLine($"Содержит 20? {list1.Contains(20)}");
        Console.WriteLine($"Содержит 50? {list1.Contains(50)}");

        list1.Remove(20);
        Console.WriteLine($"После удаления 20: {list1}");

        int removed = list1.RemoveAt(0);
        Console.WriteLine($"После удаления элемента на позиции 0 (удалено: {removed}): {list1}");

        int old = list1.Set(0, 100);
        Console.WriteLine($"После замены элемента на позиции 0 (старое значение: {old}): {list1}");

        string[] names = { "Анна", "Борис", "Виктор" };
        MyArrayList<string> list2 = new MyArrayList<string>(names);
        Console.WriteLine($"\nСписок строк из массива: {list2}");

        string[] moreNames = { "Дмитрий", "Елена" };
        list2.AddAll(moreNames);
        Console.WriteLine($"После добавления массива: {list2}");

        MyArrayList<string> subList = list2.SubList(1, 3);
        Console.WriteLine($"Подсписок [1, 3): {subList}");

        string[] array = list2.ToArray();
        Console.WriteLine("\nПреобразовано в массив:");
        foreach (string name in array)
        {
            Console.Write(name + " ");
        }

        Console.WriteLine("\n\n=== Тест увеличения ёмкости ===");
        MyArrayList<int> bigList = new MyArrayList<int>(2);
        for (int i = 0; i < 20; i++)
        {
            bigList.Add(i * 10);
        }
        Console.WriteLine($"Список из 20 элементов: {bigList}");
        Console.WriteLine($"Размер: {bigList.Size()}");
    }
}