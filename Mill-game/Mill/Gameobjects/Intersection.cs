using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Mill.Gameobjects {
    public class Intersection : InteractableObject {

        public bool Occupied;
        public List<Intersection> AdjacentPoints;

        public int Layer { get; }        

        public Intersection(int layer, Vector3 location, float detectionRadius = 0.3f) {

            Layer = layer;
            Location = new Vector3(
                location.X,
                location.Y,
                location.Z);
            DetectionRadius = detectionRadius;

            Occupied = false;
            AdjacentPoints = new List<Intersection>();
        }

        public void AddNewAdjacentPoint(Intersection adjacentPoint) {

            AdjacentPoints.Add(adjacentPoint);
        }


    }
}
