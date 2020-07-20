using System;

namespace Mentormate
{
    class Program
    {
        static void Main(string[] args)
        {
            int x;
            int y;
            int x1;
            int y1;
            int N;

            // Parsing the arguments - x, y 
            if (args.Length < 8)
            {
                Exit("Incorrect number of parameters.");
                return;
            }

            if (!int.TryParse(args[0], out x) || x < 2)
            {
                Exit("Incorrect value for 'x'.");
                return;
            }
            if (!int.TryParse(args[1], out y) || y < 2 || y >= 1000)
            {
                Exit("Incorrect value for 'y'.");
                return;
            }
            if (x > y)
            {
                Exit("'x' must be less than or equal to 'y'.");
                return;
            }

            if (args.Length < 5 + y)
            {
                Exit("Incorrect number of parameters.");
                return;
            }

            // Parsing the content of Generation 0
            Grid grid;
            try
            {
                grid = new Grid(x, y, args.AsSpan(2, y));
            }
            catch (Exception e)
            {
                Exit(e.Message);
                return;
            }

            // Parsing the remaining 3 arguments - x1, y1 and N
            int index = 2 + y;
            if (!int.TryParse(args[index], out x1) || x1 < 0 || x1 >= x)
            {
                Exit("Incorrect value for 'x1'.");
                return;
            }
            index++;
            if (!int.TryParse(args[index], out y1) || y1 < 0 || y1 >= y)
            {
                Exit("Incorrect value for 'y1'.");
                return;
            }
            index++;
            if (!int.TryParse(args[index], out N) || N <= 0)
            {
                Exit("Incorrect value for 'N'.");
                return;
            }
            
            // Calculations
            grid.Print();
            int count = grid[x1, y1] != Color.Red ? 1 : 0;
            for (index = 0; index < N; index++)
            {
                grid.MoveNext();
                if (grid[x1, y1] != Color.Red)
                {
                    count++;
                }
                grid.Print();
            }
            Exit("Count: " + count.ToString());
        }

        static void Exit(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.ReadKey(true);
        }
    }

}
