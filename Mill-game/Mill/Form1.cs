﻿using System;
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
using Mill.Engine;
using Mill.Gameobjects;
using Mill.GameStates;

namespace Mill {
    public partial class Form1 : Form {

        // Engine essetials
        private FastLoop _fastLoop;
        private StateSystem _system;
        private Input _input;
        private GameData _gameData;

        public Form1() {

            InitializeComponent();
        }

        private void mainPanel_Load(object sender, EventArgs e) {
            
            // Instantiate main game loop
            _fastLoop = new FastLoop(GameLoop);

            _gameData = new GameData(Utils.GameType.NineMorris);

            Engine.Text.FontTextureID = Engine.Text.LoadTexture(Engine.Text.FontBitmapFilename);

            InitializeInputs();
            InitializeGameStates();
            InitializeDisplay();
        }

        private void InitializeGameStates() {

            _system = new StateSystem();
            _system.AddState("game_begin", new GameBegin(_system, _input, _gameData));
            _system.AddState("inner_game", new InnerGame(_system, _input, _gameData));
            _system.AddState("game_over", new GameOver(_system, _input, _gameData));
            _system.ChangeState("game_begin");
        }

        private void InitializeInputs() {

            _input = new Input();
            _input.Mouse = new MouseInput(this, mainPanel);
            _input.Keyboard = new KeyboardInput(mainPanel);
        }

        private void InitializeDisplay() {

            //Utils.MainCamera = new Camera();
            Utils.MainCamera = new Camera(new Vector3(0f, 0f, 15f), new Vector3(0f, 0f, 0f));

            new DrawUtils(mainPanel.ClientRectangle.Size);
            DrawUtils.instance.InitGL();
        }

        void GameLoop(double elapsedTime) {

            //
            // Update code here
            //
            UpdateInput(elapsedTime);
            _system.Update(elapsedTime);

            //
            // Render code here
            //            
            DrawUtils.instance.OnRender();
            _system.Render();

            //
            // Refresh screen
            //
            mainPanel.SwapBuffers();
            mainPanel.Refresh();
        }

        private void UpdateInput(double elapsedTime) {

            _input.Update(elapsedTime);
        }

    }
}
