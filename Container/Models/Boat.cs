using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Placement.Models
{
    public class Boat
    {
        public int Width { get; set; }
        public int Length { get; set; }
        public int weight { get; set; }
        private IList<Grid> grids { get; set; }

        public Boat(int width, int length, int weight)
        {
            Width = width;
            Length = length;
            this.weight = weight;
        }
        public IList<Grid> Grids
        {
            get => grids;
            set
            {
                var weight = value.Sum(grid => grid.Container.Weight);

                if (weight < this.weight / 2)
                {
                    throw new ArgumentException("De boot is niet genoeg beladen");
                }
                else
                {
                    grids = value;
                }
            }
        }

    }
}
