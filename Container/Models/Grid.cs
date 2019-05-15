using System;
using System.Collections.Generic;
using System.Text;

namespace Placement.Models
{
    public class Grid
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Container Container { get; set; }

        public Grid(int x, int y, int z, Container container)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Container = container;
        }
    }
}
