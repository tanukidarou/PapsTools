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

    public interface IBFSM<TState, TTrigger> : IFSM<TState, TTrigger>
    {
        void AddBehaviourToState(TState state, IBFSMStateBehaviour<TState, TTrigger> behaviour);
        void RemoveBehaviourFromState(TState state, IBFSMStateBehaviour<TState, TTrigger> behaviour);
    }
}
