using CommunityToolkit.Mvvm.ComponentModel;
using ModbusMasterUI.Protocol;
using System.Collections.Generic;

namespace ModbusMasterUI.Model
{
    public class ModbusPoolModel : ObservableObject
    {
        public ModbusPoolModel()
        {
            StartAddress = 1;
            AddressLength = 10;
        }

        private ushort startAddress = 1;

        public ushort StartAddress
        {
            get => startAddress;
            set => SetProperty(ref startAddress, value);
        }

        private ushort addressLength = 20;

        public ushort AddressLength
        {
            get => addressLength;
            set => SetProperty(ref addressLength, value);
        }

    }
}
