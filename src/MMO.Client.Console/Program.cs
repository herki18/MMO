using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net.Config;
using MMO.Base.Infrastructure;
using MMO.Client.Infrastructure;

namespace MMO.Client.Console
{
    class Program
    {
        static void Main(string[] args) {
            BasicConfigurator.Configure();

            var transport = new PhotonClientTransport();
            var context = new ConsoleContext(new SimpleSerializer(), transport);
            context.TypeRegistry.ScanAssembly(typeof(Program).Assembly);


            transport.Connect("herki.cloudapp.net:5055", "MMO.Server.Master");

            while (true) {
                transport.Service();
                Thread.Sleep(10);
            }
        }
    }
}
