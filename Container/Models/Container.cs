using System;
using System.Collections.Generic;
using System.Text;

namespace Placement.Models
{
    public enum ContainerType
    {
        Normal = 1,
        Cooled = 2,
        Valued = 3,
        CooledValued = 4,
    }
    public class Container
    {
        private int weight;
        private ContainerType type;
        public int Weight {
            get { return weight; }
            private set
            {
                if(value < 4000)
                {
                    throw new ArgumentException("Weight must be at least 4000");               
                }
                if(value > 300000)
                {
                    throw new ArgumentException("Weight can't be greater than 30 ton");
                }
                weight = value;
            }
        }

        public int Id { get; private set; }

        public ContainerType Type {
            get { return type; }
            private set
            {
                type = value;
            }
        }

        public Container(int id, int weight, ContainerType type)
        {
            this.Id = id;
            this.Weight = weight;
            this.Type = type;
        }

    }
}
