using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paps.StateMachines
{
    public interface IBFSMState<TState, TTrigger> : IFSMState<TState, TTrigger>
    {
        void AddBehaviour(IBFSMStateBehaviour<TState, TTrigger> behaviour);
        void RemoveBehaviour(IBFSMStateBehaviour<TState, TTrigger> behaviour);

        bool ContainsBehaviour(IBFSMStateBehaviour<TState, TTrigger> behaviour);

        T GetBehaviour<T>();
        T[] GetBehaviours<T>();
    }
}
