using ModbusMasterUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.ViewModel.Message
{
    public class RemoveSlaveMessage
    {
        public RemoveSlaveMessage(ModbusModel removeModel)
        {
            RemoveModel = removeModel ?? throw new ArgumentNullException(nameof(removeModel));
        }

        public ModbusModel RemoveModel { get; }
    }
}
