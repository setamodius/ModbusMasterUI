using Akka.Actor;
using Akka.Configuration;
using ModbusMasterUI.Actor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ModbusMasterUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public ActorSystem? ActorSystem;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var config = ConfigurationFactory.ParseString(@"
            akka {
                stdout-loglevel = INFO
                loglevel = INFO
                log-config-on-start = on
                log-dead-letters-during-shutdown = off
                log-dead-letters = 0
                actor {
                    debug {
                         receive = on
                         autoreceive = on
                         lifecycle = on
                         event-stream = on
                         unhandled = on
                    }
                }
            }  ");

            ActorSystem = ActorSystem.Create("ModbusActorSystem", config);

            var manageractor = ActorSystem.ActorOf(Props.Create<ManagerActor>(), "ManagerActor");


        }
    }
}
