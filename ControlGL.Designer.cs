namespace SharpGL
{
    partial class ControlGL
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ControlGL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ControlGL";
            this.Size = new System.Drawing.Size(225, 231);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ControlGL_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlGL_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ControlGL_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ControlGL_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
