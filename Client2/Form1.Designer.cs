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
            ((System.ComponentModel.ISupportInitialize)(this.YourSea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentSea)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(12, 12);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(144, 20);
            this.txtIpAddress.TabIndex = 0;
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.Location = new System.Drawing.Point(162, 12);
            this.btnCheckConnection.Name = "btnCheckConnection";
            this.btnCheckConnection.Size = new System.Drawing.Size(100, 23);
            this.btnCheckConnection.TabIndex = 1;
            this.btnCheckConnection.Text = "Подключиться";
            this.btnCheckConnection.UseVisualStyleBackColor = true;
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
            this.YourSea.Location = new System.Drawing.Point(135, 77);
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
            this.btnConfirm.Location = new System.Drawing.Point(12, 77);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "Утвердить";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 106);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rbHorisontal
            // 
            this.rbHorisontal.AutoSize = true;
            this.rbHorisontal.Location = new System.Drawing.Point(13, 136);
            this.rbHorisontal.Name = "rbHorisontal";
            this.rbHorisontal.Size = new System.Drawing.Size(102, 17);
            this.rbHorisontal.TabIndex = 5;
            this.rbHorisontal.TabStop = true;
            this.rbHorisontal.Text = "Горизонтально";
            this.rbHorisontal.UseVisualStyleBackColor = true;
            // 
            // rbVertical
            // 
            this.rbVertical.AutoSize = true;
            this.rbVertical.Location = new System.Drawing.Point(13, 159);
            this.rbVertical.Name = "rbVertical";
            this.rbVertical.Size = new System.Drawing.Size(91, 17);
            this.rbVertical.TabIndex = 6;
            this.rbVertical.TabStop = true;
            this.rbVertical.Text = "Вертикально";
            this.rbVertical.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(724, 9);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(64, 23);
            this.btnDisconnect.TabIndex = 7;
            this.btnDisconnect.Text = "Выйти";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(135, 353);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(127, 58);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "ОТПРАВИТЬ";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // OpponentSea
            // 
            this.OpponentSea.AllowUserToAddRows = false;
            this.OpponentSea.AllowUserToDeleteRows = false;
            this.OpponentSea.AllowUserToResizeColumns = false;
            this.OpponentSea.AllowUserToResizeRows = false;
            this.OpponentSea.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.OpponentSea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OpponentSea.ColumnHeadersVisible = false;
            this.OpponentSea.Enabled = false;
            this.OpponentSea.Location = new System.Drawing.Point(475, 77);
            this.OpponentSea.MultiSelect = false;
            this.OpponentSea.Name = "OpponentSea";
            this.OpponentSea.ReadOnly = true;
            this.OpponentSea.RowHeadersVisible = false;
            this.OpponentSea.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.OpponentSea.Size = new System.Drawing.Size(250, 250);
            this.OpponentSea.TabIndex = 9;
            this.OpponentSea.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OpponentSea_CellDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

