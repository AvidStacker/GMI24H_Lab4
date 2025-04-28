namespace HashTablexUnitTests
{
    public class GoodInputTests
    {
        [Fact]
        public void AddAndGet_ShouldReturnCorrectStudent()
        {
            var table = new LinkedListHashTable();
            var student = new Student("Anna", "Smith", "h001");
            table.Add(student.ID, student);

            var result = table.Get(student.ID);

            Assert.NotNull(result);
            Assert.Equal(student.FirstName, ((Student)result).FirstName);
        }

        [Fact]
        public void Remove_ShouldDeleteStudent()
        {
            var table = new LinkedListHashTable();
            var student = new Student("Brian", "Johnson", "h002");
            table.Add(student.ID, student);
            table.Remove(student.ID);

            var result = table.Get(student.ID);

            Assert.Null(result);
        }

        [Fact]
        public void Add_TwoKeysSameBucket_Chaining_ShouldStoreBoth()
        {
            var table = new LinkedListHashTable();
            table.Add("abc", new Student("Alice", "Brown", "abc"));
            table.Add("acb", new Student("Bob", "White", "acb")); // Kollision!

            Assert.NotNull(table.Get("abc"));
            Assert.NotNull(table.Get("acb"));
        }

        [Fact]
        public void AddManyStudents_ShouldTriggerResize_AndStillBeAccessible()
        {
            var table = new ListHashTable();
            for (int i = 0; i < 100; i++)
            {
                table.Add("id" + i, new Student("Name" + i, "Surname", "id" + i));
            }

            for (int i = 0; i < 100; i++)
            {
                var result = table.Get("id" + i);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void OpenAddressing_LinearProbing_ShouldHandleCollision()
        {
            var table = new LinearProbingHashTable();
            table.Add("key1", new Student("First", "User", "key1"));
            table.Add("key2", new Student("Second", "User", "key2")); // Samma hash?

            Assert.NotNull(table.Get("key1"));
            Assert.NotNull(table.Get("key2"));
        }

        [Fact]
        public void HashFunction_SameKey_ShouldReturnSameHash()
        {
            var table = new ArrayHashTable();
            int hash1 = table.ComputeHash("samekey");
            int hash2 = table.ComputeHash("samekey");

            Assert.Equal(hash1, hash2);
        }
    }
}