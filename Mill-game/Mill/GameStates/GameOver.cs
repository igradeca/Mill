using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mill.Engine;
using Mill.Gameobjects;
using OpenTK;

namespace Mill.GameStates {
    public class GameOver : IGameObject {

        private StateSystem _system;
        private Input _input;

        public GameOver(StateSystem system, Input input) {

            _system = system;
            _input = input;
        }

        public void Update(double elapsedTime) {
            //throw new NotImplementedException();
        }

        public void Render() {
            //throw new NotImplementedException();
        }


    }
}
