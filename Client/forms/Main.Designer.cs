namespace Client.forms
{
    partial class Main
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
            this.dgvYourSea = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvOpponentSea = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.butStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYourSea)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpponentSea)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvYourSea
            // 
            this.dgvYourSea.AllowUserToAddRows = false;
            this.dgvYourSea.AllowUserToDeleteRows = false;
            this.dgvYourSea.AllowUserToResizeColumns = false;
            this.dgvYourSea.AllowUserToResizeRows = false;
            this.dgvYourSea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvYourSea.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvYourSea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvYourSea.ColumnHeadersVisible = false;
            this.dgvYourSea.Location = new System.Drawing.Point(3, 3);
            this.dgvYourSea.Name = "dgvYourSea";
            this.dgvYourSea.ReadOnly = true;
            this.dgvYourSea.RowHeadersVisible = false;
            this.dgvYourSea.RowHeadersWidth = 20;
            this.dgvYourSea.RowTemplate.Height = 25;
            this.dgvYourSea.Size = new System.Drawing.Size(252, 253);
            this.dgvYourSea.TabIndex = 0;
            this.dgvYourSea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvYourSea_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvYourSea);
            this.panel1.Location = new System.Drawing.Point(63, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 259);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvOpponentSea);
            this.panel2.Location = new System.Drawing.Point(510, 48);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(258, 259);
            this.panel2.TabIndex = 2;
            // 
            // dgvOpponentSea
            // 
            this.dgvOpponentSea.AllowUserToAddRows = false;
            this.dgvOpponentSea.AllowUserToDeleteRows = false;
            this.dgvOpponentSea.AllowUserToResizeColumns = false;
            this.dgvOpponentSea.AllowUserToResizeRows = false;
            this.dgvOpponentSea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOpponentSea.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOpponentSea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOpponentSea.ColumnHeadersVisible = false;
            this.dgvOpponentSea.Enabled = false;
            this.dgvOpponentSea.Location = new System.Drawing.Point(3, 3);
            this.dgvOpponentSea.Name = "dgvOpponentSea";
            this.dgvOpponentSea.ReadOnly = true;
            this.dgvOpponentSea.RowHeadersVisible = false;
            this.dgvOpponentSea.RowHeadersWidth = 20;
            this.dgvOpponentSea.RowTemplate.Height = 25;
            this.dgvOpponentSea.Size = new System.Drawing.Size(252, 253);
            this.dgvOpponentSea.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ваше поле:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(510, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Поле противника:";
            // 
            // butStart
            // 
            this.butStart.Location = new System.Drawing.Point(66, 387);
            this.butStart.Name = "butStart";
            this.butStart.Size = new System.Drawing.Size(118, 40);
            this.butStart.TabIndex = 5;
            this.butStart.Text = "Я готов!";
            this.butStart.UseVisualStyleBackColor = true;
            this.butStart.Click += new System.EventHandler(this.butStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 314);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Rjhf,ktq ";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.butStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Main";
            this.Text = "Морской бой";
            ((System.ComponentModel.ISupportInitialize)(this.dgvYourSea)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpponentSea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvYourSea;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvOpponentSea;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button butStart;
        private System.Windows.Forms.Label label3;
    }
}