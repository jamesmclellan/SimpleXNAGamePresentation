using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class DebounceItem
    {
        Action _action;
        bool _flag;

        public DebounceItem(Action actionToTake)
        {
            _action = actionToTake;
            _flag = false;
        }

        public void Update(bool activate)
        {
            if (activate)
            {
                // take action if the flag isn't set
                if (!_flag)
                {
                    _action();
                }
            }
            _flag = activate;
        }
    }

    public class DebounceItem<T>
    {
        Action<T> _action;
        bool _flag;

        public DebounceItem(Action<T> actionToTake)
        {
            _action = actionToTake;
            _flag = false;
        }

        public void Update(bool activate, T arguments)
        {
            if (activate)
            {
                // take action if the flag isn't set
                if (!_flag)
                {
                    _action(arguments);
                }
            }
            _flag = activate;
        }
    }
}
