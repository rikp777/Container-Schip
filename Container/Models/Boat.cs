using System;
using System.Collections.Generic;
using System.Text;

namespace Placement.Models
{
    public class Boat
    {
        public int Width { get; private set; }
        public int Lenght { get; private set; }

        public Boat(int width, int lenght)
        {
            this.Width = width;
            this.Lenght = lenght;
        }
        public IList<Grid> Grids { get; set; }

    }
}
