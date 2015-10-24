using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClapCountServer
{
   public class ClapHub : Hub
    {
        public void SendClap(int count)
        {
            Clients.All.NewClaps(count);
        }
    }
}
