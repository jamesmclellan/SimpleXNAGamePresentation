using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Utilities
{
    public class LoopingList
    {
        private int _currentIndex;
        private int _min = 0;
        private int _max;

        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
        }

        public LoopingList(int count)
        {
            _max = count - 1;
        }

        public void Set(int value)
        {
            if ((value > _max) || (value < _min))
            {
                _currentIndex = _min;
            }
            else
            {
                _currentIndex = value;
            }
        }

        public void Increment()
        {
            if ((_currentIndex + 1) > _max)
            {
                _currentIndex = _min;
            }
            else
            {
                _currentIndex += 1;
            }
        }

        public void Decrement()
        {
            if ((_currentIndex - 1) < _min)
            {
                _currentIndex = _max;
            }
            else
            {
                _currentIndex -= 1;
            }
        }

    }
}
