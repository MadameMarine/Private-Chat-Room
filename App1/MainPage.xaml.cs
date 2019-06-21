using Microsoft.AspNet.SignalR.Client;
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
        private HubConnection myHubConnection;
        private IHubProxy myProxy;


        //Départ!
        public MainPage()
        {
            
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
           
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            myHubConnection = new HubConnection("http://localhost:52527");
            myProxy = myHubConnection.CreateHubProxy("chatHub");

            

            //Get informations from browser
            myProxy.On("addNewMessageToPage", message =>
            {
                _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    messages.Text += message.Username + ": " + message.Message;
                });
            }); ;
        }

        //public HubConnection MyHubConnection { get; set; }
        //public IHubProxy MyHubProxy { get; set; }
        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            userTextbox.IsEnabled = false;
            joinButton.IsEnabled = false;

            if (myHubConnection.State != ConnectionState.Connected)
            {
                Console.WriteLine("Compositeur is connecting to server...");
                await myHubConnection.Start();

            }

            //Join la room            
            Console.WriteLine("Compositeur joining group Chat/Compositeur...");
            await myProxy.Invoke("joinGroup", "Compositeur");
            Console.WriteLine("Compositeur group joined");

            //TOTO : rendre visible une textbox pour envoyer un message

            await myProxy.Invoke("Send", "Compositeur", userTextbox.Text, "salut");
           


        }
    }
}
