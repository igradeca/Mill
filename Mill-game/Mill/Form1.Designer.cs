using System;
using OpenTK;
using System.Windows.Forms;
using System.Drawing;

namespace Mill.Engine {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mainPanel = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.Black;
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(500, 500);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.VSync = true;
            this.mainPanel.Load += new System.EventHandler(this.mainPanel_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.mainPanel);
            this.Name = "Form1";
            this.Text = "Mill";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl mainPanel;
    }
}

