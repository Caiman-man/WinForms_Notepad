
namespace Ivanov_WF_NotePad
{
    partial class Themes
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
            this.lightButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.limeButton = new System.Windows.Forms.RadioButton();
            this.darkGreyButton = new System.Windows.Forms.RadioButton();
            this.oceanBlueButton = new System.Windows.Forms.RadioButton();
            this.darkBlueButton = new System.Windows.Forms.RadioButton();
            this.darkRaspberryButton = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lightButton
            // 
            this.lightButton.AutoSize = true;
            this.lightButton.Location = new System.Drawing.Point(6, 32);
            this.lightButton.Name = "lightButton";
            this.lightButton.Size = new System.Drawing.Size(59, 23);
            this.lightButton.TabIndex = 0;
            this.lightButton.TabStop = true;
            this.lightButton.Text = "Light";
            this.lightButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.limeButton);
            this.groupBox1.Controls.Add(this.darkGreyButton);
            this.groupBox1.Controls.Add(this.oceanBlueButton);
            this.groupBox1.Controls.Add(this.darkBlueButton);
            this.groupBox1.Controls.Add(this.darkRaspberryButton);
            this.groupBox1.Controls.Add(this.lightButton);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 129);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose theme:";
            // 
            // limeButton
            // 
            this.limeButton.AutoSize = true;
            this.limeButton.Location = new System.Drawing.Point(123, 90);
            this.limeButton.Name = "limeButton";
            this.limeButton.Size = new System.Drawing.Size(58, 23);
            this.limeButton.TabIndex = 5;
            this.limeButton.TabStop = true;
            this.limeButton.Text = "Lime";
            this.limeButton.UseVisualStyleBackColor = true;
            // 
            // darkGreyButton
            // 
            this.darkGreyButton.AutoSize = true;
            this.darkGreyButton.Location = new System.Drawing.Point(6, 90);
            this.darkGreyButton.Name = "darkGreyButton";
            this.darkGreyButton.Size = new System.Drawing.Size(87, 23);
            this.darkGreyButton.TabIndex = 4;
            this.darkGreyButton.TabStop = true;
            this.darkGreyButton.Text = "DarkGrey";
            this.darkGreyButton.UseVisualStyleBackColor = true;
            // 
            // oceanBlueButton
            // 
            this.oceanBlueButton.AutoSize = true;
            this.oceanBlueButton.Location = new System.Drawing.Point(123, 61);
            this.oceanBlueButton.Name = "oceanBlueButton";
            this.oceanBlueButton.Size = new System.Drawing.Size(98, 23);
            this.oceanBlueButton.TabIndex = 3;
            this.oceanBlueButton.TabStop = true;
            this.oceanBlueButton.Text = "OceanBlue";
            this.oceanBlueButton.UseVisualStyleBackColor = true;
            // 
            // darkBlueButton
            // 
            this.darkBlueButton.AutoSize = true;
            this.darkBlueButton.Location = new System.Drawing.Point(6, 61);
            this.darkBlueButton.Name = "darkBlueButton";
            this.darkBlueButton.Size = new System.Drawing.Size(86, 23);
            this.darkBlueButton.TabIndex = 2;
            this.darkBlueButton.TabStop = true;
            this.darkBlueButton.Text = "DarkBlue";
            this.darkBlueButton.UseVisualStyleBackColor = true;
            // 
            // darkRaspberryButton
            // 
            this.darkRaspberryButton.AutoSize = true;
            this.darkRaspberryButton.Location = new System.Drawing.Point(123, 32);
            this.darkRaspberryButton.Name = "darkRaspberryButton";
            this.darkRaspberryButton.Size = new System.Drawing.Size(122, 23);
            this.darkRaspberryButton.TabIndex = 1;
            this.darkRaspberryButton.TabStop = true;
            this.darkRaspberryButton.Text = "DarkRaspberry";
            this.darkRaspberryButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(75, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.okButton_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(174, 147);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(93, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Themes
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(279, 176);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Themes";
            this.Text = "Themes";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton lightButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton limeButton;
        private System.Windows.Forms.RadioButton darkGreyButton;
        private System.Windows.Forms.RadioButton oceanBlueButton;
        private System.Windows.Forms.RadioButton darkBlueButton;
        private System.Windows.Forms.RadioButton darkRaspberryButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}