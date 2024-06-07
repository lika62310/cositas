namespace AOC2
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
                string[] words = line.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    if (i > 2 && words[i][0] == 'b' && int.Parse(words[i - 1]) > 14) list.Remove(line);
                    if (i > 2 && words[i][0] == 'g' && int.Parse(words[i - 1]) > 13) list.Remove(line);
                    if (i > 2 && words[i][0] == 'r' && int.Parse(words[i - 1]) > 12) list.Remove(line);
                }
                line = sr.ReadLine();
            }
            sr.Close();

            int total = 0;
            foreach (string lines in list)
            {
                Console.WriteLine(lines);
                char[] delim = { ' ', ':' };
                string[] words = lines.Split(delim);
                total += int.Parse(words[1]);
            }
            Console.WriteLine(total);
        }
    }
}
