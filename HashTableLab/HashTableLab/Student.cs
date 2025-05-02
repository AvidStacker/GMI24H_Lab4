using System.Text.RegularExpressions;

namespace HashTableChaining
{
    public class Student
    {
        private string _name;
        private readonly int _id;
        private string _email;
        private string _major;
        private static int _lastUsedId = 0;

        public Student(string name, string email, string major)
        {
            this._name = name;
            this._email = email;
            this._major = major;
            this._id = GenerateUniqueId();
        }

        private static int GenerateUniqueId()
        {
            _lastUsedId++;
            return _lastUsedId;
        }

        public string Name
        {
            get { return this._name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be empty or null.");
                }
                if (value.Length < 2)
                {
                    throw new ArgumentException("Name must be at least 2 characters long.");
                }
                this._name = value;
            }
        }

        public int Id => this._id;

        public string Email
        {
            get { return this._email; }
            set
            {
                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    throw new ArgumentException("Invalid email address.");
                }
                this._email = value;
            }
        }

        public string Major
        {
            get { return this._major; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Major cannot be empty or null.");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("Major must be at least 3 characters long.");
                }
                this._major = value;
            }
        }

        // Override ToString() to return the Id as a string for hashing
        public override string ToString()
        {
            return this._id.ToString();  // Return Id as the string representation
        }

        // Override Equals and GetHashCode for proper comparison in hash tables
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Student other = (Student)obj;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();  // Use the Id as the hash code
        }
    }
}
