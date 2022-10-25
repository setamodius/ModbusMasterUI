using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.Model
{
    public class ModbusSlaveModel : ObservableObject
    {
        private string slaveIp = "";

        public string SlaveIp
        {
            get => slaveIp;
            set => SetProperty(ref slaveIp, value);
        }

        private int slavePort;

        public int SlavePort
        {
            get => slavePort;
            set => SetProperty(ref slavePort, value);
        }

        private int deviceId;

        public int DeviceId
        {
            get => deviceId;
            set => SetProperty(ref deviceId, value);
        }

    }
}
