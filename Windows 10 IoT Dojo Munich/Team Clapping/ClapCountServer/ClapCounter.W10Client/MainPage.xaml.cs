using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClapCounter.W10Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
    
        public MainPage()
        {
            this.InitializeComponent();
            var connection = new HubConnection("http://clapcountserver.azurewebsites.net");

            var hubProxy = connection.CreateHubProxy("ClapHub");
            hubProxy.On<int>("newClaps", UpdateClaps);

            connection.Start();
        }

        private async void UpdateClaps(int count)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.txtField.Text = count.ToString());
        }
    }
}
