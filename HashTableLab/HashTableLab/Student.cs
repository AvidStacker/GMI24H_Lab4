using System;
using System.Text.RegularExpressions;

namespace HashTableChaining
{
    internal class Student
    {
        // Privat fält för att lagra studentdata
        private string _name;
        private readonly int _id;
        private string _email;
        private string _major;

        // Statisk variabel för att hålla reda på senaste använt Id
        private static int _lastUsedId = 0;

        // Konstruktör som sätter de initiala värdena och genererar ett unikt Id
        public Student(string name, string email, string major)
        {
            // Använd this för att referera till instansvariabler
            this._name = name;
            this._email = email;
            this._major = major;
            this._id = GenerateUniqueId(); // Generera ett unikt Id när studenten skapas
        }

        // Statisk metod för att generera ett unikt Id
        private static int GenerateUniqueId()
        {
            // Öka det senaste använda Id med 1 och returnera det nya Id
            _lastUsedId++;
            return _lastUsedId;
        }

        // Publika egenskaper som ger kontrollerad åtkomst till de privata fälten
        public string Name
        {
            get { return this._name; }
            set
            {
                // Validering av Namn: inte tomt eller null
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Namn kan inte vara tomt eller null.");
                }
                // Extra validering: Namn bör vara minst 2 tecken långt
                if (value.Length < 2)
                {
                    throw new ArgumentException("Namn måste vara minst 2 tecken långt.");
                }
                this._name = value;
            }
        }

        public int Id
        {
            get { return this._id; }
        }

        public string Email
        {
            get { return this._email; }
            set
            {
                // Validering för e-postformat: kontrollera om det matchar ett grundläggande e-postmönster
                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    throw new ArgumentException("Ogiltig e-postadress.");
                }
                this._email = value;
            }
        }

        public string Major
        {
            get { return this._major; }
            set
            {
                // Validering av huvudämne: inte tomt eller null
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Huvudämnet kan inte vara tomt eller null.");
                }
                // Extra validering: Huvudämnet bör vara minst 3 tecken långt
                if (value.Length < 3)
                {
                    throw new ArgumentException("Huvudämnet måste vara minst 3 tecken långt.");
                }
                this._major = value;
            }
        }

        // Överskrivning av Equals-metoden
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Student other = (Student)obj;
            return this.Id == other.Id; // Betrakta studenter som lika om deras Id är samma.
        }

        // Överskrivning av GetHashCode-metoden
        public override int GetHashCode()
        {
            return this.Id.GetHashCode(); // Använder Id som den unika identifieraren för hashing.
        }
    }
}
