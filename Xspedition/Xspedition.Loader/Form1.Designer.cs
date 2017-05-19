namespace Xspedition.Loader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnP1 = new System.Windows.Forms.Button();
            this.btnP2 = new System.Windows.Forms.Button();
            this.btnP3 = new System.Windows.Forms.Button();
            this.btnP4 = new System.Windows.Forms.Button();
            this.lblLoad = new System.Windows.Forms.Label();
            this.lblP1 = new System.Windows.Forms.Label();
            this.lblP2 = new System.Windows.Forms.Label();
            this.lblP3 = new System.Windows.Forms.Label();
            this.lblP4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(65, 31);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(102, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load Data";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.OnBtnLoadClick);
            // 
            // btnP1
            // 
            this.btnP1.Location = new System.Drawing.Point(65, 73);
            this.btnP1.Name = "btnP1";
            this.btnP1.Size = new System.Drawing.Size(102, 23);
            this.btnP1.TabIndex = 1;
            this.btnP1.Text = "Phase One";
            this.btnP1.UseVisualStyleBackColor = true;
            this.btnP1.Click += new System.EventHandler(this.OnBtnP1Click);
            // 
            // btnP2
            // 
            this.btnP2.Location = new System.Drawing.Point(65, 115);
            this.btnP2.Name = "btnP2";
            this.btnP2.Size = new System.Drawing.Size(102, 23);
            this.btnP2.TabIndex = 2;
            this.btnP2.Text = "Phase Two";
            this.btnP2.UseVisualStyleBackColor = true;
            this.btnP2.Click += new System.EventHandler(this.OnBtnP2Click);
            // 
            // btnP3
            // 
            this.btnP3.Location = new System.Drawing.Point(65, 157);
            this.btnP3.Name = "btnP3";
            this.btnP3.Size = new System.Drawing.Size(102, 23);
            this.btnP3.TabIndex = 3;
            this.btnP3.Text = "Phase Three";
            this.btnP3.UseVisualStyleBackColor = true;
            this.btnP3.Click += new System.EventHandler(this.OnBtnP3Click);
            // 
            // btnP4
            // 
            this.btnP4.Location = new System.Drawing.Point(65, 199);
            this.btnP4.Name = "btnP4";
            this.btnP4.Size = new System.Drawing.Size(102, 23);
            this.btnP4.TabIndex = 4;
            this.btnP4.Text = "Phase Four";
            this.btnP4.UseVisualStyleBackColor = true;
            this.btnP4.Click += new System.EventHandler(this.OnBtnP4Click);
            // 
            // lblLoad
            // 
            this.lblLoad.AutoSize = true;
            this.lblLoad.Location = new System.Drawing.Point(216, 36);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(0, 13);
            this.lblLoad.TabIndex = 5;
            // 
            // lblP1
            // 
            this.lblP1.AutoSize = true;
            this.lblP1.Location = new System.Drawing.Point(216, 78);
            this.lblP1.Name = "lblP1";
            this.lblP1.Size = new System.Drawing.Size(0, 13);
            this.lblP1.TabIndex = 6;
            // 
            // lblP2
            // 
            this.lblP2.AutoSize = true;
            this.lblP2.Location = new System.Drawing.Point(216, 120);
            this.lblP2.Name = "lblP2";
            this.lblP2.Size = new System.Drawing.Size(0, 13);
            this.lblP2.TabIndex = 7;
            // 
            // lblP3
            // 
            this.lblP3.AutoSize = true;
            this.lblP3.Location = new System.Drawing.Point(216, 162);
            this.lblP3.Name = "lblP3";
            this.lblP3.Size = new System.Drawing.Size(0, 13);
            this.lblP3.TabIndex = 8;
            // 
            // lblP4
            // 
            this.lblP4.AutoSize = true;
            this.lblP4.Location = new System.Drawing.Point(216, 204);
            this.lblP4.Name = "lblP4";
            this.lblP4.Size = new System.Drawing.Size(0, 13);
            this.lblP4.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 252);
            this.Controls.Add(this.lblP4);
            this.Controls.Add(this.lblP3);
            this.Controls.Add(this.lblP2);
            this.Controls.Add(this.lblP1);
            this.Controls.Add(this.lblLoad);
            this.Controls.Add(this.btnP4);
            this.Controls.Add(this.btnP3);
            this.Controls.Add(this.btnP2);
            this.Controls.Add(this.btnP1);
            this.Controls.Add(this.btnLoad);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnP1;
        private System.Windows.Forms.Button btnP2;
        private System.Windows.Forms.Button btnP3;
        private System.Windows.Forms.Button btnP4;
        private System.Windows.Forms.Label lblLoad;
        private System.Windows.Forms.Label lblP1;
        private System.Windows.Forms.Label lblP2;
        private System.Windows.Forms.Label lblP3;
        private System.Windows.Forms.Label lblP4;
    }
}

