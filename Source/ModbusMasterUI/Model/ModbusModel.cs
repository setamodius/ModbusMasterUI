using Akka.Actor;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ModbusMasterUI.ViewModel.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModbusMasterUI.Model
{
    public class ModbusModel : ObservableObject
    {
        public ModbusModel()
        {
            CloseCommand = new RelayCommand<ModbusModel>(RemoveSelectedSlave);
            PoolCommand = new RelayCommand(PoolSlave);
        }

        private void PoolSlave()
        {
            WeakReferenceMessenger.Default.Send(new SendPoolMessage(this));
        }

        private void RemoveSelectedSlave(ModbusModel? obj)
        {
            if (obj == null)
            {
                return;
            }
            WeakReferenceMessenger.Default.Send(new RemoveSlaveMessage(obj));
        }

        internal void SetResults(PoolResultMessage m)
        {
            CoilStatusPools.SetBoolValues(m.CoilStatusReadData);
            InputStatusPools.SetBoolValues(m.InputStatusReadData);
            HoldingRegisterPools.SetUshortValues(m.HoldingRegisterReadData);
            InputRegisterPools.SetUshortValues(m.InputRegisterReadData);
        }

        private ModbusTCPSlaveModel modbusSlave = new()
        {
            SlaveIp = "localhost",
            SlavePort = 502,
            DeviceId = 1
        };

        public ModbusTCPSlaveModel ModbusSlave
        {
            get => modbusSlave;
            set => SetProperty(ref modbusSlave, value);
        }

        public ModbusPoolCollectionModel CoilStatusPools { get; set; } = new ModbusPoolCollectionModel();
        public ModbusPoolCollectionModel InputStatusPools { get; set; } = new ModbusPoolCollectionModel();
        public ModbusPoolCollectionModel HoldingRegisterPools { get; set; } = new ModbusPoolCollectionModel();
        public ModbusPoolCollectionModel InputRegisterPools { get; set; } = new ModbusPoolCollectionModel();
        
        public IRelayCommand<ModbusModel> CloseCommand { get; }

        
        public IRelayCommand PoolCommand { get; }
    }
}
