using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTablexUnitTests
{
    public class BadInputTests
    {
        [Fact]
        public void AddStudent_NoCheckIfActuallyAdded()
        {
            var table = new LinkedListHashTable();
            table.Add("badkey", new Student("Bad", "Test", "badkey"));
            // Inget assert – testet säger inget om vad som hände!
        }

        [Fact]
        public void RemoveStudent_ButForgetToAddFirst()
        {
            var table = new LinkedListHashTable();
            table.Remove("nonexisting");
            // Ingen koll om något faktiskt tas bort eller om Remove kraschar.
        }

        [Fact]
        public void GetStudent_WithoutAddingAnything()
        {
            var table = new ListHashTable();
            var result = table.Get("randomkey");
            // Ingen Assert, testet testar inget.
        }

        [Fact]
        public void AddTwoStudents_WithoutTestingCollision()
        {
            var table = new LinkedListHashTable();
            table.Add("keyA", new Student("A", "Student", "keyA"));
            table.Add("keyB", new Student("B", "Student", "keyB"));
            // Testar inte om de hamnade i samma bucket eller inte (missar collision-test).
        }

        [Fact]
        public void JustCallComputeHash_DoNothingElse()
        {
            var table = new ArrayHashTable();
            table.ComputeHash("key123");
            // Ingen koll på om hash-värdet är korrekt.
        }
    }
}
