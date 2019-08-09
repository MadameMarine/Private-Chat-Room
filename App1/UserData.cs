using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class UserMessage
    {
        public string MyUsername { get; set; }

        public string MyMessage { get; set; }

        public string MyBackground{ get; set; }

        public override string ToString()
        {
            return MyUsername;
        }

    }
}
