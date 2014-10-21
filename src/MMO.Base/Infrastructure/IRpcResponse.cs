using System.Collections.Generic;

namespace MMO.Base.Infrastructure {
    public interface IRpcResponse {
        IEnumerable<string> OperationErrors { get; }
        bool IsValid { get; }
        object UntypedResult { get; }
    }
}