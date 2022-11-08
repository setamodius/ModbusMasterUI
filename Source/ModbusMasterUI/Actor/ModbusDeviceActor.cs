using Akka.Actor;
using Akka.Event;
using ModbusMasterUI.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static ModbusMasterUI.Actor.ModbusMasterActor.ModbusMasterMessage;
using static ModbusMasterUI.Actor.ViewActor.ViewActorMessage;

namespace ModbusMasterUI.Actor
{
    public class ModbusDeviceActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Logging.GetLogger(Context);
        private readonly CreateTcpDeviceMessage slaveData;
        private bool IsOnline = false;
        private DateTime lastPoolingDate = DateTime.Now;
        private IActorRef? modbusActor;
        public ModbusDeviceActor(CreateTcpDeviceMessage message)
        {
            slaveData = message;
            Receive<ModbusDeviceMessage.SelfCheckReceived>(SelfCheck);
            Receive<PoolResult>(PoolResultReceived);
            Receive<SetNewPool>(SetPool);
        }

        private void PoolResultReceived(PoolResult obj)
        {
            Context.Parent.Forward(obj);
        }

        private void SetPool(SetNewPool obj)
        {
            if (modbusActor == null)
            {
                GenerateModbusMasterActor();
            }
            modbusActor.Forward(obj);
        }

        private void SelfCheck(ModbusDeviceMessage.SelfCheckReceived obj)
        {
            if (DateTime.Now.Subtract(lastPoolingDate) > TimeSpan.FromSeconds(10))
            {
                SetConnectionStatus(false);

                var child = Context.Child("ModbusMasterActor");

                if (child == ActorRefs.Nobody)
                {
                    var childProps = Props.Create<ModbusMasterActor>(slaveData);
                    modbusActor = Context.ActorOf(childProps, "ModbusMasterActor");
                }
                else
                {
                    Context.Stop(modbusActor);
                }
            }
        }

        private void SetConnectionStatus(bool IsConnected)
        {
            if (IsOnline != IsConnected)
            {
                IsOnline = IsConnected;
                Context.Parent.Tell(new ModbusDeviceMessage.DeviceStatusChangedMessage(IsOnline));
                if (IsOnline)
                {
                    logger.Info($"Modbus online - {slaveData.FullName}");
                }
                else
                {
                    logger.Warning($"Modbus failed - {slaveData.FullName}");
                }
            }
        }
        private void GenerateModbusMasterActor()
        {
            var childProps = Props.Create<ModbusMasterActor>(slaveData);
            modbusActor = Context.ActorOf(childProps, "ModbusMasterActor");

        }

        public class ModbusDeviceMessage
        {
            public class StartPooling { }
            public class DeviceStatusChangedMessage
            {
                public DeviceStatusChangedMessage(bool isOnline)
                {
                    IsOnline = isOnline;
                }
                public bool IsOnline { get; private set; }
            }

            public class SelfCheckReceived { }


        }
    }
}
