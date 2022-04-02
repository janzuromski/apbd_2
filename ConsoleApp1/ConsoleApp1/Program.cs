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
        
        public static async Task Main(string[] args)
        {
            var path = args[0];
            // var fi = new FileInfo(path);
            FileInfo fi = new(path);

            var fileContent = new List<string>();
            using (StreamReader stream = new(fi.OpenRead()))
            {
                // Analogicznie do ""
                //string line = string.Empty;
                string line = null;

                while ((line = await stream.ReadLineAsync()) != null)
                {
                    fileContent.Add(line);
                }

            }

            foreach (var item in fileContent)
            {
                Console.WriteLine(item);
            }

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
                    Logger.Write(entry);
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
        
    }

    class StudentComparer : EqualityComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            // INDEX NUMBER
            if (x.IndexNumer.CompareTo(y.IndexNumer) != 0)
            {
                return x.IndexNumer.CompareTo(y.IndexNumer);
            }

            // NAME
            if (string.CompareOrdinal(x.Name, y.Name) != 0)
            {
                return string.CompareOrdinal(x.Name, y.Name);
            }

            // SURNAME
            if (string.CompareOrdinal(x.Surname, y.Surname) != 0)
            {
                return string.CompareOrdinal(x.Surname, y.Surname);
            }

            // FACULTY
            if (string.CompareOrdinal(x.Faculty, y.Faculty) != 0)
            {
                return string.CompareOrdinal(x.Faculty, y.Faculty);
            }

            // COURSE TYPE
            if (string.CompareOrdinal(x.CourseType, y.CourseType) != 0)
            {
                return string.CompareOrdinal(x.CourseType, y.CourseType);
            }
            // STUDIES START
            if (x.StudiesStart.CompareTo(y.StudiesStart) != 0)
            {
                return x.StudiesStart.CompareTo(y.StudiesStart);
            }
            // EMAIL
            if (string.CompareOrdinal(x.Email, y.Email) != 0)
            {
                return string.CompareOrdinal(x.Email, y.Email);
            }
            // FATHER NAME
            if (string.CompareOrdinal(x.FatherName, y.FatherName) != 0)
            {
                return string.CompareOrdinal(x.FatherName, y.FatherName);
            }

            return string.CompareOrdinal(x.MotherName, y.MotherName);
        }

        public override bool Equals(Student? x, Student? y)
        {
            return x.IndexNumer.Equals(y.IndexNumer) & x.Name.Equals(y.Name) & x.Surname.Equals(y.Surname) &
                   x.Faculty.Equals(y.Faculty) & x.StudiesStart.Equals(y.StudiesStart) & x.Email.Equals(y.Email) &
                   x.FatherName.Equals(y.FatherName) & x.MotherName.Equals(y.MotherName);
        }

        public override int GetHashCode(Student s)
        {
            return s.IndexNumer * 17 + s.Name.GetHashCode() + s.Surname.GetHashCode() + s.Email.GetHashCode() +
                   s.Faculty.GetHashCode() + s.CourseType.GetHashCode() + s.StudiesStart.GetHashCode() +
                   s.FatherName.GetHashCode() + s.MotherName.GetHashCode();
        }
    }
}
