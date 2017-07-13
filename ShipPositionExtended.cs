using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Player.Interface;

namespace BattleshipBot
{
    class ShipPositionExtended : IShipPosition
    {
        private bool horizontal;
        public IGridSquare StartingSquare { get;  }
        public IGridSquare EndingSquare { get; }

        private ShipPositionExtended() {}

        public ShipPositionExtended(IGridSquare start, int direction, int length)
        {
            StartingSquare = start;
            switch (direction)
            {
                case 0:
                    horizontal = true;
                    EndingSquare = new GridSquare((char)(StartingSquare.Row + length - 1), StartingSquare.Column);
                    break;
                case 1:
                    horizontal = false;
                    EndingSquare = new GridSquare(StartingSquare.Row, StartingSquare.Column + length - 1);
                    break;
                case 2:
                    horizontal = true;
                    EndingSquare = new GridSquare((char)(StartingSquare.Row - length + 1), StartingSquare.Column);
                    break;
                case 3:
                    horizontal = false;
                    EndingSquare = new GridSquare(StartingSquare.Row, StartingSquare.Column-length +1);
                    break;
            }
        }

        public bool isValid()
        {
            return Between('A', 'I', StartingSquare.Row) && Between('A', 'I', EndingSquare.Row) &&
                   Between(1, 10, StartingSquare.Column) && Between(1, 10, EndingSquare.Column);
        }
        public bool Contains(IGridSquare square)
        {
            if (horizontal && square.Row == StartingSquare.Row)
            {
                return Between(StartingSquare.Column, EndingSquare.Column, square.Column);
            }
            else if (!horizontal && square.Column == StartingSquare.Column)
            {
                return Between(StartingSquare.Row, EndingSquare.Row, square.Row);
            }
            else return false;
        }

        private static bool Between<T>(T start, T end, T test) where T : IComparable
        {
            if (start.CompareTo(end) <= 0)
            {
                return start.CompareTo(test) <= 0 && test.CompareTo(end) <= 0;
            }
            else
            {
                return end.CompareTo(test) <= 0 && test.CompareTo(start) <= 0;
            }
        }
    }
}
