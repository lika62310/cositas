namespace AOC2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("input.txt");
            string line = sr.ReadLine();
            int total = 0;
            int a = 0;
            int b = 0;
            int c = 0;
            while (line != null)
            {
                int pack = 0;
                string[] words = line.Split('x');
                a = (2 * int.Parse(words[0]) * int.Parse(words[1]));
                b = (2 * int.Parse(words[1]) * int.Parse(words[2]));
                c = (2 * int.Parse(words[0]) * int.Parse(words[2]));
                if (c < a && c < b) pack = (c / 2) + a + b + c;               
                else if (b < a && b < c) pack = (b / 2) + a + b + c;
                else if (a < b && a < c) pack = (a / 2) + a + b + c;
                total += pack;
                line = sr.ReadLine();
            }
            sr.Close();
            Console.WriteLine(total);
        }
    }
}
