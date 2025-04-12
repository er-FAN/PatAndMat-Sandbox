using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.App.Models
{
    public class Street : ISimulableObject, IHasRelations
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public int SpeedLimit { get; set; }
        public Node Start { get; set; } = new Node();
        public Node End { get; set; } = new Node();

        public List<IRelation> Relations { get; set; } = [];

        public Street(string name, int speedLimit)
        {
            Id = Guid.NewGuid();
            Name = name;
            SpeedLimit = speedLimit;
        }
    }
}
