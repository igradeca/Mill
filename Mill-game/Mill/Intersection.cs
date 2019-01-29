using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Mill.Engine {
    public class Intersection : InteractableObject {

        public bool occupied;
        public List<Intersection> adjacentPoints;

        public int layer { get; }        

        public Intersection(int layer, Vector3 location) {

            this.layer = layer;
            this.location = new Vector3(
                location.X,
                location.Y,
                location.Z);
            
            occupied = false;
            adjacentPoints = new List<Intersection>();
        }

        public void AddNewAdjacentPoint(Intersection adjacentPoint) {

            adjacentPoints.Add(adjacentPoint);
        }


    }
}
