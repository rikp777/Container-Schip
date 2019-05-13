using Placement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Placement
{
    class Program
    {
        public static List<Container> Containers = new List<Container>();
        public static void Initialise()
        {       
            for(int I = 0; I < 5; I++)
            {
                Container container = new Container(Containers.Count, 15000, ContainerType.Normal);
                Containers.Add(container);
            }
            for (int I = 0; I < 10; I++)
            {
                Container container = new Container(Containers.Count, 25000, ContainerType.Normal);
                Containers.Add(container);
            }
            for (int I = 0; I < 35; I++)
            {
                Container container = new Container(Containers.Count, 6000, ContainerType.Normal);
                Containers.Add(container);
            }
            for (int I = 0; I < 28; I++)
            {
                Container container = new Container(Containers.Count, 4520, ContainerType.Cooled);
                Containers.Add(container);
            }
            for (int I = 0; I < 7; I++)
            {
                Container container = new Container(Containers.Count, 18000, ContainerType.Valued);
                Containers.Add(container);
            }
            for (int I = 0; I < 12; I++)
            {
                Container container = new Container(Containers.Count, 21000, ContainerType.CooledValued);
                Containers.Add(container);
            }
        }
        static void Main(string[] args)
        {
            Initialise();
            Algorithm algorithm = new Algorithm();
            var boats = algorithm.OrderOnBoats(Containers);
            foreach (var boat in boats)
            {
                Console.WriteLine("Boat: " + (boats.IndexOf(boat) + 1));
                if (boat.Grids != null)
                {
                    var grids = boat.Grids;
                    var gridsGroupedByHeight = grids.GroupBy(grid => grid.Z);
                    foreach (var gridHeight in gridsGroupedByHeight)
                    {
                        Console.WriteLine("Layer: " + (gridHeight.Key + 1));
                        
                        var gridsGroupedByDepth = gridHeight.GroupBy(grid => grid.Y);
                        foreach (var gridDepth in gridsGroupedByDepth)
                        {
                            Console.WriteLine("Depth: " + (gridDepth.Key + 1));

                            var sorted = gridHeight.OrderBy(grid => grid.X).ThenBy(grid => grid.Y);
                            foreach (var grid in sorted)
                            {
                                //Console.WriteLine("Container: " + grid.Container.Id + " - (Width: " + grid.X + ", Depth: " + grid.Y + ") - Type: " + grid.Container.Type );
                                Console.WriteLine("Container: " + grid.Container.Id + " - (Width: " + grid.X +
                                                  ") - Type: " + grid.Container.Type);
                            }
                        }
                    }
                }
            }
            Console.ReadLine();
//            Initialise();
//            Algorithm algorithm = new Algorithm();
//            var boats = algorithm.OrderOnBoats(Containers);
//            foreach (var boat in boats)
//            {
//                Console.WriteLine("Boat: " + (boats.IndexOf(boat) + 1));
//                if (boat.Grids != null)
//                {
//                    var grids = boat.Grids;
//                    var gridsGroupedByHeight = grids.GroupBy(grid => grid.Z);
//                    foreach (var gridHeight in gridsGroupedByHeight)
//                    {
//                        Console.WriteLine("Layer: " + gridHeight.Key);
//                        var sorted = gridHeight.OrderBy(grid => grid.X).ThenBy(grid => grid.Y);
//                        foreach (var grid in sorted)
//                        {
//                            //Console.WriteLine("Container: " + grid.Container.Id + " - (Width: " + grid.X + ", Depth: " + grid.Y + ") - Type: " + grid.Container.Type );
//                            Console.WriteLine("Container: " + grid.Container.Id + " - (Width: " + grid.X + ") - Type: " + grid.Container.Type );
//                        }
//                    }
//                }
//            }
//            Console.ReadLine();
        }
    }
}
