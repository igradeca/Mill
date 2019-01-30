using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mill.Engine;
using Mill.Gameobjects;
using OpenTK;
using System.Windows.Forms;

namespace Mill.GameStates {
    public class GameOver : IGameObject {

        private StateSystem _system;
        private Input _input;
        private GameManager _gameManager;

        public GameOver(StateSystem system, Input input, GameManager gameManager) {

            _system = system;
            _input = input;
            _gameManager = gameManager;
        }

        public void Update(double elapsedTime) {

            if (_input.Keyboard.IsKeyPressed(Keys.Enter)) {
                Console.WriteLine("Game Over!");
                Console.WriteLine("Player " + ((_gameManager.Winner == true) ? "blue" : "red") + " won!");
            }
            //
            //throw new NotImplementedException();
        }

        public void Render() {
            //throw new NotImplementedException();
        }


    }
}
