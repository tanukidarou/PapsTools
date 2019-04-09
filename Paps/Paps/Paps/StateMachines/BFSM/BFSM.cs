using System;
using System.Collections;
using System.Collections.Generic;

namespace Paps.StateMachines
{
    public class BFSM<TState, TTrigger> : FSM<TState, TTrigger>, IBFSM<TState, TTrigger>
    {
        public TState InnerCurrentState
        {
            get
            {
                return CurrentState.InnerState;
            }
        }

        public BFSM(Func<TState, TState, bool> stateComparator = null, Func<TTrigger, TTrigger, bool> triggerComparator = null) : base(stateComparator, triggerComparator)
        {

        }

        public override void AddState(IFSMState<TState, TTrigger> state)
        {
            throw new NotSupportedException("Behavioural State Machines can't add custom states. Use AddState(TState state) then you can add behaviours to your states");
        }

        public void AddState(TState state)
        {
            base.AddState(new BFSMEmptyState(this, state));
        }

        public void AddBehaviourToState(TState state, IBFSMStateBehaviour<TState, TTrigger> behaviour)
        {
            IBFSMState<TState, TTrigger> stateObj = GetStateByInnerState<IBFSMState<TState, TTrigger>>(state);

            stateObj.AddBehaviour(behaviour);
        }

        public void RemoveBehaviourFromState(TState state, IBFSMStateBehaviour<TState, TTrigger> behaviour)
        {
            IBFSMState<TState, TTrigger> stateObj = GetStateByInnerState<IBFSMState<TState, TTrigger>>(state);

            stateObj.RemoveBehaviour(behaviour);
        }

        public T GetBehaviour<T>()
        {
            for(int i = 0; i < states.Count; i++)
            {
                var current = (IBFSMState<TState, TTrigger>)states[i];

                var behaviour = current.GetBehaviour<T>();

                if(behaviour != default)
                {
                    return behaviour;
                }

            }

            return default;
        }

        public T[] GetBehaviours<T>()
        {
            List<T> list = null;

            for(int i = 0; i < states.Count; i++)
            {
                T[] behaviours = ((IBFSMState<TState, TTrigger>)states[i]).GetBehaviours<T>();

                if(behaviours != null)
                {
                    if(list == null)
                    {
                        list = new List<T>();
                    }

                    list.AddRange(behaviours);
                }
            }

            if(list == null)
            {
                return null;
            }
            else
            {
                return list.ToArray();
            }
        }

        public IBFSMState<TState, TTrigger> GetOwnerOf(IBFSMStateBehaviour<TState, TTrigger> behaviour)
        {
            for (int i = 0; i < states.Count; i++)
            {
                var current = (IBFSMState<TState, TTrigger>)states[i];

                if(current.ContainsBehaviour(behaviour))
                {
                    return current;
                }
            }

            return default;
        }


        private sealed class BFSMEmptyState : FSMState<TState, TTrigger>, IBFSMState<TState, TTrigger>
        {
            private List<IBFSMStateBehaviour<TState, TTrigger>> behaviours;

            public BFSMEmptyState(IFSM<TState, TTrigger> stateMachine, TState state) : base(stateMachine, state)
            {
                StateMachine = stateMachine;

                InnerState = state;

                behaviours = new List<IBFSMStateBehaviour<TState, TTrigger>>();
            }

            public void AddBehaviour(IBFSMStateBehaviour<TState, TTrigger> behaviour)
            {
                behaviours.Add(behaviour);
            }

            public void RemoveBehaviour(IBFSMStateBehaviour<TState, TTrigger> behaviour)
            {
                behaviours.Remove(behaviour);
            }

            public bool ContainsBehaviour(IBFSMStateBehaviour<TState, TTrigger> behaviour)
            {
                return behaviours.Contains(behaviour);
            }

            public override bool HandleEvent(TTrigger trigger)
            {
                for (int i = 0; i < behaviours.Count; i++)
                {
                    if (behaviours[i].HandleEvent(trigger))
                    {
                        return true;
                    }
                }

                return false;
            }

            protected override void OnEnter()
            {
                for (int i = 0; i < behaviours.Count; i++)
                {
                    behaviours[i].OnEnterState();
                }
            }

            protected override void OnExit()
            {
                for (int i = 0; i < behaviours.Count; i++)
                {
                    behaviours[i].OnExitState();
                }
            }

            protected override void OnUpdate()
            {
                for (int i = 0; i < behaviours.Count; i++)
                {
                    behaviours[i].OnUpdateState();
                }
            }

            public T GetBehaviour<T>()
            {
                for (int i = 0; i < behaviours.Count; i++)
                {
                    var current = behaviours[i];

                    if (current is T behaviour)
                    {
                        return behaviour;
                    }
                }

                return default;
            }

            public T[] GetBehaviours<T>()
            {
                List<T> list = null;

                for (int i = 0; i < behaviours.Count; i++)
                {
                    var current = behaviours[i];

                    if (current is T behaviour)
                    {
                        if (list == null)
                        {
                            list = new List<T>();
                        }

                        list.Add(behaviour);
                    }
                }

                if (list == null)
                {
                    return null;
                }
                else
                {
                    return list.ToArray();
                }
            }
        }
    }
}
