using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paps.StateMachines
{
    public interface IFSMState<TState>
    {
        string DebugName { get; set; }

        TState InnerState { get; }
        IFSM<TState> StateMachine { get; }

        void Enter();
        void Update();
        void Exit();

        void ChangeInnerState(TState innerState);
    }

    public interface IFSMState<TState, TTrigger>
    {
        string DebugName { get; set; }

        TState InnerState { get; }
        IFSM<TState, TTrigger> StateMachine { get; }

        void Enter();
        void Update();
        void Exit();

        bool HandleEvent(TTrigger trigger);

        void ChangeInnerState(TState innerState);
    }
}
