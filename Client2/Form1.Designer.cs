namespace Client2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.btnCheckConnection = new System.Windows.Forms.Button();
            this.YourSea = new System.Windows.Forms.DataGridView();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rbHorisontal = new System.Windows.Forms.RadioButton();
            this.rbVertical = new System.Windows.Forms.RadioButton();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.OpponentSea = new System.Windows.Forms.DataGridView();
            this.btnRandomPlacement = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.YourSea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentSea)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtIpAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIpAddress.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtIpAddress.ForeColor = System.Drawing.Color.DarkBlue;
            this.txtIpAddress.Location = new System.Drawing.Point(12, 22);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(144, 16);
            this.txtIpAddress.TabIndex = 0;
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCheckConnection.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCheckConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCheckConnection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(1)))), ((int)(((byte)(53)))), ((int)(((byte)(120)))));
            this.btnCheckConnection.Location = new System.Drawing.Point(162, 13);
            this.btnCheckConnection.Name = "btnCheckConnection";
            this.btnCheckConnection.Size = new System.Drawing.Size(100, 30);
            this.btnCheckConnection.TabIndex = 1;
            this.btnCheckConnection.Text = "Подключиться";
            this.btnCheckConnection.UseVisualStyleBackColor = false;
            this.btnCheckConnection.Click += new System.EventHandler(this.btnCheckConnection_Click);
            // 
            // YourSea
            // 
            this.YourSea.AllowUserToAddRows = false;
            this.YourSea.AllowUserToDeleteRows = false;
            this.YourSea.AllowUserToResizeColumns = false;
            this.YourSea.AllowUserToResizeRows = false;
            this.YourSea.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.YourSea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.YourSea.ColumnHeadersVisible = false;
            this.YourSea.Location = new System.Drawing.Point(135, 83);
            this.YourSea.MultiSelect = false;
            this.YourSea.Name = "YourSea";
            this.YourSea.ReadOnly = true;
            this.YourSea.RowHeadersVisible = false;
            this.YourSea.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.YourSea.Size = new System.Drawing.Size(250, 250);
            this.YourSea.TabIndex = 2;
            this.YourSea.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.YourSea_CellMouseClick);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.LightBlue;
            this.btnConfirm.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnConfirm.FlatAppearance.BorderSize = 2;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(1)))), ((int)(((byte)(72)))), ((int)(((byte)(125)))));
            this.btnConfirm.Location = new System.Drawing.Point(12, 83);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(92, 36);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "Утвердить";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RosyBrown;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCancel.FlatAppearance.BorderSize = 2;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(12, 125);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 34);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rbHorisontal
            // 
            this.rbHorisontal.AutoSize = true;
            this.rbHorisontal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rbHorisontal.Location = new System.Drawing.Point(13, 166);
            this.rbHorisontal.Name = "rbHorisontal";
            this.rbHorisontal.Size = new System.Drawing.Size(101, 18);
            this.rbHorisontal.TabIndex = 5;
            this.rbHorisontal.TabStop = true;
            this.rbHorisontal.Text = "Горизонтально";
            this.rbHorisontal.UseVisualStyleBackColor = true;
            // 
            // rbVertical
            // 
            this.rbVertical.AutoSize = true;
            this.rbVertical.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rbVertical.Location = new System.Drawing.Point(13, 191);
            this.rbVertical.Name = "rbVertical";
            this.rbVertical.Size = new System.Drawing.Size(91, 18);
            this.rbVertical.TabIndex = 6;
            this.rbVertical.TabStop = true;
            this.rbVertical.Text = "Вертикально";
            this.rbVertical.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(724, 10);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(64, 25);
            this.btnDisconnect.TabIndex = 7;
            this.btnDisconnect.Text = "Выйти";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Visible = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSend.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSend.FlatAppearance.BorderSize = 3;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Magneto", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(30)))), ((int)(((byte)(80)))), ((int)(((byte)(150)))));
            this.btnSend.Location = new System.Drawing.Point(135, 380);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(189, 62);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "ОТПРАВИТЬ";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // OpponentSea
            // 
            this.OpponentSea.AllowUserToAddRows = false;
            this.OpponentSea.AllowUserToDeleteRows = false;
            this.OpponentSea.AllowUserToResizeColumns = false;
            this.OpponentSea.AllowUserToResizeRows = false;
            this.OpponentSea.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.OpponentSea.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.OpponentSea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OpponentSea.ColumnHeadersVisible = false;
            this.OpponentSea.Enabled = false;
            this.OpponentSea.Location = new System.Drawing.Point(475, 83);
            this.OpponentSea.MultiSelect = false;
            this.OpponentSea.Name = "OpponentSea";
            this.OpponentSea.ReadOnly = true;
            this.OpponentSea.RowHeadersVisible = false;
            this.OpponentSea.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.OpponentSea.Size = new System.Drawing.Size(250, 250);
            this.OpponentSea.TabIndex = 9;
            this.OpponentSea.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OpponentSea_CellDoubleClick);
            // 
            // btnRandomPlacement
            // 
            this.btnRandomPlacement.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnRandomPlacement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRandomPlacement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRandomPlacement.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnRandomPlacement.Location = new System.Drawing.Point(12, 299);
            this.btnRandomPlacement.Name = "btnRandomPlacement";
            this.btnRandomPlacement.Size = new System.Drawing.Size(92, 34);
            this.btnRandomPlacement.TabIndex = 10;
            this.btnRandomPlacement.Text = "Рандом?";
            this.btnRandomPlacement.UseVisualStyleBackColor = true;
            this.btnRandomPlacement.Click += new System.EventHandler(this.btnRandomPlacement_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(35)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(800, 485);
            this.Controls.Add(this.btnRandomPlacement);
            this.Controls.Add(this.OpponentSea);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.rbVertical);
            this.Controls.Add(this.rbHorisontal);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.YourSea);
            this.Controls.Add(this.btnCheckConnection);
            this.Controls.Add(this.txtIpAddress);
            this.Font = new System.Drawing.Font("Microsoft Uighur", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.YourSea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentSea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Button btnCheckConnection;
        private System.Windows.Forms.DataGridView YourSea;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbHorisontal;
        private System.Windows.Forms.RadioButton rbVertical;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.DataGridView OpponentSea;
        private System.Windows.Forms.Button btnRandomPlacement;
    }
}

