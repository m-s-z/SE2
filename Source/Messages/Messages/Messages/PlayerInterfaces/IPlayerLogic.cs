using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xsd2;

namespace Messages.PlayerInterfaces
{
    public interface IPlayerLogic
    {
        GameMessage ChooseNextMessage(Messages.PlayerInterfaces.IConnection connection, Data gameMessage);
        GameMessage AnswerForGameMessage(Messages.PlayerInterfaces.IConnection connection, Game gameMessage);
        void SetReceivedData(TaskField[] taskFields, GoalField[] goalFields, bool gameFinished, Xsd2.Piece[] pieces, Location location);
    }
}
