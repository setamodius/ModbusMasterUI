using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.Model
{
    public class ModbusPoolModel : ObservableObject
    {
        
        private int poolingRate;

        public int PoolingRate
        {
            get => poolingRate;
            set => SetProperty(ref poolingRate, value);
        }        

        private List<string> modbusFunctions = new()
        {
            "01: Coil Status",
            "02: Input Status",
            "03: Holding Registers",
            "04: Input Registers"
        };

        public List<string> ModbusFunctions
        {
            get => modbusFunctions;
            set => SetProperty(ref modbusFunctions, value);
        }

        private string selectedFunction = "";

        public string SelectedFunction
        {
            get => selectedFunction;
            set => SetProperty(ref selectedFunction, value);
        }

        private ushort startAddress;

        public ushort StartAddress
        {
            get => startAddress;
            set => SetProperty(ref startAddress, value);
        }

        private ushort addressLength;

        public ushort AddressLength
        {
            get => addressLength;
            set => SetProperty(ref addressLength, value);
        }

    }
}
