using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class PieceInfo
    {
        private Xsd2.Location _location;
        private Xsd2.Piece _piece;

        public Xsd2.Location Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public Xsd2.Piece Piece
        {
            get { return _piece; }
            set { _piece = value; }
        }
        public PieceInfo(Xsd2.Location location, Xsd2.Piece piece)
        {
            _location = location;
            _piece = piece;
        }
    }
}
