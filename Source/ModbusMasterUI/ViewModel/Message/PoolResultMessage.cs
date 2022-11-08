using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.ViewModel.Message
{
    public class PoolResultMessage
    {
        public PoolResultMessage(string fullname, IReadOnlyDictionary<ushort, bool> coilStatusReadData, IReadOnlyDictionary<ushort, bool> ınputStatusReadData, IReadOnlyDictionary<ushort, ushort> holdingRegisterReadData, IReadOnlyDictionary<ushort, ushort> ınputRegisterReadData)
        {
            Fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
            CoilStatusReadData = coilStatusReadData ?? throw new ArgumentNullException(nameof(coilStatusReadData));
            InputStatusReadData = ınputStatusReadData ?? throw new ArgumentNullException(nameof(ınputStatusReadData));
            HoldingRegisterReadData = holdingRegisterReadData ?? throw new ArgumentNullException(nameof(holdingRegisterReadData));
            InputRegisterReadData = ınputRegisterReadData ?? throw new ArgumentNullException(nameof(ınputRegisterReadData));
        }

        public string Fullname { get; private set; }
        public IReadOnlyDictionary<ushort, bool> CoilStatusReadData { get; private set; }
        public IReadOnlyDictionary<ushort, bool> InputStatusReadData { get; private set; }
        public IReadOnlyDictionary<ushort, ushort> HoldingRegisterReadData { get; private set; }
        public IReadOnlyDictionary<ushort, ushort> InputRegisterReadData { get; private set; }
    }
}
