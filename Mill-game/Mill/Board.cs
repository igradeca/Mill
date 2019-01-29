using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class Board {

        public List<Intersection> boardPoints;
        public int layersNum;
        public int boardPointsNum;

        public Board(Utils.GameType gameType) {

            SetBoard(gameType);
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

            float intersecStart = Utils.gridPadding;
            float intersecEnd = Utils.gridSize.Width - (intersecStart);

            boardPoints = new List<Intersection>();

            for (int i = 0; i < layersNum; i++) {
                boardPoints.Add(new Intersection(i, new Vector3(
                    intersecStart + (Utils.layerOffset * i),
                    Utils.gridPadding + (Utils.layerOffset * i),
                    -0.1f)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    Utils.gridPadding + ((intersecEnd - intersecStart) / 2f),
                    Utils.gridPadding + (Utils.layerOffset * i),
                    -0.1f)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    intersecEnd - (Utils.layerOffset * i),
                    Utils.gridPadding + (Utils.layerOffset * i),
                    -0.1f)));

                boardPoints.Add(new Intersection(i, new Vector3(
                    intersecStart + (Utils.layerOffset * i),
                    Utils.gridPadding + ((intersecEnd - intersecStart) / 2f),
                    -0.1f)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    intersecEnd - (Utils.layerOffset * i),
                    Utils.gridPadding + ((intersecEnd - intersecStart) / 2f),
                    -0.1f)));

                boardPoints.Add(new Intersection(i, new Vector3(
                    intersecStart + (Utils.layerOffset * i),
                    intersecEnd - (Utils.layerOffset * i),
                    -0.1f)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    Utils.gridPadding + ((intersecEnd - intersecStart) / 2f),
                    intersecEnd - (Utils.layerOffset * i),
                    -0.1f)));
                boardPoints.Add(new Intersection(i, new Vector3(
                    intersecEnd - (Utils.layerOffset * i),
                    intersecEnd - (Utils.layerOffset * i),
                    -0.1f)));
            }
        }

        private void SetNineMorrisLines() {

            for (int i = 0; i < boardPoints.Count; i++) {

                boardPoints[i].adjacentPoints = new List<Intersection>();
                FindAdjacentPoints(i);
            }
        }

        private void FindAdjacentPoints(int index) {

            bool lookUp    = true;
            bool lookDown  = true;
            bool lookLeft  = true;
            bool lookRight = true;

            for (int i = 1; i <= (layersNum - boardPoints[index].layer); i++) {
                for (int j = 0; j < boardPoints.Count; j++) {
                    if (j == index) {
                        continue;
                    } else {                        
                        if (lookLeft) {
                            if (CheckPoint(boardPoints[index].location.X + (Utils.layerOffset * i),
                                boardPoints[index].location.Y, j)) {
                                boardPoints[index].adjacentPoints.Add(boardPoints[j]);
                                lookLeft = false;
                            }
                        }
                        
                        if (lookRight) {
                            if (CheckPoint(boardPoints[index].location.X - (Utils.layerOffset * i),
                                boardPoints[index].location.Y, j)) {
                                boardPoints[index].adjacentPoints.Add(boardPoints[j]);
                                lookRight = false;
                            }
                        }
                        
                        if (lookUp) {
                            if (CheckPoint(boardPoints[index].location.X,
                                boardPoints[index].location.Y + (Utils.layerOffset * i), j)) {
                                boardPoints[index].adjacentPoints.Add(boardPoints[j]);
                                lookUp = false;
                            }
                        }
                        
                        if (lookDown) {
                            if (CheckPoint(boardPoints[index].location.X,
                                boardPoints[index].location.Y - (Utils.layerOffset * i), j)) {
                                boardPoints[index].adjacentPoints.Add(boardPoints[j]);
                                lookDown = false;
                            }
                        }
                    }
                }
            }
        }

        private bool CheckPoint(float X, float Y, int index) {

            if (Math.Abs(boardPoints[index].location.X - X) < 0.1f && Math.Abs(boardPoints[index].location.Y - Y) < 0.1f) {
                return true;
            } else {
                return false;
            }
        }


    }
}
