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
    {   //A supprimer quand code validé-------------------------------
        //private void SignalR()
        //{
        //    //Connect to the url 
        //    var MyHubConnection = new HubConnection("http://localhost:52527/Home/Chat/Compositeur");
        //    //ChatHub is the hub name defined in the host program
        //    //proxy deals with the interaction with a specific hub
        //    var MyHubProxy = MyHubConnection.CreateHubProxy("ChatHub");

        //    //Connect to hub
        //    App myApp = (Application.Current as App);            
        //    if (myApp.MyHubConnection.State != ConnectionState.Connected)
        //    {
        //        try
        //        {
        //            myApp.MyHubConnection.Start();
        //        }
        //        catch
        //        {
        //            Console.WriteLine("Can't connect to server...");
        //            return;
        //        }
        //    }
        //}
        //A supprimer quand code validé-------------------------------
        public HubConnection MyHubConnection { get; set; }
        public IHubProxy MyHubProxy { get; set; }

        //Départ!
        public MainPage()
        {
            this.InitializeComponent();
            //SignalR();
            //Connect to hub
            App myApp = (Application.Current as App);
            if (myApp.MyHubConnection.State != ConnectionState.Connected)
            {
                try
                {
                    myApp.MyHubConnection.Start();
                }
                catch
                {
                    Console.WriteLine("Can't connect to server...");
                    return;
                }
            }

            //Get informations from browser
            myApp.MyHubProxy.On("addNewMessageToPage", message => 
            {
                Console.WriteLine(message);
                userTextbox.Text = message;

            }); ;
        }

      
        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            App myApp = (Application.Current as App);

            //Join la room            
            Console.WriteLine("Compositeur joining group Room1...");
            myApp.MyHubProxy.Invoke("joinGroup", "Room1");
            Console.WriteLine("Compositeur group joined");
            Frame.Navigate(typeof(Hub));
        }
    }
}
