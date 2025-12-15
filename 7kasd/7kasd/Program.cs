using System;
using System.Collections.Generic;
using System.IO;

public class Request
{
    public int Priority { get; set; }
    public int Number { get; set; }
    public int StepAdded { get; set; }
    public int StepRemoved { get; set; } = -1;

    public int WaitingTime
    {
        get
        {
            if (StepRemoved == -1) return 0;
            return StepRemoved - StepAdded;
        }
    }
}

public class MyPriorityQueue
{
    private List<Request> requests = new List<Request>();

    public void Enqueue(Request request)
    {
        requests.Add(request);
    }

    public Request Dequeue()
    {
        if (requests.Count == 0)
            return null;
        int maxIndex = 0;
        for (int i = 1; i < requests.Count; i++)
        {
            if (requests[i].Priority > requests[maxIndex].Priority)
            {
                maxIndex = i;
            }
        }
        Request result = requests[maxIndex];
        requests.RemoveAt(maxIndex);
        return result;
    }
    public bool IsEmpty()
    {
        return requests.Count == 0;
    }
    public int Count()
    {
        return requests.Count;
    }
}

public class RequestProcessor
{
    private MyPriorityQueue queue = new MyPriorityQueue();
    private List<Request> allRequests = new List<Request>();
    private Random random = new Random();
    private StreamWriter logFile;
    private int maxWaitingTime = 0;
    private Request maxWaitingRequest = null;
    private int requestCounter = 1;
    public RequestProcessor(string logFileName)
    {
        logFile = new StreamWriter(logFileName);
    }

    public void ProcessSteps(int N)
    {
        for (int step = 1; step <= N || !queue.IsEmpty(); step++)
        {
            Console.WriteLine($"\n=== Шаг {step} ===");
            if (step <= N)
            {
                AddRequests(step);
            }
            if (!queue.IsEmpty())
            {
                RemoveRequest(step);
            }
            else if (step > N)
            {
                Console.WriteLine("Очередь пуста!");
                break;
            }
            Console.WriteLine($"Заявок в очереди: {queue.Count()}");
        }
    }

    private void AddRequests(int step)
    {
        int requestsToAdd = random.Next(1, 11);
        Console.WriteLine($"Добавляем {requestsToAdd} заявок");
        for (int i = 0; i < requestsToAdd; i++)
        {
            Request newRequest = new Request
            {
                Number = requestCounter,
                Priority = random.Next(1, 6),
                StepAdded = step
            };
            queue.Enqueue(newRequest);
            allRequests.Add(newRequest);
            LogOperation("ADD", newRequest, step);
            Console.WriteLine($"  Добавлена заявка {requestCounter} с приоритетом {newRequest.Priority}");
            requestCounter++;
        }
    }

    private void RemoveRequest(int step)
    {
        Request removedRequest = queue.Dequeue();
        removedRequest.StepRemoved = step;

        UpdateMaxWaitingTime(removedRequest);

        LogOperation("REMOVE", removedRequest, step);
        Console.WriteLine($"  Удалена заявка {removedRequest.Number} с приоритетом {removedRequest.Priority}");
        Console.WriteLine($"  Время ожидания: {removedRequest.WaitingTime} шагов");
    }

    private void UpdateMaxWaitingTime(Request request)
    {
        if (request.WaitingTime > maxWaitingTime)
        {
            maxWaitingTime = request.WaitingTime;
            maxWaitingRequest = request;
        }
    }

    private void LogOperation(string operation, Request request, int step)
    {
        logFile.WriteLine($"{operation} {request.Number} {request.Priority} {step}");
    }

    public void PrintResults()
    {
        Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ===");
        Console.WriteLine($"Всего заявок: {allRequests.Count}");
        if (maxWaitingRequest != null)
        {
            Console.WriteLine("\nЗаявка с максимальным временем ожидания:");
            Console.WriteLine($"Номер заявки: {maxWaitingRequest.Number}");
            Console.WriteLine($"Приоритет: {maxWaitingRequest.Priority}");
            Console.WriteLine($"Шаг добавления: {maxWaitingRequest.StepAdded}");
            Console.WriteLine($"Шаг удаления: {maxWaitingRequest.StepRemoved}");
            Console.WriteLine($"Время ожидания: {maxWaitingTime} шагов");
        }
        else
        {
            Console.WriteLine("Заявок не было!");
        }
    }

    public void Close()
    {
        logFile.Close();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите количество шагов добавления заявок (N): ");
        int N = int.Parse(Console.ReadLine());
        RequestProcessor processor = new RequestProcessor("log.txt");
        try
        {
            processor.ProcessSteps(N);
            processor.PrintResults();
        }
        finally
        {
            processor.Close();
        }

        Console.WriteLine("\nЛоги сохранены в файл log.txt");
        Console.ReadKey();
    }
}