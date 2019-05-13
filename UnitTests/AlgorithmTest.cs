using Placement;
using Placement.Models;
using System.Collections.Generic;
using Xunit; 

namespace UnitTests
{
    public class AlgorithmTest
    {
        [Fact]
        public void NextPositionToLoad_WeightAllTheSame_12()
        {
            List<Grid> grids = new List<Grid>();
            for(int i = 0; i < 12; i++)
            {
                Container container = new Container(i, 300000, ContainerType.Cooled );
                Grid grid = new Grid(i, 0, 0, container);
                grids.Add(grid);
            }
            Algorithm algorithm = new Algorithm();
            int position = algorithm.NextXPositionToLoad(grids, 0);

            Assert.Equal(12, position);
        }
        
        [Fact]
        public void CanWeightBeOnTopXY_WeightOverMax()
        {
            List<Grid> grids = new List<Grid>();
            for (int i = 0; i < 3; i++)
            {
                Container container = new Container(i, 15000, ContainerType.Cooled);
                Grid grid = new Grid(0, 0, i, container);
                grids.Add(grid);
            }
            Algorithm algorithm = new Algorithm();
            bool succes = algorithm.CanWeightBeOnTopXY(grids, 0, 0);

            Assert.False(succes);
        }
        [Fact]
        public void CanWeightBeOnTopXY_WeightUnderMax()
        {
            List<Grid> grids = new List<Grid>();
            for (int i = 0; i < 3; i++)
            {
                Container container = new Container(i, 4000, ContainerType.Cooled);
                Grid grid = new Grid(1, 0, i, container);
                grids.Add(grid);
            }
            Algorithm algorithm = new Algorithm();
            bool succes = algorithm.CanWeightBeOnTopXY(grids, 1, 0);

            Assert.True(succes);
        }
    }
}
