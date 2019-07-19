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
        private string idMaestro_ = "Maestro_";
        //@idUnivers: nom de l'univers
        private string idUnivers = "Monde";
        GetIdGoodUnique stockIdGoodUnique = new GetIdGoodUnique();

        //Départ!
        public MainPage()
        {
            
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
          
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

        public  class GetIdGoodUnique
        {
            public string IdGoodUnique { get; set; }
        }

        public class Session
        {
            public string AdminConnectionId { get; set; }
            public string PublicUrl { get; set; }
            public string Id { get; set; }
            public string CurrentActivity { get; set; }
        }


        private async void ButtonAskConnection_Click(object sender, RoutedEventArgs e)
        {
            //Connection au ChatHub
            if (myHubConnection.State != ConnectionState.Connected)
            {
                Console.WriteLine(idMaestro_ + " is connecting to server...");
                await myHubConnection.Start(); 

            }

            //Create unique id url
            var url = Regex.Replace(idUnivers, @"&", ""); ;
            var urlGood = Uri.EscapeDataString(url);
            var rng = new Random();
            var idGoodUnique = urlGood + rng.Next(10, 99).ToString() + rng.Next(10, 99).ToString();           

            //Create Session
            //var res = await httpClient.GetStringAsync(baseUrl + "/Home/CreateSession/" + idGoodUnique);

            //-----------------WIP-------------------------------------------
            var checkResult = await myProxy.Invoke<Session>("CreateSession", idGoodUnique, idMaestro_);
            //-----------------WIP-------------------------------------------
    
            Console.WriteLine("url : " + checkResult);
            TextUrl.Text = checkResult.PublicUrl;
            stockIdGoodUnique.IdGoodUnique = checkResult.Id;

            //Join la room            
            Console.WriteLine(idMaestro_ + "joining group du compositeur...");
            await myProxy.Invoke("JoinSession", checkResult.Id);        
            await myProxy.Invoke("SendNote", checkResult.Id, idMaestro_, "connected");
            Console.WriteLine(idMaestro_ + "group joined");

            ButtonPriseDeNotes.IsEnabled = true;
        }


        private  async void ButtonPriseDeNotes_Click(object sender, RoutedEventArgs e)
        {
            string myTakingNotes = "TakingNotes";
            //Connection to ChatHub
            if (myHubConnection.State != ConnectionState.Connected)
            {
                Console.WriteLine(idMaestro_ + " is connecting to server...");
                await myHubConnection.Start();
            }

            //MAJ Current Activity
            string groupId = stockIdGoodUnique.IdGoodUnique;
            await myProxy.Invoke("StartActivity", groupId, myTakingNotes);

        }
    }
}
