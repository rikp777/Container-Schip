using Placement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Placement
{
    public class Algorithm
    {
        /// <summary>Container-Schip
        ///
        ///     validatie punten 
        ///     // boven container max 30ton
        ///     // container max 30ton, leeg 4000kg
        ///     // container met waarde mag geen container boven zich 
        ///     // container met waarde alleen voor of achter
        ///     // gekoelde container alleen voor
        ///     // min 50% van gewicht schip moet geladen zijn
        ///     // gewicht links en rechts moet gelijk zijn met marge van 20%
        /// 
        ///
        ///     #1.    groepeer alle containers in types
        ///     #2.    probeer gekoelde containers intedelen
        ///     #3.    zorg dat gekoelde containers gelijk worden geladen met een marge van 20%
        ///     #4.    check of er nog een container op de onderliggende container mag het maximale gewicht bovenop is 30 Ton inc nieuwe container
        ///     5.    check of het maximale gewicht bijna wordt overschrede
        ///     6.    pak de best passende container uit de waarde groep en plaats deze bovenop dit geld alleen voor de eerste en de laatste rij
        ///     7.    als de eerste rij vol is wordt de laatste rij geladen van het schip hier mogen alleen normale containers met hierboven waardevolle containers
        ///     8.    de rest van het schip wordt geladen met normale containers deze blijven uiteraard wel in evenwicht
        ///     9.    er wordt gecontroleerd of de geladen containers een gewicht hebben van meer dan 50% van het gewicht
        /// 
        /// </summary>

        
        
        /// <summary>
        ///
        ///     x = breedte 
        ///     y = diepte 
        ///     z = hoogte
        /// 
        /// </summary>
        private const int Width = 5;
        private const int Length = 10;
        private const int Weight = 300000;
        private const int MaxOnTop = 30000;
        public List<Boat> OrderOnBoats(List<Container> collectionContainers)
        {
            // Sorteer containers op soort 
            var containers = collectionContainers;
            var cooled = containers.FindAll(container => container.Type == ContainerType.Cooled).OrderBy(container => container.Weight);
            var valued = containers.FindAll(container => container.Type == ContainerType.Valued).OrderBy(container => container.Weight);
            var normals = containers.FindAll(container => container.Type == ContainerType.Normal)
                .OrderBy(container => container.Weight);
            
            
            var boats = new List<Boat>();
            var boat = new Boat(Width, Length, Weight);
            var grids = new List<Grid>();
            var newPositionToLoad = new Grid(0,0, 0, null);


            if (containers.Count <= 0) return boats;
            

            //grids.Add(new Grid(8, 0, z, new Container(Containers.Count, 6000, ContainerType.Normal)));
            if (containers.Count > 0)
            {
                foreach (var cool in cooled)
                {
                    newPositionToLoad = NextXPositionToLoad(grids, newPositionToLoad);
                    newPositionToLoad.Container = cool;

                    if (!isOutOfRange(newPositionToLoad))
                    {
                        if (CanWeightBeOnTopXY(grids, newPositionToLoad))
                        {
                            grids.Add(newPositionToLoad);
                            containers.Remove(cool);
                        }
                        else
                        {
                            newPositionToLoad.Z = 0;
                        }
                    }
                    else
                    {
                        newPositionToLoad.Z += 1;   
                    }
                }

                newPositionToLoad.Y = 1;
                foreach (var normal in normals)
                {
                    newPositionToLoad = NextXPositionToLoad(grids, newPositionToLoad);
                    newPositionToLoad.Container = normal;

                    
                    if (!isOutOfRange(newPositionToLoad))
                    {
                        if (CanWeightBeOnTopXY(grids, newPositionToLoad))
                        {
                            if (!(newPositionToLoad.Y+1 >= Length))
                            {
                                grids.Add(newPositionToLoad);
                                containers.Remove(normal);
                            }
                        }
                        else
                        {
                            newPositionToLoad.Y += 1;
                            newPositionToLoad.Z = 0;
                        }
                    }
                    else
                    {
                        newPositionToLoad.Z += 1;   
                    }
                }

                foreach (var value in valued)
                {
                    newPositionToLoad = NextXPositionToLoad(grids, newPositionToLoad);
                    newPositionToLoad.Container = value;
                    
                    if (!isOutOfRange(newPositionToLoad))
                    {
                        if (CanWeightBeOnTopXY(grids, newPositionToLoad))
                        {
                            if (newPositionToLoad.Y+1 == Length)
                            {
                                grids.Add(newPositionToLoad);
                                containers.Remove(value);
                            }
                        }
                    }
                }
            }


            boat.Grids = grids;
            boats.Add(boat);
            return boats;
        }

        public static bool isOutOfRange(Grid newPositionToLoad)
        {
            return newPositionToLoad.X > Width || newPositionToLoad.X<= 0;
        }
        
        public static bool CanWeightBeOnTopXY(List<Grid> grids, Grid newPostionToLoad)
        {
            var success = false;
            var gridContainers = grids.FindAll(grid => grid.X == newPostionToLoad.X && grid.Y == newPostionToLoad.Y);
            var totalXY = TotalWeight(gridContainers) + newPostionToLoad.Container.Weight;
            if (totalXY <= MaxOnTop)
            {
                success = true;
            }
            return success;
        }

        public static bool CanValuedBeOnTop(List<Grid> grids, int x, int y, List<Container> valued)
        {
            if (y != Length) return false;
            
            var success = false;
            var gridContainers = grids.FindAll(grid => grid.X == x && grid.Y == y);
            // can container be on top as last with a total los of 20% on efficiency
            foreach (var value in valued)
            {
                var totalXY = TotalWeight(gridContainers) + value.Weight;
                 
                if (totalXY <= MaxOnTop)
                {
                    success = true;
                }
            }
            return success;
        }

        public Grid NextXPositionToLoad(List<Grid> grids, Grid newPositionToLoad)
        {
            var middle = (int)Math.Ceiling((double)Width / 2);
            var position = new Grid(middle, newPositionToLoad.Y, newPositionToLoad.Z, null);
            if (grids.Count == 0) return position;

            //RECHTS
            var gridContainersRight = grids.FindAll(grid => grid.Y == position.Y && grid.X >= middle);
            if (gridContainersRight.Count == 0) return position;
            var gridContainerMostTopRight = gridContainersRight.FindAll(grid => grid.Z == position.Z);
            if (gridContainerMostTopRight.Count == 0) return position;
            var gridContainerMostRight = gridContainerMostTopRight.Max(grid => grid.X);
            var totalRight = TotalWeight(gridContainersRight);
            
            //LINKS
            var gridContainersLeft = grids.FindAll(grid => grid.Y == position.Y && grid.X <= middle);
            var gridContainersMostTopLeft = gridContainersLeft.Where(grid => grid.Z == position.Z);
            var gridContainerMostLeft = gridContainersMostTopLeft.Min(grid => grid.X);
            var totalLeft = TotalWeight(gridContainersLeft);
            
            // Links met een marge van 20% erbij kleiner dan rechts 
            if(totalLeft * 1.2 < totalRight )
            {
                position.X = gridContainerMostLeft - 1;
                if (position.X < 0)
                {
                    position.X = gridContainerMostRight + 1;
                }
            }else {     
                position.X = gridContainerMostRight + 1;
                if (position.X > Width)
                {
                    position.X = gridContainerMostLeft - 1;
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
