using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mill.Engine;
using Mill.Gameobjects;
using OpenTK;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Mill.GameStates {
    public class GameOver : IGameObject {

        private StateSystem _system;
        private Input _input;
        private GameData _gameData;

        private string _textToDisplay;

        public GameOver(StateSystem system, Input input, GameData gameData) {

            _system = system;
            _input = input;
            _gameData = gameData;

            _textToDisplay = "";
        }

        public void Update(double elapsedTime) {

            if (_textToDisplay == "") {
                _textToDisplay = "Game Over!\\n" + ((_gameData.Winner == true) ? "Blue" : "Red") + " player won!";
                _textToDisplay += "\\n\\nPress Enter to start new game...";
            }

            if (_input.Keyboard.IsKeyPressed(Keys.Enter)) {
                _textToDisplay = "";
                _gameData.Winner = null;
                _system.ChangeState("game_begin");
            }
        }

        public void Render() {

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(Color.White);

            DrawUtils.instance.RenderText(new Vector3(-5f, 1f, -4.9f), _textToDisplay, 50, 0.04f);
        }


    }
}
