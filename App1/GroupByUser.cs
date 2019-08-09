using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class UserMessages
    {

        public string Username { get; set; }
        public ObservableCollection<UserMessage> Messages { get; set; } = new ObservableCollection<UserMessage>();
    }
}
