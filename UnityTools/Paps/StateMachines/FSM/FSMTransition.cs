using System;
using System.Collections.Generic;

namespace Paps.StateMachines
{
    public interface IFSMTransition<TState, TTrigger>
    {
        TState stateFrom { get; }
        TTrigger trigger { get; }
        TState stateTo { get; }
        
        bool IsValidTransition();
    }

    public class FSMTransition<TState, TTrigger> : IFSMTransition<TState, TTrigger>, IEquatable<FSMTransition<TState, TTrigger>>
    {
        public TState stateFrom { get; protected set; }
        public TTrigger trigger { get; protected set; }
        public TState stateTo { get; protected set; }

        protected Func<TState, TState, bool> stateComparator;
        protected Func<TTrigger, TTrigger, bool> triggerComparator;

        protected List<IValidation<FSMTransition<TState, TTrigger>>> guardConditions;

        public bool disabled;
        public bool blocked;

        public FSMTransition(TState stateFrom, TTrigger trigger, TState stateTo, Func<TState, TState, bool> stateComparator = null, Func<TTrigger, TTrigger, bool> triggerComparator = null)
        {
            this.stateFrom = stateFrom;
            this.trigger = trigger;
            this.stateTo = stateTo;

            guardConditions = new List<IValidation<FSMTransition<TState, TTrigger>>>();

            this.stateComparator = stateComparator;
            this.triggerComparator = triggerComparator;
            
            if(this.stateComparator == null)
            {
                stateComparator = DefaultStateComparator;
            }
            
            if(this.triggerComparator == null)
            {
                triggerComparator = DefaultTriggerComparator;
            }
        }

        public void AddGuardCondition(IValidation<FSMTransition<TState, TTrigger>> guardCondition)
        {
            guardConditions.Add(guardCondition);
        }

        public void RemoveGuardCondition(IValidation<FSMTransition<TState, TTrigger>> guardCondition)
        {
            guardConditions.Remove(guardCondition);
        }

        private bool DefaultStateComparator(TState s1, TState s2)
        {
            return s1.Equals(s2);
        }

        private bool DefaultTriggerComparator(TTrigger t1, TTrigger t2)
        {
            return t1.Equals(t2);
        }

        public bool IsValidTransition()
        {
            if(disabled)
            {
                return true;
            }
            
            if(blocked)
            {
                return false;
            }

            for(int i = 0; i < guardConditions.Count; i++)
            {
                if(guardConditions[i].IsValid(this) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(FSMTransition<TState, TTrigger> other)
        {
            return stateComparator(stateFrom, other.stateFrom) && triggerComparator(trigger, other.trigger);
        }
    }
}
