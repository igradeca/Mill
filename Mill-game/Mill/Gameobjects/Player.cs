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
        public Intersection ManToMove;

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

        public void PlaceManAt(Intersection point) {

            MenOnBoard.Add(point);
            --NumberOfUnplaceMen;
        }

        public void MoveManAt(Intersection movingPoint) {

            ManToMove.Occupied = false;
            MenOnBoard.Remove(ManToMove);
            ManToMove = null;

            MenOnBoard.Add(movingPoint);
            movingPoint.Occupied = true;
        }

        public void Update(double elapsedTime) {

        }

        public void Render() {

            // Intersections   
            GL.PointSize(20);
            for (int i = 0; i < MenOnBoard.Count; i++) {
                GL.Begin(PrimitiveType.Points);
                GL.Color3((Name == "Blue") ? Color.Blue : Color.Red);
                GL.Vertex3(
                    MenOnBoard[i].Location.X,
                    MenOnBoard[i].Location.Y, 
                    0.2f);
                GL.End();
            }
            
            if (state == Utils.PlayerState.MovingMen && ManToMove != null) {

                GL.LineWidth(5);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3((Name == "Blue") ? Color.Blue : Color.Red);
                for (int i = 0; i < ManToMove.AdjacentPoints.Count; i++) {
                    if (!ManToMove.AdjacentPoints[i].Occupied) {
                        GL.Vertex3(
                            ManToMove.Location.X, 
                            ManToMove.Location.Y, 
                            0.2f);
                        GL.Vertex3(
                            ManToMove.AdjacentPoints[i].Location.X,
                            ManToMove.AdjacentPoints[i].Location.Y,
                            0.2f);
                    }                    
                }
                GL.End();
            }
        }



        
    }
}
