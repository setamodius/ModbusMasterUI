using Akka.Actor;
using ModbusMasterUI.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModbusMasterUI.Actor.ModbusMasterActor.ModbusMasterMessage;

namespace ModbusMasterUI.Actor
{
    public  class ManagerActor : ReceiveActor
    {
        IActorRef viewActor;
        public ManagerActor()
        {
            viewActor = Context.ActorOf(Props.Create(() => new ViewActor()), "ViewActor");
            Receive<ViewActor.ViewActorMessage.CreateTcpDeviceMessage>(CreateTcpDevice);
            Receive<ViewActor.ViewActorMessage.SetNewPool>(SendPoolToDevice);
            Receive<PoolResult>(PoolResultReceived);
        }

        private void PoolResultReceived(PoolResult obj)
        {
            viewActor.Forward(obj);
        }      

        private void SendPoolToDevice(ViewActor.ViewActorMessage.SetNewPool obj)
        {
            var devicename = obj.FullName;
            var child = Context.Child(devicename);

            if (child == ActorRefs.Nobody)
            {
                return;
            }

            child.Forward(obj);
        }

        private void CreateTcpDevice(ViewActor.ViewActorMessage.CreateTcpDeviceMessage obj)
        {
            var devicename = obj.FullName;
            var child = Context.Child(devicename);

            if (child == ActorRefs.Nobody)
            {
                var props = Props.Create(() => new ModbusDeviceActor(obj));
                child = Context.ActorOf(props, devicename);               
            }

            
        }

        public class ManagerMessage
        {          

            
        }
    }
}
