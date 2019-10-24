namespace repuve_tracker
{
    partial class ControlBar
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lbconected = new System.Windows.Forms.Label();
            this.imdispStatus = new System.Windows.Forms.PictureBox();
            this.btConnect = new System.Windows.Forms.Button();
            this.btOptions = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imdispStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "REPUVE";
            // 
            // lbconected
            // 
            this.lbconected.AutoSize = true;
            this.lbconected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbconected.ForeColor = System.Drawing.Color.White;
            this.lbconected.Location = new System.Drawing.Point(130, 16);
            this.lbconected.Name = "lbconected";
            this.lbconected.Size = new System.Drawing.Size(65, 20);
            this.lbconected.TabIndex = 3;
            this.lbconected.Text = "waiting";
            // 
            // imdispStatus
            // 
            this.imdispStatus.Image = global::repuve_tracker.Properties.Resources.Sem_Rojo;
            this.imdispStatus.Location = new System.Drawing.Point(220, 13);
            this.imdispStatus.Name = "imdispStatus";
            this.imdispStatus.Size = new System.Drawing.Size(33, 29);
            this.imdispStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imdispStatus.TabIndex = 4;
            this.imdispStatus.TabStop = false;
            // 
            // btConnect
            // 
            this.btConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(79)))), ((int)(((byte)(101)))));
            this.btConnect.BackgroundImage = global::repuve_tracker.Properties.Resources.icon_io;
            this.btConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btConnect.Location = new System.Drawing.Point(285, 8);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(37, 39);
            this.btConnect.TabIndex = 5;
            this.btConnect.UseVisualStyleBackColor = false;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // btOptions
            // 
            this.btOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(79)))), ((int)(((byte)(101)))));
            this.btOptions.BackgroundImage = global::repuve_tracker.Properties.Resources.icon_gear;
            this.btOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btOptions.Location = new System.Drawing.Point(328, 8);
            this.btOptions.Name = "btOptions";
            this.btOptions.Size = new System.Drawing.Size(37, 39);
            this.btOptions.TabIndex = 6;
            this.btOptions.UseVisualStyleBackColor = false;
            this.btOptions.Click += new System.EventHandler(this.btOptions_Click);
            // 
            // ControlBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(79)))), ((int)(((byte)(101)))));
            this.Controls.Add(this.btOptions);
            this.Controls.Add(this.btConnect);
            this.Controls.Add(this.imdispStatus);
            this.Controls.Add(this.lbconected);
            this.Controls.Add(this.label1);
            this.Name = "ControlBar";
            this.Size = new System.Drawing.Size(380, 60);
            ((System.ComponentModel.ISupportInitialize)(this.imdispStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbconected;
        private System.Windows.Forms.PictureBox imdispStatus;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Button btOptions;
    }
}
