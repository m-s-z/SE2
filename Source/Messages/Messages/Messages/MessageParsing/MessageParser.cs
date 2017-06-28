using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.MessageParsing
{
    public class MessageParser
    {
        public string Parse(ref string msg, out int specialByteIndex)
        {
            specialByteIndex = msg.IndexOf((char)23);
            if (specialByteIndex == -1)
            {
                return "";
            }
            string currentMsg = msg.Substring(0, specialByteIndex);
            if (msg.Length < specialByteIndex)
            {
                msg = "";
            }
            else
            {
                msg = msg.Substring(specialByteIndex + 1);
            }
            return currentMsg;
        }
    }
}
