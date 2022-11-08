using ModbusMasterUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.ViewModel.Message
{
    public class AddSlaveMessage
    {
        public AddSlaveMessage(ModbusModel addModel)
        {
            AddModel = addModel ?? throw new ArgumentNullException(nameof(addModel));
        }
        public ModbusModel AddModel { get; }
    }
}
