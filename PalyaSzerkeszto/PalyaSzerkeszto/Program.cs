using System;
using System.IO;
using System.Linq;

namespace PalyaSzerkeszto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static string NyelvValaszto()
            {
                Console.Write("Válassz nyevlet/Choose language:\n1) Magyar/Hungarian\n2) Angol/English\n\nVálasztásod/Your choice: ");
                int nyelvNum = int.Parse(Console.ReadLine());
                string nyelv = "HU";
                if (nyelvNum == 1)
                {
                    nyelv = "HU";
                }
                else if (nyelvNum == 2)
                {
                    nyelv = "EN";
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Hibás adat/Wrong data");
                    Console.ForegroundColor = ConsoleColor.White;
                    NyelvValaszto();
                }
                return nyelv;
            }
            string nyelv = NyelvValaszto();
            string[] datas = File.ReadAllLines($"data{nyelv}.txt");
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(datas[0]+nyelv);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            static string FajlnevBekeres(string nyelv)
            {
                if (nyelv == "HU")
                {
                    Console.Write("Válassz lehetőséget:\n1) Új pálya\n2) Pálya betöltés\n\nVálasztásod: ");
                }
                else if (nyelv == "EN")
                {
                    Console.Write("Choose option:\n1) New map\n2) Load map\n\nYour choice: ");
                }
                int modNum = int.Parse(Console.ReadLine());
                string mod = "uj";
                if (modNum == 1)
                {
                    mod = "uj";
                }
                else if (modNum == 2)
                {
                    mod = "betolt";
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (nyelv == "HU")
                    {
                        Console.WriteLine("Hibás adat");
                    }
                    else if (nyelv == "EN")
                    {
                        Console.WriteLine("Wrong data");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    FajlnevBekeres(nyelv);
                }
                return mod;
            }

            char[,] palya = new char[6, 24];
            if (FajlnevBekeres(nyelv) == "uj")
            {
                palya = new char[6, 24];
                for (int sor = 0; sor < palya.GetLength(0); sor++)
                {
                    for (int oszlop = 0; oszlop < palya.GetLength(1); oszlop++)
                    {
                        palya[sor, oszlop] = '*';
                    }
                }
            }
            else if (true)
            {
                Console.Write(datas[1]);
                string eleres = Console.ReadLine();
                string[] txt = File.ReadAllLines(eleres);
                palya = new char[txt.Length, txt[0].Length];
                for (int sor = 0; sor < palya.GetLength(0); sor++)
                {
                    for (int oszlop = 0; oszlop < palya.GetLength(1); oszlop++)
                    {
                        palya[sor, oszlop] = txt[sor][oszlop];
                    }
                }
            }
            Console.Clear();


            static void PalyaFrissites(int X, int Y, char elem, char[,] palya)
            {
                palya[X, Y] = elem;
            }

            static void PalyaKiiras(char[,] palya)
            {
                Console.WriteLine();
                Console.Clear();
                for (int i = 0; i < palya.GetLength(0); i++)
                {
                    for (int j = 0; j < palya.GetLength(1); j++)
                    {
                        Console.Write(palya[i, j]);
                    }
                    Console.WriteLine();
                }

            }

            static int SzobakSzama(char[,] palya)
            {
                int szobakSzama = 0;
                for (int i = 0; i < palya.GetLength(0); i++)
                {
                    for (int j = 0; j < palya.GetLength(1); j++)
                    {
                        if (palya[i, j] == '█')
                        {
                            szobakSzama++;
                        }
                    }
                }
                return szobakSzama;
            }

            static int KijaratokSzama(char[,] palya)
            {
                int kijaratokSzama = 0;
                string balOldal = "╬═╦╩╣╗╝";
                string jobbOldal = "╬═╦╩╠╚╔";
                string teteje = "╬╩║╣╠╝╚";
                string alja = "╬╦║╣╠╗╔";
                for (int i = 0; i < palya.GetLength(0); i++)
                {
                    for (int j = 0; j < palya.GetLength(1); j++)
                    {
                        if (balOldal.Contains(palya[i, 0]) || jobbOldal.Contains(palya[i, palya.GetLength(1)-1]) || teteje.Contains(palya[0, j]) || alja.Contains(palya[palya.GetLength(0)-1, j]))
                        {
                            kijaratokSzama++;
                        }
                    }
                }
                return kijaratokSzama;
            }

            PalyaKiiras(palya);
            Console.SetCursorPosition(0, 7);
            Console.WriteLine(datas[2]);
            datas[3] = datas[3].Replace("\\n", "\n");
            Console.WriteLine(datas[3]);

            Console.SetCursorPosition(0, 0);

            int pozX = 0;
            int pozY = 0;

            while (true)
            {
                Console.SetCursorPosition(pozX, pozY);
                ConsoleKey bill = Console.ReadKey().Key;
                if (bill == ConsoleKey.UpArrow)
                {
                    if (pozY > 0) 
                    { 
                        pozY -= 1;
                    }
                }
                else if (bill == ConsoleKey.LeftArrow)
                {
                    if (pozX > 0)
                    {
                        pozX -= 1;
                    }
                }
                else if (bill == ConsoleKey.DownArrow)
                {
                    if (pozY < palya.GetLength(0)-1)
                    {
                        pozY += 1;
                    }
                }
                else if (bill == ConsoleKey.RightArrow)
                {
                    if (pozX < palya.GetLength(1)-1)
                    {
                        pozX += 1;
                    }
                }
                else if (bill == ConsoleKey.Delete)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '.', palya);
                }
                else if (bill == ConsoleKey.F1)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '█', palya);
                }
                else if (bill == ConsoleKey.F2)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╬', palya);
                }
                else if (bill == ConsoleKey.F3)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '═', palya);
                }
                else if (bill == ConsoleKey.F4)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╦', palya);
                }
                else if (bill == ConsoleKey.F5)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╩', palya);
                }
                else if (bill == ConsoleKey.F6)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '║', palya);
                }
                else if (bill == ConsoleKey.F7)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╣', palya);
                }
                else if (bill == ConsoleKey.F8)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╠', palya);
                }
                else if (bill == ConsoleKey.F9)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╗', palya);
                }
                else if (bill == ConsoleKey.F10)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╝', palya);
                }
                else if (bill == ConsoleKey.Insert)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╚', palya);
                }
                else if (bill == ConsoleKey.F12)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╔', palya);
                }
                else if (bill == ConsoleKey.F12)
                {
                    PalyaFrissites(Console.CursorTop, Console.CursorLeft, '╔', palya);
                }
                else if (bill == ConsoleKey.Home)
                {
                    PalyaKiiras(palya);
                    Console.SetCursorPosition(0, 7);
                    Console.WriteLine(datas[2]);
                    datas[3] = datas[3].Replace("\\n", "\n");
                    Console.WriteLine(datas[3]);

                    Console.SetCursorPosition(0, 0);
                }
                else if (bill == ConsoleKey.End)
                {
                    Console.Clear();
                    PalyaKiiras(palya);
                    Console.SetCursorPosition(0, 7);

                    Console.Write(datas[4]);

                    string fajl = Console.ReadLine();
                    string[] sorok = new string[palya.GetLength(0)];
                    string sor = "";
                    for (int i = 0; i < palya.GetLength(0); i++)
                    {
                        for (int j = 0; j < palya.GetLength(1); j++)
                        {
                            sor += palya[i, j];
                        }
                        sorok[i] = sor;
                        sor = "";
                    }

                    if (fajl.Contains(".txt"))
                    {
                        File.WriteAllLines(fajl, sorok);
                    }
                    else
                    {
                        File.WriteAllLines($"{fajl}.txt", sorok);
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(datas[5]);
                }
                else if (bill == ConsoleKey.Enter)
                {
                    Console.Clear();
                    PalyaKiiras(palya);
                    Console.SetCursorPosition(0, 7);
                    datas[6] = datas[6].Replace("\\n", "\n");
                    Console.WriteLine(datas[6]);
                    if (SzobakSzama(palya) == 0 && KijaratokSzama(palya) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(datas[7]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (KijaratokSzama(palya) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(datas[8]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (SzobakSzama(palya) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(datas[9]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.SetCursorPosition(0, 11);
                }
                Console.SetCursorPosition(pozX, pozY);
                Console.Write(palya[Console.CursorTop,Console.CursorLeft]);
            }
        }
    }
}