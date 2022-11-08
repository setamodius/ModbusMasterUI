using Akka.Actor;
using Akka.Event;
using CommunityToolkit.Mvvm.Messaging;
using ModbusMasterUI.Protocol;
using ModbusMasterUI.ViewModel;
using ModbusMasterUI.ViewModel.Message;
using System;
using System.Collections.Generic;
using static ModbusMasterUI.Actor.ModbusMasterActor.ModbusMasterMessage;
using static ModbusMasterUI.Actor.ViewActor.ViewActorMessage;

namespace ModbusMasterUI.Actor
{
    public class ViewActor : ReceiveActor
    {
        private readonly MainViewModel? mainView;
        private readonly ILoggingAdapter logger = Logging.GetLogger(Context);
        private readonly IUntypedActorContext? thisContext;

        public ViewActor()
        {
            mainView = App.Current.Resources["MainViewModel"] as MainViewModel;
            thisContext = Context;
            Receive<PoolResult>(PoolResultReceived);

            WeakReferenceMessenger.Default.Register<RemoveSlaveMessage>(this, (r, m) =>
            {

            });
            WeakReferenceMessenger.Default.Register<AddSlaveMessage>(this, (r, m) =>
            {
                thisContext.Parent.Tell(new CreateTcpDeviceMessage(
                    m.AddModel.ModbusSlave.FullName,
                    m.AddModel.ModbusSlave.SlaveIp,
                    m.AddModel.ModbusSlave.SlavePort,
                    m.AddModel.ModbusSlave.DeviceId,
                    m.AddModel.ModbusSlave.PoolingRate));
            });
            WeakReferenceMessenger.Default.Register<SendPoolMessage>(this, (r, m) =>
            {                
                thisContext.Parent.Tell(new SetNewPool(
                    m.CurrentModbusModel.ModbusSlave.FullName,
                    m.CurrentModbusModel.CoilStatusPools.GetPools(),
                    m.CurrentModbusModel.InputStatusPools.GetPools(),
                    m.CurrentModbusModel.HoldingRegisterPools.GetPools(),
                    m.CurrentModbusModel.InputRegisterPools.GetPools()));
            });
        }

        private void PoolResultReceived(PoolResult obj)
        {
            WeakReferenceMessenger.Default.Send(new PoolResultMessage(
                obj.Fullname,
                obj.CoilStatusReadData,
                obj.InputStatusReadData,
                obj.HoldingRegisterReadData,
                obj.InputRegisterReadData));
        }

        public class ViewActorMessage
        {
            public class CreateTcpDeviceMessage
            {
                public CreateTcpDeviceMessage(string fullname, string slaveIp, int slavePort, int deviceId, int poolingrate)
                {
                    FullName = fullname ?? throw new ArgumentNullException(nameof(fullname));
                    SlaveIp = slaveIp ?? throw new ArgumentNullException(nameof(slaveIp));
                    SlavePort = slavePort;
                    DeviceId = deviceId;
                    PoolingRate = poolingrate;
                }
                public string FullName { get; private set; }
                public string SlaveIp { get; private set; }
                public int SlavePort { get; private set; }
                public int DeviceId { get; private set; }
                public int PoolingRate { get; private set; }

            }
            public class DeviceStatusChangedMessage
            {
                public DeviceStatusChangedMessage(bool isOnline)
                {
                    IsOnline = isOnline;
                }

                public bool IsOnline { get; private set; }
            }

            public class SelfCheckReceived { }

            public class SetNewPool
            {
                public SetNewPool(
                    string fullName,
                    IReadOnlyList<(ushort startaddress, ushort length)> coilStatusPools,
                    IReadOnlyList<(ushort startaddress, ushort length)> inputStatusPools,
                    IReadOnlyList<(ushort startaddress, ushort length)> holdingRegisterPools,
                    IReadOnlyList<(ushort startaddress, ushort length)> inputRegisterPools)
                {
                    FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
                    CoilStatusPools = coilStatusPools ?? throw new ArgumentNullException(nameof(coilStatusPools));
                    InputStatusPools = inputStatusPools ?? throw new ArgumentNullException(nameof(inputStatusPools));
                    HoldingRegisterPools = holdingRegisterPools ?? throw new ArgumentNullException(nameof(holdingRegisterPools));
                    InputRegisterPools = inputRegisterPools ?? throw new ArgumentNullException(nameof(inputRegisterPools));
                }

                public string FullName { get; private set; }
                public IReadOnlyList<(ushort startaddress, ushort length)> CoilStatusPools { get; private set; }
                public IReadOnlyList<(ushort startaddress, ushort length)> InputStatusPools { get; private set; }
                public IReadOnlyList<(ushort startaddress, ushort length)> HoldingRegisterPools { get; private set; }
                public IReadOnlyList<(ushort startaddress, ushort length)> InputRegisterPools { get; private set; }



            }


        }
    }
}
