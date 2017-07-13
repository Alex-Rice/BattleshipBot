using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Schema;
using Battleships.Player.Interface;

namespace BattleshipBot
{
    public class MyBot : IBattleshipsBot
    {
        private IGridSquare lastTarget;
        private Random randomGen = new Random();
        private static char[] letters = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'};

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            lastTarget = null; // Forget all our history when we start a new game
            List<ShipPositionExtended> ships = null;
            while (!Valid(ships))
            {
                ships = new List<ShipPositionExtended>
                {
                    GetShipPosition(5), // Aircraft Carrier
                    GetShipPosition(4), // Battleship
                    GetShipPosition(3), // Destroyer
                    GetShipPosition(2), // Submarine
                    GetShipPosition(2) // Patrol boat
                };
            }
            return ships;
        }

        private ShipPositionExtended GetShipPosition(int length)
        {
            return new ShipPositionExtended(new GridSquare(letters[randomGen.Next(9)],randomGen.Next(1,10)),randomGen.Next(3),length);
        }

        private static bool Valid(List<ShipPositionExtended> ships)
        {
            if (ships == null)
            {
                return false;
            }
            foreach (var ship in ships)
            {
                if (!ship.isValid())
                    return false;
            }
            for (int column = 1; column <= 10; column++)
            {
                foreach (var row in letters)
                {
                    var grid = new GridSquare(row, column);
                    if (ships.Select(x => x.Contains(grid) ? 1 : 0).Aggregate((x, y) => x + y) > 1)
                        return false;
                }
            }
            return true;
        }

        public IGridSquare SelectTarget()
        {
            var nextTarget = GetNextTarget();
            lastTarget = nextTarget;
            return nextTarget;
        }

        private IGridSquare GetNextTarget()
        {
            if (lastTarget == null)
            {
                return new GridSquare('A', 1);
            }

            var row = lastTarget.Row;
            var col = lastTarget.Column + 1;
            if (lastTarget.Column != 10)
            {
                return new GridSquare(row, col);
            }

            row = (char) (row + 1);
            if (row > 'J')
            {
                row = 'A';
            }
            col = 1;
            return new GridSquare(row, col);
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            // Ignore whether we're successful
        }

        public void HandleOpponentsShot(IGridSquare square)
        {
            // Ignore what our opponent does
        }

        public string Name => "Barely Floating";
    }
}
