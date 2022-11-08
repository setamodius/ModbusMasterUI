using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NModbus;
using static ModbusMasterUI.Actor.ViewActor.ViewActorMessage;
using ModbusMasterUI.Protocol;

namespace ModbusMasterUI.Actor
{
    public class ModbusMasterActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Logging.GetLogger(Context);
        private readonly CreateTcpDeviceMessage SlaveData;
        private SetNewPool PoolData;
        private DateTime pooldate;
        private readonly IModbusMaster master;
        private readonly TcpClient tcpClient;

        public ModbusMasterActor(CreateTcpDeviceMessage slavedata)
        {
            SlaveData = slavedata;
            logger.Info($"Modbus Connection Actor created - {SlaveData.FullName}");
            ReceiveAsync<ModbusMasterMessage.PoolSlave>(PoolSlave);
            Receive<SetNewPool>(SetPool);
            tcpClient = new TcpClient(SlaveData.SlaveIp, SlaveData.SlavePort);
            var factory = new ModbusFactory();
            master = factory.CreateMaster(tcpClient);
            Context.System.Scheduler.ScheduleTellRepeatedly(
             TimeSpan.FromSeconds(.5),
             TimeSpan.FromMilliseconds(SlaveData.PoolingRate),
             Self,
             new ModbusMasterMessage.PoolSlave(),
             Self);
        }
        private void SetPool(SetNewPool obj)
        {
            PoolData = obj;
        }
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(maxNrOfRetries: 0, withinTimeRange:
                TimeSpan.FromSeconds(300),
                localOnlyDecider: exception =>
                {
                    return Directive.Restart;
                }
                );
        }

        private async Task PoolSlave(ModbusMasterMessage.PoolSlave _)
        {
            if (PoolData == null)
            {
                return;
            }
            if (DateTime.Now.Subtract(pooldate) < TimeSpan.FromMilliseconds(SlaveData.PoolingRate))
            {
                return;
            }

            pooldate = DateTime.Now;

            if (master == null)
            {
                return;
            }

            Dictionary<ushort, bool> coilstatusdata = new();
            Dictionary<ushort, bool> inputstatusdata = new();
            Dictionary<ushort, ushort> holdingregisterdata = new();
            Dictionary<ushort, ushort> inputregisterdata = new();

            foreach (var (startaddress, length) in PoolData.CoilStatusPools)
            {
                bool[] coilstatus = await master.ReadCoilsAsync((byte)SlaveData.DeviceId, (ushort)(startaddress - 1), length);
                for (int i = 0; i < length; i++)
                {
                    coilstatusdata.Add((ushort)(startaddress + i), coilstatus[i]);
                }
            }

            foreach (var (startaddress, length) in PoolData.InputStatusPools)
            {
                bool[] inputstatus = await master.ReadInputsAsync((byte)SlaveData.DeviceId, (ushort)(startaddress - 1), length);
                for (int i = 0; i < length; i++)
                {
                    inputstatusdata.Add((ushort)(startaddress + i), inputstatus[i]);
                }
            }

            foreach (var (startaddress, length) in PoolData.HoldingRegisterPools)
            {
                ushort[] holdingregister = await master.ReadHoldingRegistersAsync((byte)SlaveData.DeviceId, (ushort)(startaddress - 1), length);
                for (int i = 0; i < length; i++)
                {
                    holdingregisterdata.Add((ushort)(startaddress + i), holdingregister[i]);
                }
            }

            foreach (var (startaddress, length) in PoolData.InputRegisterPools)
            {
                ushort[] inputregister = await master.ReadInputRegistersAsync((byte)SlaveData.DeviceId, (ushort)(startaddress - 1), length);
                for (int i = 0; i < length; i++)
                {
                    inputregisterdata.Add((ushort)(startaddress + i), inputregister[i]);
                }
            }
            Context.Parent.Tell(new ModbusMasterMessage.PoolResult(SlaveData.FullName, coilstatusdata, inputstatusdata, holdingregisterdata, inputregisterdata));

        }

        public class ModbusMasterMessage
        {
            public class PoolResult
            {
                public PoolResult(string fullname, IReadOnlyDictionary<ushort, bool> coilStatusReadData, IReadOnlyDictionary<ushort, bool> ınputStatusReadData, IReadOnlyDictionary<ushort, ushort> holdingRegisterReadData, IReadOnlyDictionary<ushort, ushort> inputRegisterReadData)
                {
                    Fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
                    CoilStatusReadData = coilStatusReadData ?? throw new ArgumentNullException(nameof(coilStatusReadData));
                    InputStatusReadData = ınputStatusReadData ?? throw new ArgumentNullException(nameof(ınputStatusReadData));
                    HoldingRegisterReadData = holdingRegisterReadData ?? throw new ArgumentNullException(nameof(holdingRegisterReadData));
                    InputRegisterReadData = inputRegisterReadData ?? throw new ArgumentNullException(nameof(inputRegisterReadData));
                }

                public string Fullname { get; private set; }
                public IReadOnlyDictionary<ushort, bool> CoilStatusReadData { get; private set; }               
                public IReadOnlyDictionary<ushort, bool> InputStatusReadData { get; private set; }
                public IReadOnlyDictionary<ushort, ushort> HoldingRegisterReadData { get; private set; }
                public IReadOnlyDictionary<ushort, ushort> InputRegisterReadData { get; private set; }

            }

           
            internal class PoolSlave
            {
            }


        }
    }
}
