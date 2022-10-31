using CommunityToolkit.Mvvm.ComponentModel;
using ModbusMasterUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {           
            Slaves.Add(new ModbusModel());
            Slaves.Add(new ModbusModel());
            Slaves.Add(new ModbusModel());
            Slaves[2].ModbusSlave.SlaveIp = "10.3.4.247";
        }
        
        private ModbusModel selectedSlave ;

        public ModbusModel SelectedSlave
        {
            get => selectedSlave;
            set => SetProperty(ref selectedSlave, value);
        }

        private ObservableCollection<ModbusModel> slaves = new ObservableCollection<ModbusModel>();

        public ObservableCollection<ModbusModel> Slaves
        {
            get => slaves;
            set => SetProperty(ref slaves, value);
        }
    }
}
