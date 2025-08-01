
namespace CapaVisual_Login
{
    partial class FrmReimpresionDocumentos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbl_Utiliarios = new System.Windows.Forms.Label();
            this.DtReinHasta = new System.Windows.Forms.DateTimePicker();
            this.DtReinDesde = new System.Windows.Forms.DateTimePicker();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btn_pg5_reporteX = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lbl_hasta = new System.Windows.Forms.Label();
            this.lbl_desde = new System.Windows.Forms.Label();
            this.CbTipoDocumento = new System.Windows.Forms.ComboBox();
            this.lbl_TipoDocumento = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.gexMensajesDANA = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gexMensajesDANA)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Utiliarios
            // 
            this.lbl_Utiliarios.AutoSize = true;
            this.lbl_Utiliarios.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Utiliarios.Location = new System.Drawing.Point(35, 35);
            this.lbl_Utiliarios.Name = "lbl_Utiliarios";
            this.lbl_Utiliarios.Size = new System.Drawing.Size(101, 25);
            this.lbl_Utiliarios.TabIndex = 45;
            this.lbl_Utiliarios.Text = "Utilitarios";
            // 
            // DtReinHasta
            // 
            this.DtReinHasta.CustomFormat = "dd/MM/yyyy";
            this.DtReinHasta.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.DtReinHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtReinHasta.Location = new System.Drawing.Point(478, 178);
            this.DtReinHasta.Name = "DtReinHasta";
            this.DtReinHasta.Size = new System.Drawing.Size(127, 27);
            this.DtReinHasta.TabIndex = 43;
            // 
            // DtReinDesde
            // 
            this.DtReinDesde.CustomFormat = "dd/MM/yyyy";
            this.DtReinDesde.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.DtReinDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtReinDesde.Location = new System.Drawing.Point(337, 179);
            this.DtReinDesde.Name = "DtReinDesde";
            this.DtReinDesde.Size = new System.Drawing.Size(127, 27);
            this.DtReinDesde.TabIndex = 42;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(184)))), ((int)(((byte)(52)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(337, 374);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(101, 32);
            this.button5.TabIndex = 41;
            this.button5.Text = "Imprimir";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(156)))));
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(625, 176);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(96, 34);
            this.button6.TabIndex = 40;
            this.button6.Text = "Buscar";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btn_pg5_reporteX
            // 
            this.btn_pg5_reporteX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.btn_pg5_reporteX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pg5_reporteX.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.btn_pg5_reporteX.ForeColor = System.Drawing.Color.White;
            this.btn_pg5_reporteX.Location = new System.Drawing.Point(727, 176);
            this.btn_pg5_reporteX.Name = "btn_pg5_reporteX";
            this.btn_pg5_reporteX.Size = new System.Drawing.Size(98, 34);
            this.btn_pg5_reporteX.TabIndex = 39;
            this.btn_pg5_reporteX.Text = "Reporte X";
            this.btn_pg5_reporteX.UseVisualStyleBackColor = false;
            this.btn_pg5_reporteX.Click += new System.EventHandler(this.btn_pg5_reporteX_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(184)))), ((int)(((byte)(52)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(831, 177);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 34);
            this.button4.TabIndex = 38;
            this.button4.Text = "Reporte Z";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lbl_hasta
            // 
            this.lbl_hasta.AutoSize = true;
            this.lbl_hasta.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.lbl_hasta.Location = new System.Drawing.Point(475, 150);
            this.lbl_hasta.Name = "lbl_hasta";
            this.lbl_hasta.Size = new System.Drawing.Size(56, 21);
            this.lbl_hasta.TabIndex = 37;
            this.lbl_hasta.Text = "Hasta";
            // 
            // lbl_desde
            // 
            this.lbl_desde.AutoSize = true;
            this.lbl_desde.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.lbl_desde.Location = new System.Drawing.Point(333, 150);
            this.lbl_desde.Name = "lbl_desde";
            this.lbl_desde.Size = new System.Drawing.Size(59, 21);
            this.lbl_desde.TabIndex = 36;
            this.lbl_desde.Text = "Desde";
            // 
            // CbTipoDocumento
            // 
            this.CbTipoDocumento.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.CbTipoDocumento.FormattingEnabled = true;
            this.CbTipoDocumento.Location = new System.Drawing.Point(168, 181);
            this.CbTipoDocumento.Name = "CbTipoDocumento";
            this.CbTipoDocumento.Size = new System.Drawing.Size(160, 29);
            this.CbTipoDocumento.TabIndex = 35;
            // 
            // lbl_TipoDocumento
            // 
            this.lbl_TipoDocumento.AutoSize = true;
            this.lbl_TipoDocumento.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.lbl_TipoDocumento.Location = new System.Drawing.Point(164, 150);
            this.lbl_TipoDocumento.Name = "lbl_TipoDocumento";
            this.lbl_TipoDocumento.Size = new System.Drawing.Size(164, 21);
            this.lbl_TipoDocumento.TabIndex = 34;
            this.lbl_TipoDocumento.Text = "Tipo de documento";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(173)))));
            this.label10.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(30, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1048, 35);
            this.label10.TabIndex = 33;
            this.label10.Text = "Reimpresión de documentos";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gexMensajesDANA
            // 
            this.gexMensajesDANA.AllowUserToAddRows = false;
            this.gexMensajesDANA.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gexMensajesDANA.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gexMensajesDANA.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.gexMensajesDANA.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gexMensajesDANA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gexMensajesDANA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gexMensajesDANA.DefaultCellStyle = dataGridViewCellStyle2;
            this.gexMensajesDANA.EnableHeadersVisualStyles = false;
            this.gexMensajesDANA.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.gexMensajesDANA.Location = new System.Drawing.Point(168, 216);
            this.gexMensajesDANA.Name = "gexMensajesDANA";
            this.gexMensajesDANA.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gexMensajesDANA.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gexMensajesDANA.RowHeadersVisible = false;
            this.gexMensajesDANA.RowHeadersWidth = 51;
            this.gexMensajesDANA.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gexMensajesDANA.Size = new System.Drawing.Size(270, 152);
            this.gexMensajesDANA.TabIndex = 55;
            // 
            // FrmReimpresionDocumentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1157, 710);
            this.Controls.Add(this.gexMensajesDANA);
            this.Controls.Add(this.lbl_Utiliarios);
            this.Controls.Add(this.DtReinHasta);
            this.Controls.Add(this.DtReinDesde);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btn_pg5_reporteX);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.lbl_hasta);
            this.Controls.Add(this.lbl_desde);
            this.Controls.Add(this.CbTipoDocumento);
            this.Controls.Add(this.lbl_TipoDocumento);
            this.Controls.Add(this.label10);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmReimpresionDocumentos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReimpresionDocumentos";
            this.Load += new System.EventHandler(this.FrmReimpresionDocumentos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gexMensajesDANA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Utiliarios;
        private System.Windows.Forms.DateTimePicker DtReinHasta;
        private System.Windows.Forms.DateTimePicker DtReinDesde;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btn_pg5_reporteX;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lbl_hasta;
        private System.Windows.Forms.Label lbl_desde;
        private System.Windows.Forms.ComboBox CbTipoDocumento;
        private System.Windows.Forms.Label lbl_TipoDocumento;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView gexMensajesDANA;
    }
}