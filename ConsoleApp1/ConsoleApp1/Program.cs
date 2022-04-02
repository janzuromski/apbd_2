using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        private HashSet<Student> Students;

        public Program()
        {
            Students = new(new StudentComparer());
        }

        public async Task ParseStudents(string path)
        {
            using StreamReader stream = new(path);

            string line = null;

            while ((line = await stream.ReadLineAsync()) != null)
            {
                Students.Add(new Student(line));
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
        public static string LoggerPath = "Data/log.txt";
        public StreamWriter Logger;
        

        public Student(string entry)
        {
            Logger = new(LoggerPath, append: true);
            string[] entrysplit = entry.Split(",");
            for (int i = 0; i < entrysplit.Length; i++)
            {
                if (entrysplit[i].Equals(""))
                {
                    Logger.Write($"({DateTime.Now})Faulty Entry: ");
                    Logger.Write(entry);
                    Logger.Write("\n");
                    break;
                }
            }

            int counter = 0;
            Name = entrysplit[counter++];
            Surname = entrysplit[counter++];
            Faculty = entrysplit[counter++];
            CourseType = entrysplit[counter++];
            IndexNumer = int.Parse(entrysplit[counter++]);
            StudiesStart = DateTime.Parse(entrysplit[counter++]);
            Email = entrysplit[counter++];
            FatherName = entrysplit[counter++];
            MotherName = entrysplit[counter];
            
            Logger.Close();
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
        public override bool Equals(Student? x, Student? y)
        {
            return x.IndexNumer.Equals(y.IndexNumer) & x.Name.Equals(y.Name) & x.Surname.Equals(y.Surname);
        }

        public override int GetHashCode(Student s)
        {
            return s.IndexNumer * 17 + s.Name.GetHashCode() + s.Surname.GetHashCode();
        }
    }
}
