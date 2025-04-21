
namespace CapaVisual_Login
{
    partial class FrmListaFactura
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmListaFactura));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LblListadoFactura = new System.Windows.Forms.Label();
            this.DtpHasta = new System.Windows.Forms.DateTimePicker();
            this.DtpDesde = new System.Windows.Forms.DateTimePicker();
            this.Btnlupa = new System.Windows.Forms.Button();
            this.txtPagina_Fin = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbPagina_Ini = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btn = new System.Windows.Forms.Button();
            this.DgvListaFacturas1 = new System.Windows.Forms.DataGridView();
            this.CbxEstatus = new System.Windows.Forms.ComboBox();
            this.btnPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListaFacturas1)).BeginInit();
            this.SuspendLayout();
            // 
            // LblListadoFactura
            // 
            this.LblListadoFactura.AutoSize = true;
            this.LblListadoFactura.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblListadoFactura.ForeColor = System.Drawing.Color.Black;
            this.LblListadoFactura.Location = new System.Drawing.Point(12, 21);
            this.LblListadoFactura.Name = "LblListadoFactura";
            this.LblListadoFactura.Size = new System.Drawing.Size(366, 23);
            this.LblListadoFactura.TabIndex = 16;
            this.LblListadoFactura.Text = "Listado de Facturas y Notas de Crédito";
            // 
            // DtpHasta
            // 
            this.DtpHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpHasta.Location = new System.Drawing.Point(194, 83);
            this.DtpHasta.Name = "DtpHasta";
            this.DtpHasta.Size = new System.Drawing.Size(133, 27);
            this.DtpHasta.TabIndex = 49;
            this.DtpHasta.ValueChanged += new System.EventHandler(this.DtpHasta_ValueChanged);
            // 
            // DtpDesde
            // 
            this.DtpDesde.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDesde.Location = new System.Drawing.Point(16, 83);
            this.DtpDesde.Name = "DtpDesde";
            this.DtpDesde.Size = new System.Drawing.Size(143, 27);
            this.DtpDesde.TabIndex = 48;
            // 
            // Btnlupa
            // 
            this.Btnlupa.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Btnlupa.BackColor = System.Drawing.Color.Transparent;
            this.Btnlupa.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Btnlupa.BackgroundImage")));
            this.Btnlupa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Btnlupa.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Btnlupa.FlatAppearance.BorderSize = 0;
            this.Btnlupa.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(185)))), ((int)(((byte)(166)))));
            this.Btnlupa.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(185)))), ((int)(((byte)(166)))));
            this.Btnlupa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btnlupa.ForeColor = System.Drawing.Color.Transparent;
            this.Btnlupa.Location = new System.Drawing.Point(550, 83);
            this.Btnlupa.Name = "Btnlupa";
            this.Btnlupa.Size = new System.Drawing.Size(50, 30);
            this.Btnlupa.TabIndex = 52;
            this.Btnlupa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Btnlupa.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Btnlupa.UseVisualStyleBackColor = false;
            this.Btnlupa.Click += new System.EventHandler(this.Btnlupa_Click);
            // 
            // txtPagina_Fin
            // 
            this.txtPagina_Fin.Enabled = false;
            this.txtPagina_Fin.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPagina_Fin.Location = new System.Drawing.Point(569, 640);
            this.txtPagina_Fin.Margin = new System.Windows.Forms.Padding(2);
            this.txtPagina_Fin.Name = "txtPagina_Fin";
            this.txtPagina_Fin.Size = new System.Drawing.Size(38, 22);
            this.txtPagina_Fin.TabIndex = 59;
            this.txtPagina_Fin.WordWrap = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(547, 644);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 15);
            this.label12.TabIndex = 58;
            this.label12.Text = "de";
            this.label12.Visible = false;
            // 
            // cbPagina_Ini
            // 
            this.cbPagina_Ini.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPagina_Ini.FormattingEnabled = true;
            this.cbPagina_Ini.Location = new System.Drawing.Point(502, 641);
            this.cbPagina_Ini.Margin = new System.Windows.Forms.Padding(2);
            this.cbPagina_Ini.Name = "cbPagina_Ini";
            this.cbPagina_Ini.Size = new System.Drawing.Size(43, 23);
            this.cbPagina_Ini.TabIndex = 57;
            this.cbPagina_Ini.Visible = false;
            this.cbPagina_Ini.SelectionChangeCommitted += new System.EventHandler(this.cbPagina_Ini_SelectionChangeCommitted);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(454, 644);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 15);
            this.label11.TabIndex = 56;
            this.label11.Text = "Página";
            this.label11.Visible = false;
            // 
            // btn
            // 
            this.btn.BackColor = System.Drawing.SystemColors.Window;
            this.btn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn.ForeColor = System.Drawing.Color.Gray;
            this.btn.Location = new System.Drawing.Point(886, 18);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(102, 30);
            this.btn.TabIndex = 60;
            this.btn.Text = "PDF";
            this.btn.UseVisualStyleBackColor = false;
            this.btn.Visible = false;
            this.btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // DgvListaFacturas1
            // 
            this.DgvListaFacturas1.AllowUserToAddRows = false;
            this.DgvListaFacturas1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvListaFacturas1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvListaFacturas1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DgvListaFacturas1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvListaFacturas1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.DgvListaFacturas1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListaFacturas1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.DgvListaFacturas1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvListaFacturas1.DefaultCellStyle = dataGridViewCellStyle11;
            this.DgvListaFacturas1.EnableHeadersVisualStyles = false;
            this.DgvListaFacturas1.GridColor = System.Drawing.Color.Indigo;
            this.DgvListaFacturas1.Location = new System.Drawing.Point(16, 151);
            this.DgvListaFacturas1.Margin = new System.Windows.Forms.Padding(4);
            this.DgvListaFacturas1.Name = "DgvListaFacturas1";
            this.DgvListaFacturas1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListaFacturas1.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.DgvListaFacturas1.RowHeadersVisible = false;
            this.DgvListaFacturas1.RowHeadersWidth = 51;
            this.DgvListaFacturas1.Size = new System.Drawing.Size(1032, 429);
            this.DgvListaFacturas1.TabIndex = 61;
            this.DgvListaFacturas1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvListaFacturas1_CellContentClick);
            // 
            // CbxEstatus
            // 
            this.CbxEstatus.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxEstatus.ForeColor = System.Drawing.Color.Gray;
            this.CbxEstatus.FormattingEnabled = true;
            this.CbxEstatus.Items.AddRange(new object[] {
            "Facturas",
            "Notas de Crédito"});
            this.CbxEstatus.Location = new System.Drawing.Point(364, 83);
            this.CbxEstatus.Name = "CbxEstatus";
            this.CbxEstatus.Size = new System.Drawing.Size(162, 29);
            this.CbxEstatus.TabIndex = 62;
            this.CbxEstatus.Text = "Tipo Documento";
            this.CbxEstatus.SelectionChangeCommitted += new System.EventHandler(this.CbxEstatus_SelectionChangeCommitted);
            this.CbxEstatus.Enter += new System.EventHandler(this.CbxEstatus_Enter);
            // 
            // btnPDF
            // 
            this.btnPDF.BackColor = System.Drawing.Color.Transparent;
            this.btnPDF.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPDF.BackgroundImage")));
            this.btnPDF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPDF.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPDF.FlatAppearance.BorderSize = 0;
            this.btnPDF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(185)))), ((int)(((byte)(166)))));
            this.btnPDF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(185)))), ((int)(((byte)(166)))));
            this.btnPDF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPDF.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPDF.ForeColor = System.Drawing.Color.Gray;
            this.btnPDF.Location = new System.Drawing.Point(978, 83);
            this.btnPDF.Name = "btnPDF";
            this.btnPDF.Size = new System.Drawing.Size(70, 30);
            this.btnPDF.TabIndex = 63;
            this.btnPDF.UseVisualStyleBackColor = false;
            this.btnPDF.Click += new System.EventHandler(this.btnPDF_Click);
            // 
            // FrmListaFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1173, 749);
            this.Controls.Add(this.btnPDF);
            this.Controls.Add(this.CbxEstatus);
            this.Controls.Add(this.DgvListaFacturas1);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.txtPagina_Fin);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbPagina_Ini);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Btnlupa);
            this.Controls.Add(this.DtpHasta);
            this.Controls.Add(this.DtpDesde);
            this.Controls.Add(this.LblListadoFactura);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmListaFactura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ListaFactura";
            this.Load += new System.EventHandler(this.FrmListaFactura_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvListaFacturas1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LblListadoFactura;
        private System.Windows.Forms.DateTimePicker DtpHasta;
        private System.Windows.Forms.DateTimePicker DtpDesde;
        public System.Windows.Forms.Button Btnlupa;
        private System.Windows.Forms.TextBox txtPagina_Fin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbPagina_Ini;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.DataGridView DgvListaFacturas1;
        private System.Windows.Forms.ComboBox CbxEstatus;
        private System.Windows.Forms.Button btnPDF;
    }
}