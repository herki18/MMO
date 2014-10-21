namespace MMO.Base.Infrastructure {
    public enum OperationParameter : byte {
        MethodId,
        ArgumentBytes,
        SystemInvokeId,
        ContextType,
        ResultBytes,
        ResponseIsValid,
        ResponseOperationErrors
    }
}