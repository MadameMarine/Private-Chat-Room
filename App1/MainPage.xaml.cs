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
        public MainPage()
        {
            this.InitializeComponent();
        }
       
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = new TextBox();
            textBox.Text = string.Empty;

            //Get informations from browser
            App myApp = (Application.Current as App);
            if (myApp.MyHubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            {
                //get username and message              
                //textBox.Text = userName + ":" + message;
            }
            else
            {
                //textBox.Text = $"Can't connect to server {myApp.MyHubConnection.Url}";
                Console.WriteLine("Can't connect to server...");
            }
        }
    }
}
