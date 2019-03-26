using System;
using System.Collections;
using System.Collections.Generic;

namespace Paps.StateMachines
{
    public class FSM<TState, TTrigger> : IFSM<TState, TTrigger>
    {
        protected List<IFSMState<TState, TTrigger>> states;
        protected List<IFSMTransition<TState, TTrigger>> transitions;

        public int StateCount
        {
            get
            {
                return states.Count;
            }
        }

        public bool Active { get; set; } = true;

        public IFSMState<TState, TTrigger> CurrentState { get; protected set; }

        public IFSMState<TState, TTrigger> InitialState { get; protected set; }

        public event ChangedStateEvent<TState, TTrigger> onStateChanged;

        protected Func<TState, TState, bool> stateComparator;
        protected Func<TTrigger, TTrigger, bool> triggerComparator;

        public bool Started { get; protected set; }

        public FSM(Func<TState, TState, bool> stateComparator = null, Func<TTrigger, TTrigger, bool> triggerComparator = null)
        {
            states = new List<IFSMState<TState, TTrigger>>();
            transitions = new List<IFSMTransition<TState, TTrigger>>();

            this.stateComparator = stateComparator;
            this.triggerComparator = triggerComparator;

            if (this.stateComparator == null)
            {
                stateComparator = DefaultStateComparator;
            }

            if (this.triggerComparator == null)
            {
                triggerComparator = DefaultTriggerComparator;
            }
        }

        public void SetStateComparator(Func<TState, TState, bool> stateComparator)
        {
            this.stateComparator = stateComparator;
        }
        
        public void SetTriggerComparator(Func<TTrigger, TTrigger, bool> triggerComparator)
        {
            this.triggerComparator = triggerComparator;
        }

        private bool DefaultStateComparator(TState s1, TState s2)
        {
            return s1.Equals(s2);
        }

        private bool DefaultTriggerComparator(TTrigger t1, TTrigger t2)
        {
            return t1.Equals(t2);
        }

        public virtual void AddState(IFSMState<TState, TTrigger> state)
        {
            if(ContainsState(state.InnerState) == false)
            {
                states.Add(state);
            }
        }

        public void Start()
        {
            if(Started == false)
            {
                CurrentState = InitialState;

                CurrentState.Enter();

                Started = true;
            }
        }

        public void MakeTransition(TState stateFrom, TTrigger trigger, TState stateTo)
        {
            if (ContainsTransition(stateFrom, trigger) == false)
            {
                transitions.Add(new FSMTransition<TState, TTrigger>(stateFrom, trigger, stateTo));
            }
        }

        public void MakeTransition(IFSMTransition<TState, TTrigger> transition)
        {
            if (ContainsTransition(transition.stateFrom, transition.trigger) == false)
            {
                transitions.Add(transition);
            }
        }

        public void BreakTransition(IFSMTransition<TState, TTrigger> transition)
        {
            transitions.Remove(transition);
        }

        public void BreakTransition(TState stateFrom, TTrigger trigger)
        {
            IFSMTransition<TState, TTrigger> transition = GetTransition(stateFrom, trigger);

            transitions.Remove(transition);
        }

        public IFSMTransition<TState, TTrigger> GetTransition(TState stateFrom, TTrigger trigger)
        {
            for(int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> current = transitions[i];

                if(stateComparator(current.stateFrom, stateFrom) && triggerComparator(current.trigger, trigger))
                {
                    return current;
                }
            }

            return null;
        }

        public IFSMTransition<TState, TTrigger>[] GetTransitionsWithStateFrom(TState stateFrom)
        {
            List<IFSMTransition<TState, TTrigger>> list = null;

            for (int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> current = transitions[i];

                if (stateComparator(current.stateFrom, stateFrom))
                {
                    if (list == null)
                    {
                        list = new List<IFSMTransition<TState, TTrigger>>();
                    }

                    list.Add(current);
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

        public IFSMTransition<TState, TTrigger>[] GetTransitionsWithStateTo(TState stateTo)
        {
            List<IFSMTransition<TState, TTrigger>> list = null;

            for (int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> current = transitions[i];

                if (stateComparator(current.stateTo, stateTo))
                {
                    if (list == null)
                    {
                        list = new List<IFSMTransition<TState, TTrigger>>();
                    }

                    list.Add(current);
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

        public IFSMTransition<TState, TTrigger>[] GetTransitionsWithTrigger(TTrigger trigger)
        {
            List<IFSMTransition<TState, TTrigger>> list = null;

            for (int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> current = transitions[i];

                if (triggerComparator(current.trigger, trigger))
                {
                    if (list == null)
                    {
                        list = new List<IFSMTransition<TState, TTrigger>>();
                    }

                    list.Add(current);
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

        public void BreakAllTransitionRelatedTo(TState state)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> current = transitions[i];

                if (stateComparator(current.stateFrom, state) && stateComparator(current.stateTo, state))
                {
                    transitions.RemoveAt(i);
                    i--;
                }
            }
        }

        public T GetState<T>()
        {
            for(int i = 0; i < states.Count; i++)
            {
                if(states[i] is T state)
                {
                    return state;
                }
            }

            return default;
        }

        public T GetStateByInnerState<T>(TState innerState)
        {
            for(int i = 0; i < states.Count; i++)
            {
                IFSMState<TState, TTrigger> current = states[i];

                if (stateComparator(current.InnerState, innerState) && current is T state)
                {
                    return state;
                }
            }

            return default;
        }

        public T[] GetStates<T>()
        {
            List<T> requested = null;

            for(int i = 0; i < states.Count; i++)
            {
                IFSMState<TState, TTrigger> current = states[i];

                if(current is T state)
                {
                    if(requested == null)
                    {
                        requested = new List<T>();
                    }

                    requested.Add(state);
                }
            }

            if(requested == null)
            {
                return default;
            }
            else
            {
                return requested.ToArray();
            }
        }

        public bool ContainsState(TState state)
        {
            return GetStateByInnerState<IFSMState<TState, TTrigger>>(state) != null;
        }

        public bool ContainsState(IFSMState<TState, TTrigger> state)
        {
            return states.Contains(state);
        }

        public bool ContainsTransition(TState state, TTrigger trigger)
        {
            return GetTransition(state, trigger) != null;
        }

        public virtual void Update()
        {
            if (CurrentState != null && Active)
            {
                CurrentState.Update();
            }
        }

        public void SetInitialState(IFSMState<TState, TTrigger> state)
        {
            AddState(state);

            InternalSetInitialState(state);
        }

        public void SetInitialState(TState state)
        {
            InternalSetInitialState(GetStateByInnerState<IFSMState<TState, TTrigger>>(state));
        }

        private void InternalSetInitialState(IFSMState<TState, TTrigger> state)
        {
            InitialState = state;
        }

        protected bool Transitionate(TTrigger trigger)
        {
            IFSMTransition<TState, TTrigger> transition = GetTransition(CurrentState.InnerState, trigger);

            if(transition != null && ContainsState(transition.stateTo) && transition.IsValidTransition())
            {
                IFSMState<TState, TTrigger> previous = CurrentState;

                previous.Exit();

                CurrentState = GetStateByInnerState<IFSMState<TState, TTrigger>>(transition.stateTo);

                CurrentState.Enter();

                if (onStateChanged != null)
                {
                    onStateChanged(previous, transition.trigger, CurrentState);
                }

                return true;
            }

            return false;
        }

        public bool SendEvent(TTrigger trigger)
        {
            if(CurrentState != null && CurrentState.HandleEvent(trigger) == false)
            {
                return Transitionate(trigger);
            }

            return false;
        }

        public void RemoveState(TState state)
        {
            if(stateComparator(CurrentState.InnerState, state))
            {
                throw new InvalidOperationException("Cannot remove current running state");
            }
            else
            {
                states.Remove(GetStateByInnerState<FSMState<TState, TTrigger>>(state));
            }
        }

        public void RemoveState(IFSMState<TState, TTrigger> state)
        {
            if(stateComparator(CurrentState.InnerState, state.InnerState))
            {
                throw new InvalidOperationException("Cannot remove current running state");
            }
            else
            {
                states.Remove(state);
            }
        }

        public IEnumerator<IFSMState<TState, TTrigger>> GetEnumerator()
        {
            return states.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
