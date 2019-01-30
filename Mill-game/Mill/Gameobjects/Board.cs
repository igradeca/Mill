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

        public Engine.Point[] boardBackground;

        public List<Intersection> boardPoints;
        public int layersNum;
        public int boardPointsNum;

        public Board(Utils.GameType gameType) {

            SetBoardBackground();
            SetBoard(gameType);
        }

        #region SettingBoard

        private void SetBoardBackground() {

            boardBackground = new Engine.Point[4];

            boardBackground[0] = new Engine.Point(
                Utils.GridSize.Width / 2f, 
                Utils.GridSize.Height / 2f);
            boardBackground[1] = new Engine.Point(
                -Utils.GridSize.Width / 2f,
                Utils.GridSize.Height / 2f);
            boardBackground[2] = new Engine.Point(
                -Utils.GridSize.Width / 2f,
                -Utils.GridSize.Height / 2f);
            boardBackground[3] = new Engine.Point(
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

            layersNum = 3;
            boardPointsNum = 8;

            SetNineMorrisPoints();
            SetNineMorrisLines();
        }

        private void SetNineMorrisPoints() {

            float intersecStart = Utils.GridPadding;
            float intersecEnd = Utils.GridSize.Width - (intersecStart);

            float depth = 0f;

            boardPoints = new List<Intersection>();

            for (int i = 0; i < layersNum; i++) {

                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + intersecStart + (Utils.LayerOffset * i),
                    boardBackground[2].Y + Utils.GridPadding + (Utils.LayerOffset * i),
                    depth)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    boardBackground[2].Y + Utils.GridPadding + (Utils.LayerOffset * i),
                    depth)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + intersecEnd - (Utils.LayerOffset * i),
                    boardBackground[2].Y + Utils.GridPadding + (Utils.LayerOffset * i),
                    depth)));

                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + intersecStart + (Utils.LayerOffset * i),
                    boardBackground[2].Y + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    depth)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + intersecEnd - (Utils.LayerOffset * i),
                    boardBackground[2].Y + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    depth)));

                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + intersecStart + (Utils.LayerOffset * i),
                    boardBackground[2].Y + intersecEnd - (Utils.LayerOffset * i),
                    depth)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + Utils.GridPadding + ((intersecEnd - intersecStart) / 2f),
                    boardBackground[2].Y + intersecEnd - (Utils.LayerOffset * i),
                    depth)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    boardBackground[2].X + intersecEnd - (Utils.LayerOffset * i),
                    boardBackground[2].Y + intersecEnd - (Utils.LayerOffset * i),
                    depth)));
            }
        }

        private void SetNineMorrisLines() {

            for (int i = 0; i < boardPoints.Count; i++) {

                boardPoints[i].AdjacentPoints = new List<Intersection>();
                FindAdjacentPoints(i);
            }
        }

        private void FindAdjacentPoints(int index) {

            bool lookUp    = true;
            bool lookDown  = true;
            bool lookLeft  = true;
            bool lookRight = true;

            for (int i = 1; i <= (layersNum - boardPoints[index].Layer); i++) {
                for (int j = 0; j < boardPoints.Count; j++) {
                    if (j == index) {
                        continue;
                    } else {                        
                        if (lookLeft) {
                            if (CheckPoint(boardPoints[index].Location.X + (Utils.LayerOffset * i),
                                boardPoints[index].Location.Y, j)) {
                                boardPoints[index].AdjacentPoints.Add(boardPoints[j]);
                                lookLeft = false;
                            }
                        }
                        
                        if (lookRight) {
                            if (CheckPoint(boardPoints[index].Location.X - (Utils.LayerOffset * i),
                                boardPoints[index].Location.Y, j)) {
                                boardPoints[index].AdjacentPoints.Add(boardPoints[j]);
                                lookRight = false;
                            }
                        }
                        
                        if (lookUp) {
                            if (CheckPoint(boardPoints[index].Location.X,
                                boardPoints[index].Location.Y + (Utils.LayerOffset * i), j)) {
                                boardPoints[index].AdjacentPoints.Add(boardPoints[j]);
                                lookUp = false;
                            }
                        }
                        
                        if (lookDown) {
                            if (CheckPoint(boardPoints[index].Location.X,
                                boardPoints[index].Location.Y - (Utils.LayerOffset * i), j)) {
                                boardPoints[index].AdjacentPoints.Add(boardPoints[j]);
                                lookDown = false;
                            }
                        }
                    }
                }
            }
        }

        private bool CheckPoint(float X, float Y, int index) {

            if (Math.Abs(boardPoints[index].Location.X - X) < 0.1f && Math.Abs(boardPoints[index].Location.Y - Y) < 0.1f) {
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
            for (int i = 0; i < boardBackground.Length; i++) {
                GL.Vertex3(
                boardBackground[i].X,
                boardBackground[i].Y,
                -0.05f);
            }
            GL.End();

            // Intersections   
            GL.PointSize(10);
            for (int i = 0; i < boardPoints.Count; i++) {
                if (boardPoints[i].Occupied) {
                    GL.Color3(Color.Red);
                } else {
                    GL.Color3(Color.Black);
                }
                GL.Begin(PrimitiveType.Points);
                GL.Vertex3(boardPoints[i].Location);
                GL.End();
            }

            // Lines
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.SaddleBrown);
            for (int i = 0; i < boardPoints.Count; i++) {
                for (int j = 0; j < boardPoints[i].AdjacentPoints.Count; j++) {
                    GL.Vertex3(boardPoints[i].Location);
                    GL.Vertex3(boardPoints[i].AdjacentPoints[j].Location);
                }
            }
            GL.End();
        }


    }
}
