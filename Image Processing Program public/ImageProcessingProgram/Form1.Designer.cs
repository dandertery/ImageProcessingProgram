using ScottPlot;
namespace ImageProcessingProgram
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.filenameLabel = new System.Windows.Forms.Label();
            this.copyPasteBox = new System.Windows.Forms.TextBox();
            this.PSNR = new ScottPlot.FormsPlot();
            this.imPlot1 = new ScottPlot.FormsPlot();
            this.imPlot2 = new ScottPlot.FormsPlot();
            this.imPlot3 = new ScottPlot.FormsPlot();
            this.Intensity1 = new ScottPlot.FormsPlot();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Intensity2 = new ScottPlot.FormsPlot();
            this.MSNR = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.filenameLabel.Location = new System.Drawing.Point(807, 0);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(120, 54);
            this.filenameLabel.TabIndex = 4;
            this.filenameLabel.Text = "lenna";
            // 
            // copyPasteBox
            // 
            this.copyPasteBox.Location = new System.Drawing.Point(12, 66);
            this.copyPasteBox.Multiline = true;
            this.copyPasteBox.Name = "copyPasteBox";
            this.copyPasteBox.Size = new System.Drawing.Size(290, 733);
            this.copyPasteBox.TabIndex = 9;
            // 
            // PSNR
            // 
            this.PSNR.Location = new System.Drawing.Point(309, 142);
            this.PSNR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PSNR.Name = "PSNR";
            this.PSNR.Size = new System.Drawing.Size(517, 520);
            this.PSNR.TabIndex = 10;
            // 
            // imPlot1
            // 
            this.imPlot1.Location = new System.Drawing.Point(-1, 828);
            this.imPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imPlot1.Name = "imPlot1";
            this.imPlot1.Size = new System.Drawing.Size(474, 402);
            this.imPlot1.TabIndex = 12;
            this.imPlot1.Load += new System.EventHandler(this.imPlot1_Load);
            // 
            // imPlot2
            // 
            this.imPlot2.Location = new System.Drawing.Point(481, 828);
            this.imPlot2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imPlot2.Name = "imPlot2";
            this.imPlot2.Size = new System.Drawing.Size(474, 402);
            this.imPlot2.TabIndex = 13;
            this.imPlot2.Load += new System.EventHandler(this.imPlot2_Load);
            // 
            // imPlot3
            // 
            this.imPlot3.Location = new System.Drawing.Point(970, 828);
            this.imPlot3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imPlot3.Name = "imPlot3";
            this.imPlot3.Size = new System.Drawing.Size(474, 402);
            this.imPlot3.TabIndex = 14;
            this.imPlot3.Load += new System.EventHandler(this.imPlot3_Load);
            // 
            // Intensity1
            // 
            this.Intensity1.Location = new System.Drawing.Point(1452, 142);
            this.Intensity1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Intensity1.Name = "Intensity1";
            this.Intensity1.Size = new System.Drawing.Size(517, 520);
            this.Intensity1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(203, 828);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "8";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(610, 828);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "16";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1019, 828);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "24";
            // 
            // Intensity2
            // 
            this.Intensity2.Location = new System.Drawing.Point(1452, 697);
            this.Intensity2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Intensity2.Name = "Intensity2";
            this.Intensity2.Size = new System.Drawing.Size(517, 520);
            this.Intensity2.TabIndex = 18;
            // 
            // MSNR
            // 
            this.MSNR.Location = new System.Drawing.Point(834, 142);
            this.MSNR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MSNR.Name = "MSNR";
            this.MSNR.Size = new System.Drawing.Size(517, 520);
            this.MSNR.TabIndex = 19;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2010, 1229);
            this.Controls.Add(this.MSNR);
            this.Controls.Add(this.Intensity2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imPlot3);
            this.Controls.Add(this.imPlot2);
            this.Controls.Add(this.imPlot1);
            this.Controls.Add(this.Intensity1);
            this.Controls.Add(this.PSNR);
            this.Controls.Add(this.copyPasteBox);
            this.Controls.Add(this.filenameLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label filenameLabel;
        private TextBox copyPasteBox;
        private FormsPlot PSNR;
        private FormsPlot imPlot1;
        private FormsPlot imPlot2;
        private FormsPlot imPlot3;
        private FormsPlot Intensity1;
        private Label label1;
        private Label label2;
        private Label label3;
        private FormsPlot Intensity2;
        private FormsPlot MSNR;
    }
}