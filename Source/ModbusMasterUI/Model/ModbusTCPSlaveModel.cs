using CommunityToolkit.Mvvm.ComponentModel;

namespace ModbusMasterUI.Model
{
    public class ModbusTCPSlaveModel : ObservableObject
    {
        public ModbusTCPSlaveModel()
        {
            SlaveIp = "localhost";
            SlavePort = 502;
            DeviceId = 1;
            poolingRate = 1000;
        }

        private string slaveIp = "";
        public string SlaveIp
        {
            get => slaveIp;
            set
            {
                SetProperty(ref slaveIp, value);
                SetFullName();
            }
        }

        private int slavePort;
        public int SlavePort
        {
            get => slavePort;
            set
            {
                SetProperty(ref slavePort, value);
                SetFullName();
            }
        }
        private int deviceId;
        public int DeviceId
        {
            get => deviceId;
            set
            {
                SetProperty(ref deviceId, value);
                SetFullName();
            }
        }

        private string fullName = "";
        public string FullName
        {
            get => fullName;
            private set => SetProperty(ref fullName, value);
        }

        private int poolingRate;
        public int PoolingRate
        {
            get => poolingRate;
            set => SetProperty(ref poolingRate, value);
        }

        private void SetFullName()
        {
            FullName = $"TCP-{SlaveIp}:{SlavePort}-{DeviceId}";
        }
    }
}
