using System;

namespace Paps.StateMachines
{
    public delegate void ChangedStateEvent<TState, TTrigger>(IFSMState<TState, TTrigger> previous, TTrigger trigger, IFSMState<TState, TTrigger> current);

    public interface IFSMState<TState, TTrigger>
    {
        string DebugName { get; }

        TState InnerState { get; }
        IFSM<TState, TTrigger> StateMachine { get; }

        void Enter();
        void Update();
        void Exit();

        bool HandleEvent(TTrigger trigger);
    }

    public abstract class FSMState<TState, TTrigger> : IFSMState<TState, TTrigger>
    {
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
    }
}
