using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public interface IHasRelations
    {
        List<IRelation> Relations { get; }
    }
}
