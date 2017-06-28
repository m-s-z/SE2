using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class GameBoardTile
    {
        // UNDER CONSTRUCTION
        private bool _occupied;
        private int _pieceID; // 0 - no piece, 1 - there is a piece, -1 - there is sham
        private int _manhattanDistance;

        #region GettersAndSetters
        public bool Occupied
        {
            get
            {
                return _occupied;
            }

            set
            {
                _occupied = value;
            }
        }

        public int PieceID
        {
            get
            {
                return _pieceID;
            }

            set
            {
                _pieceID = value;
            }
        }

        #endregion

        public GameBoardTile()
        {
            _occupied = false;
            _pieceID = 0;
        }
    }
}
