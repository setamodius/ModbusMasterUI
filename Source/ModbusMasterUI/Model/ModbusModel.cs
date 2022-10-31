using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.Model
{
    public class ModbusModel : ObservableObject
    {
        private ModbusSlaveModel modbusSlave = new()
        {
            SlaveIp = "localhost",
            SlavePort = 502,
            DeviceId = 1
        };

        public ModbusSlaveModel ModbusSlave
        {
            get => modbusSlave;
            set => SetProperty(ref modbusSlave, value);
        }

        private ModbusPoolModel modbusPool = new()
        {
            PoolingRate = 1000,
        };

        public ModbusPoolModel ModbusPool
        {
            get => modbusPool;
            set => SetProperty(ref modbusPool, value);
        }

    }
}
