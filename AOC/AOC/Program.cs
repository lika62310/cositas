namespace AOC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            StreamReader sr = new StreamReader("input.txt");
            string line = sr.ReadLine();
            while (line != null)
            {
                string tonumbers = line;
                for (int i = 0; i < tonumbers.Length; i++)
                {
                    if (i < tonumbers.Length - 1)
                    {
                        if (tonumbers[i] == 'o') tonumbers = tonumbers.Replace("one", "o1ne");
                        else if (tonumbers[i] == 't' && tonumbers[i + 1] == 'w') tonumbers = tonumbers.Replace("two", "t2wo");
                        else if (tonumbers[i] == 't' && tonumbers[i + 1] == 'h') tonumbers = tonumbers.Replace("three", "th3ree");
                        else if (tonumbers[i] == 'f' && tonumbers[i + 1] == 'o') tonumbers = tonumbers.Replace("four", "fo4ur");
                        else if (tonumbers[i] == 'f' && tonumbers[i + 1] == 'i') tonumbers = tonumbers.Replace("five", "fi5ve");
                        else if (tonumbers[i] == 's' && tonumbers[i + 1] == 'i') tonumbers = tonumbers.Replace("six", "s6ix");
                        else if (tonumbers[i] == 's' && tonumbers[i + 1] == 'e') tonumbers = tonumbers.Replace("seven", "se7en");
                        else if (tonumbers[i] == 'e') tonumbers = tonumbers.Replace("eight", "ei8ght");
                        else if (tonumbers[i] == 'n') tonumbers = tonumbers.Replace("nine", "ni9ne");
                    }
                }
                list.Add(tonumbers);
                Console.WriteLine(tonumbers);
                line = sr.ReadLine();
            }

            sr.Close();


            //Console.WriteLine(list[10]);
            List<string> numlist = new List<string>();
            foreach (string sline in list)
            {
                string numstring = "";
                for (int i = 0; i < sline.Length; i++)
                {
                    if ('0' <= sline[i] && sline[i] <= '9')
                    {
                        numstring = numstring + sline[i];
                    }
                }
                numlist.Add(numstring[0] + "" + numstring.Last());

            }

            int total = 0;
            foreach (string nline in numlist)
            {
                total += int.Parse(nline);
            }

            Console.WriteLine(total);
        }
    }
}
