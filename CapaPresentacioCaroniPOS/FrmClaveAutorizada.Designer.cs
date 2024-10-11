
namespace CapaVisual_Login
{
    partial class FrmClaveAutorizada
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
            this.LblClaveAutorizada = new System.Windows.Forms.Label();
            this.LblSelecGerente = new System.Windows.Forms.Label();
            this.CbxSelecGerent = new System.Windows.Forms.ComboBox();
            this.LblGeneraCodigo = new System.Windows.Forms.Label();
            this.BtnGenerar = new System.Windows.Forms.Button();
            this.LblClave = new System.Windows.Forms.Label();
            this.TxtClave = new System.Windows.Forms.TextBox();
            this.BtnCancelar = new System.Windows.Forms.Button();
            this.BtnGuardar = new System.Windows.Forms.Button();
            this.LblClaveAleatoria = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label36 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblClaveAutorizada
            // 
            this.LblClaveAutorizada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(125)))), ((int)(((byte)(123)))));
            this.LblClaveAutorizada.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblClaveAutorizada.ForeColor = System.Drawing.Color.White;
            this.LblClaveAutorizada.Location = new System.Drawing.Point(0, 0);
            this.LblClaveAutorizada.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblClaveAutorizada.Name = "LblClaveAutorizada";
            this.LblClaveAutorizada.Size = new System.Drawing.Size(821, 46);
            this.LblClaveAutorizada.TabIndex = 0;
            this.LblClaveAutorizada.Text = "Clave Autorizada";
            this.LblClaveAutorizada.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // LblSelecGerente
            // 
            this.LblSelecGerente.AutoSize = true;
            this.LblSelecGerente.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSelecGerente.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LblSelecGerente.Location = new System.Drawing.Point(51, 124);
            this.LblSelecGerente.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblSelecGerente.Name = "LblSelecGerente";
            this.LblSelecGerente.Size = new System.Drawing.Size(298, 23);
            this.LblSelecGerente.TabIndex = 1;
            this.LblSelecGerente.Text = "Seleccione Gerente Regional";
            // 
            // CbxSelecGerent
            // 
            this.CbxSelecGerent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CbxSelecGerent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxSelecGerent.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxSelecGerent.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.CbxSelecGerent.FormattingEnabled = true;
            this.CbxSelecGerent.Location = new System.Drawing.Point(51, 171);
            this.CbxSelecGerent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CbxSelecGerent.Name = "CbxSelecGerent";
            this.CbxSelecGerent.Size = new System.Drawing.Size(305, 31);
            this.CbxSelecGerent.TabIndex = 2;
            this.CbxSelecGerent.SelectedIndexChanged += new System.EventHandler(this.CbxSelecGerent_SelectedIndexChanged);
            this.CbxSelecGerent.Enter += new System.EventHandler(this.CbxSelecGerent_Enter);
            this.CbxSelecGerent.Leave += new System.EventHandler(this.CbxSelecGerent_Leave);
            // 
            // LblGeneraCodigo
            // 
            this.LblGeneraCodigo.AutoSize = true;
            this.LblGeneraCodigo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblGeneraCodigo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LblGeneraCodigo.Location = new System.Drawing.Point(599, 124);
            this.LblGeneraCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblGeneraCodigo.Name = "LblGeneraCodigo";
            this.LblGeneraCodigo.Size = new System.Drawing.Size(169, 23);
            this.LblGeneraCodigo.TabIndex = 3;
            this.LblGeneraCodigo.Text = "Generar Código";
            // 
            // BtnGenerar
            // 
            this.BtnGenerar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(185)))), ((int)(((byte)(64)))));
            this.BtnGenerar.FlatAppearance.BorderSize = 0;
            this.BtnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGenerar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGenerar.ForeColor = System.Drawing.Color.White;
            this.BtnGenerar.Location = new System.Drawing.Point(604, 161);
            this.BtnGenerar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnGenerar.Name = "BtnGenerar";
            this.BtnGenerar.Size = new System.Drawing.Size(164, 54);
            this.BtnGenerar.TabIndex = 4;
            this.BtnGenerar.Text = "Generar";
            this.BtnGenerar.UseVisualStyleBackColor = false;
            this.BtnGenerar.Click += new System.EventHandler(this.BtnGenerar_Click);
            // 
            // LblClave
            // 
            this.LblClave.AutoSize = true;
            this.LblClave.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblClave.Location = new System.Drawing.Point(139, 327);
            this.LblClave.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblClave.Name = "LblClave";
            this.LblClave.Size = new System.Drawing.Size(68, 23);
            this.LblClave.TabIndex = 5;
            this.LblClave.Text = "Clave";
            // 
            // TxtClave
            // 
            this.TxtClave.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtClave.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtClave.Location = new System.Drawing.Point(256, 330);
            this.TxtClave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtClave.Name = "TxtClave";
            this.TxtClave.Size = new System.Drawing.Size(376, 25);
            this.TxtClave.TabIndex = 6;
            this.TxtClave.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtClave_KeyPress);
            // 
            // BtnCancelar
            // 
            this.BtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(38)))), ((int)(((byte)(56)))));
            this.BtnCancelar.FlatAppearance.BorderSize = 0;
            this.BtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancelar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelar.ForeColor = System.Drawing.Color.White;
            this.BtnCancelar.Location = new System.Drawing.Point(475, 417);
            this.BtnCancelar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnCancelar.Name = "BtnCancelar";
            this.BtnCancelar.Size = new System.Drawing.Size(135, 48);
            this.BtnCancelar.TabIndex = 11;
            this.BtnCancelar.Text = "Cancelar";
            this.BtnCancelar.UseVisualStyleBackColor = false;
            this.BtnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // BtnGuardar
            // 
            this.BtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(185)))), ((int)(((byte)(50)))));
            this.BtnGuardar.FlatAppearance.BorderSize = 0;
            this.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGuardar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGuardar.ForeColor = System.Drawing.Color.White;
            this.BtnGuardar.Location = new System.Drawing.Point(636, 417);
            this.BtnGuardar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnGuardar.Name = "BtnGuardar";
            this.BtnGuardar.Size = new System.Drawing.Size(135, 48);
            this.BtnGuardar.TabIndex = 12;
            this.BtnGuardar.Text = "Guardar ";
            this.BtnGuardar.UseVisualStyleBackColor = false;
            this.BtnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // LblClaveAleatoria
            // 
            this.LblClaveAleatoria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblClaveAleatoria.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblClaveAleatoria.Location = new System.Drawing.Point(603, 219);
            this.LblClaveAleatoria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblClaveAleatoria.Name = "LblClaveAleatoria";
            this.LblClaveAleatoria.Size = new System.Drawing.Size(165, 53);
            this.LblClaveAleatoria.TabIndex = 13;
            this.LblClaveAleatoria.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DimGray;
            this.panel5.Location = new System.Drawing.Point(0, 497);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1069, 12);
            this.panel5.TabIndex = 14;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Location = new System.Drawing.Point(0, -10);
            this.panel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1069, 12);
            this.panel6.TabIndex = 15;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.DimGray;
            this.panel7.Location = new System.Drawing.Point(812, 1);
            this.panel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(13, 559);
            this.panel7.TabIndex = 16;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.DimGray;
            this.panel8.Location = new System.Drawing.Point(-11, -2);
            this.panel8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(13, 559);
            this.panel8.TabIndex = 17;
            // 
            // label36
            // 
            this.label36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(135)))), ((int)(((byte)(129)))));
            this.label36.Location = new System.Drawing.Point(253, 327);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(381, 31);
            this.label36.TabIndex = 120;
            // 
            // FrmClaveAutorizada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(817, 503);
            this.Controls.Add(this.TxtClave);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.LblClaveAleatoria);
            this.Controls.Add(this.BtnGuardar);
            this.Controls.Add(this.BtnCancelar);
            this.Controls.Add(this.LblClave);
            this.Controls.Add(this.BtnGenerar);
            this.Controls.Add(this.LblGeneraCodigo);
            this.Controls.Add(this.CbxSelecGerent);
            this.Controls.Add(this.LblSelecGerente);
            this.Controls.Add(this.LblClaveAutorizada);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmClaveAutorizada";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmClaveAutorizada";
            this.Load += new System.EventHandler(this.FrmClaveAutorizada_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblClaveAutorizada;
        private System.Windows.Forms.Label LblSelecGerente;
        private System.Windows.Forms.ComboBox CbxSelecGerent;
        private System.Windows.Forms.Label LblGeneraCodigo;
        private System.Windows.Forms.Button BtnGenerar;
        private System.Windows.Forms.Label LblClave;
        private System.Windows.Forms.TextBox TxtClave;
        private System.Windows.Forms.Button BtnCancelar;
        private System.Windows.Forms.Button BtnGuardar;
        private System.Windows.Forms.Label LblClaveAleatoria;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label36;
    }
}