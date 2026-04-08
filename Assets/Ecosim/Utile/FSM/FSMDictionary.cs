using System;
using System.Collections.Generic;

namespace Untils
{
    public abstract class FSMDictionary<TEnum> : IFSM<TEnum, Dictionary<TEnum, IFSMState<TEnum>>> where TEnum : Enum
    {
        public Dictionary<TEnum, IFSMState<TEnum>> States { get; protected set; } = new();
        public Stack<IFSMState<TEnum>> StackStates { get; protected set; } = new();

        public bool HasState => StackStates.Count > 0;

        /// <summary>
        /// Переход в стадию c выходом из предыдущей
        /// </summary>
        public virtual void EnterIn(TEnum state)
        {
            if (States.TryGetValue(state, out IFSMState<TEnum> stateObj))
            {
                if (HasState)
                {
                    StackStates.Pop().Exit();
                }

                PushAndEnterIn(stateObj);
            }
            else
            {
                ThrowStateNotRegistered(state);
            }
        }

        /// <summary>
        /// Переход в стадию с приостановкой текущего состояния и его постановкай в стек
        /// </summary>
        public virtual void SuspendAndEnterIn(TEnum state)
        {
            if (States.TryGetValue(state, out IFSMState<TEnum> stateObj))
            {
                if (HasState)
                {
                    var iSuspendStateObj = (ISuspendFSMState<TEnum>)StackStates.Peek();
                    iSuspendStateObj.Suspend();
                    PushAndEnterIn(stateObj);
                }
            }
            else
            {
                ThrowStateNotRegistered(state);
            }
        }

        /// <summary>
        /// Выход из текущей стадии и возобновление выполнения предыдущей
        /// </summary>
        public virtual void ExitAndResume()
        {
            if (StackStates.Count > 1)
            {
                StackStates.Pop().Exit();
                var iSuspendStateObj = (ISuspendFSMState<TEnum>)StackStates.Peek();
                iSuspendStateObj.Resume();
            }
            else
            {
                ThrowLastStateExit();
            }
        }

        private void ThrowStateNotRegistered(TEnum state) => throw new Exception($"[{GetType().Name}] Стадия '{state}' не зарегестрирована");
        private void ThrowLastStateExit() => throw new Exception($"[{GetType().Name}] Недопустимая попытка выхода из последнего активного состояния");

        private void PushAndEnterIn(IFSMState<TEnum> stateObj)
        {
            StackStates.Push(stateObj);
            StackStates.Peek().Enter();
        }
    }
}
