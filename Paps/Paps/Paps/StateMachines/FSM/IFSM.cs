using System;
using System.Collections.Generic;

namespace Paps.StateMachines
{
    public interface IFSM
    {
        bool Active { get; }
        void Update();
    }

    public interface IFSM<TState, TTrigger> : IFSM
    {
        IFSMState<TState, TTrigger> CurrentState { get; }
        IFSMState<TState, TTrigger> InitialState { get; }

        int TransitionCount { get; }
        int StateCount { get; }

        event ChangedStateEvent<TState, TTrigger> onStateChanged;
        event ChangedStateEvent<TState, TTrigger> onBeforeTransitionate;

        void AddState(IFSMState<TState, TTrigger> state);
        void RemoveState(TState state);
        void RemoveState(IFSMState<TState, TTrigger> state);
        void SetInitialState(TState state);
        bool ContainsState(TState state);
        bool ContainsState(IFSMState<TState, TTrigger> state);
        void ReplaceState(IFSMState<TState, TTrigger> state);

        bool SendEvent(TTrigger trigger);
        void MakeTransition(IFSMTransition<TState, TTrigger> transition);
        void BreakTransition(IFSMTransition<TState, TTrigger> transition);
        void BreakAllTransitionsRelatedTo(TState state);
        void BreakAllTransitions();
        bool ContainsTransition(IFSMTransition<TState, TTrigger> transition);

        void ForeachState(Action<IFSMState<TState, TTrigger>> action);
        void ForeachTransition(Action<IFSMTransition<TState, TTrigger>> action);
    } 

    
}
