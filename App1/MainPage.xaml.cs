using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
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
        HttpClient httpClient = new HttpClient();
        private string baseUrl = "http://localhost:52527";
        private string idMaextro_ = "Maextro_";
        //@idUnivers: nom de l'univers
        private string idUnivers = "Super &@#voitze";

        

        //Départ!
        public MainPage()
        {
            
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            //id maestro unique---
            var ticks = DateTime.Now.ToString("HH:mm:ss").ToString();
            var rng = new Random();
            var idMaextro_Unique = idMaextro_ + Regex.Replace(ticks, @":","") + rng.Next(10).ToString() + rng.Next(10).ToString() + rng.Next(10).ToString();
            //--------WIP-----------
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            myHubConnection = new HubConnection(baseUrl);
            myProxy = myHubConnection.CreateHubProxy("chatHub");

          
            //Get informations from browser
            myProxy.On("addNewMessageToPage", message =>
            {
                _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {

                    messages.Text += message.Name + ": " + message.Message;

                });
            });
           
        }

        public class CreateSessionResult
        {
            public string publicUrl { get; set; }
            public string groupId { get; set; }
        }
        private async void ButtonAskConnection_Click(object sender, RoutedEventArgs e)
        {

            //Connection au ChatHub
            if (myHubConnection.State != ConnectionState.Connected)
            {
                Console.WriteLine(idMaextro_ + " is connecting to server...");
                await myHubConnection.Start(); 

            }
            var url = Regex.Replace(idUnivers, @"&", ""); ;
            var urlgood = Uri.EscapeDataString(url);
            
            var res = await httpClient.GetStringAsync(baseUrl + "/Home/CreateSession/" + urlgood);     
            var checkResult = JsonConvert.DeserializeObject<CreateSessionResult>(res);

            //idUnivers unique---CAS 2 si je me trompe id
            var ticks = DateTime.Now.ToString("HH:mm:ss").ToString();
            var rng = new Random();
            var idUniversUnique = checkResult.groupId + Regex.Replace(ticks, @":", "") + rng.Next(10).ToString() + rng.Next(10).ToString() + rng.Next(10).ToString();
            //--------WIP-------------------------------

            Console.WriteLine("url : " + checkResult);
            TextUrl.Text = checkResult.publicUrl;
           
            //Join la room            
            Console.WriteLine(idMaextro_ + "joining group du compositeur...");
            await myProxy.Invoke("joinGroup", checkResult.groupId);            
            await myProxy.Invoke("Send", checkResult.groupId, idMaextro_, "connected");
            Console.WriteLine(idMaextro_ + "group joined");

        }


    }
}
