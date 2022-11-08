using Akka.Routing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModbusMasterUI.Actor.ModbusMasterActor.ModbusMasterMessage;

namespace ModbusMasterUI.Model
{
    public class ModbusPoolCollectionModel : ObservableObject
    {
        public ModbusPoolCollectionModel()
        {
            AddPool = new RelayCommand(AddPoolToCollection);
            RemovePool = new RelayCommand<ModbusPoolModel>(RemovePoolFromCollection);
            PoolCollection.Add(new ModbusPoolModel());
        }

        private void RemovePoolFromCollection(ModbusPoolModel? obj)
        {
            if (obj == null)
            {
                return;
            }
            PoolCollection.Remove(obj);
        }
       
        private void AddPoolToCollection()
        {
            ModbusPoolModel poolModel = new()
            {
                StartAddress = StartAddress,
                AddressLength = AddressLength
            };
            PoolCollection.Add(poolModel);
            FillModbusValueCollection();
        }

        public ObservableCollection<ModbusPoolModel> PoolCollection { get; set; } = new ObservableCollection<ModbusPoolModel>();

        public ObservableCollection<ModbusValueModel> ModbusValueCollection { get; set; } = new ObservableCollection<ModbusValueModel>();        

        public IRelayCommand AddPool { get; }

        public List<(ushort,ushort)> GetPools()
        {
            List <(ushort startaddress, ushort length)> pools = new ();
            foreach (var item in PoolCollection)
            {
                pools.Add((item.StartAddress, item.AddressLength));
            }
            return pools;
        }

        public IRelayCommand<ModbusPoolModel> RemovePool { get; }

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

        private void FillModbusValueCollection()
        {
            ModbusValueCollection.Clear();
            foreach (var pool in PoolCollection)
            {
                for (int i = pool.StartAddress - 1 ; i < pool.AddressLength ; i++)
                {
                    var valueitem = (from item in ModbusValueCollection
                                    where item.Address == (pool.StartAddress + i ) 
                                     select item).FirstOrDefault();
                    if (valueitem != null)
                    {
                        continue;
                    }
                    ModbusValueModel valueModel = new()
                    {
                        Address = (ushort)(pool.StartAddress + i )
                    };
                    ModbusValueCollection.Add(valueModel);
                }
            }
        }

        internal void SetBoolValues(IReadOnlyDictionary<ushort, bool> boolreaddata)
        {
            foreach (var boolvalue in boolreaddata)
            {
                var valueitem = (from item in ModbusValueCollection
                                 where item.Address == (boolvalue.Key)
                                 select item).FirstOrDefault();
                if (valueitem == null)
                {
                    continue;
                }
                valueitem.SetValue(boolvalue.Value);
            }    
        }

        internal void SetUshortValues(IReadOnlyDictionary<ushort, ushort> ushortreaddata)
        {
            foreach (var ushortvalue in ushortreaddata)
            {
                var valueitem = (from item in ModbusValueCollection
                                 where item.Address == (ushortvalue.Key)
                                 select item).FirstOrDefault();
                if (valueitem == null)
                {
                    continue;
                }
                valueitem.SetValue(ushortvalue.Value);
            }
        }
    }
}
