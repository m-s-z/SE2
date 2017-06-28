using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xsd2;
namespace Messages
{
    public class RandomMessageGenerator
    {
        private Random random = new Random();
        private GameMessage[] messages = new GameMessage[]
        {
            new Move(),
            new TestPiece(),
            new PlacePiece(),
            new Discover(),
            new PickUpPiece()
        };
        public GameMessage GetRandomMessage()
        {
            int index = random.Next() % messages.Length;
            return messages[index];
        }
    }
}
