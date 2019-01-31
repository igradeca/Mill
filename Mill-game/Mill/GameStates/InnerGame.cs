using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mill.Engine;
using Mill.Gameobjects;
using OpenTK;

namespace Mill.GameStates {
    public class InnerGame : IGameObject {

        private StateSystem _system;
        private Input _input;

        //private double _gameTime;

        // Gameobjects
        private GameData _gameData;
        private GameManager _gameManager;
        private Board _board;

        public InnerGame(StateSystem system, Input input, GameData gameData) {

            _system = system;
            _input = input;
            _gameData = gameData;

            OnGameStart();
        }

        public void OnGameStart() {

            //_gameTime = 5f; // current game time
            _gameData.Winner = null;

            //_gameMaster = new GameManager(Utils.GameType.NineMorris);
            _board = new Board(_gameData.GameType);

            _gameManager = new GameManager(_gameData, _input, _board);
        }

        public void Update(double elapsedTime) {

            if (_gameData.Winner.HasValue) {
                OnGameStart();                
                _system.ChangeState("game_over");
            }

            _gameManager.Update(elapsedTime);
        }

        public void Render() {

            _board.Render();
            _gameManager.Render();
        }



    }
}
