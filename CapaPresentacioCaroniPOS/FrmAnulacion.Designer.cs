
namespace CapaVisual_Login
{
    partial class FrmAnulacion
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
            this.LblAnulacion = new System.Windows.Forms.Label();
            this.BtnGuardar = new System.Windows.Forms.Button();
            this.BtnCancelar = new System.Windows.Forms.Button();
            this.CbxSelecResp = new System.Windows.Forms.ComboBox();
            this.LblResponsable = new System.Windows.Forms.Label();
            this.LblMotivo = new System.Windows.Forms.Label();
            this.CbxSelectMotivo = new System.Windows.Forms.ComboBox();
            this.TxtObservaciones = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // LblAnulacion
            // 
            this.LblAnulacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(125)))), ((int)(((byte)(123)))));
            this.LblAnulacion.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAnulacion.ForeColor = System.Drawing.Color.White;
            this.LblAnulacion.Location = new System.Drawing.Point(-1, -1);
            this.LblAnulacion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblAnulacion.Name = "LblAnulacion";
            this.LblAnulacion.Size = new System.Drawing.Size(767, 46);
            this.LblAnulacion.TabIndex = 1;
            this.LblAnulacion.Text = "Anulación";
            this.LblAnulacion.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // BtnGuardar
            // 
            this.BtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(185)))), ((int)(((byte)(50)))));
            this.BtnGuardar.FlatAppearance.BorderSize = 0;
            this.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGuardar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGuardar.ForeColor = System.Drawing.Color.White;
            this.BtnGuardar.Location = new System.Drawing.Point(569, 546);
            this.BtnGuardar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnGuardar.Name = "BtnGuardar";
            this.BtnGuardar.Size = new System.Drawing.Size(135, 48);
            this.BtnGuardar.TabIndex = 16;
            this.BtnGuardar.Text = "Guardar ";
            this.BtnGuardar.UseVisualStyleBackColor = false;
            this.BtnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // BtnCancelar
            // 
            this.BtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(38)))), ((int)(((byte)(56)))));
            this.BtnCancelar.FlatAppearance.BorderSize = 0;
            this.BtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancelar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelar.ForeColor = System.Drawing.Color.White;
            this.BtnCancelar.Location = new System.Drawing.Point(407, 546);
            this.BtnCancelar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnCancelar.Name = "BtnCancelar";
            this.BtnCancelar.Size = new System.Drawing.Size(135, 48);
            this.BtnCancelar.TabIndex = 15;
            this.BtnCancelar.Text = "Cancelar";
            this.BtnCancelar.UseVisualStyleBackColor = false;
            this.BtnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // CbxSelecResp
            // 
            this.CbxSelecResp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CbxSelecResp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxSelecResp.DropDownWidth = 123;
            this.CbxSelecResp.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxSelecResp.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.CbxSelecResp.FormattingEnabled = true;
            this.CbxSelecResp.Location = new System.Drawing.Point(220, 151);
            this.CbxSelecResp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CbxSelecResp.Name = "CbxSelecResp";
            this.CbxSelecResp.Size = new System.Drawing.Size(307, 27);
            this.CbxSelecResp.TabIndex = 14;
            this.CbxSelecResp.SelectionChangeCommitted += new System.EventHandler(this.CbxSelecResp_SelectionChangeCommitted);
            // 
            // LblResponsable
            // 
            this.LblResponsable.AutoSize = true;
            this.LblResponsable.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblResponsable.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LblResponsable.Location = new System.Drawing.Point(215, 106);
            this.LblResponsable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblResponsable.Name = "LblResponsable";
            this.LblResponsable.Size = new System.Drawing.Size(220, 19);
            this.LblResponsable.TabIndex = 13;
            this.LblResponsable.Text = "Responsable de anulación ";
            // 
            // LblMotivo
            // 
            this.LblMotivo.AutoSize = true;
            this.LblMotivo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMotivo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LblMotivo.Location = new System.Drawing.Point(215, 242);
            this.LblMotivo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblMotivo.Name = "LblMotivo";
            this.LblMotivo.Size = new System.Drawing.Size(60, 19);
            this.LblMotivo.TabIndex = 17;
            this.LblMotivo.Text = "Motivo";
            this.LblMotivo.Click += new System.EventHandler(this.label2_Click);
            // 
            // CbxSelectMotivo
            // 
            this.CbxSelectMotivo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CbxSelectMotivo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxSelectMotivo.DropDownWidth = 524;
            this.CbxSelectMotivo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxSelectMotivo.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.CbxSelectMotivo.FormattingEnabled = true;
            this.CbxSelectMotivo.Location = new System.Drawing.Point(220, 295);
            this.CbxSelectMotivo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CbxSelectMotivo.Name = "CbxSelectMotivo";
            this.CbxSelectMotivo.Size = new System.Drawing.Size(307, 27);
            this.CbxSelectMotivo.TabIndex = 18;
            // 
            // TxtObservaciones
            // 
            this.TxtObservaciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtObservaciones.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtObservaciones.Location = new System.Drawing.Point(44, 363);
            this.TxtObservaciones.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtObservaciones.Multiline = true;
            this.TxtObservaciones.Name = "TxtObservaciones";
            this.TxtObservaciones.Size = new System.Drawing.Size(659, 162);
            this.TxtObservaciones.TabIndex = 20;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.DimGray;
            this.panel7.Location = new System.Drawing.Point(741, -33);
            this.panel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(13, 678);
            this.panel7.TabIndex = 21;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Location = new System.Drawing.Point(-11, -33);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(13, 690);
            this.panel1.TabIndex = 22;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Location = new System.Drawing.Point(1, -10);
            this.panel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(704, 12);
            this.panel6.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Location = new System.Drawing.Point(3, 610);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(805, 12);
            this.panel2.TabIndex = 24;
            // 
            // FrmAnulacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(745, 614);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.TxtObservaciones);
            this.Controls.Add(this.CbxSelectMotivo);
            this.Controls.Add(this.LblMotivo);
            this.Controls.Add(this.BtnGuardar);
            this.Controls.Add(this.BtnCancelar);
            this.Controls.Add(this.CbxSelecResp);
            this.Controls.Add(this.LblResponsable);
            this.Controls.Add(this.LblAnulacion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmAnulacion";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmAnulacion";
            this.Load += new System.EventHandler(this.FrmAnulacion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblAnulacion;
        private System.Windows.Forms.Button BtnGuardar;
        private System.Windows.Forms.Button BtnCancelar;
        private System.Windows.Forms.ComboBox CbxSelecResp;
        private System.Windows.Forms.Label LblResponsable;
        private System.Windows.Forms.Label LblMotivo;
        private System.Windows.Forms.ComboBox CbxSelectMotivo;
        private System.Windows.Forms.TextBox TxtObservaciones;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel2;
    }
}