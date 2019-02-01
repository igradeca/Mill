using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mill.Engine;
using Mill.Gameobjects;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Drawing;

namespace Mill.GameStates {
    public class InnerGame : IGameObject {

        private StateSystem _system;
        private Input _input;
        /*
        private double _animationLenght;
        private double _animationTime;
        */
        // Gameobjects
        private GameData _gameData;
        private GameManager _gameManager;
        private Board _board;

        /*
        private Vector3 _pointLocation;
        Vector3 start;
        Vector3 end;
        */

        public InnerGame(StateSystem system, Input input, GameData gameData) {

            _system = system;
            _input = input;
            _gameData = gameData;

            OnGameStart();
        }

        public void OnGameStart() {

            //_gameTime = 5f; // current game time
            //_gameData.Winner = null;

            //_gameMaster = new GameManager(Utils.GameType.NineMorris);
            _board = new Board(_gameData.GameType);

            _gameManager = new GameManager(_gameData, _input, _board);

            /*
            // test
            start = new Vector3(-1f, 0f, 1f);
            end = new Vector3(1f, 0f, 1f);
            _animationTime = 5;
            _animationLenght = 5;
            */
        }

        public void Update(double elapsedTime) {
            /*
            _animationTime -= elapsedTime;
            if (_animationTime > 0) {
                _pointLocation = Utils.Lerp(start, end, (float)(_animationTime / _animationLenght));
            }
            */

            KeyboardInput();

            if (_gameData.Winner.HasValue) {
                OnGameStart();                
                _system.ChangeState("game_over");
            }

            _gameManager.Update(elapsedTime);

            
        }

        private void KeyboardInput() {

            if (_input.Keyboard.IsKeyPressed(Keys.R)) {

                OnGameStart();
                _system.ChangeState("game_begin");
            }
        }

        public void Render() {
            /*
            GL.PointSize(20);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.RoyalBlue);
            GL.Vertex3(
                _pointLocation);
            GL.End();
            */

            _board.Render();
            _gameManager.Render();
        }



    }
}
