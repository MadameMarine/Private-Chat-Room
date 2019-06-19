using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitilizeHub();
        }

        private void RichTextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private async void InitilizeHub()
        {
            var hubConnection = new ChatHub(); //---WIP
            //var hubProxy = hubConnection.CreateHubProxy("BroadcastHub");
            //hubProxy.On<DateTime>("Broadcast",async data => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() => BroadcastResults.Text =data.ToString()));
            await hubConnection.Start();
        }
    }
}
