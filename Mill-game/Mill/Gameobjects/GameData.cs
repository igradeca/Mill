using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class GameData {

        public bool? Winner { get; set; }

        public Utils.GameType GameType { get; private set; }
        /// <summary>
        /// Number of pieces to lose the game.
        /// </summary>
        public int MinPiecesNumber { get; private set; }
        /// <summary>
        /// Number of pieces that each player gets at the beginning.
        /// </summary>
        public int MaxPiecesNumber { get; private set; }

        //public bool 

        /// <summary>
        /// Starts new game.
        /// </summary>
        public GameData(Utils.GameType gameType) {

            Winner = null;

            SetGameRules(gameType);
        }

        private void SetGameRules(Utils.GameType gameType) {

            switch (gameType) {
                case Utils.GameType.ThreeMoriss:
                    break;

                case Utils.GameType.SixMorris:
                    break;

                case Utils.GameType.NineMorris:
                    GameType = gameType;
                    MinPiecesNumber = 2;
                    MaxPiecesNumber = 9;
                    break;

                case Utils.GameType.TwelveMorris:
                    break;

                case Utils.GameType.LaskerMorris:
                    break;
            }
        }

    }
}
