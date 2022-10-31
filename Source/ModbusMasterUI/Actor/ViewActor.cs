using Akka.Actor;
using Akka.Event;
using ModbusMasterUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusMasterUI.Actor
{
    public class ViewActor : ReceiveActor
    {
        private readonly MainViewModel? mainView;
        private readonly ILoggingAdapter logger = Logging.GetLogger(Context);

        public ViewActor()
        {
            mainView = App.Current.Resources["MainViewModel"] as MainViewModel;

        }
    }
}
