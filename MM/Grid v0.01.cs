/* Old version, more generic, easier to modify.

using System;

namespace Mentormate
{
    [Obsolete]
    public sealed class Grid
    {
        private readonly int x;
        private readonly int y;

        private Color[] buffer;
        private Color[] currentState;

        public int Generation { get; private set; }

        public Grid(int x, int y, Span<string> data)
        {
            int size = x * y;
            int index = 0;
            currentState = new Color[size];
            for (int i = 0; i < data.Length; i++)
            {
                string s = data[i];
                if(s.Length != y)
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
                    else if(c == '1')
                    {
                        currentState[index] = Color.Green;
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


            buffer = new Color[size];
        }

        private void CountNeighbours(int x, int y, out int red, out int green)
        {
            int r = 0;

            // top left
            int index = (y - 1) * this.x + x - 1;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top middle
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top right
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // middle right
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // middle left
            index -= 2;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom left
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom middle
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom right
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 8 - r;
        }

        private void CountNeighboursTopLeft(out int red, out int green)
        {
            int r = 0;

            // middle right
            if (currentState[1] == Color.Red)
            {
                r++;
            }

            // bottom middle
            if (currentState[this.x] == Color.Red)
            {
                r++;
            }

            // bottom right
            if (currentState[this.x + 1] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 3 - r;
        }

        private void CountNeighboursTopRight(out int red, out int green)
        {
            int r = 0;
            int index;

            // middle left
            index = this.x - 2;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom left
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom middle
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 3 - r;
        }

        private void CountNeighboursBottomLeft(out int red, out int green)
        {
            int r = 0;
            int index;

            // top middle
            index = (y - 2) * this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top right
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // middle right
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 3 - r;
        }

        private void CountNeighboursBottomRight(out int red, out int green)
        {
            int r = 0;
            int index;

            // middle left
            index = this.x * this.y - 2;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top left
            index -= this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top middle
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 3 - r;
        }

        private void CountNeighboursTop(int x, out int red, out int green)
        {
            int r = 0;
            int index;

            // middle left
            index = this.x - 1;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // middle right
            index += 2;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom right
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom middle
            index--;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom left
            index--;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 5 - r;
        }

        private void CountNeighboursBottom(int x, out int red, out int green)
        {
            int r = 0;
            int index;

            // middle left
            index = (this.y - 1) * this.x - 1;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // middle right
            index += 2;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top right
            index -= this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top middle
            index--;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top left
            index--;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 5 - r;
        }

        private void CountNeighboursLeft(int y, out int red, out int green)
        {
            int r = 0;
            int index;

            // top middle
            index = (y - 1) * this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top right
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // middle right
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom right
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom middle
            index--;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 5 - r;
        }

        private void CountNeighboursRight(int y, out int red, out int green)
        {
            int r = 0;
            int index;

            // top middle
            index = y * this.x - 1;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // top left
            index--;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // middle left
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom left
            index += this.x;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            // bottom middle
            index++;
            if (currentState[index] == Color.Red)
            {
                r++;
            }

            red = r;
            green = 5 - r;
        }


        private Color ApplyRules(Color currentColor, int greenNeighbours, int redNeighbours) =>
            currentColor == Color.Red
                ? greenNeighbours == 3 || greenNeighbours == 6 ? Color.Green : Color.Red
                : greenNeighbours == 2 || greenNeighbours == 3 || greenNeighbours == 6 ? Color.Green : Color.Red;
    }
}

*/