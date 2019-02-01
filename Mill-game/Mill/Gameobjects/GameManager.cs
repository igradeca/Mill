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

        private string _playerInfoText;
        private string _statusText;

        // Animation things
        private double? _animationTime;
        private double? _animationPassedTime;
        private Vector3 startAnim, endAnim, currentAnim;
        private Intersection destination;

        public GameManager(GameData gameData, Input input, Board board) {

            _gameData = gameData;
            _input = input;
            _board = board;

            _bluePlayer = new Player("Blue", _gameData.MaxPiecesNumber);
            _redPlayer = new Player("Red", _gameData.MaxPiecesNumber);

            _playerInfoText = "";
            _statusText = "";

            _playerAtTurn = _redPlayer;
            StartNextTurn();
        }

        public void Update(double elapsedTime) {

            if (_animationTime.HasValue) {
                _animationPassedTime -= elapsedTime;

                if (_animationPassedTime <= 0) {
                    _playerAtTurn.MoveManAt(destination);
                    _animationTime = null;
                    AfterMoveIsMade();

                } else {
                    //currentAnim = Utils.Lerp(startAnim, endAnim, 1 - (float)(_animationPassedTime / _animationTime));
                    currentAnim = Vector3.Lerp(startAnim, endAnim, 1 - (float)(_animationPassedTime / _animationTime));
                }
            }

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
                    Console.WriteLine("Game over.");
                    break;
            }
        }
        public void PlaceNewMan() {

            if (_board.HoveringPoint.Occupied == false) {
                StatusToRender(Utils.GameInfo.MenPlaced);
                Console.WriteLine(_playerAtTurn.Name + " is placing man...");
                _board.HoveringPoint.Occupied = true;
                _playerAtTurn.PlaceManAt(_board.HoveringPoint);
                destination = _board.HoveringPoint;
                AfterMoveIsMade();
            } else {
                StatusToRender(Utils.GameInfo.CannotPlace);
                Console.WriteLine("This point is occupied! You cannot place your man here!");

            }
        }

        public void MoveMan() {

            if (_board.HoveringPoint.Occupied && _playerAtTurn.MenOnBoard.Exists(x => x.Location == _board.HoveringPoint.Location)) {
                _playerAtTurn.SelectedMan = _board.HoveringPoint;

            } else if (_playerAtTurn.SelectedMan != null && !_board.HoveringPoint.Occupied && ArePointsAdjacent(_playerAtTurn.SelectedMan, _board.HoveringPoint)) {
                //_playerAtTurn.MoveManAt(_board.HoveringPoint);
                // Animation
                Move();
                //AfterMoveIsMade();
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
                //_playerAtTurn.MoveManAt(_board.HoveringPoint);
                // Animation()
                Move();
                //AfterMoveIsMade();
            }
        }

        public void RemoveOpponentsMan() {

            Player opponent = (_playerAtTurn == _bluePlayer) ? _redPlayer : _bluePlayer;
            StatusToRender(Utils.GameInfo.SelectOpponent);
            Console.WriteLine("Opponent is " + opponent.Name + " player");

            if (_board.HoveringPoint.Occupied && opponent.MenOnBoard.Exists(x => x == _board.HoveringPoint)) {
                opponent.RemoveMan(_board.HoveringPoint);
                StatusToRender(Utils.GameInfo.ManDown);
                Console.WriteLine("One man down.");
                EndTurn();
            }
        }

        private void Move() {

            if (_playerAtTurn.state == Utils.PlayerState.MovingMen || _playerAtTurn.state == Utils.PlayerState.Fly) {
                Console.WriteLine("Animating...");
                destination = _board.HoveringPoint;
                Animate();
                _playerAtTurn.RemoveSelectedMan();
            }
        }

        private void AfterMoveIsMade() {

            if (_playerAtTurn.ThreeManLined(destination)) {
                StatusToRender(Utils.GameInfo.ThreeMenLined);
                Console.WriteLine("Three Men Lined! BINGO!");
                destination = null;
                _playerAtTurn.state = Utils.PlayerState.RemoveOpponentsMan;
            } else {
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

            StatusToRender(Utils.GameInfo.TurnBegin);
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

            RenderMovement();

            GL.Enable(EnableCap.Texture2D);
            GL.Color3((_playerAtTurn.Name == "Blue") ? Color.RoyalBlue : Color.Red);
            DrawUtils.instance.RenderText(new Vector3(-6f, 4f, 4f), _playerInfoText, 40, 0.02f);
            GL.Color3(Color.White);
            DrawUtils.instance.RenderText(new Vector3(-6f, -4.5f, 4f), _statusText, 40, 0.02f);
            GL.Disable(EnableCap.Texture2D);            
        }
        
        public void StatusToRender(Utils.GameInfo status) {

            switch (status) {
                case Utils.GameInfo.TurnBegin:
                    _playerInfoText = _playerAtTurn.Name + " player\\n"
                        + "Men on board: " + _playerAtTurn.MenOnBoard.Count;

                    _statusText = _playerAtTurn.Name + "'s turn!";
                    if (_playerAtTurn.NumberOfUnplaceMen > 0) {
                        _statusText += " Place new man.";
                    } else if (_playerAtTurn.MenOnBoard.Count > 3) {
                        _statusText += " Move your men.";
                    } else {
                        _statusText += " Fly mode!";
                    }
                    break;
                case Utils.GameInfo.MenPlaced:
                    _statusText = "Man is placed.";
                    break;
                case Utils.GameInfo.ThreeMenLined:
                    _statusText = "Three men lined! Remove one opponents.";
                    break;
                case Utils.GameInfo.CannotPlace:
                    _statusText = "Can't place here.";
                    break;
                case Utils.GameInfo.SelectOpponent:
                    _statusText = "Select one of opponents men.";
                    break;
                case Utils.GameInfo.ManDown:
                    _statusText = "One man down!";
                    break;
            }
        }

        private void Animate() {          
            // from _playerAtTurn.SelectedMan
            // to _board.HoveringPoint
            startAnim = _playerAtTurn.SelectedMan.Location;
            endAnim = destination.Location;

            double time = Vector3.Distance(startAnim, endAnim) * 0.2d;

            _animationPassedTime = time;
            _animationTime = time;
        }

        private void RenderMovement() {

            if (_animationTime == null) {
                return;
            }

            GL.PointSize(20);
            GL.Begin(PrimitiveType.Points);
            GL.Color3((_playerAtTurn.Name == "Blue") ? Color.RoyalBlue : Color.Red);
            GL.Vertex3(
                currentAnim.X,
                currentAnim.Y,
                currentAnim.Z + 0.2f);
            GL.End();
        }


    }
}
