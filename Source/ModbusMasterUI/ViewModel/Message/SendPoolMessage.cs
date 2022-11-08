using ModbusMasterUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.ViewModel.Message
{
    public class SendPoolMessage
    {
        public SendPoolMessage(ModbusModel modbusModel)
        {
            CurrentModbusModel = modbusModel ?? throw new ArgumentNullException(nameof(modbusModel));
        }

        public ModbusModel CurrentModbusModel { get; }
    }
}
