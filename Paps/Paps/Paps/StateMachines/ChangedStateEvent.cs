using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paps.StateMachines
{
    public delegate void ChangedStateEvent<TState, TTrigger>(IFSMState<TState, TTrigger> previous, TTrigger trigger, IFSMState<TState, TTrigger> current);
    public delegate void ChangedStateEvent<TState>(IFSMState<TState> previous, IFSMState<TState> current);
}
