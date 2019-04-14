using System;

namespace Paps.StateMachines
{
    public abstract class FSMState<TState, TTrigger> : IFSMState<TState, TTrigger>
    {
        public string DebugName { get; set; }

        public TState InnerState { get; protected set; }

        public IFSM<TState, TTrigger> StateMachine { get; protected set; }

        protected FSMState(IFSM<TState, TTrigger> stateMachine, TState state)
        {
            this.StateMachine = stateMachine;
            InnerState = state;
        }

        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnEnter()
        {

        }

        protected virtual void OnExit()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        public virtual bool HandleEvent(TTrigger trigger)
        {
            return false;
        }

        public void ChangeInnerState(TState innerState)
        {
            if(StateMachine.ContainsState(this) && StateMachine.ContainsState(innerState))
            {
                throw new InvalidOperationException("State id could not be changed because state machine already has a state with such id");
            }
            else
            {
                InnerState = innerState;
            }
        }
    }
}
