namespace MMO.Base.Infrastructure {
    public enum EventCode : byte {
        SyncSystemComponentMap,
        AddSystem,
        RemoveSystem,
        InvokeMethodOnSystem,
    }
}