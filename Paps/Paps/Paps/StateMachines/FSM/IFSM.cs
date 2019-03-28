using System;
using System.Collections.Generic;

namespace Paps.StateMachines
{
    public interface IFSM
    {
        bool Active { get; }
        void Update();
    }

    public interface IFSM<TState, TTrigger> : IFSM, IEnumerable<IFSMState<TState, TTrigger>>
    {
        IFSMState<TState, TTrigger> CurrentState { get; }

        event ChangedStateEvent<TState, TTrigger> onStateChanged;

        void AddState(IFSMState<TState, TTrigger> state);
        void RemoveState(TState state);
        void RemoveState(IFSMState<TState, TTrigger> state);
        void SetInitialState(TState state);
        bool ContainsState(TState state);
        bool ContainsState(IFSMState<TState, TTrigger> state);

        bool SendEvent(TTrigger trigger);
        void MakeTransition(IFSMTransition<TState, TTrigger> transition);
        void BreakTransition(IFSMTransition<TState, TTrigger> transition);
    } 

    
}
