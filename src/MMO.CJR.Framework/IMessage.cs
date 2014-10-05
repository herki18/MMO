using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO.CJR.Framework
{
    public interface IMessage
    {
        MessageType MessageType { get; }
        byte Code { get; }
        int? SubCode { get; }
        Dictionary<byte, object> Parameteres { get; } 
    }
}
