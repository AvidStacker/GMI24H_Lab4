using System.Text.RegularExpressions;

namespace HashTableChaining
{
    /// <summary>
    /// Represents a student with a unique ID, name, email, and major.
    /// Used as a key in hash table implementations.
    /// </summary>
    public class Student
    {
        private string _name;
        private readonly int _id;
        private string _email;
        private string _major;
        private static int _lastUsedId = 0;

        /// <summary>
        /// Represents a student with a unique ID, name, email, and major.
        /// Used as a key in hash table implementations.
        /// </summary>
        public Student(string name, string email, string major)
        {
            this._name = name;
            this._email = email;
            this._major = major;
            this._id = GenerateUniqueId();
        }

        /// <summary>
        /// Generates a unique, auto-incremented ID for each student instance.
        /// </summary>
        private static int GenerateUniqueId()
        {
            _lastUsedId++;
            return _lastUsedId;
        }

        /// <summary>
        /// Gets or sets the name of the student.
        /// Throws an exception if the value is null, empty, or too short.
        /// </summary>
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

        /// <summary>
        /// Gets the unique identifier of the student.
        /// </summary>
        public int Id => this._id;

        /// <summary>
        /// Gets or sets the student's email.
        /// Throws an exception if the email format is invalid.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the student's major.
        /// Throws an exception if the major is null, empty, or too short.
        /// </summary>
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

        /// <summary>
        /// Returns the string representation of the student.
        /// Used for hashing (e.g., in hash table indexing).
        /// </summary>
        public override string ToString()
        {
            return this._id.ToString();  // Return Id as the string representation
        }

        /// <summary>
        /// Compares two Student objects based on their ID.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Student other = (Student)obj;
            return this.Id == other.Id;
        }

        /// <summary>
        /// Returns a hash code based on the student's ID.
        /// Ensures compatibility with hash-based collections.
        /// </summary>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
