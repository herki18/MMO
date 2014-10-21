using MMO.Base.Infrastructure;

namespace MMO.Base.Components.Systems {
    [ComponentInterface]
    public interface ILoginSystemServer {
        void BasicTest(string message);
    }

    [ComponentInterface]
    public interface ILoginSystemClient {
        void BasicClientTest(string message);
    }
}