using System;

namespace Untils
{
    public interface IFSMState<TEnum> where TEnum : Enum
    {
        public TEnum State { get; }

        public void Enter();
        public void Exit();
    }
    
    public interface ISuspendFSMState<TEnum> : IFSMState<TEnum> where TEnum : Enum
    {
        void Suspend();
        void Resume();
    }
}