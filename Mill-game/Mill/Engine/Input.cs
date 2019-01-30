using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class Input {

        public MouseInput Mouse { get; set; }
        public KeyboardInput Keyboard { get; set; }

        public void Update(double elapsedTime) {

            Mouse.Update(elapsedTime);
            Keyboard.Process();
        }


    }
}
