﻿namespace MMO.Base.Infrastructure {
    public interface IRpcResponse<TResult> : IRpcResponse {
        TResult Result { get; }
    }
}