using System;

namespace Mentormate
{
    public sealed class Grid
    {
        private readonly int x;
        private readonly int y;

        private Color[,] buffer; // Contains the intermediate state between switching of generations
        private Color[,] currentState;

        public int Generation { get; private set; }

        public Color this[int x, int y] => currentState[y, x];

        public Grid(int x, int y, Span<string> data)
        {
            currentState = new Color[y, x];
            for (int i = 0; i < data.Length; i++)
            {
                string s = data[i];
                if (s.Length != x)
                {
                    throw new ArgumentException("Failed to parse the data.");
                }
                for (int j = 0; j < s.Length; j++)
                {
                    char c = s[j];
                    if (c == '0')
                    {
                        currentState[i, j] = Color.Red;
                    }
                    else if (c == '1')
                    {
                        currentState[i, j] = Color.Green;
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect symbol found in data.");
                    }
                }
            }

            this.x = x;
            this.y = y;
            Generation = 0;
            buffer = new Color[y, x];
        }

        // Returns the next color of a cell depending on the number of its green neighbours
        private Color ApplyRules(Color currentColor, int greenNeighbours) =>
            currentColor == Color.Red
                ? greenNeighbours == 3 || greenNeighbours == 6 ? Color.Green : Color.Red
                : greenNeighbours == 2 || greenNeighbours == 3 || greenNeighbours == 6 ? Color.Green : Color.Red;

        // Calculates the next generation
        public void MoveNext()
        {
            int index_x = x - 2;
            int index_y = y - 2;

            // In order to reduce the number of if's, there is a separate method for each "special" case where the cell doesn't have 8 neighbours

            // the 4 corners
            DoTopLeft();
            DoTopRight();
            DoBottomLeft();
            DoBottomRight();

            // the 1st and the last lines without the corners
            DoTop(1, index_x);
            DoBottom(1, index_x);

            // the left and the right columns without the corners
            DoLeft(1, index_y);
            DoRight(1, index_y);

            // the remaining cells
            DoRectangle(1, index_x, 1, index_y);

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
                for (int j = 0; j < x; j++)
                {
                    if (currentState[i, j] == Color.Red)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("1");
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private void DoRectangle(int startIndexX, int stopIndexX, int startIndexY, int stopIndexY)
        {
            int count;
            int index;
            int index_x;
            int index_y;

            for (; startIndexY <= stopIndexY; startIndexY++)
            {
                for (index = startIndexX; index <= stopIndexX; index++)
                {
                    count = 0;

                    // top left
                    index_x = index - 1;
                    index_y = startIndexY - 1;
                    if (currentState[index_y, index_x] != Color.Red)
                    {
                        count++;
                    }

                    // top middle
                    if (currentState[index_y, index] != Color.Red)
                    {
                        count++;
                    }

                    // top right
                    index_x += 2;
                    if (currentState[index_y, index_x] != Color.Red)
                    {
                        count++;
                    }

                    // middle right
                    if (currentState[startIndexY, index_x] != Color.Red)
                    {
                        count++;
                    }

                    // middle left
                    index_x -= 2;
                    if (currentState[startIndexY, index_x] != Color.Red)
                    {
                        count++;
                    }

                    // bottom left
                    index_y += 2;
                    if (currentState[index_y, index_x] != Color.Red)
                    {
                        count++;
                    }

                    // bottom middle
                    if (currentState[index_y, index] != Color.Red)
                    {
                        count++;
                    }

                    // bottom right
                    index_x += 2;
                    if (currentState[index_y, index_x] != Color.Red)
                    {
                        count++;
                    }

                    buffer[startIndexY, index] = ApplyRules(currentState[startIndexY, index], count);
                }
            }
        }

        private void DoTopLeft()
        {
            int count = 0;

            // middle right
            if (currentState[0, 1] != Color.Red)
            {
                count++;
            }

            // bottom middle
            if (currentState[1, 1] != Color.Red)
            {
                count++;
            }

            // bottom right
            if (currentState[1, 0] != Color.Red)
            {
                count++;
            }

            buffer[0, 0] = ApplyRules(currentState[0, 0], count);
        }

        private void DoTopRight()
        {
            int count = 0;
            int index;

            // middle left
            index = x - 2;
            if (currentState[0, index] != Color.Red)
            {
                count++;
            }

            // bottom left
            if (currentState[1, index] != Color.Red)
            {
                count++;
            }

            // bottom middle
            index++;
            if (currentState[1, index] != Color.Red)
            {
                count++;
            }

            buffer[0, index] = ApplyRules(currentState[0, index], count);
        }

        private void DoBottomLeft()
        {
            int count = 0;
            int index;

            // top middle
            index = y - 2;
            if (currentState[index, 0] != Color.Red)
            {
                count++;
            }

            // top right
            if (currentState[index, 1] != Color.Red)
            {
                count++;
            }

            // middle right
            index++;
            if (currentState[index, 1] != Color.Red)
            {
                count++;
            }

            buffer[index, 0] = ApplyRules(currentState[index, 0], count);
        }

        private void DoBottomRight()
        {
            int count = 0;
            int index_x;
            int index_y;

            // middle left
            index_x = x - 2;
            index_y = y - 1;
            if (currentState[index_y, index_x] != Color.Red)
            {
                count++;
            }

            // top left
            index_y--;
            if (currentState[index_y, index_x] != Color.Red)
            {
                count++;
            }

            // top middle
            index_x++;
            if (currentState[index_y, index_x] != Color.Red)
            {
                count++;
            }

            index_y++;
            buffer[index_y, index_x] = ApplyRules(currentState[index_y, index_x], count);
        }

        private void DoTop(int startIndex, int stopIndex)
        {
            int count;
            int index_x;

            for (; startIndex <= stopIndex; startIndex++)
            {
                count = 0;

                // middle left
                index_x = startIndex - 1;
                if (currentState[0, index_x] != Color.Red)
                {
                    count++;
                }

                // bottom left
                if (currentState[1, index_x] != Color.Red)
                {
                    count++;
                }

                // middle right
                index_x += 2;
                if (currentState[0, index_x] != Color.Red)
                {
                    count++;
                }

                // bottom right
                if (currentState[1, index_x] != Color.Red)
                {
                    count++;
                }

                // bottom middle
                if (currentState[1, startIndex] != Color.Red)
                {
                    count++;
                }

                buffer[0, startIndex] = ApplyRules(currentState[0, startIndex], count);
            }
        }

        private void DoBottom(int startIndex, int stopIndex)
        {
            int count;
            int index_x;
            int index_y;

            for (; startIndex <= stopIndex; startIndex++)
            {
                count = 0;

                // top left
                index_x = startIndex - 1;
                index_y = y - 2;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                // top middle
                if (currentState[index_y, startIndex] != Color.Red)
                {
                    count++;
                }

                // top right
                index_x += 2;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                // middle right
                index_y++;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                // middle left
                index_x -= 2;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                buffer[index_y, startIndex] = ApplyRules(currentState[index_y, startIndex], count);
            }
        }

        private void DoLeft(int startIndex, int stopIndex)
        {
            int count;
            int index_y;

            for (; startIndex <= stopIndex; startIndex++)
            {
                count = 0;

                // top middle
                index_y = startIndex - 1;
                if (currentState[index_y, 0] != Color.Red)
                {
                    count++;
                }

                // top right
                if (currentState[index_y, 1] != Color.Red)
                {
                    count++;
                }

                // middle right
                if (currentState[startIndex, 1] != Color.Red)
                {
                    count++;
                }

                // bottom right
                index_y += 2;
                if (currentState[index_y, 1] != Color.Red)
                {
                    count++;
                }

                // bottom middle
                if (currentState[index_y, 0] != Color.Red)
                {
                    count++;
                }

                buffer[startIndex, 0] = ApplyRules(currentState[startIndex, 0], count);
            }
        }

        private void DoRight(int startIndex, int stopIndex)
        {
            int count;
            int index_x;
            int index_y;

            for (; startIndex <= stopIndex; startIndex++)
            {
                count = 0;

                // top left
                index_x = x - 2;
                index_y = startIndex - 1;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                // middle left
                if (currentState[startIndex, index_x] != Color.Red)
                {
                    count++;
                }

                // bottom left
                index_y += 2;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                // bottom middle
                index_x++;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                // top middle
                index_y -= 2;
                if (currentState[index_y, index_x] != Color.Red)
                {
                    count++;
                }

                buffer[startIndex, index_x] = ApplyRules(currentState[startIndex, index_x], count);
            }
        }
    }
}