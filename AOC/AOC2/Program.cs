namespace AOC2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            List<int> min = new List<int>();
            StreamReader sr = new StreamReader("input.txt");
            string line = sr.ReadLine();
            while (line != null)
            {
                list.Add(line);
                string[] words = line.Split(' ');
                int blue = 0;
                int green = 0;
                int red = 0;
                for (int i = 0; i < words.Length; i++)
                {
                    if (i > 2 && words[i][0] == 'b' && int.Parse(words[i - 1]) > blue) blue = int.Parse(words[i - 1]);
                    if (i > 2 && words[i][0] == 'g' && int.Parse(words[i - 1]) > green) green = int.Parse(words[i - 1]);
                    if (i > 2 && words[i][0] == 'r' && int.Parse(words[i - 1]) > red) red = int.Parse(words[i - 1]);
                }
                min.Add(red * green * blue);
                line = sr.ReadLine();
            }
            sr.Close();

            int total = 0;
            foreach(int num in min)
            {
                total += num;
            }
            Console.WriteLine(total);
        }
    }
}
