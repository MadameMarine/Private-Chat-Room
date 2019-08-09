using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
        private string idUnivers = "Monde";
        GetIdGoodUnique stockIdGoodUnique = new GetIdGoodUnique();

        
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
            myProxy.On("sendingNote", onNoteReceived);

        }

        List<UserMessage> allMessages = new List<UserMessage>();

        ObservableCollection<UserMessages> allMessagesGrouped = new ObservableCollection<UserMessages>();

        private void onNoteReceived(dynamic message)
        {
            string color = getBackgroundColor(message.ParticipantId.ToString());

            allMessages.Add(new UserMessage { MyMessage = message.Message, MyUsername = message.Name, MyBackground = color });

            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {//modify user interface in UI thread
                GroupUser.ItemsSource = from x in allMessages
                                        group x by x.MyUsername into grp
                                        select grp;
            });
        }

        static string[] backgroundColors = new string[] { "Gold", "Orange", "LawnGreen", "DeepSkyBlue", "LightPink" };
        private static string getBackgroundColor(string participantId)
            => backgroundColors[(int)participantId[participantId.Length - 1] % 5];

        public class GetIdGoodUnique
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
            string myTakingNotes = "TakingNotes";

            //Connection au ChatHub
            if (myHubConnection.State != ConnectionState.Connected)
            {
                await myHubConnection.Start(); 

            }

            if (TextUrl.Text == "")
            {
                TextUrl.Text = "Creating...";

                //Create unique id url
                var url = Regex.Replace(idUnivers, @"&", ""); ;
                var urlGood = Uri.EscapeDataString(url);
                var rng = new Random();
                var idGoodUnique = urlGood + rng.Next(10, 99).ToString();


                //Call CreateSession
                var checkResult = await myProxy.Invoke<Session>("CreateSession", idGoodUnique, idMaestro_);

                TextUrl.Text = checkResult.PublicUrl;
                stockIdGoodUnique.IdGoodUnique = checkResult.Id;

                //Join room            
                Console.WriteLine(idMaestro_ + "joining group du compositeur...");
                await myProxy.Invoke("JoinSession", checkResult.Id);

                //MAJ Current Activity - Start Activity
                string groupId = stockIdGoodUnique.IdGoodUnique;
                await myProxy.Invoke("StartActivity", groupId, myTakingNotes);

                ButtonCloseSession.IsEnabled = true;
            }
            else
            {
                //MAJ Current Activity - Start Activity
                string groupId = stockIdGoodUnique.IdGoodUnique;
                await myProxy.Invoke("StartActivity", groupId, myTakingNotes);
            }

        }

        
        private async void ButtonCloseSession_Click(object sender, RoutedEventArgs e)
        {
            string myTakingNotes = "closed";

            //MAJ Current Activity
            string groupId = stockIdGoodUnique.IdGoodUnique;
            await myProxy.Invoke("StopActivity", groupId, myTakingNotes);

        }

       
    }
}
    