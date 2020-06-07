using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace W1
{
    public interface IWrite
    {
        public abstract void ListenersCount(List<Concert> writer);
        public abstract void DayWithLow(List<Concert> writer);
    }

    public abstract class Writer
    {
        public string SurName { get; set; }
        public string NumOfBooks { get; set; }
        public string Language { get; set; }

        public abstract void ListenersCount(List<Concert> writer);
        public abstract void DayWithLow(List<Concert> writer);
    }
    public class Concert : Writer, IWrite
    {
        public string Date { get; set; }
        public string Place { get; set; }
        public string Listeners { get; set; }

        public override void ListenersCount(List<Concert> writer)
        {
            Console.Clear();
            var Group = from ls in writer
                        group ls by ls.Listeners into g
                        select new
                        {
                            Name = g.Key,
                            Count = g.Count(),
                            Works = from p in g select p
                        };
            float list = 0;
            foreach (var group in Group)
            {

                foreach (var gr in group.Works)
                {
                    list += Convert.ToInt32(gr.Listeners);
                }

                Console.WriteLine();
            }
            Console.WriteLine($"{list} listeners");
        }
        public override void DayWithLow(List<Concert> writer)
        {
            Console.Clear();
            var Group = from low in writer
                        group low by low.Date into g
                        select new
                        {
                            Name = g.Key,
                            Count = g.Count(),
                            Works = from p in g select p
                        };
            foreach (var group in Group)
            {
                float ls = 0;
                foreach (var gr in group.Works)
                {
                    ls += Convert.ToInt32(gr.Listeners);
                }
                Console.WriteLine($"{group.Name} : {ls} listeners");
                Console.WriteLine();
            }
        }
    }
    class Program
    {
        private static string FileName = "Data.json";
        private static string FilePath = @"Data.json";
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("╔════════════╤══════════════════════════════╗");
                Console.WriteLine("   Hot key   │            Function       ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      A      │          Add new concert  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      C      │          Change concert  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      D      │          Delete concert ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      T      │        Show all concerts  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      H      │         All listeners  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      P      │        Lenth of surname");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      M      │ Day with the least listeners");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("    Space    │         Clear console  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("     Esc     │          Exit program  ");
                Console.WriteLine("╚════════════╧══════════════════════════════╝");
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Close();
                }
                var Writer = JsonConvert.DeserializeObject<List<Concert>>(File.ReadAllText(FilePath));
                Concert cp = new Concert();
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        if (Writer == null)
                        {
                            Writer = new List<Concert>();
                            Writer.Add(CreateNewDay());
                        }
                        else
                        {
                            Writer.Add(CreateNewDay());
                        }
                        break;
                    case ConsoleKey.C:
                        ChangeData(Writer);
                        break;
                    case ConsoleKey.D:
                        DelteDay(Writer);
                        break;
                    case ConsoleKey.T:
                        ShowAll(Writer);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.H:
                        cp.ListenersCount(Writer);
                        break;
                    case ConsoleKey.P:
                        SurnLen(Writer);
                        break;
                    case ConsoleKey.M:
                        cp.DayWithLow(Writer);
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        break;
                }
                string serialize = JsonConvert.SerializeObject(Writer, Formatting.Indented);
                if (serialize.Count() > 1)
                {
                    if (!File.Exists(FileName))
                    {
                        File.Create(FileName).Close();
                    }
                    File.WriteAllText(FilePath, serialize, Encoding.UTF8);
                }
            }
        }
        public static Concert CreateNewDay()
        {
            Console.Clear();
            Concert Day = new Concert();
            Console.WriteLine("Enter name of writer");
            Day.SurName = Console.ReadLine();
            Console.WriteLine("Enter num of books");
            Day.NumOfBooks = Console.ReadLine();
            Console.WriteLine("Enter date of day like 01.02.2000");
            Day.Date = Console.ReadLine();
            Console.WriteLine("Enter place");
            Day.Place = Console.ReadLine();
            Console.WriteLine("Enter count of listeners");
            Day.Listeners = Console.ReadLine();
            Console.WriteLine("Enter language");
            Day.Language = Console.ReadLine();
            return Day;
        }
        public static void ChangeData(List<Concert> Writer)
        {
            Console.WriteLine("Enter surname of writer that`s you want to change");
            var s = Console.ReadLine();
            Concert day = Writer.Find(x => x.SurName == s);
            if (day != null)
            {
                Console.WriteLine("Enter value of day that`s you want to change\n1)Surname\n2)Num of books\n3)Date like 01.02.2000\n4)PLace\n5)Listeners\n6)Language");
                char a = Console.ReadKey().KeyChar;
                Console.WriteLine("Enter new value");
                switch (a)
                {
                    case '1':
                        day.SurName = Console.ReadLine();
                        break;
                    case '2':
                        day.NumOfBooks = Console.ReadLine();
                        break;
                    case '3':
                        day.Date = Console.ReadLine();
                        break;
                    case '4':
                        day.Place = Console.ReadLine();
                        break;
                    case '5':
                        day.Listeners = Console.ReadLine();
                        break;
                    case '6':
                        day.Language = Console.ReadLine();
                        break;
                }
            }
        }
        public static void DelteDay(List<Concert> Writer)
        {
            if (Writer != null)
            {
                Console.WriteLine("Enter surname of writer that`s you want to delete");
                var s = Console.ReadLine();
                Writer.RemoveAll(x => x.SurName == s);

            }
        }
        public static void ShowAll(List<Concert> wr)
        {
            Console.Clear();
            Console.WriteLine("╔════════════╤═════════════╤══════════╤═════════════╤══════════════╪══════════════╗");
            Console.WriteLine("    Surname  │Count of book│   Date   │    PLace    │  Listeners   │  Language");
            Console.WriteLine("╠════════════╪═════════════╪══════════╪═════════════╪══════════════╪══════════════╣");
            for (int i = 0; i < wr.Count; i++)
            {
                Console.WriteLine("{0,10} {1, 10} {2, 16} {3, 10} {4, 13} {5, 12}", wr[i].SurName, wr[i].NumOfBooks, wr[i].Date, wr[i].Place, wr[i].Listeners, wr[i].Language);
                Console.WriteLine("╠════════════╪═════════════╪══════════╪═════════════╪══════════════╪══════════════╣");
            }
            Console.WriteLine("╚════════════╧═════════════╧══════════╧═════════════╧══════════════╪══════════════╝");
        }
        public static void SurnLen(List<Concert> writer)
        {
            for (int i = 0; i < writer.Count; i++)
            {
                int count = 0;
                for (int k = 0; k< writer[i].SurName.Length; k++)
                {
                    count++;
                }
                Console.WriteLine($"In {writer[i].SurName} : {count} letters");
            }
        }
        
    }
}
