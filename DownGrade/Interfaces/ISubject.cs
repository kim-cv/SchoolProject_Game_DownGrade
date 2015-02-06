using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DownGrade.Interfaces
{
    public interface ISubject
    {
        void register(Sprite o);
        void unregister(Sprite o);
        void notify(Sprite sp1, Sprite sp2);
    }
}
