using Placement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Placement
{
    public class Algorithm
    {
        private const int Width = 15;
        private const int Length = 55;
        private const int MaxOnTop = 300000;
        
        //boven container max 30ton
        //container max 30ton, leeg 4000kg 
        //container met waarde mag geen container boven zich 
        //container met waarde alleen voor of achter 
        //gekoelde container alleen voor 
        //min 50% van gewicht schip moet geladen zijn 
        //gewicht links en rechts moet gelijk zijn met marge van 20%

        
        // Sorteer containers op soort 
        // Begin op de eerste rij bereken de plaats om container te laden 
        // plaats de juuste container op deze plaats 
        // wanneer de eerste rij vol ga naar 2e rij enz kijk hierbij welke containers hier niet mogen 
        // wanneer eerste xy laag vol is ga naar de 2 laag van z 
        

        public List<Boat> OrderOnBoats(List<Container> containers)
        {
            // x is breedte 
            // y is diepte 
            // z is hoogte 
            
            // Sorteer containers op soort 
            var Containers = containers;
            var Cooled = containers.FindAll(container => container.Type == ContainerType.Cooled);
            var Valued = containers.FindAll(container => container.Type == ContainerType.Valued);
            //var CooledValued = containers.FindAll(container => container.Type == ContainerType.CooledValued);
            var Normals = containers.FindAll(container => container.Type == ContainerType.Normal);
            var boats = new List<Boat>();
            var boat = new Boat(Width, Length);
            var grids = new List<Grid>();
            var y = 0;
            var z = 0;

            
            if (containers.Count <= 0) return boats;
            

            //grids.Add(new Grid(8, 0, z, new Container(Containers.Count, 6000, ContainerType.Normal)));
            if (Containers.Count > 0)
            {
                foreach (var Cool in Cooled)
                {
                    var x = NextXPositionToLoad(grids, y, z);

                    // if x is groter of kleiner dan de breedte 
                    if (x > Width || x <= 0)
                    {
                        z += 1;
                    }
                    else
                    {
                        x = NextXPositionToLoad(grids, y, z);
                        if (CanWeightBeOnTopXY(grids, x, y))
                        {
                            var grid = new Grid(x, y, z, Cool);
                            grids.Add(grid);
                            Containers.Remove(Cool);
                        }
                    }
                }

                foreach (var Value in Valued)
                {
                    
                }

                foreach (var Normal in Normals)
                {
                    var x = NextXPositionToLoad(grids, y, z);

                    // if x is groter of kleiner dan de breedte 
                    if (x > Width || x <= 0)
                    {
                        z += 1;
                    }
                    else
                    {
                        x = NextXPositionToLoad(grids, y, z);
                        if (CanWeightBeOnTopXY(grids, x, y))
                        {
                            var grid = new Grid(x, y, z, Normal);
                            grids.Add(grid);
                            Containers.Remove(Normal);
                        }
                    }
                }
            }


            boat.Grids = grids;
            boats.Add(boat);
            return boats;
        }

        public Grid placeContainerzInGridWithValidation(int x, int y, int z)
        {
            Grid grid = null;
            return grid;
        }
        
        public bool CanWeightBeOnTopXY(List<Grid> grids, int x, int y)
        {
            var success = false;
            var gridContainers = grids.FindAll(grid => grid.X == x && grid.Y == y);
            var totalXY = TotalWeight(gridContainers);
            if (totalXY <= MaxOnTop)
            {
                success = true;
            }
            return success;
        }
                
        public bool CanTypetBeOnTopXY(List<Grid> grids, int x, int y)
        {
            var success = false;
            var gridContainers = grids.FindAll(grid => grid.Container.Type == ContainerType.Valued || grid.Container.Type == ContainerType.CooledValued);
            if(!(gridContainers.Count > 0))
            {
                success = true;
            }
            return success;
        }

        public int NextXPositionToLoad(List<Grid> grids, int y, int z)
        {
            var middle = (int)Math.Ceiling((double)Width / 2);
            var position = middle;
            if (grids.Count == 0) return position;

            //RECHTS
            var gridContainersRight = grids.FindAll(grid => grid.Y == y && grid.X >= middle);
            if (gridContainersRight.Count == 0) return position;
            var gridContainerMostTopRight = gridContainersRight.FindAll(grid => grid.Z == z);
            if (gridContainerMostTopRight.Count == 0) return position;
            var gridContainerMostRight = gridContainerMostTopRight.Max(grid => grid.X);
            var totalRight = TotalWeight(gridContainersRight);
            
            //LINKS
            var gridContainersLeft = grids.FindAll(grid => grid.Y == y && grid.X <= middle);
            var gridContainersMostTopLeft = gridContainersLeft.Where(grid => grid.Z == z);
            var gridContainerMostLeft = gridContainersMostTopLeft.Min(grid => grid.X);
            var totalLeft = TotalWeight(gridContainersLeft);
            
            // Links met een marge van 20% erbij kleiner dan rechts 
            if(totalLeft * 1.2 < totalRight )
            {
                position = gridContainerMostLeft - 1;
                if (position < 0)
                {
                    position = gridContainerMostRight + 1;
                }
            }else {     
                position = gridContainerMostRight + 1;
                if (position > Width)
                {
                    position = gridContainerMostLeft - 1;
                }
            }

            return position;
        }

        public static int TotalWeight(IEnumerable<Grid> grids)
        {
            return grids.Sum(grid => grid.Container.Weight);
        }
    }
}
