using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Mill.Engine;
using OpenTK;

namespace Mill.Gameobjects {
    class GameManager : IGameObject {

        private double? _nextTurnTime;

        private GameData _gameData;
        private Input _input;
        private Board _board;

        private Player _bluePlayer;
        private Player _redPlayer;

        private Player _playerAtTurn;

        public GameManager(GameData gameData, Input input, Board board) {

            _gameData = gameData;
            _input = input;
            _board = board;

            _bluePlayer = new Player("Blue", _gameData.MaxPiecesNumber);
            _redPlayer = new Player("Red", _gameData.MaxPiecesNumber);

            _playerAtTurn = _bluePlayer;
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
                _gameData.Winner = ((_playerAtTurn.Name == _redPlayer.Name)) ? true : false;
            }

            MouseInput();
        }

        private void MouseInput() {

            if (!ActionIsAllowed()) {
                return;
            }

            if (_input.Mouse.LeftPressed) {

                Vector3 rayCoordinates = DrawUtils.instance.CameraToWorldPosition(_input.Mouse.Position.X, _input.Mouse.Position.Y);
                //Console.WriteLine(rayCoordinates.X + " " + rayCoordinates.Y + " " + rayCoordinates.Z);
                if (PointSelected() && Utils.HasRayHitGameObject(rayCoordinates, _board.SelectedPoint)) {
                    MakeMove();
                }

            } else if (_input.Mouse.RightPressed) {

                _playerAtTurn.ManToMove = null;

            } else {

                Vector3 rayCoordinates = DrawUtils.instance.CameraToWorldPosition(_input.Mouse.Position.X, _input.Mouse.Position.Y);
                Utils.FindClosestIntersectionHitByRay(rayCoordinates, _board.BoardPoints, ref _board.SelectedPoint);

            }
        }

        private void KeyboardInput() {
            /*
            if (_input.Keyboard.IsKeyPressed(Keys.Up)) {

                Console.WriteLine("Up!");
            }

            if (_input.Keyboard.IsKeyPressed(Keys.T)) {

                Console.WriteLine("R");
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

        public void MakeMove() {

            switch (_playerAtTurn.state) {
                case Utils.PlayerState.PlacingMen:
                    PlaceNewMan();
                    break;
                case Utils.PlayerState.MovingMen:
                    MoveMan();
                    break;
                case Utils.PlayerState.Fly:
                    break;
                case Utils.PlayerState.GameOver:
                    break;
            }
        }
        public void PlaceNewMan() {

            if (_board.SelectedPoint.Occupied == false) {
                Console.WriteLine(_playerAtTurn.Name + " is placing man...");
                _board.SelectedPoint.Occupied = true;
                _playerAtTurn.PlaceManAt(_board.SelectedPoint);
                EndTurn();
            } else {
                Console.WriteLine("This point is occupied! You cannot place your man here!");
            }
        }

        public void MoveMan() {

            if (_board.SelectedPoint.Occupied && _playerAtTurn.MenOnBoard.Exists(x => x.Location == _board.SelectedPoint.Location)) {
                _playerAtTurn.ManToMove = _board.SelectedPoint;
            } else if (_playerAtTurn.ManToMove != null && !_board.SelectedPoint.Occupied) {
                //_playerAtTurn.ManToMove = null;                
                _playerAtTurn.MoveManAt(_board.SelectedPoint);
                
                EndTurn();
            }
        }

        public void EndTurn() {

            _nextTurnTime = 1f;
        }

        private bool IsGameOver() {

            if (_playerAtTurn.state == Utils.PlayerState.GameOver) {
                return true;
            } else {
                return false;
            }
        }

        private void StartNextTurn() {

            _playerAtTurn = (_playerAtTurn.Name == _bluePlayer.Name) ? _redPlayer : _bluePlayer;
            _playerAtTurn.StartTurn();
            Console.WriteLine(_playerAtTurn.Name + "'s turn! " + _playerAtTurn.state.ToString());

            _board.SelectedPoint = null;
            _nextTurnTime = null;
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
