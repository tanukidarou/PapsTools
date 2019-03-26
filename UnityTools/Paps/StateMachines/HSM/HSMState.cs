using System;
using System.Collections;
using System.Collections.Generic;

namespace Paps.StateMachines
{
    public abstract class HSMState<TState, TTrigger> : IEnumerable<HSMState<TState, TTrigger>>
    {
        public delegate void StateChangeEvent(HSMState<TState, TTrigger> stateFrom, TTrigger trigger, HSMState<TState, TTrigger> stateTo);
        public delegate void HSMRefAction<TRefParameter>(HSMState<TState, TTrigger> state, ref TRefParameter parameter);

        public string DebugName { get; set; }

        public TState StateId { get; private set; }

        protected List<HSMState<TState, TTrigger>> parents;
        protected List<HSMState<TState, TTrigger>> childs;
        protected List<HSMState<TState, TTrigger>> parallelChilds;

        protected List<IFSMTransition<TState, TTrigger>> transitions;

        public TState InitialChildState { get; private set; }

        public HSMState<TState, TTrigger> ActiveParent { get; protected set; }
        public HSMState<TState, TTrigger> ActiveNonParallelChild { get; protected set; }

        private HSMStateEnumerator enumerator;

        protected Func<TState, TState, bool> stateIdComparer;
        protected Func<TTrigger, TTrigger, bool> triggerComparer;

        public int ChildCount
        {
            get
            {
                return childs.Count + parallelChilds.Count;
            }
        }

        public int NonParallelChildCount
        {
            get
            {
                return childs.Count;
            }
        }

        public int ParallelChildCount
        {
            get
            {
                return parallelChilds.Count;
            }
        }

        public int TransitionCount
        {
            get
            {
                return transitions.Count;
            }
        }

        public event StateChangeEvent onStateChanged;
        public event StateChangeEvent onBeforeTransitionate;

        public event StateChangeEvent onAnyStateChanged
        {
            add
            {
                GetRoot().AddOnAnyStateChangedListener(value);
            }

            remove
            {
                GetRoot().RemoveOnAnyStateChangedListener(value);
            }
        }

        public event StateChangeEvent onBeforeAnyTransitionate
        {
            add
            {
                GetRoot().AddOnBeforeAnyTransitionateListener(value);
            }

            remove
            {
                GetRoot().RemoveOnBeforeAnyTransitionateListener(value);
            }
        }

        protected HSMState(TState stateId, string debugName = null, Func<TState, TState, bool> stateIdComparer = null, Func<TTrigger, TTrigger, bool> triggerComparer = null)
        {
            StateId = stateId;

            childs = new List<HSMState<TState, TTrigger>>();
            parallelChilds = new List<HSMState<TState, TTrigger>>();
            parents = new List<HSMState<TState, TTrigger>>();
            transitions = new List<IFSMTransition<TState, TTrigger>>();

            if (debugName != null)
            {
                DebugName = debugName;
            }
            else
            {
                DebugName = GetType().Name;
            }

            if (stateIdComparer != null)
            {
                this.stateIdComparer = stateIdComparer;
            }
            else
            {
                this.stateIdComparer = DefaultComparer<TState>;
            }

            if (triggerComparer != null)
            {
                this.triggerComparer = triggerComparer;
            }
            else
            {
                this.triggerComparer = DefaultComparer<TTrigger>;
            }
        }

        public void ChangeStateId(TState stateId)
        {
            for (int i = 0; i < parents.Count; i++)
            {
                var parent = parents[i];

                var possiblyChild = parent.GetImmediateChildState(stateId);

                if (possiblyChild != null)
                {
                    throw new InvalidOperationException(@"State id could not be changed due to parent NAME: "
                    + parent.DebugName + " STATE ID: " + parent.StateId.ToString() + 
                    " having such state id on child state NAME: " + possiblyChild.DebugName);
                }
            }

            StateId = stateId;
        }

        private void SetActiveParent(HSMState<TState, TTrigger> parent)
        {
            ActiveParent = parent;
        }

        public void Enter()
        {
            InternalEnter();

            InternalOnEnter();
        }

        private void InternalEnter()
        {
            ActiveNonParallelChild = GetImmediateChildState(InitialChildState);

            EnterParallelChilds();
            EnterActiveNonParallelChild();
        }

        private void EnterParallelChilds()
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                var current = parallelChilds[i];

                current.SetActiveParent(this);
                current.InternalEnter();
            }
        }

        private void EnterActiveNonParallelChild()
        {
            if (ActiveNonParallelChild != null)
            {
                ActiveNonParallelChild.SetActiveParent(this);
                ActiveNonParallelChild.InternalEnter();
            }
        }

        private void InternalOnEnter()
        {
            OnEnter();

            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].InternalOnEnter();
            }

            if (ActiveNonParallelChild != null)
            {
                ActiveNonParallelChild.InternalOnEnter();
            }
        }

        public virtual void Update()
        {
            UpdateNonParallelActiveChild();

            UpdateParallelChilds();

            OnUpdate();
        }

        private void UpdateNonParallelActiveChild()
        {
            if (ActiveNonParallelChild != null)
            {
                ActiveNonParallelChild.Update();
            }
        }

        private void UpdateParallelChilds()
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].Update();
            }
        }

        public void Exit()
        {
            ExitActiveNonParallelChild();

            ExitParallelChilds();

            OnExit();
        }

        private void ExitParallelChilds()
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                var current = parallelChilds[i];

                current.SetActiveParent(null);
                current.Exit();
            }
        }

        private void ExitActiveNonParallelChild()
        {
            if (ActiveNonParallelChild != null)
            {
                ActiveNonParallelChild.Exit();
                ActiveNonParallelChild.SetActiveParent(null);
                ActiveNonParallelChild = null;
            }
        }

        protected virtual void OnEnter()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void OnExit()
        {

        }

        private void AddOnAnyStateChangedListener(StateChangeEvent listener)
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].AddOnAnyStateChangedListener(listener);
            }

            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].AddOnAnyStateChangedListener(listener);
            }

            onStateChanged -= listener; //avoids repetition
            onStateChanged += listener;
        }

        private void RemoveOnAnyStateChangedListener(StateChangeEvent listener)
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].RemoveOnAnyStateChangedListener(listener);
            }

            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].RemoveOnAnyStateChangedListener(listener);
            }

            onStateChanged -= listener;
        }

        private void AddOnBeforeAnyTransitionateListener(StateChangeEvent listener)
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].AddOnBeforeAnyTransitionateListener(listener);
            }

            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].AddOnBeforeAnyTransitionateListener(listener);
            }

            onBeforeTransitionate -= listener; //avoids repetition
            onBeforeTransitionate += listener;
        }

        private void RemoveOnBeforeAnyTransitionateListener(StateChangeEvent listener)
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].RemoveOnBeforeAnyTransitionateListener(listener);
            }

            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].RemoveOnBeforeAnyTransitionateListener(listener);
            }

            onBeforeTransitionate -= listener;
        }

        public void AddChildToState<T>(HSMState<TState, TTrigger> child) where T : class
        {
            if (GetState<T>() is HSMState<TState, TTrigger> state)
            {
                state.AddChild(child);
            }
        }

        public void AddParallelChildToState<T>(HSMState<TState, TTrigger> child) where T : class
        {
            if (GetState<T>() is HSMState<TState, TTrigger> state)
            {
                state.AddParallelChild(child);
            }
        }

        public void AddChild(HSMState<TState, TTrigger> child)
        {
            if (ContainsImmediateChildState(child.StateId) == false)
            {
                childs.Add(child);
                child.AddParent(this);
            }
        }

        public void AddParallelChild(HSMState<TState, TTrigger> child)
        {
            if (ContainsImmediateChildState(child.StateId) == false)
            {
                parallelChilds.Add(child);
                child.AddParent(this);
            }
        }

        private void AddParent(HSMState<TState, TTrigger> parent)
        {
            parents.Add(parent);
        }

        public HSMState<TState, TTrigger> RemoveChildFromState<T>(TState stateId) where T : class
        {
            if (GetState<T>() is HSMState<TState, TTrigger> state)
            {
                return state.RemoveChild(stateId);
            }

            return null;
        }

        public HSMState<TState, TTrigger> RemoveChild(TState stateId)
        {
            var state = RemoveNonParallelChild(stateId);

            if (state == null)
            {
                state = RemoveParallelChild(stateId);
            }

            return state;
        }

        public HSMState<TState, TTrigger> RemoveNonParallelChild(TState stateId)
        {
            HSMState<TState, TTrigger> toRemove = GetImmediateChildState(stateId);

            if (childs.Remove(toRemove))
            {
                toRemove.RemoveParent(this);

                return toRemove;
            }

            return null;
        }

        public HSMState<TState, TTrigger> RemoveNonParallelChild(TState stateId, bool breakAllRelatedTransitions)
        {
            var removed = RemoveNonParallelChild(stateId);

            if (removed != null && breakAllRelatedTransitions)
            {
                BreakAllTransitionRelatedTo(stateId);
            }

            return removed;
        }

        public HSMState<TState, TTrigger> RemoveParallelChild(TState stateId)
        {
            HSMState<TState, TTrigger> toRemove = GetImmediateChildState(stateId);

            if (parallelChilds.Remove(toRemove))
            {
                toRemove.RemoveParent(this);
                return toRemove;
            }

            return null;
        }

        private void RemoveParent(HSMState<TState, TTrigger> parent)
        {
            parents.Remove(parent);
        }

        private bool DefaultComparer<T>(T arg1, T arg2)
        {
            return arg1.Equals(arg2);
        }

        public void SetStateComparer(Func<TState, TState, bool> comparerMethod)
        {
            if (comparerMethod == null)
            {
                throw new NullReferenceException("comparer method was null");
            }

            stateIdComparer = comparerMethod;
        }

        public void SetTriggerComparer(Func<TTrigger, TTrigger, bool> comparerMethod)
        {
            if (comparerMethod == null)
            {
                throw new NullReferenceException("comparer method was null");
            }

            triggerComparer = comparerMethod;
        }

        public HSMState<TState, TTrigger> GetNonParallelActiveLeaf()
        {
            HSMState<TState, TTrigger> current = this;

            while (current.ActiveNonParallelChild != null)
            {
                current = current.ActiveNonParallelChild;
            }

            return current;
        }

        public HSMState<TState, TTrigger> GetRoot()
        {
            HSMState<TState, TTrigger> current = this;

            while (current.parents.Count > 0)
            {
                current = current.parents[0];
            }

            return current;
        }

        public void ReplaceChild(HSMState<TState, TTrigger> newState)
        {
            HSMState<TState, TTrigger> state = GetImmediateChildState(newState.StateId);

            if (state == null)
            {
                throw new InvalidOperationException("state " + newState.StateId + " was not added to state machine");
            }
            else
            {
                int nonParallelIndex = childs.IndexOf(state);

                if (nonParallelIndex != -1)
                {
                    childs[nonParallelIndex] = newState;
                }
                else
                {
                    int parallelIndex = parallelChilds.IndexOf(state);

                    parallelChilds[parallelIndex] = newState;
                }
            }
        }

        public HSMState<TState, TTrigger> GetImmediateChildState(TState state)
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                var current = parallelChilds[i];

                if (stateIdComparer(current.StateId, state))
                {
                    return current;
                }
            }

            for (int i = 0; i < childs.Count; i++)
            {
                HSMState<TState, TTrigger> current = childs[i];

                if (stateIdComparer(current.StateId, state))
                {
                    return current;
                }
            }

            return default;
        }

        public HSMState<TState, TTrigger> GetImmediateNonParallelChildState(TState state)
        {
            for (int i = 0; i < childs.Count; i++)
            {
                var current = childs[i];

                if (stateIdComparer(current.StateId, state))
                {
                    return current;
                }
            }

            return null;
        }

        public HSMState<TState, TTrigger> GetImmediateParallelChildState(TState state)
        {
            for (int i = 0; i < parallelChilds.Count; i++)
            {
                var current = parallelChilds[i];

                if (stateIdComparer(current.StateId, state))
                {
                    return current;
                }
            }

            return null;
        }


        public bool ContainsImmediateChildState(TState state)
        {
            return GetImmediateChildState(state) != null;
        }

        public bool ContainsImmediateNonParallelChildState(TState state)
        {
            return GetImmediateNonParallelChildState(state) != null;
        }

        public bool ContainsImmediateParallelChildState(TState state)
        {
            return GetImmediateParallelChildState(state) != null;
        }

        public bool IsOnState(TState state, HSMState<TState, TTrigger> beginPoint)
        {
            return beginPoint.InternalIsOnState(state);
        }

        public bool IsOnState(HSMState<TState, TTrigger> state, HSMState<TState, TTrigger> beginPoint)
        {
            return beginPoint.InternalIsOnState(state);
        }

        public bool IsOnState(TState state)
        {
            var root = GetRoot();

            return root.InternalIsOnState(state);
        }

        public bool IsOnState(HSMState<TState, TTrigger> state)
        {
            var root = GetRoot();

            return root.InternalIsOnState(state);
        }

        private bool InternalIsOnState(TState state)
        {
            if (stateIdComparer(StateId, state))
            {
                return true;
            }

            for (int i = 0; i < parallelChilds.Count; i++)
            {
                if (parallelChilds[i].InternalIsOnState(state))
                {
                    return true;
                }
            }

            if (ActiveNonParallelChild != null)
            {
                return ActiveNonParallelChild.InternalIsOnState(state);
            }

            return false;
        }

        private bool InternalIsOnState(HSMState<TState, TTrigger> state)
        {
            if (this == state)
            {
                return true;
            }

            for (int i = 0; i < parallelChilds.Count; i++)
            {
                if (parallelChilds[i].InternalIsOnState(state))
                {
                    return true;
                }
            }

            if (ActiveNonParallelChild != null)
            {
                return ActiveNonParallelChild.InternalIsOnState(state);
            }

            return false;
        }

        public void SetInitialState(TState stateId)
        {
            if (ContainsImmediateChildState(stateId))
            {
                InitialChildState = stateId;
            }
            else
            {
                throw new InvalidOperationException("Initial state was not added to state machine");
            }

        }

        public bool MakeChildTransition(TState childStateFrom, TTrigger trigger, TState childStateTo)
        {
            FSMTransition<TState, TTrigger> transition = new FSMTransition<TState, TTrigger>(childStateFrom, trigger, childStateTo);

            if(ContainsChildTransition(childStateFrom, trigger) == false)
            {
                transitions.Add(transition);
                return true;
            }

            return false;
        }

        public bool MakeChildTransition(IFSMTransition<TState, TTrigger> transition)
        {
            if (ContainsImmediateChildState(transition.stateFrom) && ContainsImmediateChildState(transition.stateTo))
            {
                if (ContainsChildTransition(transition.stateFrom, transition.trigger))
                {
                    transitions.Add(transition);
                    return true;
                }
            }

            return false;
        }

        public bool ContainsChildTransition(TState childStateFrom, TTrigger trigger)
        {
            return GetTransition(childStateFrom, trigger) != null;
        }

        public bool IsChildStateAbleToTransitionateTo(TState childStateFrom, TState childStateTo)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> transition = transitions[i];

                if (stateIdComparer(childStateFrom, transition.stateFrom) && stateIdComparer(childStateTo, transition.stateTo))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAbleToTransitionateTo(TState stateTo)
        {
            for (int i = 0; i < parents.Count; i++)
            {
                HSMState<TState, TTrigger> parent = parents[i];

                if (parent.IsChildStateAbleToTransitionateTo(StateId, stateTo))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsParallelOf(HSMState<TState, TTrigger> state)
        {
            int parentIndex = parents.IndexOf(state);

            if (parentIndex != -1)
            {
                var parent = parents[parentIndex];

                return parent.parallelChilds.Contains(this);
            }

            return false;
        }

        public void BreakTransition(TState stateFrom, TTrigger trigger)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> current = transitions[i];

                if (stateIdComparer(current.stateFrom, stateFrom) && triggerComparer(current.trigger, trigger))
                {
                    transitions.RemoveAt(i);
                    break;
                }
            }
        }

        public void BreakAllTransitionRelatedTo(TState state)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                IFSMTransition<TState, TTrigger> current = transitions[i];

                if (stateIdComparer(current.stateFrom, state) || stateIdComparer(current.stateTo, state))
                {
                    transitions.RemoveAt(i);
                    i--;
                }
            }
        }

        public void BreakAllTransitions()
        {
            transitions.Clear();
        }

        public IFSMTransition<TState, TTrigger> GetTransition(TState stateFrom, TTrigger trigger)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                var current = transitions[i];

                if (stateIdComparer(current.stateFrom, stateFrom) && triggerComparer(current.trigger, trigger))
                {
                    return current;
                }
            }

            return default;
        }

        public IFSMTransition<TState, TTrigger>[] GetAllTransitions()
        {
            return transitions.ToArray();
        }

        public IFSMTransition<TState, TTrigger>[] GetTransitionsWithTargetState(TState stateTo)
        {
            List<IFSMTransition<TState, TTrigger>> transitions = null;

            for (int i = 0; i < this.transitions.Count; i++)
            {
                if (stateIdComparer(this.transitions[i].stateTo, stateTo))
                {
                    if (transitions == null)
                    {
                        transitions = new List<IFSMTransition<TState, TTrigger>>();
                    }

                    transitions.Add(this.transitions[i]);
                }
            }

            if (transitions.Count > 0)
            {
                return transitions.ToArray();
            }

            return null;
        }

        public IFSMTransition<TState, TTrigger>[] GetTransitionsWithStartState(TState stateFrom)
        {
            List<IFSMTransition<TState, TTrigger>> transitions = null;

            for (int i = 0; i < this.transitions.Count; i++)
            {
                if (stateIdComparer(this.transitions[i].stateFrom, stateFrom))
                {
                    if (transitions == null)
                    {
                        transitions = new List<IFSMTransition<TState, TTrigger>>();
                    }

                    transitions.Add(this.transitions[i]);
                }
            }

            if (transitions.Count > 0)
            {
                return transitions.ToArray();
            }

            return null;
        }

        public IFSMTransition<TState, TTrigger>[] GetTransitionsWithTrigger(TTrigger trigger)
        {
            List<IFSMTransition<TState, TTrigger>> transitions = null;

            for (int i = 0; i < this.transitions.Count; i++)
            {
                if (triggerComparer(this.transitions[i].trigger, trigger))
                {
                    if (transitions == null)
                    {
                        transitions = new List<IFSMTransition<TState, TTrigger>>();
                    }

                    transitions.Add(this.transitions[i]);
                }
            }

            if (transitions.Count > 0)
            {
                return transitions.ToArray();
            }

            return null;
        }

        public T GetNonParallelState<T>() where T : class
        {
            var root = GetRoot();

            return root.InternalGetNonParallelState<T>();
        }

        public T GetParallelState<T>() where T : class
        {
            var root = GetRoot();

            return root.InternalGetParallelState<T>();
        }

        public T GetState<T>() where T : class
        {
            var root = GetRoot();

            return root.InternalGetState<T>();
        }

        private T InternalGetState<T>() where T : class
        {
            var parallel = InternalGetParallelState<T>();

            if (parallel != null)
            {
                return parallel;
            }

            var nonParallel = InternalGetNonParallelState<T>();

            if (nonParallel != null)
            {
                return nonParallel;
            }

            return null;
        }

        private T InternalGetNonParallelState<T>() where T : class
        {
            if (this is T cached)
            {
                return cached;
            }

            for (int i = 0; i < childs.Count; i++)
            {
                var current = childs[i];

                var state = current.InternalGetState<T>();

                if (state != null)
                {
                    return state;
                }
            }

            return null;

        }

        private T InternalGetParallelState<T>() where T : class
        {
            if (this is T cached)
            {
                return cached;
            }

            for (int i = 0; i < parallelChilds.Count; i++)
            {
                var current = parallelChilds[i];

                var state = current.InternalGetState<T>();

                if (state != null)
                {
                    return state;
                }
            }

            return null;
        }

        public void ForeachInActiveHierarchy(Action<HSMState<TState, TTrigger>> action)
        {
            var root = GetRoot();

            root.InternalForeachInActiveHierarchy(action);
        }

        private void InternalForeachInActiveHierarchy(Action<HSMState<TState, TTrigger>> action)
        {
            action(this);

            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].InternalForeachInActiveHierarchy(action);
            }

            if (ActiveNonParallelChild != null)
            {
                ActiveNonParallelChild.InternalForeachInActiveHierarchy(action);
            }
        }

        public void ForeachInActiveHierarchyContinuesIfTrue(Func<HSMState<TState, TTrigger>, bool> predicate)
        {
            InternalForeachInActiveHierarchyContinuesIfTrue(predicate);
        }

        private void InternalForeachInActiveHierarchyContinuesIfTrue(Func<HSMState<TState, TTrigger>, bool> predicate)
        {
            if (predicate(this) == false)
            {
                return;
            }

            for (int i = 0; i < parallelChilds.Count; i++)
            {
                parallelChilds[i].InternalForeachInActiveHierarchyContinuesIfTrue(predicate);
            }

            if (ActiveNonParallelChild != null)
            {
                ActiveNonParallelChild.InternalForeachInActiveHierarchyContinuesIfTrue(predicate);
            }
        }

        public int GetActiveHierarchyStateCount()
        {
            var root = GetRoot();

            int cont = 0;

            return root.InternalGetActiveHierarchyCount(ref cont);
        }

        private int InternalGetActiveHierarchyCount(ref int cont)
        {
            cont += ParallelChildCount + 1;

            if (ActiveNonParallelChild != null)
            {
                return ActiveNonParallelChild.InternalGetActiveHierarchyCount(ref cont);
            }

            return cont;
        }

        public bool SendEvent(TTrigger trigger)
        {
            var root = GetRoot();

            return root.InternalSendEvent(trigger);
        }

        private bool InternalSendEvent(TTrigger trigger)
        {
            HSMState<TState, TTrigger> current = GetNonParallelActiveLeaf();

            while (current != null)
            {
                for (int i = 0; i < current.parallelChilds.Count; i++)
                {
                    if (current.parallelChilds[i].InternalSendEvent(trigger))
                    {
                        return true;
                    }
                }

                if (current.HandleEvent(trigger) == false)
                {
                    if (current.childs.Count > 0)
                    {
                        IFSMTransition<TState, TTrigger> transition = current.GetTransition(current.ActiveNonParallelChild.StateId, trigger);

                        if (transition != null
                            && current.ContainsImmediateNonParallelChildState(transition.stateFrom)
                            && current.ContainsImmediateNonParallelChildState(transition.stateTo)
                            && transition.IsValidTransition())
                        {
                            current.Transitionate(transition);

                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }

                if (current.IsParallelOf(current.ActiveParent))
                {
                    return false;
                }

                current = current.ActiveParent;
            }

            return false;
        }

        protected virtual bool HandleEvent(TTrigger trigger)
        {
            return false;
        }

        private void Transitionate(IFSMTransition<TState, TTrigger> transition)
        {
            var stateFrom = ActiveNonParallelChild;
            var stateTo = GetImmediateChildState(transition.stateTo);

            if (onBeforeTransitionate != null)
            {
                onBeforeTransitionate(stateFrom, transition.trigger, stateTo);
            }

            ExitActiveNonParallelChild();

            ActiveNonParallelChild = stateTo;

            ActiveNonParallelChild.SetActiveParent(this);
            ActiveNonParallelChild.InternalEnter();

            if (onStateChanged != null)
            {
                onStateChanged(stateFrom, transition.trigger, stateTo);
            }

            ActiveNonParallelChild.InternalOnEnter();
        }

        public int GetUniqueStateCountFromRoot()
        {
            return GetRoot().GetUniqueStateCount();
        }

        public int GetUniqueStateCount()
        {
            int cont = 0;

            foreach (HSMState<TState, TTrigger> state in this)
            {
                cont++;
            }

            return cont;
        }

        public uint GetHeightFromRoot()
        {
            return GetRoot().GetHeight();
        }

        public uint GetHeight()
        {
            uint height = 1;

            for (int i = 0; i < childs.Count; i++)
            {
                uint currentHeight = 1;
                currentHeight += childs[i].GetHeight();

                if (currentHeight > height)
                {
                    height = currentHeight;
                }
            }

            return height;
        }

        public void ForeachInHighestHierarchy(Action<HSMState<TState, TTrigger>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }

            action(this);

            int indexOfHighest = -1;
            uint height = 0;

            for (int i = 0; i < childs.Count; i++)
            {
                uint currentHeight = childs[i].GetHeight();

                if (currentHeight > height)
                {
                    height = currentHeight;
                    indexOfHighest = i;
                }
            }

            if (indexOfHighest != -1)
            {
                HSMState<TState, TTrigger> state = childs[indexOfHighest];
                state.ForeachInHighestHierarchy(action);
            }
        }

        public void ForeachInHighestHierarchy<TRefParameter>(HSMRefAction<TRefParameter> action, ref TRefParameter refParameter)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }

            action(this, ref refParameter);

            int indexOfHighest = -1;
            uint height = 0;

            for (int i = 0; i < childs.Count; i++)
            {
                uint currentHeight = childs[i].GetHeight();

                if (currentHeight > height)
                {
                    height = currentHeight;
                    indexOfHighest = i;
                }
            }

            if (indexOfHighest != -1)
            {
                HSMState<TState, TTrigger> state = childs[indexOfHighest];
                state.ForeachInHighestHierarchy(action, ref refParameter);
            }
        }

        public IEnumerator<HSMState<TState, TTrigger>> GetEnumerator()
        {
            if (enumerator == null)
            {
                enumerator = new HSMStateEnumerator(this);
            }

            enumerator.Reset();

            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class HSMStateEnumerator : IEnumerator<HSMState<TState, TTrigger>>
        {
            public HSMState<TState, TTrigger> Current { get; set; }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            private HashSet<HSMState<TState, TTrigger>> enumeratedStates;
            private Stack<HSMState<TState, TTrigger>> stateStack;
            private HSMState<TState, TTrigger> root;


            public HSMStateEnumerator(HSMState<TState, TTrigger> state)
            {
                enumeratedStates = new HashSet<HSMState<TState, TTrigger>>();
                stateStack = new Stack<HSMState<TState, TTrigger>>();

                root = state;
            }

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                if (stateStack.Count > 0)
                {
                    Current = stateStack.Pop();
                    if (enumeratedStates.Contains(Current))
                    {
                        Current = null;
                        return MoveNext();
                    }
                    else
                    {
                        enumeratedStates.Add(Current);

                        for (int i = 0; i < Current.parallelChilds.Count; i++)
                        {
                            stateStack.Push(Current.parallelChilds[i]);
                        }

                        for (int i = 0; i < Current.childs.Count; i++)
                        {
                            stateStack.Push(Current.childs[i]);
                        }

                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                enumeratedStates.Clear();
                stateStack.Clear();
                stateStack.Push(root);
            }
        }

    }
}

