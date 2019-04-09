namespace Paps.StateMachines
{
    public interface IBFSM<TState, TTrigger> : IFSM<TState, TTrigger>
    {
        void AddBehaviourToState(TState state, IBFSMStateBehaviour<TState, TTrigger> behaviour);
        void RemoveBehaviourFromState(TState state, IBFSMStateBehaviour<TState, TTrigger> behaviour);
    }
}
