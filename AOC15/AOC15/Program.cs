namespace AOC1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("input.txt");
            string line = sr.ReadLine();
            int floor = 0;
            while (line != null)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '(') floor++;
                    else if (line[i] ==  ')') floor--;
                    if (floor == -1)
                    {
                        Console.WriteLine(i+1);
                        break;
                    }
                }
                line = sr.ReadLine();
            }
            sr.Close();
            Console.WriteLine(floor);
        }
    }
}
