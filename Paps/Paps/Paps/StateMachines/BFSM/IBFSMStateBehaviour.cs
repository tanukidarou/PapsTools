using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paps.StateMachines
{
    public interface IBFSMStateBehaviour<TState, TTrigger>
    {
        IFSM<TState, TTrigger> StateMachine { get; }

        TState ParentState { get; }

        void OnEnterState();
        void OnUpdateState();
        void OnExitState();

        bool HandleEvent(TTrigger trigger);
    }
}
