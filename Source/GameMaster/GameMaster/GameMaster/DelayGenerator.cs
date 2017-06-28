using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public class DelayGenerator
    {
        public void DelayMessage(Connection connection, Messages.XmlHandling.Serializer serializer, uint timeToWait )
        { 
            Task delay = new Task(() => {
                System.Threading.Thread.Sleep((int)timeToWait);
                connection.SendMessage(serializer.Serialize());
                // TO BE DELETED
                Console.WriteLine("I have waited for: " + (int)connection.GameState.ActionCosts.MoveDelay);
            });
            delay.Start();
       }
    }
}
