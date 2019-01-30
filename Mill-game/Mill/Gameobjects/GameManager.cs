using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class GameManager {

        public bool? Winner { get; set; }

        public Utils.GameType GameType;
        /// <summary>
        /// Number of pieces to lose the game.
        /// </summary>
        public int MinPiecesNumber;
        /// <summary>
        /// Number of pieces that each player gets at the beginning.
        /// </summary>
        public int MaxPiecesNumber;

        //public bool 

        /// <summary>
        /// Starts new game.
        /// </summary>
        public GameManager(Utils.GameType gameType) {

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
