
namespace CapaVisual_Login
{
    partial class FrmCierredeCaja
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
            this.tcCierreCaja = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Lbl_Tap1_DatosPersonal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCierreHora = new System.Windows.Forms.TextBox();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.lblPaso = new System.Windows.Forms.Label();
            this.btnSiguientePaso2 = new System.Windows.Forms.Button();
            this.btnAtrasPaso1 = new System.Windows.Forms.Button();
            this.tcCierreCaja.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcCierreCaja
            // 
            this.tcCierreCaja.Controls.Add(this.tabPage1);
            this.tcCierreCaja.Controls.Add(this.tabPage2);
            this.tcCierreCaja.Location = new System.Drawing.Point(76, 24);
            this.tcCierreCaja.Name = "tcCierreCaja";
            this.tcCierreCaja.SelectedIndex = 0;
            this.tcCierreCaja.Size = new System.Drawing.Size(1047, 517);
            this.tcCierreCaja.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblPaso);
            this.tabPage1.Controls.Add(this.btnSiguiente);
            this.tabPage1.Controls.Add(this.txtCierreHora);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.Lbl_Tap1_DatosPersonal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1039, 491);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnAtrasPaso1);
            this.tabPage2.Controls.Add(this.btnSiguientePaso2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1039, 491);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Lbl_Tap1_DatosPersonal
            // 
            this.Lbl_Tap1_DatosPersonal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(156)))));
            this.Lbl_Tap1_DatosPersonal.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Tap1_DatosPersonal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Lbl_Tap1_DatosPersonal.Location = new System.Drawing.Point(0, 67);
            this.Lbl_Tap1_DatosPersonal.Name = "Lbl_Tap1_DatosPersonal";
            this.Lbl_Tap1_DatosPersonal.Size = new System.Drawing.Size(1036, 38);
            this.Lbl_Tap1_DatosPersonal.TabIndex = 86;
            this.Lbl_Tap1_DatosPersonal.Text = "Cierre fuera de horario";
            this.Lbl_Tap1_DatosPersonal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(156)))));
            this.label1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.label1.Location = new System.Drawing.Point(2, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1036, 41);
            this.label1.TabIndex = 87;
            this.label1.Text = "Cierre punto de venta";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(156)))));
            this.label2.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.label2.Location = new System.Drawing.Point(111, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(811, 28);
            this.label2.TabIndex = 88;
            this.label2.Text = "Ingrese el mótivo del cierre";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCierreHora
            // 
            this.txtCierreHora.Location = new System.Drawing.Point(113, 148);
            this.txtCierreHora.Multiline = true;
            this.txtCierreHora.Name = "txtCierreHora";
            this.txtCierreHora.Size = new System.Drawing.Size(808, 77);
            this.txtCierreHora.TabIndex = 89;
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(184)))), ((int)(((byte)(52)))));
            this.btnSiguiente.FlatAppearance.BorderSize = 0;
            this.btnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguiente.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSiguiente.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSiguiente.Location = new System.Drawing.Point(916, 442);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(105, 31);
            this.btnSiguiente.TabIndex = 164;
            this.btnSiguiente.Text = "Siguiente";
            this.btnSiguiente.UseVisualStyleBackColor = false;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            // 
            // lblPaso
            // 
            this.lblPaso.AutoSize = true;
            this.lblPaso.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaso.Location = new System.Drawing.Point(865, 22);
            this.lblPaso.Name = "lblPaso";
            this.lblPaso.Size = new System.Drawing.Size(78, 25);
            this.lblPaso.TabIndex = 165;
            this.lblPaso.Text = "Paso 1";
            // 
            // btnSiguientePaso2
            // 
            this.btnSiguientePaso2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(184)))), ((int)(((byte)(52)))));
            this.btnSiguientePaso2.FlatAppearance.BorderSize = 0;
            this.btnSiguientePaso2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguientePaso2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSiguientePaso2.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSiguientePaso2.Location = new System.Drawing.Point(916, 442);
            this.btnSiguientePaso2.Name = "btnSiguientePaso2";
            this.btnSiguientePaso2.Size = new System.Drawing.Size(105, 31);
            this.btnSiguientePaso2.TabIndex = 165;
            this.btnSiguientePaso2.Text = "Siguiente";
            this.btnSiguientePaso2.UseVisualStyleBackColor = false;
            // 
            // btnAtrasPaso1
            // 
            this.btnAtrasPaso1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(44)))), ((int)(((byte)(59)))));
            this.btnAtrasPaso1.FlatAppearance.BorderSize = 0;
            this.btnAtrasPaso1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAtrasPaso1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAtrasPaso1.ForeColor = System.Drawing.SystemColors.Window;
            this.btnAtrasPaso1.Location = new System.Drawing.Point(805, 442);
            this.btnAtrasPaso1.Name = "btnAtrasPaso1";
            this.btnAtrasPaso1.Size = new System.Drawing.Size(105, 31);
            this.btnAtrasPaso1.TabIndex = 166;
            this.btnAtrasPaso1.Text = "Atras";
            this.btnAtrasPaso1.UseVisualStyleBackColor = false;
            this.btnAtrasPaso1.Click += new System.EventHandler(this.btnAtrasPaso1_Click);
            // 
            // FrmCierredeCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1121, 668);
            this.Controls.Add(this.tcCierreCaja);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmCierredeCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmCierredeCaja";
            this.tcCierreCaja.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcCierreCaja;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Lbl_Tap1_DatosPersonal;
        private System.Windows.Forms.TextBox txtCierreHora;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Label lblPaso;
        private System.Windows.Forms.Button btnSiguientePaso2;
        public System.Windows.Forms.Button btnAtrasPaso1;
    }
}