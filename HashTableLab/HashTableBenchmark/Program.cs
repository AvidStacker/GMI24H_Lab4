using System;
using System.Collections.Generic;
using System.Diagnostics;
using HashTableChaining;
using HashTableOpenAddressing;

class Program
{
    /// <summary>
    /// Number of student entries to insert, lookup, and remove in each benchmark
    /// </summary>
    const int NumItems = 1000;

    static void Main()
    {
        Console.WriteLine("Benchmarking Hash Tables...");
        // Benchmark: Chaining with arrays (array of arrays)
        Benchmark("Chaining with ArrayHashTable", new ArrayHashTable<Student, int>());

        // Benchmark: Chaining with lists (List<T> as buckets)
        Benchmark("Chaining with ListHashTable", new ListHashTable<Student, int>());

        // Benchmark: Chaining with linked lists (custom Node class)
        Benchmark("Chaining with LinkedListHashTable", new LinkedListHashTable<Student, int>());

        // Benchmark: Linear probing hash table using Djb2 hash function
        Benchmark("Linear Probing with ArrayHashTable", new LinearProbingArrayHashTable<Student, int>(hashFunction: HashFunctions.Djb2Hash));

        // Benchmark: Quadratic probing hash table using SimpleMurmurHash
        Benchmark("Quadratic Probing with ArrayHashTable", new QuadraticProbingArrayHashTable<Student, int>(hashFunction: HashFunctions.SimpleMurmurHash));
    }

    /// <summary>
    /// Benchmarks the performance of a hash table implementation by measuring time taken
    /// for insertion, lookup, and removal of a specified number of Student keys.
    /// </summary>
    /// <param name="name">A label for the hash table being benchmarked.</param>
    /// <param name="table">The hash table instance implementing IHashTable.</param>
    static void Benchmark(string name, IHashTable<Student, int> table)
    {
        Console.WriteLine($"\n{name}");

        // Generate a list of unique student keys
        var students = GenerateKeys(NumItems);

        // Measure the time it takes to insert all students into the hash table
        var insertWatch = Stopwatch.StartNew();
        for (int i = 0; i < NumItems; i++)
        {
            table.Add(students[i], i);
        }
        insertWatch.Stop();
        Console.WriteLine($"Insertion Time: {insertWatch.ElapsedMilliseconds} ms");

        // Measure the time it takes to check and retrieve each key from the table
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

        // Measure the time it takes to remove each key from the table
        var removeWatch = Stopwatch.StartNew();
        for (int i = 0; i < NumItems; i++)
        {
            if (table.ContainsKey(students[i]))
            {
                table.Remove(students[i]); // Deletion operation
            }
            else
            {
                Console.WriteLine($"Key for student {students[i].Id} not found.");
            }
        }
        removeWatch.Stop();
        Console.WriteLine($"Removal Time: {removeWatch.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// Generates an array of Student objects with unique names, emails, and IDs.
    /// These are used as keys in the benchmarking of hash table implementations.
    /// </summary>
    /// <param name="count">The number of students to generate.</param>
    /// <returns>An array of Student objects.</returns>
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