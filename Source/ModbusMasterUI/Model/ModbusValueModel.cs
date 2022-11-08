using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.Model
{
    public class ModbusValueModel:ObservableObject
    {
        private string description = "";
        private ushort innerushortvalue;
        private bool innerboolvalue;
        private enum ShowType
        {
            None,
            PlainBool,
            PlainUshort
        }

        private ShowType innerType = ShowType.None;

        public string Description
        {
            get => description;
            set 
            {                
                SetProperty(ref description, value);
                Prepeare();
            }
        }

        private ushort address ;

        public ushort Address
        {
            get => address;
            set
            {                
                SetProperty(ref address, value);
                Prepeare();
            }
        }

        private string title = "";

        public string Title
        {
            get => title;
            set
            {                
                SetProperty(ref title, value);
            }
        }        

        public void SetValue(bool value)
        {
            innerboolvalue = value;
            innerType = ShowType.PlainBool;
            Prepeare();
        }

        public void SetValue(ushort value)
        {
            innerushortvalue = value;
            innerType = ShowType.PlainUshort;
            Prepeare();
        }

        public void Prepeare()
        {
            string pretitle = $"{Address}"; ;
            if (!string.IsNullOrEmpty(Description))
            {
                pretitle = $"{Description}({Address})";
            }            

            switch (innerType)
            {
                case ShowType.None:
                    Title = $"{pretitle} : -";
                    break;
                case ShowType.PlainBool:
                    Title = $"{pretitle} : {innerboolvalue}";
                    break;
                case ShowType.PlainUshort:
                    Title = $"{pretitle} : {innerushortvalue}";
                    break;
                default:
                    break;
            }
        }

    }
}
