using System;
using System.IO;
using System.Text;

public class MyArrayList<T>
{
    private T[] elementData;
    private int size;
    private const int DefaultCapacity = 10;
    public MyArrayList()
    {
        elementData = new T[DefaultCapacity];
        size = 0;
    }

    public MyArrayList(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        size = a.Length;
    }

    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));
        elementData = new T[capacity];
        size = 0;
    }

    public int Size => size;
    public int Capacity => elementData.Length;
    public bool IsEmpty => size == 0;
    public void Add(T e)
    {
        EnsureCapacity(size + 1);
        elementData[size++] = e;
    }

    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        EnsureCapacity(size + a.Length);
        Array.Copy(a, 0, elementData, size, a.Length);
        size += a.Length;
    }

    public void Clear()
    {
        for (int i = 0; i < size; i++)
        {
            elementData[i] = default(T);
        }
        size = 0;
    }

    public bool Contains(object o)
    {
        return IndexOf(o) >= 0;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        foreach (var item in a)
        {
            if (!Contains(item))
                return false;
        }
        return true;
    }

    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public void RemoveAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        foreach (var item in a)
        {
            while (Remove(item)) { }
        }
    }

    public void RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        var newArray = new T[size];
        int newSize = 0;
        for (int i = 0; i < size; i++)
        {
            if (Array.IndexOf(a, elementData[i]) >= 0)
            {
                newArray[newSize++] = elementData[i];
            }
        }
        elementData = newArray;
        size = newSize;
    }

    public T[] ToArray()
    {
        T[] result = new T[size];
        Array.Copy(elementData, result, size);
        return result;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null)
            return ToArray();
        if (a.Length < size)
        {
            return ToArray();
        }
        Array.Copy(elementData, a, size);
        if (a.Length > size)
        {
            a[size] = default(T);
        }
        return a;
    }

    public void Add(int index, T e)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity(size + 1);
        if (index < size)
        {
            Array.Copy(elementData, index, elementData, index + 1, size - index);
        }
        elementData[index] = e;
        size++;
    }

    public void AddAll(int index, T[] a)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        EnsureCapacity(size + a.Length);
        if (index < size)
        {
            Array.Copy(elementData, index, elementData, index + a.Length, size - index);
        }
        Array.Copy(a, 0, elementData, index, a.Length);
        size += a.Length;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }
    public int IndexOf(object o)
    {
        for (int i = 0; i < size; i++)
        {
            if (Equals(elementData[i], o))
                return i;
        }
        return -1;
    }
    public int LastIndexOf(object o)
    {
        for (int i = size - 1; i >= 0; i--)
        {
            if (Equals(elementData[i], o))
                return i;
        }
        return -1;
    }
    public T RemoveAt(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        T removed = elementData[index];
        int numMoved = size - index - 1;
        if (numMoved > 0)
        {
            Array.Copy(elementData, index + 1, elementData, index, numMoved);
        }
        elementData[--size] = default(T);
        return removed;
    }

    public T Set(int index, T e)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }

    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();
        int newSize = toIndex - fromIndex;
        T[] newArray = new T[newSize];
        Array.Copy(elementData, fromIndex, newArray, 0, newSize);
        return new MyArrayList<T>(newArray);
    }

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

    public T this[int index]
    {
        get => Get(index);
        set => Set(index, value);
    }

    public override string ToString()
    {
        var array = ToArray();
        return $"[{string.Join(", ", array)}] (Size: {size}, Capacity: {Capacity})";
    }
}

class Program
{
    private class TagRecord
    {
        public string OriginalTag { get; set; }
        public string NormalizedValue { get; set; }
    }

    static void Main()
    {
        try
        {
            string[] lines = File.ReadAllLines("input.txt");
            MyArrayList<TagRecord> tagRecords = new MyArrayList<TagRecord>();
            MyArrayList<string> finalTags = new MyArrayList<string>();

            foreach (string line in lines)
            {
                AnalyzeLineForTags(line, tagRecords, finalTags);
            }
            Console.WriteLine("Найденные уникальные теги:");
            Console.WriteLine("==========================");
            for (int i = 0; i < finalTags.Size; i++)
            {
                Console.WriteLine($"{i + 1}. {finalTags.Get(i)}");
            }
            Console.WriteLine($"\nВсего найдено уникальных тегов: {finalTags.Size}");
            SaveTagsToFile("output.txt", finalTags);
            Console.WriteLine("Результат сохранен в файл 'output.txt'");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Ошибка: Файл 'input.txt' не найден!");
            Console.WriteLine("Убедитесь, что файл находится в той же папке, что и программа.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
        Console.WriteLine("\nДля завершения нажмите любую клавишу...");
        Console.ReadKey();
    }

    static void AnalyzeLineForTags(string textLine, MyArrayList<TagRecord> records, MyArrayList<string> resultTags)
    {
        int position = 0;
        while (position < textLine.Length)
        {
            if (textLine[position] == '<')
            {
                int startPosition = position;
                int endPosition = -1;
                for (int searchPos = position + 1; searchPos < textLine.Length; searchPos++)
                {
                    if (textLine[searchPos] == '>')
                    {
                        endPosition = searchPos;
                        break;
                    }
                }
                if (endPosition > startPosition)
                {
                    string potentialTag = textLine.Substring(startPosition, endPosition - startPosition + 1);
                    if (ValidateTagStructure(potentialTag))
                    {
                        string normalizedTag = GetNormalizedTag(potentialTag);
                        bool tagIsNew = true;
                        for (int checkIndex = 0; checkIndex < records.Size; checkIndex++)
                        {
                            if (records.Get(checkIndex).NormalizedValue == normalizedTag)
                            {
                                tagIsNew = false;
                                break;
                            }
                        }
                        if (tagIsNew)
                        {
                            records.Add(new TagRecord
                            {
                                OriginalTag = potentialTag,
                                NormalizedValue = normalizedTag
                            });
                            resultTags.Add(potentialTag);
                        }
                    }
                    position = endPosition;
                }
            }
            position++;
        }
    }

    static bool ValidateTagStructure(string tagCandidate)
    {
        if (tagCandidate.Length < 3)
            return false;
        if (tagCandidate[0] != '<' || tagCandidate[tagCandidate.Length - 1] != '>')
            return false;
        int contentStartIndex = 1;
        if (tagCandidate[1] == '/')
        {
            contentStartIndex = 2;
            if (tagCandidate.Length < 4)
                return false;
        }
        if (!char.IsLetter(tagCandidate[contentStartIndex]))
            return false;
        for (int charIndex = contentStartIndex + 1; charIndex < tagCandidate.Length - 1; charIndex++)
        {
            if (!char.IsLetterOrDigit(tagCandidate[charIndex]))
            {
                return false;
            }
        }
        return true;
    }

    static string GetNormalizedTag(string tag)
    {
        string tagContent = tag.Substring(1, tag.Length - 2);
        if (tagContent.Length > 0 && tagContent[0] == '/')
        {
            tagContent = tagContent.Substring(1);
        }
        return tagContent.ToLower();
    }

    static void SaveTagsToFile(string filename, MyArrayList<string> tagsCollection)
    {
        using (StreamWriter fileWriter = new StreamWriter(filename, false, Encoding.UTF8))
        {
            fileWriter.WriteLine("УНИКАЛЬНЫЕ ТЕГИ ИЗ ФАЙЛА input.txt");
            fileWriter.WriteLine("===================================");
            fileWriter.WriteLine($"Количество тегов: {tagsCollection.Size}");
            fileWriter.WriteLine();
            for (int tagIndex = 0; tagIndex < tagsCollection.Size; tagIndex++)
            {
                fileWriter.WriteLine(tagsCollection.Get(tagIndex));
            }
            fileWriter.WriteLine();
            fileWriter.WriteLine("===================================");
            fileWriter.WriteLine($"Обработка завершена: {DateTime.Now}");
        }
    }
}