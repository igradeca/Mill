using Mill.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Mill.GameStates {
    public class GameBegin : IGameObject {

        private StateSystem _system;
        private Input _input;
        private GameData _gameData;

        private string _textToDisplay;

        public GameBegin(StateSystem system, Input input, GameData gameData) {

            _system = system;
            _input = input;
            _gameData = gameData;

            _textToDisplay =
            "Mill - Nine Men's Morris\\n\\n" +
            "How To Play:\\n" +
            "2 players board game. Each player has 9 men.\\n\\n" +
            "Placing men phase: Left click on empty point\\nto place your man.\\n" +
            "Moving men phase: Left click to select your\\nman and then select adjacent point to move it.\\n" +
            "Fly phase: Starts if you have 3 men left on\\nboard. You can move your men anywhere you want.\\n" +
            "Remove Opponent: If you have three men on\\ncontiguous points in a straight line, verticallyor horizontally, then" +
            " you have formed a mill andyou can remove one opponent's man.\\n\\n" +
            "Press Enter to start the game...";
        }

        public void Update(double elapsedTime) {

            if (_input.Keyboard.IsKeyPressed(System.Windows.Forms.Keys.Enter)) {
                Console.WriteLine("Start game...");
                //Text.RemoveTexture();
                _system.ChangeState("inner_game");
            }
        }

        public void Render() {

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);

            DrawUtils.instance.RenderText(new OpenTK.Vector3(-10.5f, 6.5f, -4.9f), _textToDisplay, 24, 0.04f);
        }


    }
}
