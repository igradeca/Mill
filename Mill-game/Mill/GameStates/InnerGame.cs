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

        private double _gameTime;

        // Gameobjects
        private GameMaster _gameMaster;
        private Board _board;

        public InnerGame(StateSystem system, Input input) {

            _system = system;
            _input = input;

            OnGameStart();
        }

        public void OnGameStart() {

            _gameTime = 10f; // current game time

            _gameMaster = new GameMaster(Utils.GameType.NineMorris);
            _board = new Board(Utils.GameType.NineMorris);
        }

        public void Update(double elapsedTime) {

            _gameTime -= elapsedTime;

            if (_gameTime <= 0) {
                OnGameStart();
                _system.ChangeState("game_over");
            }

            MouseInput();
        }

        public void Render() {

            _board.Render();
        }


        private void MouseInput() {

            if (_input.Mouse.LeftPressed) {

                Vector3 rayCoordinates = DrawUtils.instance.CameraToWorldPosition(_input.Mouse.Position.X, _input.Mouse.Position.Y);
                //Console.WriteLine(rayCoordinates.X + " " + rayCoordinates.Y + " " + rayCoordinates.Z);
                Utils.FindClosestIntersectionHitByRay(rayCoordinates, _board.boardPoints);
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
