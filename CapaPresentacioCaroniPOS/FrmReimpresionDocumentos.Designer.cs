
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbl_Utiliarios = new System.Windows.Forms.Label();
            this.gexMensajesDANA = new System.Windows.Forms.DataGridView();
            this.NúmeroDeDocumento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.gexMensajesDANA)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Utiliarios
            // 
            this.lbl_Utiliarios.AutoSize = true;
            this.lbl_Utiliarios.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Utiliarios.Location = new System.Drawing.Point(35, 35);
            this.lbl_Utiliarios.Name = "lbl_Utiliarios";
            this.lbl_Utiliarios.Size = new System.Drawing.Size(95, 25);
            this.lbl_Utiliarios.TabIndex = 45;
            this.lbl_Utiliarios.Text = "Utiliarios";
            this.lbl_Utiliarios.Click += new System.EventHandler(this.lbl_Utiliarios_Click);
            // 
            // gexMensajesDANA
            // 
            this.gexMensajesDANA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gexMensajesDANA.BackgroundColor = System.Drawing.Color.White;
            this.gexMensajesDANA.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gexMensajesDANA.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(173)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gexMensajesDANA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gexMensajesDANA.ColumnHeadersHeight = 25;
            this.gexMensajesDANA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gexMensajesDANA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NúmeroDeDocumento,
            this.Fecha});
            this.gexMensajesDANA.EnableHeadersVisualStyles = false;
            this.gexMensajesDANA.Location = new System.Drawing.Point(160, 202);
            this.gexMensajesDANA.Name = "gexMensajesDANA";
            this.gexMensajesDANA.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gexMensajesDANA.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.gexMensajesDANA.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.gexMensajesDANA.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.gexMensajesDANA.Size = new System.Drawing.Size(551, 150);
            this.gexMensajesDANA.TabIndex = 44;
            // 
            // NúmeroDeDocumento
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.NúmeroDeDocumento.DefaultCellStyle = dataGridViewCellStyle5;
            this.NúmeroDeDocumento.FillWeight = 202.7865F;
            this.NúmeroDeDocumento.HeaderText = "Número de documento";
            this.NúmeroDeDocumento.Name = "NúmeroDeDocumento";
            // 
            // Fecha
            // 
            this.Fecha.FillWeight = 94.04593F;
            this.Fecha.HeaderText = "Fecha";
            this.Fecha.Name = "Fecha";
            // 
            // DtReinHasta
            // 
            this.DtReinHasta.Location = new System.Drawing.Point(501, 167);
            this.DtReinHasta.Name = "DtReinHasta";
            this.DtReinHasta.Size = new System.Drawing.Size(210, 20);
            this.DtReinHasta.TabIndex = 43;
            // 
            // DtReinDesde
            // 
            this.DtReinDesde.Location = new System.Drawing.Point(276, 167);
            this.DtReinDesde.Name = "DtReinDesde";
            this.DtReinDesde.Size = new System.Drawing.Size(215, 20);
            this.DtReinDesde.TabIndex = 42;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(184)))), ((int)(((byte)(52)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(624, 365);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(89, 24);
            this.button5.TabIndex = 41;
            this.button5.Text = "Imprimir";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(156)))));
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(732, 165);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(89, 24);
            this.button6.TabIndex = 40;
            this.button6.Text = "Buscar";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btn_pg5_reporteX
            // 
            this.btn_pg5_reporteX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.btn_pg5_reporteX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pg5_reporteX.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_pg5_reporteX.ForeColor = System.Drawing.Color.White;
            this.btn_pg5_reporteX.Location = new System.Drawing.Point(834, 165);
            this.btn_pg5_reporteX.Name = "btn_pg5_reporteX";
            this.btn_pg5_reporteX.Size = new System.Drawing.Size(89, 24);
            this.btn_pg5_reporteX.TabIndex = 39;
            this.btn_pg5_reporteX.Text = "Reporte X";
            this.btn_pg5_reporteX.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(184)))), ((int)(((byte)(52)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(934, 165);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(89, 24);
            this.button4.TabIndex = 38;
            this.button4.Text = "Reporte Z";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // lbl_hasta
            // 
            this.lbl_hasta.AutoSize = true;
            this.lbl_hasta.Location = new System.Drawing.Point(498, 150);
            this.lbl_hasta.Name = "lbl_hasta";
            this.lbl_hasta.Size = new System.Drawing.Size(35, 13);
            this.lbl_hasta.TabIndex = 37;
            this.lbl_hasta.Text = "Hasta";
            // 
            // lbl_desde
            // 
            this.lbl_desde.AutoSize = true;
            this.lbl_desde.Location = new System.Drawing.Point(275, 150);
            this.lbl_desde.Name = "lbl_desde";
            this.lbl_desde.Size = new System.Drawing.Size(38, 13);
            this.lbl_desde.TabIndex = 36;
            this.lbl_desde.Text = "Desde";
            // 
            // CbTipoDocumento
            // 
            this.CbTipoDocumento.FormattingEnabled = true;
            this.CbTipoDocumento.Location = new System.Drawing.Point(160, 166);
            this.CbTipoDocumento.Name = "CbTipoDocumento";
            this.CbTipoDocumento.Size = new System.Drawing.Size(102, 21);
            this.CbTipoDocumento.TabIndex = 35;
            // 
            // lbl_TipoDocumento
            // 
            this.lbl_TipoDocumento.AutoSize = true;
            this.lbl_TipoDocumento.Location = new System.Drawing.Point(157, 150);
            this.lbl_TipoDocumento.Name = "lbl_TipoDocumento";
            this.lbl_TipoDocumento.Size = new System.Drawing.Size(99, 13);
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
            // FrmReimpresionDocumentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1157, 710);
            this.Controls.Add(this.lbl_Utiliarios);
            this.Controls.Add(this.gexMensajesDANA);
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
        private System.Windows.Forms.DataGridView gexMensajesDANA;
        private System.Windows.Forms.DataGridViewTextBoxColumn NúmeroDeDocumento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fecha;
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
    }
}