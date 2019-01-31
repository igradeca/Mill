using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Mill.Engine;

namespace Mill.Gameobjects {
    public class Board : IGameObject {

        public Engine.Point[] BoardBackground;

        public List<Intersection> BoardPoints;
        public int LayersNum;
        public int BoardPointsNum;

        public Intersection HoveringPoint;

        public Board(Utils.GameType gameType) {

            SetBoardBackground();
            SetBoard(gameType);
        }

        #region SettingBoard

        private void SetBoardBackground() {

            BoardBackground = new Engine.Point[4];

            BoardBackground[0] = new Engine.Point(
                Utils.GridSize.Width / 2f, 
                Utils.GridSize.Height / 2f);
            BoardBackground[1] = new Engine.Point(
                -Utils.GridSize.Width / 2f,
                Utils.GridSize.Height / 2f);
            BoardBackground[2] = new Engine.Point(
                -Utils.GridSize.Width / 2f,
                -Utils.GridSize.Height / 2f);
            BoardBackground[3] = new Engine.Point(
                Utils.GridSize.Width / 2f,
                -Utils.GridSize.Height / 2f);
        }

        private void SetBoard(Utils.GameType gameType) {

            switch (gameType) {
                case Utils.GameType.ThreeMoriss:
                    break;

                case Utils.GameType.SixMorris:
                    break;

                case Utils.GameType.NineMorris:
                    NineMorrrisBoard();
                    break;

                case Utils.GameType.TwelveMorris:
                    break;

                case Utils.GameType.LaskerMorris:
                    break;
            }
        }

        private void NineMorrrisBoard() {

            LayersNum = 3;
            BoardPointsNum = 8;

            SetNineMorrisPoints();
            SetNineMorrisLines();
        }

        private void SetNineMorrisPoints() {

            float intersecStart = Utils.GridPadding;
            float intersecEnd = Utils.GridSize.Width - (intersecStart);

            float depth = 0f;

            BoardPoints = new List<Intersection>();

            for (int i = 0; i < LayersNum; i++) {

                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + intersecStart + (Utils.LayerOffset * i),
                    BoardBackground[2].Y + Utils.GridPadding + (Utils.LayerOffset * i),
                    depth)));
                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    BoardBackground[2].Y + Utils.GridPadding + (Utils.LayerOffset * i),
                    depth)));
                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + intersecEnd - (Utils.LayerOffset * i),
                    BoardBackground[2].Y + Utils.GridPadding + (Utils.LayerOffset * i),
                    depth)));

                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + intersecStart + (Utils.LayerOffset * i),
                    BoardBackground[2].Y + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    depth)));
                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + intersecEnd - (Utils.LayerOffset * i),
                    BoardBackground[2].Y + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    depth)));

                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + intersecStart + (Utils.LayerOffset * i),
                    BoardBackground[2].Y + intersecEnd - (Utils.LayerOffset * i),
                    depth)));
                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    BoardBackground[2].Y + intersecEnd - (Utils.LayerOffset * i),
                    depth)));
                BoardPoints.Add(new Intersection(i, new Vector3(
                    BoardBackground[2].X + intersecEnd - (Utils.LayerOffset * i),
                    BoardBackground[2].Y + intersecEnd - (Utils.LayerOffset * i),
                    depth)));
            }
        }

        private void SetNineMorrisLines() {

            for (int i = 0; i < BoardPoints.Count; i++) {

                BoardPoints[i].AdjacentPoints = new List<Intersection>();
                FindAdjacentPoints(i);
            }
        }

        private void FindAdjacentPoints(int index) {

            bool lookUp    = true;
            bool lookDown  = true;
            bool lookLeft  = true;
            bool lookRight = true;

            for (int i = 1; i <= (LayersNum - BoardPoints[index].Layer); i++) {
                for (int j = 0; j < BoardPoints.Count; j++) {
                    if (j == index) {
                        continue;
                    } else {                        
                        if (lookLeft) {
                            if (CheckPoint(BoardPoints[index].Location.X + (Utils.LayerOffset * i),
                                BoardPoints[index].Location.Y, j)) {
                                BoardPoints[index].AdjacentPoints.Add(BoardPoints[j]);
                                lookLeft = false;
                            }
                        }
                        
                        if (lookRight) {
                            if (CheckPoint(BoardPoints[index].Location.X - (Utils.LayerOffset * i),
                                BoardPoints[index].Location.Y, j)) {
                                BoardPoints[index].AdjacentPoints.Add(BoardPoints[j]);
                                lookRight = false;
                            }
                        }
                        
                        if (lookUp) {
                            if (CheckPoint(BoardPoints[index].Location.X,
                                BoardPoints[index].Location.Y + (Utils.LayerOffset * i), j)) {
                                BoardPoints[index].AdjacentPoints.Add(BoardPoints[j]);
                                lookUp = false;
                            }
                        }
                        
                        if (lookDown) {
                            if (CheckPoint(BoardPoints[index].Location.X,
                                BoardPoints[index].Location.Y - (Utils.LayerOffset * i), j)) {
                                BoardPoints[index].AdjacentPoints.Add(BoardPoints[j]);
                                lookDown = false;
                            }
                        }
                    }
                }
            }
        }

        private bool CheckPoint(float X, float Y, int index) {

            if (Math.Abs(BoardPoints[index].Location.X - X) < 0.1f && Math.Abs(BoardPoints[index].Location.Y - Y) < 0.1f) {
                return true;
            } else {
                return false;
            }
        }

        #endregion

        public void Update(double elapsedTime) {

        }

        public void Render() {

            // Board
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.SandyBrown);
            for (int i = 0; i < BoardBackground.Length; i++) {
                GL.Vertex3(
                BoardBackground[i].X,
                BoardBackground[i].Y,
                -0.05f);
            }
            GL.End();

            // Intersections   
            GL.PointSize(10);
            for (int i = 0; i < BoardPoints.Count; i++) {
                GL.Begin(PrimitiveType.Points);
                GL.Color3(Color.Black);
                GL.Vertex3(BoardPoints[i].Location);
                GL.End();
            }

            // Selected point
            if (HoveringPoint != null) {
                GL.LineWidth(2);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(Color.Green);
                for (int i = 0; i < 360; i++) {
                    float degInRad = i * Utils.DEG2RAD;
                    GL.Vertex3(
                        HoveringPoint.Location.X + Math.Cos(degInRad) * 0.5f,
                        HoveringPoint.Location.Y + Math.Sin(degInRad) * 0.5f,
                        0f);
                }
                GL.End();
            }

            // Lines
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.SaddleBrown);
            for (int i = 0; i < BoardPoints.Count; i++) {
                for (int j = 0; j < BoardPoints[i].AdjacentPoints.Count; j++) {
                    GL.Vertex3(BoardPoints[i].Location);
                    GL.Vertex3(BoardPoints[i].AdjacentPoints[j].Location);
                }
            }
            GL.End();
        }

    }
}
