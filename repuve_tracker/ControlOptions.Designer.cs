namespace repuve_tracker
{
    partial class ControlOptions
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
            this.Sel6204 = new System.Windows.Forms.RadioButton();
            this.SelID4000 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Sel6204
            // 
            this.Sel6204.AutoSize = true;
            this.Sel6204.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sel6204.ForeColor = System.Drawing.Color.White;
            this.Sel6204.Location = new System.Drawing.Point(117, 17);
            this.Sel6204.Name = "Sel6204";
            this.Sel6204.Size = new System.Drawing.Size(63, 24);
            this.Sel6204.TabIndex = 0;
            this.Sel6204.TabStop = true;
            this.Sel6204.Text = "6204";
            this.Sel6204.UseVisualStyleBackColor = true;            
            this.Sel6204.Click+= new System.EventHandler(this.Sel6204_CheckedChanged);
            // 
            // SelID4000
            // 
            this.SelID4000.AutoSize = true;
            this.SelID4000.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelID4000.ForeColor = System.Drawing.Color.White;
            this.SelID4000.Location = new System.Drawing.Point(262, 17);
            this.SelID4000.Name = "SelID4000";
            this.SelID4000.Size = new System.Drawing.Size(80, 24);
            this.SelID4000.TabIndex = 1;
            this.SelID4000.TabStop = true;
            this.SelID4000.Text = "ID4000";
            this.SelID4000.UseVisualStyleBackColor = true;
          
            this.SelID4000.Click += new System.EventHandler(this.SelID4000_CheckedChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(79)))), ((int)(((byte)(101)))));
            this.button1.BackgroundImage = global::repuve_tracker.Properties.Resources.arrow_left;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Location = new System.Drawing.Point(18, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 28);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ControlOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(79)))), ((int)(((byte)(101)))));
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SelID4000);
            this.Controls.Add(this.Sel6204);
            this.Name = "ControlOptions";
            this.Size = new System.Drawing.Size(380, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Sel6204;
        private System.Windows.Forms.RadioButton SelID4000;
        private System.Windows.Forms.Button button1;
    }
}
