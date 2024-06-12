namespace AOC4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            List<int> winlist = new List<int>();
            StreamReader sr = new StreamReader("input.txt");
            string line = sr.ReadLine();
            int total = 0;
            while (line != null)
            {
                total = 0;
                list.Add(line);
                char[] delim = { ':', '|' };
                string[] words = line.Split(delim);
                string[] winners = words[1].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                string[] has = words[2].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                // Console.WriteLine(winners[0], winners.Last(), has[0], has.Last());

                for (int i = 0; i < winners.Length; i++)
                {
                    for (int j = 0; j < has.Length; j++)
                    {
                        if (winners[i].Equals(has[j]) && total == 0) total = 1;
                        else if (winners[i].Equals(has[j])) total = total * 2;
                       // else Console.WriteLine($"Does not contain {winners[i]}");
                    }
                }
                winlist.Add(total);
                line = sr.ReadLine();
            }
            sr.Close();

            int scoretotal = 0;
            foreach (int score in winlist)
            {
                scoretotal += score;
            }
            Console.WriteLine(scoretotal);
        }
    }
}
