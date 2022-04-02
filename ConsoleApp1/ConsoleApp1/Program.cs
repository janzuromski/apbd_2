using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        public const string LoggerPath = "Data/log.txt";
        private HashSet<Student> Students;

        public Program()
        {
            Students = new(new StudentComparer());
        }

        public async Task ParseStudents(string path)
        {
            StreamWriter logger = new(LoggerPath, append: true);
            using StreamReader stream = new(path);

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
        }

        public void ShowStudents()
        {
            foreach (var student in Students)
            {
                Console.WriteLine(student);
            }
        }
        
        public static async Task Main(string[] args)
        {
            Program program = new();
            await program.ParseStudents(args[0]);
            program.ShowStudents();
            // DateTime - typ dla daty: metody Parse() i TryParse()
            // DateTime.Parse("2022-03-20")

            // string.isNullOrWhiteSpace(str)

            // studenci bedą przechowywani w HashSet (należy napisać comparator studentów)

            // zapisywanie do pliku: StreamWriter stream... stream.WriteLine(line)

            // serializacja do JSON: var json = JsonSerializer.Serializer(set)

        }

    }

    class Student
    {
        public string Name, Surname, Faculty, CourseType, Email, FatherName, MotherName;
        public int IndexNumer;
        public DateTime StudiesStart;

        private Student(string name, string surname, string faculty, string courseType, string email,
            string fatherName, string motherName, int indexNumer, DateTime studiesStart)
        {
            Name = name;
            Surname = surname;
            Faculty = faculty;
            CourseType = courseType;
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
            DateTime studiesStart = DateTime.Parse(entrysplit[counter++]);
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
                $"Student s{IndexNumer}: {Name} {Surname}, {Faculty}, {CourseType}," +
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
