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

            _gameManager = new GameManager(_gameData, _board);
        }

        public void Update(double elapsedTime) {

            //_gameTime -= elapsedTime;
            /*
            if (_gameTime <= 0) {                
                OnGameStart();
                _gameMaster.Winner = true;
                _system.ChangeState("game_over");
            }
            */
            MouseInput();

            _gameManager.Update(elapsedTime);


        }

        public void Render() {

            _board.Render();
            _gameManager.Render();
        }

        private void MouseInput() {

            if (!_gameManager.ActionIsAllowed()) {
                return;
            }

            if (_input.Mouse.LeftPressed) {

                Vector3 rayCoordinates = DrawUtils.instance.CameraToWorldPosition(_input.Mouse.Position.X, _input.Mouse.Position.Y);
                //Console.WriteLine(rayCoordinates.X + " " + rayCoordinates.Y + " " + rayCoordinates.Z);

                if (_board.SelectedPoint != null && Utils.HasRayHitGameObject(rayCoordinates, _board.SelectedPoint)) {
                    _gameManager.PlaceNewMan();                    
                } else {
                    Utils.FindClosestIntersectionHitByRay(rayCoordinates, _board.BoardPoints, ref _board.SelectedPoint);
                }

            }

            if (_input.Mouse.RightPressed) {

                _board.SelectedPoint = null;
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


    }
}
