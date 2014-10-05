using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO.CJR.Framework
{
    public interface IBackgroundThread {
        void Setud();
        void Run(Object threadContext);
        void Stop();
    }
}
