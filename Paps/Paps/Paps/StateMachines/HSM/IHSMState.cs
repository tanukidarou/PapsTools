using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paps.StateMachines
{
    public interface IHSMState<TState, TTrigger> : IFSM, IFSMState<TState, TTrigger>
    {
        IHSMState<TState, TTrigger> ActiveParent { get; }
        IHSMState<TState, TTrigger> CurrentChild { get; }
        IHSMState<TState, TTrigger> InitialState { get; }

        IHSMState<TState, TTrigger> GetRoot();
        IHSMState<TState, TTrigger> GetLeaf();
        
        new void Update();

        void AddParent(IHSMState<TState, TTrigger> state);

        bool IsOnState(TState state);
        bool IsOnState(IHSMState<TState, TTrigger> state);


    }
}
