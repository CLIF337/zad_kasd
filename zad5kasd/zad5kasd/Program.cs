using System;
using System.Collections.Generic;
public class MaxHeap<T> where T : IComparable<T>
{
    private List<T> elements;
    public int Count => elements.Count;
    public bool IsEmpty => elements.Count == 0;
    public MaxHeap()
    {
        elements = new List<T>();
    }
    public MaxHeap(IEnumerable<T> massiv)
    {
        if (massiv == null)
        {
            throw new ArgumentNullException(nameof(massiv), "Массив не может быть null");
        }
        elements = new List<T>(massiv);
        Heapify();
    }
    public T FindMax()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Куча пуста. Невозможно найти максимальный элемент.");
        }
        return elements[0];
    }
    public T RemoveMax()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Куча пуста. Невозможно извлечь максимальный элемент.");
        }
        T maxElement = elements[0];
        elements[0] = elements[Count - 1];
        elements.RemoveAt(Count - 1);
        if (!IsEmpty)
        {
            Sink(0);
        }
        return maxElement;
    }
    public void UpKey(int position, T newValue)
    {
        if (position < 0 || position >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Позиция находится за пределами кучи");
        }

        if (newValue.CompareTo(elements[position]) < 0)
        {
            throw new ArgumentException("Новое значение должно быть больше текущего", nameof(newValue));
        }
        elements[position] = newValue;
        Rise(position);
    }
    public void Insert(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "Нельзя добавить null в кучу");
        }
        elements.Add(item);
        Rise(Count - 1);
    }
    public MaxHeap<T> CombineWith(MaxHeap<T> otherHeap)
    {
        if (otherHeap == null)
        {
            throw new ArgumentNullException(nameof(otherHeap), "Вторая куча не может быть null");
        }

        List<T> combinedElements = new List<T>(Count + otherHeap.Count);
        combinedElements.AddRange(elements);
        combinedElements.AddRange(otherHeap.elements);

        return new MaxHeap<T>(combinedElements);
    }
    private void Rise(int position)
    {
        while (position > 0)
        {
            int parentPosition = (position - 1) / 2;

            if (elements[position].CompareTo(elements[parentPosition]) <= 0)
            {
                break;
            }

            Exchange(position, parentPosition);
            position = parentPosition;
        }
    }
    private void Sink(int position)
    {
        int currentPosition = position;
        int heapSize = Count;
        while (true)
        {
            int leftChild = 2 * currentPosition + 1;
            int rightChild = 2 * currentPosition + 2;
            int largestPosition = currentPosition;
            if (leftChild < heapSize && elements[leftChild].CompareTo(elements[largestPosition]) > 0)
            {
                largestPosition = leftChild;
            }
            if (rightChild < heapSize && elements[rightChild].CompareTo(elements[largestPosition]) > 0)
            {
                largestPosition = rightChild;
            }
            if (largestPosition == currentPosition)
            {
                break;
            }
            Exchange(currentPosition, largestPosition);
            currentPosition = largestPosition;
        }
    }
    private void Heapify()
    {
        for (int i = Count / 2 - 1; i >= 0; i--)
        {
            Sink(i);
        }
    }
    private void Exchange(int firstIndex, int secondIndex)
    {
        T temporary = elements[firstIndex];
        elements[firstIndex] = elements[secondIndex];
        elements[secondIndex] = temporary;
    }
    public void Display()
    {
        Console.WriteLine($"Куча (элементов: {Count}):");
        if (IsEmpty)
        {
            Console.WriteLine("  [пусто]");
            return;
        }

        Console.Write("  Элементы: [");
        for (int i = 0; i < Count; i++)
        {
            Console.Write(elements[i]);
            if (i < Count - 1) Console.Write(", ");
        }
        Console.WriteLine("]");

        Console.WriteLine("  Структура дерева:");
        DisplayTreeStructure();
    }
    private void DisplayTreeStructure()
    {
        if (IsEmpty) return;
        Queue<int> positionsQueue = new Queue<int>();
        positionsQueue.Enqueue(0);
        while (positionsQueue.Count > 0)
        {
            int levelCount = positionsQueue.Count;
            for (int i = 0; i < levelCount; i++)
            {
                int currentPosition = positionsQueue.Dequeue();
                Console.Write($"{elements[currentPosition]} ");
                int leftChildPosition = 2 * currentPosition + 1;
                if (leftChildPosition < Count)
                {
                    positionsQueue.Enqueue(leftChildPosition);
                }
                int rightChildPosition = 2 * currentPosition + 2;
                if (rightChildPosition < Count)
                {
                    positionsQueue.Enqueue(rightChildPosition);
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Демонстрация работы Max-кучи ===\n");
        Console.WriteLine("1. СОЗДАНИЕ КУЧИ ИЗ МАССИВА:");
        int[] initialData = { 3, 1, 4, 1, 5, 9, 2, 6 };
        MaxHeap<int> numberHeap = new MaxHeap<int>(initialData);
        numberHeap.Display();
        Console.WriteLine($"Максимальный элемент: {numberHeap.FindMax()}");

        Console.WriteLine("\n2. НАХОЖДЕНИЕ МАКСИМУМА:");
        Console.WriteLine($"Максимальный элемент (без удаления): {numberHeap.FindMax()}");
        numberHeap.Display();

        Console.WriteLine("\n3. УДАЛЕНИЕ МАКСИМАЛЬНОГО ЭЛЕМЕНТА:");
        int extractedMax = numberHeap.RemoveMax();
        Console.WriteLine($"Извлечённый элемент: {extractedMax}");
        numberHeap.Display();
        Console.WriteLine($"Новый максимум: {numberHeap.FindMax()}");

        Console.WriteLine("\n4. УВЕЛИЧЕНИЕ ЗНАЧЕНИЯ ЭЛЕМЕНТА:");
        Console.WriteLine("Увеличиваем элемент на позиции 2");
        numberHeap.UpKey(2, 15);
        numberHeap.Display();
        Console.WriteLine($"Максимум после увеличения: {numberHeap.FindMax()}");

        Console.WriteLine("\n5. ДОБАВЛЕНИЕ ЭЛЕМЕНТОВ:");
        numberHeap.Insert(10);
        Console.WriteLine("Добавлен элемент: 10");
        numberHeap.Insert(7);
        Console.WriteLine("Добавлен элемент: 7");
        numberHeap.Display();

        Console.WriteLine("\n6. СЛИЯНИЕ ДВУХ КУЧ:");
        MaxHeap<int> firstHeap = new MaxHeap<int>(new int[] { 2, 5, 6 });
        MaxHeap<int> secondHeap = new MaxHeap<int>(new int[] { 3, 15, 7 });

        Console.WriteLine("Первая куча:");
        firstHeap.Display();
        Console.WriteLine("Вторая куча:");
        secondHeap.Display();

        MaxHeap<int> mergedHeap = firstHeap.CombineWith(secondHeap);
        Console.WriteLine("Объединённая куча:");
        mergedHeap.Display();

        Console.WriteLine("7. ПОСЛЕДОВАТЕЛЬНОЕ ИЗВЛЕЧЕНИЕ ВСЕХ ЭЛЕМЕНТОВ:");
        Console.Write("Элементы в порядке убывания: ");
        while (!mergedHeap.IsEmpty)
        {
            Console.Write(mergedHeap.RemoveMax() + " ");
        }
        Console.WriteLine();

        Console.WriteLine("\n8. ПРОВЕРКА ОБРАБОТКИ ОШИБОК:");
        try
        {
            MaxHeap<int> emptyHeap = new MaxHeap<int>();
            emptyHeap.FindMax();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка при поиске максимума: {ex.Message}");
        }
        try
        {
            MaxHeap<int> testHeap = new MaxHeap<int>(new int[] { 5, 3, 8 });
            testHeap.UpKey(1, 1);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка при увеличении ключа: {ex.Message}");
        }
        Console.WriteLine("\n9. РАБОТА С ДРУГИМИ ТИПАМИ ДАННЫХ:");
        Console.WriteLine("Куча строк (сортировка по алфавиту в обратном порядке):");
        MaxHeap<string> stringHeap = new MaxHeap<string>(new string[] { "яблоко", "банан", "вишня", "груша" });
        stringHeap.Display();

        Console.WriteLine("Добавляем 'ананас':");
        stringHeap.Insert("ананас");
        stringHeap.Display();
    }
}