using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Behaviours
{
    internal class Behaviour : IBehaviour
    {
        private Action _action;

        public Behaviour(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action.Invoke();
        }
    }

    internal class Behaviour<TParam> : IBehaviour<TParam>
    {
        private Action<TParam> _action;

        public Behaviour(Action<TParam> action)
        {
            _action = action;
        }

        public void Execute(TParam param)
        {
            _action.Invoke(param);
        }
    }

    internal class Behaviour<TParam, TResult> : IBehaviour<TParam, TResult>
    {
        private Func<TParam, TResult> _func;

        public Behaviour(Func<TParam, TResult> func)
        {
            _func = func;
        }

        public TResult Execute(TParam param)
        {
            return _func.Invoke(param);
        }
    }
}
