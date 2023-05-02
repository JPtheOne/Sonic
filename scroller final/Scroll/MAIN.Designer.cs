namespace Scroll
{
    partial class MAIN
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PCT_CANVAS = new System.Windows.Forms.PictureBox();
            this.PNL_MAIN = new System.Windows.Forms.Panel();
            this.TIMER = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PCT_CANVAS)).BeginInit();
            this.PNL_MAIN.SuspendLayout();
            this.SuspendLayout();
            // 
            // PCT_CANVAS
            // 
            this.PCT_CANVAS.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PCT_CANVAS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.PCT_CANVAS.Location = new System.Drawing.Point(75, 60);
            this.PCT_CANVAS.Name = "PCT_CANVAS";
            this.PCT_CANVAS.Size = new System.Drawing.Size(1164, 645);
            this.PCT_CANVAS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PCT_CANVAS.TabIndex = 0;
            this.PCT_CANVAS.TabStop = false;
            // 
            // PNL_MAIN
            // 
            this.PNL_MAIN.BackColor = System.Drawing.Color.Black;
            this.PNL_MAIN.Controls.Add(this.PCT_CANVAS);
            this.PNL_MAIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PNL_MAIN.Location = new System.Drawing.Point(0, 0);
            this.PNL_MAIN.Name = "PNL_MAIN";
            this.PNL_MAIN.Size = new System.Drawing.Size(1321, 756);
            this.PNL_MAIN.TabIndex = 1;
            // 
            // TIMER
            // 
            this.TIMER.Enabled = true;
            this.TIMER.Interval = 25;
            this.TIMER.Tick += new System.EventHandler(this.TIMER_Tick);
            // 
            // MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 756);
            this.Controls.Add(this.PNL_MAIN);
            this.Name = "MAIN";
            this.Text = "PLATFORM";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MAIN_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MAIN_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MAIN_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.PCT_CANVAS)).EndInit();
            this.PNL_MAIN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PCT_CANVAS;
        private System.Windows.Forms.Panel PNL_MAIN;
        private System.Windows.Forms.Timer TIMER;
    }
}

