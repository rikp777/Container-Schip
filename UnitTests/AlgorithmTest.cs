using System;
using Placement;
using Placement.Models;
using System.Collections.Generic;
using System.Linq;
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
            var value = new Container(1, 5000, ContainerType.Valued);
            Algorithm algorithm = new Algorithm();
            var position = algorithm.NextXPositionToLoad(grids, new Grid(0,0,0, value));

            Assert.Equal(12, position.X);
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

            var value = new Container(1, 5000, ContainerType.Valued);
            Algorithm algorithm = new Algorithm();
            bool succes = Algorithm.CanWeightBeOnTopXY(grids, new Grid(0,0,0, value));

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
            var value = new Container(1, 5000, ContainerType.Valued);
            Algorithm algorithm = new Algorithm();
            bool succes = Algorithm.CanWeightBeOnTopXY(grids, new Grid(0,0,0, value));
            Assert.True(succes);
        }
    }
}
