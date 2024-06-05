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
                list.Add(line);
                line = sr.ReadLine();
            }

            sr.Close();
            Console.WriteLine(list.Last());
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
