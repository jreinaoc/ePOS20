
namespace CapaVisual_Login
{
    partial class FrmClaveGerente
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
            this.LblClaveGerente = new System.Windows.Forms.Label();
            this.LblClave = new System.Windows.Forms.Label();
            this.CbxSelecGerentTiend = new System.Windows.Forms.ComboBox();
            this.LblSelecGerente = new System.Windows.Forms.Label();
            this.TxtClave = new System.Windows.Forms.TextBox();
            this.BtnGuardar = new System.Windows.Forms.Button();
            this.BtnCancelar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LbIdRol = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblClaveGerente
            // 
            this.LblClaveGerente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(125)))), ((int)(((byte)(123)))));
            this.LblClaveGerente.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblClaveGerente.ForeColor = System.Drawing.Color.White;
            this.LblClaveGerente.Location = new System.Drawing.Point(-1, 0);
            this.LblClaveGerente.Name = "LblClaveGerente";
            this.LblClaveGerente.Size = new System.Drawing.Size(558, 37);
            this.LblClaveGerente.TabIndex = 1;
            this.LblClaveGerente.Text = "Clave de Gerente";
            this.LblClaveGerente.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // LblClave
            // 
            this.LblClave.AutoSize = true;
            this.LblClave.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblClave.Location = new System.Drawing.Point(87, 214);
            this.LblClave.Name = "LblClave";
            this.LblClave.Size = new System.Drawing.Size(55, 19);
            this.LblClave.TabIndex = 9;
            this.LblClave.Text = "Clave";
            // 
            // CbxSelecGerentTiend
            // 
            this.CbxSelecGerentTiend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CbxSelecGerentTiend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxSelecGerentTiend.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxSelecGerentTiend.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.CbxSelecGerentTiend.FormattingEnabled = true;
            this.CbxSelecGerentTiend.Location = new System.Drawing.Point(73, 127);
            this.CbxSelecGerentTiend.Name = "CbxSelecGerentTiend";
            this.CbxSelecGerentTiend.Size = new System.Drawing.Size(406, 29);
            this.CbxSelecGerentTiend.TabIndex = 8;
            // 
            // LblSelecGerente
            // 
            this.LblSelecGerente.AutoSize = true;
            this.LblSelecGerente.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSelecGerente.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LblSelecGerente.Location = new System.Drawing.Point(204, 76);
            this.LblSelecGerente.Name = "LblSelecGerente";
            this.LblSelecGerente.Size = new System.Drawing.Size(165, 19);
            this.LblSelecGerente.TabIndex = 7;
            this.LblSelecGerente.Text = "Seleccione Gerente ";
            // 
            // TxtClave
            // 
            this.TxtClave.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtClave.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtClave.Location = new System.Drawing.Point(3, 2);
            this.TxtClave.Name = "TxtClave";
            this.TxtClave.Size = new System.Drawing.Size(211, 20);
            this.TxtClave.TabIndex = 15;
            this.TxtClave.UseSystemPasswordChar = true;
            this.TxtClave.TextChanged += new System.EventHandler(this.TxtClave_TextChanged_1);
            this.TxtClave.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtClave_KeyPress);
            // 
            // BtnGuardar
            // 
            this.BtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(185)))), ((int)(((byte)(50)))));
            this.BtnGuardar.FlatAppearance.BorderSize = 0;
            this.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGuardar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGuardar.ForeColor = System.Drawing.Color.White;
            this.BtnGuardar.Location = new System.Drawing.Point(302, 280);
            this.BtnGuardar.Name = "BtnGuardar";
            this.BtnGuardar.Size = new System.Drawing.Size(100, 29);
            this.BtnGuardar.TabIndex = 17;
            this.BtnGuardar.Text = "Guardar ";
            this.BtnGuardar.UseVisualStyleBackColor = false;
            this.BtnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // BtnCancelar
            // 
            this.BtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(38)))), ((int)(((byte)(56)))));
            this.BtnCancelar.FlatAppearance.BorderSize = 0;
            this.BtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancelar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelar.ForeColor = System.Drawing.Color.White;
            this.BtnCancelar.Location = new System.Drawing.Point(189, 280);
            this.BtnCancelar.Name = "BtnCancelar";
            this.BtnCancelar.Size = new System.Drawing.Size(97, 29);
            this.BtnCancelar.TabIndex = 16;
            this.BtnCancelar.Text = "Cancelar";
            this.BtnCancelar.UseVisualStyleBackColor = false;
            this.BtnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(135)))), ((int)(((byte)(129)))));
            this.panel2.Controls.Add(this.TxtClave);
            this.panel2.Location = new System.Drawing.Point(185, 212);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 24);
            this.panel2.TabIndex = 12;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.DimGray;
            this.panel7.Location = new System.Drawing.Point(555, -2);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(10, 365);
            this.panel7.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Location = new System.Drawing.Point(-8, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 360);
            this.panel1.TabIndex = 19;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DimGray;
            this.panel5.Location = new System.Drawing.Point(1, 359);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(556, 10);
            this.panel5.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DimGray;
            this.panel3.Location = new System.Drawing.Point(-123, -8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(802, 10);
            this.panel3.TabIndex = 21;
            // 
            // LbIdRol
            // 
            this.LbIdRol.AutoSize = true;
            this.LbIdRol.Location = new System.Drawing.Point(497, 46);
            this.LbIdRol.Name = "LbIdRol";
            this.LbIdRol.Size = new System.Drawing.Size(0, 13);
            this.LbIdRol.TabIndex = 22;
            this.LbIdRol.Visible = false;
            // 
            // FrmClaveGerente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(557, 361);
            this.Controls.Add(this.LbIdRol);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.BtnGuardar);
            this.Controls.Add(this.BtnCancelar);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.LblClave);
            this.Controls.Add(this.CbxSelecGerentTiend);
            this.Controls.Add(this.LblSelecGerente);
            this.Controls.Add(this.LblClaveGerente);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmClaveGerente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmClaveGerente";
            this.Load += new System.EventHandler(this.FrmClaveGerente_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblClaveGerente;
        private System.Windows.Forms.Label LblClave;
        private System.Windows.Forms.ComboBox CbxSelecGerentTiend;
        private System.Windows.Forms.Label LblSelecGerente;
        private System.Windows.Forms.TextBox TxtClave;
        private System.Windows.Forms.Button BtnGuardar;
        private System.Windows.Forms.Button BtnCancelar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Label LbIdRol;
    }
}