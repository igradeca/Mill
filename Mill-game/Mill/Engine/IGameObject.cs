using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public interface IGameObject {

        void Update(double elapsedTime);
        void Render();

    }
}
