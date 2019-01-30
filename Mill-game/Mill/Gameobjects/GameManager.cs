using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Mill.Engine;

namespace Mill.Gameobjects {
    class GameManager : IGameObject {

        private double? _nextTurnTime;

        private GameData _gameData;
        private Board _board;

        private Player _bluePlayer;
        private Player _redPlayer;

        private Player _playersTurn;

        public GameManager(GameData gameData, Board board) {

            _gameData = gameData;
            _board = board;

            _bluePlayer = new Player("Blue", _gameData.MaxPiecesNumber);
            _redPlayer = new Player("Red", _gameData.MaxPiecesNumber);

            _playersTurn = _bluePlayer;
        }

        public void Update(double elapsedTime) {

            if (_nextTurnTime.HasValue) {
                _nextTurnTime -= elapsedTime;

                if (_nextTurnTime <= 0) {
                    StartNextTurn();
                }
            }

            if (IsGameOver()) {
                // true - BLUE wins
                // false - RED wins
                _gameData.Winner = ((_playersTurn.Name == _redPlayer.Name)) ? true : false;
            }


            /*
            if (PointSelected() && _nextTurnTime == null) {

                _nextTurnTime = 2f;
            }

            
            */

        }

        private bool PointSelected() {

            if (_board.SelectedPoint != null) {
                return true;
            } else {
                return false;
            }
        }

        private void StartNextTurn() {

            _playersTurn = (_playersTurn.Name == _bluePlayer.Name) ? _redPlayer : _bluePlayer;
            _playersTurn.StartTurn();
            Console.WriteLine(_playersTurn.Name + "'s turn! " + _playersTurn.state.ToString());

            _board.SelectedPoint = null;
            _nextTurnTime = null;
        }

        private bool IsGameOver() {

            if (_playersTurn.state == Utils.PlayerState.GameOver) {
                return true;
            } else {
                return false;
            }
        }

        public void PlaceNewMan() {

            if (_board.SelectedPoint.Occupied == false) {
                Console.WriteLine("placing man...");
                _board.SelectedPoint.Occupied = true;
                _playersTurn.PlaceManAt(_board.SelectedPoint.Location);
                EndTurn();
            } else {
                Console.WriteLine("This point is occupied! You cannot place your man here!");
            }
        }

        public void EndTurn() {

            _nextTurnTime = 1f;
        }
        
        public bool ActionIsAllowed() {

            if (_nextTurnTime.HasValue) {
                return false;
            } else {
                return true;
            }
        }
        
        public void Render() {

            _bluePlayer.Render();
            _redPlayer.Render();
        }


    }
}
