using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DownGrade.Interfaces
{
    public interface IObserver
    {
        void update(int value);
    }
}
