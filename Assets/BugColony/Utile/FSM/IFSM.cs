using System;
using System.Collections.Generic;

namespace Untils
{
    public interface IFSM<TEnum, TStates> where TEnum : Enum
    {
        public TStates States { get; }
        public Stack<IFSMState<TEnum>> StackStates { get; }

        bool HasState { get; }
    }
}