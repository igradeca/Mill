using Mill.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mill.Gameobjects;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Mill {
    public class Player : IGameObject {

        public string Name { get; private set; }

        public int NumberOfUnplaceMen { get; private set; }
        public List<Intersection> MenOnBoard;

        public Utils.PlayerState state { get; set; }

        public Player(string name, int maxNumberOfPieces) {

            Name = name;
            NumberOfUnplaceMen = maxNumberOfPieces;
            state = Utils.PlayerState.PlacingMen;

            MenOnBoard = new List<Intersection>();
        }

        public void StartTurn() {

            state = GetPlayerState();
        }

        private Utils.PlayerState GetPlayerState() {

            if (NumberOfUnplaceMen > 0) {
                return Utils.PlayerState.PlacingMen;
            } else if (MenOnBoard.Count > 3) {
                return Utils.PlayerState.MovingMen;
            } else if (MenOnBoard.Count <= 3 && MenOnBoard.Count > 2) {
                return Utils.PlayerState.Fly;
            } else {
                return Utils.PlayerState.GameOver;
            }
        }

        public void PlaceManAt(Vector3 location) {

            MenOnBoard.Add(new Intersection(0, location));
            --NumberOfUnplaceMen;
        }

        public void Update(double elapsedTime) {

        }

        public void Render() {

            // Intersections   
            GL.PointSize(20);
            for (int i = 0; i < MenOnBoard.Count; i++) {
                GL.Begin(PrimitiveType.Points);
                GL.Color3((Name == "Blue") ? Color.Blue : Color.Red);
                GL.Vertex3(MenOnBoard[i].Location);
                GL.End();
            }
        }

        
    }
}
