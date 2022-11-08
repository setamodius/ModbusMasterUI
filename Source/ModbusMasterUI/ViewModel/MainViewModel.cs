using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ModbusMasterUI.Model;
using ModbusMasterUI.ViewModel.Message;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace ModbusMasterUI.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            //ShowTcpDialog = true;
            Slaves.Add(new ModbusModel());
            AddDialogCommand = new RelayCommand(OpenTcpDialog);
            AddSlaveCommand = new RelayCommand(AddSlave);
            WeakReferenceMessenger.Default.Register<RemoveSlaveMessage>(this, (r, m) =>
            {
                Slaves.Remove(m.RemoveModel);
            });
            WeakReferenceMessenger.Default.Register<AddSlaveMessage>(this, (r, m) =>
            {
                Slaves.Add(m.AddModel);
            });
            WeakReferenceMessenger.Default.Register<PoolResultMessage>(this, (r, m) =>
            {
                var resultSlave = (from item in Slaves where item.ModbusSlave.FullName == m.Fullname select item).FirstOrDefault();
                if (resultSlave == null)
                {
                    return;
                }
                resultSlave.SetResults(m);

            });
        }       

        private void AddSlave()
        {
            var founditem = (from item in Slaves where item.ModbusSlave.FullName == TcpDialogModel.FullName select item).FirstOrDefault();
            if (founditem != null)
            {
                return;
            }
            ModbusModel newSlave = new() {ModbusSlave = TcpDialogModel};
            WeakReferenceMessenger.Default.Send(new AddSlaveMessage(newSlave));
            ShowTcpDialog = false;
        }

        private void OpenTcpDialog()
        {
            TcpDialogModel = new();
            ShowTcpDialog = true;                      
        }

        private ObservableCollection<ModbusModel> slaves = new();

        public ObservableCollection<ModbusModel> Slaves
        {
            get => slaves;
            set => SetProperty(ref slaves, value);
        }

        private bool showTcpDialog;

        public bool ShowTcpDialog
        {
            get => showTcpDialog;
            set => SetProperty(ref showTcpDialog, value);
        }

        private ModbusTCPSlaveModel tcpDialogModel = new();

        public ModbusTCPSlaveModel TcpDialogModel
        {
            get => tcpDialogModel;
            set => SetProperty(ref tcpDialogModel, value);
        }

        public IRelayCommand AddDialogCommand { get; }
        public IRelayCommand AddSlaveCommand { get; }
    }
}
