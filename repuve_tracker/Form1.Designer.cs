namespace repuve_tracker
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblReadTags = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dgvReaders = new System.Windows.Forms.DataGridView();
            this.lblReaderStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStatusTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnShowData = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_test = new System.Windows.Forms.Button();
            this.button_test2 = new System.Windows.Forms.Button();
            this.button_test3 = new System.Windows.Forms.Button();
            this.button_test4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReaders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(718, 546);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(170, 75);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 525);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "Total Reads:";
            // 
            // lblReadTags
            // 
            this.lblReadTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReadTags.AutoSize = true;
            this.lblReadTags.Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReadTags.ForeColor = System.Drawing.Color.Blue;
            this.lblReadTags.Location = new System.Drawing.Point(24, 558);
            this.lblReadTags.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReadTags.Name = "lblReadTags";
            this.lblReadTags.Size = new System.Drawing.Size(36, 45);
            this.lblReadTags.TabIndex = 3;
            this.lblReadTags.Text = "0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(508, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(380, 36);
            this.label2.TabIndex = 6;
            this.label2.Text = "REPUVE - 6204 Connector";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(33, 495);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 24);
            this.lblStatus.TabIndex = 7;
            // 
            // dgvReaders
            // 
            this.dgvReaders.AllowUserToAddRows = false;
            this.dgvReaders.AllowUserToDeleteRows = false;
            this.dgvReaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReaders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvReaders.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvReaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReaders.Location = new System.Drawing.Point(21, 81);
            this.dgvReaders.Margin = new System.Windows.Forms.Padding(2);
            this.dgvReaders.Name = "dgvReaders";
            this.dgvReaders.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReaders.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReaders.RowTemplate.Height = 28;
            this.dgvReaders.Size = new System.Drawing.Size(853, 412);
            this.dgvReaders.TabIndex = 8;
            // 
            // lblReaderStatus
            // 
            this.lblReaderStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReaderStatus.AutoSize = true;
            this.lblReaderStatus.Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReaderStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblReaderStatus.Location = new System.Drawing.Point(247, 558);
            this.lblReaderStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReaderStatus.Name = "lblReaderStatus";
            this.lblReaderStatus.Size = new System.Drawing.Size(46, 45);
            this.lblReaderStatus.TabIndex = 10;
            this.lblReaderStatus.Text = "--";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(249, 525);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 26);
            this.label5.TabIndex = 9;
            this.label5.Text = "Reader status:";
            // 
            // lblStatusTime
            // 
            this.lblStatusTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusTime.AutoSize = true;
            this.lblStatusTime.Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusTime.ForeColor = System.Drawing.Color.Blue;
            this.lblStatusTime.Location = new System.Drawing.Point(495, 558);
            this.lblStatusTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatusTime.Name = "lblStatusTime";
            this.lblStatusTime.Size = new System.Drawing.Size(46, 45);
            this.lblStatusTime.TabIndex = 12;
            this.lblStatusTime.Text = "--";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(498, 525);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(216, 26);
            this.label7.TabIndex = 11;
            this.label7.Text = "Last read timestamp:";
            // 
            // btnShowData
            // 
            this.btnShowData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowData.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowData.Location = new System.Drawing.Point(877, 81);
            this.btnShowData.Margin = new System.Windows.Forms.Padding(2);
            this.btnShowData.Name = "btnShowData";
            this.btnShowData.Size = new System.Drawing.Size(21, 75);
            this.btnShowData.TabIndex = 13;
            this.btnShowData.Text = "<";
            this.btnShowData.UseVisualStyleBackColor = true;
            this.btnShowData.Click += new System.EventHandler(this.btnShowData_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::repuve_tracker.Properties.Resources._2019_02_20;
            this.pictureBox1.Location = new System.Drawing.Point(21, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(296, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // button_test
            // 
            this.button_test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_test.Location = new System.Drawing.Point(466, 495);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(75, 23);
            this.button_test.TabIndex = 15;
            this.button_test.Text = "Test 1";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Visible = false;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // button_test2
            // 
            this.button_test2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_test2.Location = new System.Drawing.Point(547, 495);
            this.button_test2.Name = "button_test2";
            this.button_test2.Size = new System.Drawing.Size(75, 23);
            this.button_test2.TabIndex = 15;
            this.button_test2.Text = "Test 2";
            this.button_test2.UseVisualStyleBackColor = true;
            this.button_test2.Visible = false;
            this.button_test2.Click += new System.EventHandler(this.button_test_Click);
            // 
            // button_test3
            // 
            this.button_test3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_test3.Location = new System.Drawing.Point(628, 495);
            this.button_test3.Name = "button_test3";
            this.button_test3.Size = new System.Drawing.Size(75, 23);
            this.button_test3.TabIndex = 15;
            this.button_test3.Text = "Test 3";
            this.button_test3.UseVisualStyleBackColor = true;
            this.button_test3.Visible = false;
            this.button_test3.Click += new System.EventHandler(this.button_test_Click);
            // 
            // button_test4
            // 
            this.button_test4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_test4.Location = new System.Drawing.Point(709, 495);
            this.button_test4.Name = "button_test4";
            this.button_test4.Size = new System.Drawing.Size(75, 23);
            this.button_test4.TabIndex = 15;
            this.button_test4.Text = "Test 4";
            this.button_test4.UseVisualStyleBackColor = true;
            this.button_test4.Visible = false;
            this.button_test4.Click += new System.EventHandler(this.button_test_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 632);
            this.Controls.Add(this.button_test4);
            this.Controls.Add(this.button_test3);
            this.Controls.Add(this.button_test2);
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnShowData);
            this.Controls.Add(this.lblStatusTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblReaderStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvReaders);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblReadTags);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Sirit 6204 Demo Connector";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReaders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblReadTags;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView dgvReaders;
        private System.Windows.Forms.Label lblReaderStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStatusTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnShowData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.Button button_test2;
        private System.Windows.Forms.Button button_test3;
        private System.Windows.Forms.Button button_test4;
    }
}

