using System;
using System.Collections.Generic;
using System.Diagnostics;
using HashTableChaining;
using HashTableOpenAddressing;

class Program
{
    const int NumItems = 1000;

    static void Main()
    {
        Console.WriteLine("Benchmarking Hash Tables...");

        // You can benchmark more hash tables with custom hash function
        Benchmark("Chaining", new ArrayHashTable<Student, int>());

        // You can benchmark more hash tables with custom hash functions if needed
        Benchmark("Chaining", new ListHashTable<Student, int>());

        // You can benchmark more hash tables with custom hash functions if needed
        Benchmark("Chaining", new LinkedListHashTable<Student, int>());

        // Benchmark Linear Probing with a custom Djb2Hash
        Benchmark("Linear Probing (Custom Hash)", new LinearProbingArrayHashTable<Student, int>(hashFunction: HashFunctions.Djb2Hash));

        // Benchmark Quadratic Probing with custom SimpleMurmurHash
        Benchmark("Quadratic Probing (Default Hash)", new QuadraticProbingArrayHashTable<Student, int>(hashFunction: HashFunctions.SimpleMurmurHash));
    }

    static void Benchmark(string name, IHashTable<Student, int> table)
    {
        Console.WriteLine($"\n{name}");

        var students = GenerateKeys(NumItems);

        // Insertion Benchmark
        var insertWatch = Stopwatch.StartNew();
        for (int i = 0; i < NumItems; i++)
        {
            table.Add(students[i], i);
        }
        insertWatch.Stop();
        Console.WriteLine($"Insertion Time: {insertWatch.ElapsedMilliseconds} ms");

        // Lookup Benchmark
        var lookupWatch = Stopwatch.StartNew();
        for (int i = 0; i < NumItems; i++)
        {
            if (table.ContainsKey(students[i]))
            {
                table.Get(students[i]);
            }
            else
            {
                Console.WriteLine($"Key for student {students[i].Id} not found.");
            }
        }
        lookupWatch.Stop();
        Console.WriteLine($"Lookup Time: {lookupWatch.ElapsedMilliseconds} ms");

        // Removal Benchmark
        var removeWatch = Stopwatch.StartNew();
        for (int i = 0; i < NumItems; i++)
        {
            if (table.ContainsKey(students[i]))
            {
                table.Remove(students[i]);
            }
            else
            {
                Console.WriteLine($"Key for student {students[i].Id} not found.");
            }
        }
        removeWatch.Stop();
        Console.WriteLine($"Removal Time: {removeWatch.ElapsedMilliseconds} ms");
    }

    static Student[] GenerateKeys(int count)
    {
        var students = new Student[count];
        for (int i = 0; i < count; i++)
        {
            students[i] = new Student($"Student {i}", $"student{i}@university.com", "Computer Science");
        }
        return students;
    }
}
