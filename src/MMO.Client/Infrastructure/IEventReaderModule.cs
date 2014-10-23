using System.Collections.Generic;

namespace MMO.Client.Infrastructure {
    public interface IEventReaderModule {
        IEnumerable<EventReaderModuleRegistration> GetRegistrations();
    }
}