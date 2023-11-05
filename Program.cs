﻿using System.Data;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void Main(string[] args)
        {
            bool on = true;
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            PrintHelpMessage();
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split();
                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    on = false;
                }
                else if (command == "load")
                {
                    try 
                    {
                        if (argument.Length == 2)
                        {
                            using (StreamReader sr = new StreamReader(argument[1]))
                                addGloss(sr);
                        }
                        else if (argument.Length == 1)
                        {
                            using (StreamReader sr = new StreamReader(defaultFile))
                                addGloss(sr);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"hittade inte filen {argument[1]}");
                    }
                }
                else if (command == "list")
                {
                    foreach(SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                    }
                }
                else if (command == "new")
                {
                    if (argument.Length == 3)
                    {
                        dictionary.Add(new SweEngGloss(argument[1], argument[2]));
                    } //FIXME: Crashar
                    else if(argument.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string swe = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string eng = Console.ReadLine();
                        dictionary.Add(new SweEngGloss(swe, eng));
                    }
                }
                //FIXME: Fel om "list" om det inte finns något i listan
                else if (command == "delete")
                {
                    if (argument.Length == 3)
                    {
                        removeIndex(argument[1], argument[2]);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string swe = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string eng = Console.ReadLine();
                        removeIndex(swe, eng);
                    }
                    //FIXME:"delete" error om man tar bort något som inte finns
                }
                else if (command == "translate")    //FIXME: error om "translate" något som inte finns med i listan
                {
                    if (argument.Length == 2)
                    {
                        glossWord(argument[1]);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string swe = Console.ReadLine();
                        glossWord(swe);
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            //TODO: Ändra så quit stänger programmet
            while (on == true);
        }

        private static void glossWord(string swe)
        {
            foreach (SweEngGloss gloss in dictionary)
            {
                if (gloss.word_swe == swe)
                    Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                if (gloss.word_eng == swe)
                    Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
            }
        }

        private static void removeIndex(string swe, string eng)
        {
            int index = -1;
            for (int i = 0; i < dictionary.Count; i++)
            {
                SweEngGloss gloss = dictionary[i];
                if (gloss.word_swe == swe && gloss.word_eng == eng)
                    index = i;
            }
            dictionary.RemoveAt(index);
        }

        private static void addGloss(StreamReader sr)
        {
            dictionary = new List<SweEngGloss>(); // Empty it!
            string line = sr.ReadLine();
            while (line != null)
            {
                SweEngGloss gloss = new SweEngGloss(line);
                dictionary.Add(gloss);
                line = sr.ReadLine();
            }   //FIXME: Error om inte hittar "load "filnamn""
        }

        private static void PrintHelpMessage()
        {
            Console.WriteLine("Avaliable commands: ");
            Console.WriteLine("  delete                      - empty the word list");
            Console.WriteLine("  delete /persname/ /surname/ - delete a word");
            Console.WriteLine("  list                        - list the translated words");
            Console.WriteLine("  load                        - load word list data from the file sweeng.lis");
            Console.WriteLine("  load /file/                 - load word list data from the file");
            Console.WriteLine("  new                         - create new translation");
            Console.WriteLine("  new /persname/ /surname/    - create new translation using swedish then english");
            Console.WriteLine("  translate                   - translate a word to the other language");
            Console.WriteLine("  quit                        - quit the program");
            Console.WriteLine();
        }
    }
}