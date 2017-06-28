using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class KeepAlive
    {
        public static bool IsKeepAlive(string msg)
        {
            if(msg.Length > 0)
            {
                if(msg[0] == (char)23)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
