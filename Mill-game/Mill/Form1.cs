using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Mill.Engine {
    public partial class Form1 : Form {

        private FastLoop _fastLoop;
        private Input _input;

        private GameMaster _gameMaster;

        private List<Intersection> testList;

        public Form1() {

            InitializeComponent();
        }

        private void mainPanel_Load(object sender, EventArgs e) {
            
            // Instantiate main game loop
            _fastLoop = new FastLoop(GameLoop);

            // Instantiate inputs
            _input = new Input();
            _input.Mouse = new MouseInput(this, mainPanel);
            _input.Keyboard = new KeyboardInput(mainPanel);

            Utils.MainCamera = new Camera();
            Utils.MainCamera = new Camera(new Vector3(0f, 0f, 15f), new Vector3(0f, 0f, 0f));

            new DrawUtils(mainPanel.ClientRectangle.Size);
            DrawUtils.instance.InitGL();

            _gameMaster = new GameMaster(Utils.GameType.NineMorris);

            TestPoints();
        }

        private void TestPoints() {

            testList = new List<Intersection>();

            testList.Add(new Intersection(
                0, 
                new Vector3(0f, 0f, 0f)));
            testList.Add(new Intersection(
                0,
                new Vector3(1f, 1f, 0f)));
            testList.Add(new Intersection(
                0,
                new Vector3(0f, 1f, 0f)));
            testList.Add(new Intersection(
                0,
                new Vector3(-1f, -1f, 0f)));

        }

        void GameLoop(double elapsedTime) {

            //
            // Input code here
            //
            UpdateInput(elapsedTime);
            MouseInput();
            KeyboardInput();


            //
            // Update code here
            //
            //_gameMaster.Update(elapsedTime);


            //
            // Render code here
            //            
            DrawUtils.instance.OnRender();
            //DrawUtils.instance.RenderTestCircles(testList);
            DrawUtils.instance.RenderGrid(_gameMaster.Board.boardBackground, _gameMaster.Board.boardPoints);
            //_gameMaster.Render();


            //
            // Refresh screen
            //
            mainPanel.SwapBuffers();
            mainPanel.Refresh();
        }

        private void UpdateInput(double elapsedTime) {

            _input.Update(elapsedTime);
        }

        private void MouseInput() {

            if (_input.Mouse.LeftPressed) {
                
                Vector3 rayCoordinates = DrawUtils.instance.CameraToWorldPosition(_input.Mouse.Position.X, _input.Mouse.Position.Y);
                //Console.WriteLine(rayCoordinates.X + " " + rayCoordinates.Y + " " + rayCoordinates.Z);
                Utils.FindClosestIntersectionHitByRay(rayCoordinates, _gameMaster.Board.boardPoints);                
            }
        }

        private void KeyboardInput() {

            if (_input.Keyboard.IsKeyPressed(Keys.Up)) {

                Console.WriteLine("Up!");
            }

            if (_input.Keyboard.IsKeyPressed(Keys.T)) {

                Console.WriteLine("R");
            }
        }


    }
}
