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
                if (PointSelected() && Utils.HasRayHitGameObject(rayCoordinates, _board.HoveringPoint)) {
                    MakeMove();
                }

            } else if (_input.Mouse.RightPressed) {
                _playerAtTurn.SelectedMan = null;

            } else {
                Vector3 rayCoordinates = DrawUtils.instance.CameraToWorldPosition(_input.Mouse.Position.X, _input.Mouse.Position.Y);
                Utils.FindClosestIntersectionHitByRay(rayCoordinates, _board.BoardPoints, ref _board.HoveringPoint);
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

            if (_board.HoveringPoint != null) {
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
                    FlyMan();
                    break;
                case Utils.PlayerState.RemoveOpponentsMan:
                    RemoveOpponentsMan();
                    break;
                case Utils.PlayerState.GameOver:
                    Console.WriteLine("It is game over.");
                    break;
            }
        }
        public void PlaceNewMan() {

            if (_board.HoveringPoint.Occupied == false) {
                Console.WriteLine(_playerAtTurn.Name + " is placing man...");
                _board.HoveringPoint.Occupied = true;
                _playerAtTurn.PlaceManAt(_board.HoveringPoint);

                if (_playerAtTurn.HasThreeMenLined()) {
                    Console.WriteLine("Three Men Lined! BINGO!");
                    _board.HoveringPoint = null;
                    _playerAtTurn.state = Utils.PlayerState.RemoveOpponentsMan;
                } else {
                    EndTurn();
                }
            } else {
                Console.WriteLine("This point is occupied! You cannot place your man here!");
            }
        }

        public void MoveMan() {

            if (_board.HoveringPoint.Occupied && _playerAtTurn.MenOnBoard.Exists(x => x.Location == _board.HoveringPoint.Location)) {
                _playerAtTurn.SelectedMan = _board.HoveringPoint;
            } else if (_playerAtTurn.SelectedMan != null && !_board.HoveringPoint.Occupied && ArePointsAdjacent(_playerAtTurn.SelectedMan, _board.HoveringPoint)) {              
                _playerAtTurn.MoveManAt(_board.HoveringPoint);

                if (_playerAtTurn.HasThreeMenLined()) {
                    Console.WriteLine("Three Men Lined! BINGO!");
                    _board.HoveringPoint = null;
                    _playerAtTurn.state = Utils.PlayerState.RemoveOpponentsMan;
                } else {
                    EndTurn();
                }
            }
        }

        private bool ArePointsAdjacent(Intersection mainPoint, Intersection pointToCheck) {

            if (mainPoint.AdjacentPoints.Exists(x => x.Location == pointToCheck.Location)) {
                return true;
            } else {
                return false;
            }
        }

        public void FlyMan() {

            if (_board.HoveringPoint.Occupied && _playerAtTurn.MenOnBoard.Exists(x => x.Location == _board.HoveringPoint.Location)) {
                _playerAtTurn.SelectedMan = _board.HoveringPoint;
            } else if (_playerAtTurn.SelectedMan != null && !_board.HoveringPoint.Occupied) {
                _playerAtTurn.MoveManAt(_board.HoveringPoint);

                if (_playerAtTurn.HasThreeMenLined()) {
                    Console.WriteLine("Three Men Lined! BINGO!");
                    _board.HoveringPoint = null;
                    _playerAtTurn.state = Utils.PlayerState.RemoveOpponentsMan;
                } else {
                    EndTurn();
                }
            }
        }

        public void RemoveOpponentsMan() {

            Player opponent = (_playerAtTurn == _bluePlayer) ? _redPlayer : _bluePlayer;
            Console.WriteLine("Opponent is " + opponent.Name + " player");
            if (_board.HoveringPoint.Occupied && opponent.MenOnBoard.Exists(x => x == _board.HoveringPoint)) {
                opponent.RemoveMan(_board.HoveringPoint);
                Console.WriteLine("One man down.");
                EndTurn();
            }
        }

        public void EndTurn() {

            _board.HoveringPoint = null;
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
            Console.WriteLine(_playerAtTurn.Name + "'s turn! " + _playerAtTurn.state.ToString() + " men: " + _playerAtTurn.MenOnBoard.Count.ToString());

            _board.HoveringPoint = null;
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
