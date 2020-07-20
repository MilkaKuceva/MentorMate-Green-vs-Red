// bloated but probably more optimized
using System;

namespace Mentormate
{
    [Obsolete]
    public sealed class GridObsolete
    {
        private readonly int x;
        private readonly int y;

        private Color[] buffer;
        private Color[] currentState;

        public int Generation { get; private set; }

        public Color this[int x, int y] => currentState[y * this.x + x];

        public GridObsolete(int x, int y, Span<string> data)
        {
            int size = x * y;
            int index = 0;
            currentState = new Color[size];
            for (int i = 0; i < data.Length; i++)
            {
                string s = data[i];
                if (s.Length != y)
                {
                    throw new ArgumentException("Failed to parse the data.");
                }
                for (int j = 0; j < s.Length; j++)
                {
                    char c = s[j];
                    if (c == '0')
                    {
                        currentState[index] = Color.Red;
                    }
                    else if (c == '1')
                    {
                        currentState[index] = Color.Green;
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect symbol found in data.");
                    }
                    index++;
                }
            }

            this.x = x;
            this.y = y;
            Generation = 0;
            buffer = new Color[size];
        }

        private Color ApplyRules(Color currentColor, int greenNeighbours) =>
            currentColor == Color.Red
                ? greenNeighbours == 3 || greenNeighbours == 6 ? Color.Green : Color.Red
                : greenNeighbours == 2 || greenNeighbours == 3 || greenNeighbours == 6 ? Color.Green : Color.Red;

        public void MoveNext()
        {
            int count;
            int index;
            int i;
            int limit;

            // the 4 corners
            count = CountGreenNeighboursTopLeft();
            buffer[0] = ApplyRules(currentState[0], count);

            index = TopRightIndex;
            count = CountGreenNeighboursTopRight();
            buffer[index] = ApplyRules(currentState[index], count);

            index = BottomLeftIndex;
            count = CountGreenNeighboursBottomLeft();
            buffer[index] = ApplyRules(currentState[index], count);

            index = BottomRightIndex;
            count = CountGreenNeighboursBottomRight();
            buffer[index] = ApplyRules(currentState[index], count);

            // the 1st row
            limit = x - 1;
            for (index = 1; index < limit; index++)
            {
                count = CountGreenNeighboursTop(index);
                buffer[index] = ApplyRules(currentState[index], count);
            }

            // the last row
            index = (y - 1) * x + 1;
            for (i = 1; i < limit; i++)
            {
                count = CountGreenNeighboursBottom(i);
                buffer[index] = ApplyRules(currentState[index], count);
                index++;
            }

            // the left column
            limit = y - 1;
            index = x;
            for (i = 1; i < limit; i++)
            {
                count = CountGreenNeighboursLeft(i);
                buffer[index] = ApplyRules(currentState[index], count);
                index += x;
            }

            // the right column
            index = 2 * x - 1;
            for (i = 1; i < limit; i++)
            {
                count = CountGreenNeighboursRight(i);
                buffer[index] = ApplyRules(currentState[index], count);
                index += x;
            }

            // the rest
            int k = x - 1;
            for (i = 1; i < limit; i++)
            {
                index = i * x + 1;
                for (int j = 1; j < k; j++)
                {
                    count = CountGreenNeighbours(j, i);
                    buffer[index] = ApplyRules(currentState[index], count);
                    index++;
                }
            }

            var swap = buffer;
            buffer = currentState;
            currentState = swap;
            Generation++;
        }

        public void Print()
        {
            Console.Write("Generation: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Generation);
            for (int i = 0; i < y; i++)
            {
                int index = i * x;
                for (int j = 0; j < x; j++)
                {
                    if (currentState[index] == Color.Red)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("1");
                    }
                    index++;
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private int CountGreenNeighbours(int x, int y)
        {
            int count = 0;

            // top left
            int index = (y - 1) * this.x + x - 1;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top middle
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top right
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // middle right
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // middle left
            index -= 2;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom left
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom middle
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom right
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursTopLeft()
        {
            int count = 0;

            // middle right
            if (currentState[1] != Color.Red)
            {
                count++;
            }

            // bottom middle
            if (currentState[this.x] != Color.Red)
            {
                count++;
            }

            // bottom right
            if (currentState[this.x + 1] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursTopRight()
        {
            int count = 0;
            int index;

            // middle left
            index = this.x - 2;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom left
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom middle
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursBottomLeft()
        {
            int count = 0;
            int index;

            // top middle
            index = (y - 2) * this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top right
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // middle right
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursBottomRight()
        {
            int count = 0;
            int index;

            // middle left
            index = this.x * this.y - 2;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top left
            index -= this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top middle
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursTop(int x)
        {
            int count = 0;
            int index;

            // middle left
            index = x - 1;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // middle right
            index += 2;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom right
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom middle
            index--;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom left
            index--;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursBottom(int x)
        {
            int count = 0;
            int index;

            // middle left
            index = (this.y - 1) * this.x + x - 1;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // middle right
            index += 2;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top right
            index -= this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top middle
            index--;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top left
            index--;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursLeft(int y)
        {
            int count = 0;
            int index;

            // top middle
            index = (y - 1) * this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top right
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // middle right
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom right
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom middle
            index--;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int CountGreenNeighboursRight(int y)
        {
            int count = 0;
            int index;

            // top middle
            index = y * this.x - 1;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // top left
            index--;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // middle left
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom left
            index += this.x;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            // bottom middle
            index++;
            if (currentState[index] != Color.Red)
            {
                count++;
            }

            return count;
        }

        private int TopLeftIndex => 0;
        private int TopRightIndex => x - 1;
        private int BottomLeftIndex => (y - 1) * x;
        private int BottomRightIndex => x * y - 1;
    }
}