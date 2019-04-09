using System;

namespace Paps.StateMachines
{
    public abstract class BFSMStateBehaviour<TState, TTrigger> : IBFSMStateBehaviour<TState, TTrigger>
    {
        public IFSM<TState, TTrigger> StateMachine { get; protected set; }

        public TState ParentState { get; protected set; }

        public BFSMStateBehaviour(IBFSM<TState, TTrigger> stateMachine, TState parentState)
        {
            StateMachine = stateMachine;
            ParentState = parentState;
        }

        public virtual void OnEnterState()
        {

        }

        public virtual void OnUpdateState()
        {

        }

        public virtual void OnExitState()
        {

        }

        public virtual bool HandleEvent(TTrigger trigger)
        {
            return false;
        }
    }
}
