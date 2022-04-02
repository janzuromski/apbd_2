using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    public class Program
    {
        public const string LoggerPath = "Data/log.txt";
        public static async Task Main(string[] args)
        {
            University university = new University("Jan Żuromski");
            await university.ParseStudents(args[0]);
            await university.SaveStudents(args[1], args[2]);
        }

    }

    class University
    {
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public HashSet<Student> Students { get; set; }

        public University(string author)
        {
            Students = new HashSet<Student>(new StudentComparer());
            Author = author;
            CreatedAt = DateTime.Today;
        }
        
        public async Task ParseStudents(string path)
        {
            StreamWriter logger = new(Program.LoggerPath, append: true);
            try
            {
                StreamReader stream = new(path);
                string line;

                while ((line = await stream.ReadLineAsync()) != null)
                {
                    Student s = Student.CreateStudent(line);
                    if (s != null)
                    {
                        if (!Students.Add(Student.CreateStudent(line)))
                        {
                            await logger.WriteLineAsync($"({DateTime.Now} | Program) Duplicate Warning: {s}");
                        }
                    }
                }
                stream.Close();
            }
            catch (FileNotFoundException e)
            {
                await logger.WriteLineAsync($"({DateTime.Now} | ParseStudents) File does not exist: {path}");
                logger.Close();
            }

        }

        public async Task SaveStudents(string path, string format)
        {
            var json = JsonSerializer.Serialize(
                this,
                new JsonSerializerOptions 
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }
            );
            try
            {
                if (!path.Contains("json"))
                {
                    throw new ArgumentException();
                }
                StreamWriter writer = new(path);
                await writer.WriteAsync(json);
                writer.Close();
            }
            catch (ArgumentException e)
            {
                StreamWriter logger = new(Program.LoggerPath, append: true);
                await logger.WriteLineAsync($"({DateTime.Now} | SaveStudents) File Path is Invalid: {path}");
                logger.Close();
            }
        }
    }

    class Student
    {
        internal class StudiesDescription
        {
            public string Name { get; set; }
            public string Mode { get; set; }

            public StudiesDescription(string name, string mode)
            {
                this.Name = name;
                this.Mode = mode;
            }
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public StudiesDescription Studies { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int IndexNumer { get; set; }
        public DateTime StudiesStart { get; set; }

        private Student(string name, string surname, string faculty, string courseType, string email,
            string fatherName, string motherName, int indexNumer, DateTime studiesStart)
        {
            Name = name;
            Surname = surname;
            Studies = new(faculty, courseType);
            Email = email;
            FatherName = fatherName;
            MotherName = motherName;
            IndexNumer = indexNumer;
            StudiesStart = studiesStart;
        }

        public static Student CreateStudent(string entry)
        {
            StreamWriter logger = new(Program.LoggerPath, append: true);
            string[] entrysplit = entry.Split(",");
            if (entrysplit.Length != 9)
            {
                logger.WriteLine($"({DateTime.Now} | Student) Not Enough Data: {entry}");
                logger.Close();
                return null;
            }
            for (int i = 0; i < entrysplit.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(entrysplit[i]))
                {
                    logger.WriteLine($"({DateTime.Now} | Student) Empty Values: {entry}");
                    logger.Close();
                    return null;
                }
            }

            int counter = 0;
            string name = entrysplit[counter++];
            string surname = entrysplit[counter++];
            string faculty = entrysplit[counter++];
            string courseType = entrysplit[counter++];
            int indexNumber = int.Parse(entrysplit[counter++]);
            DateTime studiesStart = DateTime.Parse(
                entrysplit[counter++], 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.RoundtripKind);
            string email = entrysplit[counter++];
            string fatherName = entrysplit[counter++];
            string motherName = entrysplit[counter];
            
            logger.Close();

            return new Student(name, surname, faculty, courseType, email, fatherName, motherName, indexNumber,
                studiesStart);
        }

        public override string ToString()
        {
            return
                $"Student s{IndexNumer}: {Name} {Surname}, {Studies.Name}, {Studies.Mode}," +
                $"{StudiesStart}, {Email}, {FatherName}, {MotherName}";
        }
    }

    class StudentComparer : EqualityComparer<Student>
    { 
        public override bool Equals(Student x, Student y)
        {
            return x != null && y != null && x.IndexNumer.Equals(y.IndexNumer) & x.Name.Equals(y.Name) &
                x.Surname.Equals(y.Surname);
        }

        public override int GetHashCode(Student s)
        {
            return s.IndexNumer * 17 + s.Name.GetHashCode() + s.Surname.GetHashCode();
        }
    }
}
