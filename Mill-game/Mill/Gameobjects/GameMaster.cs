using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    class GameMaster : IGameObject {

        Player bluePlayer;
        Player redPlayer;

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
        public GameMaster(Utils.GameType gameType) {

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

        public void Update(double elapsedTime) {

        }

        public void Render() {

        }


    }
}
