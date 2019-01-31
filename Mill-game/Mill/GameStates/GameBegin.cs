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

        public GameBegin(StateSystem system, Input input) {

            _system = system;
            _input = input;
        }

        public void Update(double elapsedTime) {
            throw new NotImplementedException();
        }

        public void Render() {

            GL.ClearColor(Color.Black);

        }


    }
}
