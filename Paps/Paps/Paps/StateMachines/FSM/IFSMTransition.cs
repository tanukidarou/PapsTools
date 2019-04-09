using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paps.StateMachines
{
    public interface IFSMTransition<TState, TTrigger>
    {
        TState stateFrom { get; }
        TTrigger trigger { get; }
        TState stateTo { get; }

        bool IsValidTransition();
    }
}
