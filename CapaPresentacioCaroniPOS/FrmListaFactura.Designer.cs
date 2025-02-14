
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmListaFactura));
            this.DgvListaFacturas = new System.Windows.Forms.DataGridView();
            this.LblListadoFactura = new System.Windows.Forms.Label();
            this.LblHasta = new System.Windows.Forms.Label();
            this.Lbldesde = new System.Windows.Forms.Label();
            this.DtpHasta = new System.Windows.Forms.DateTimePicker();
            this.DtpDesde = new System.Windows.Forms.DateTimePicker();
            this.Btnlupa = new System.Windows.Forms.Button();
            this.txtPagina_Fin = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbPagina_Ini = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListaFacturas)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvListaFacturas
            // 
            this.DgvListaFacturas.AllowUserToAddRows = false;
            this.DgvListaFacturas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvListaFacturas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvListaFacturas.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DgvListaFacturas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvListaFacturas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.DgvListaFacturas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListaFacturas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvListaFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvListaFacturas.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgvListaFacturas.EnableHeadersVisualStyles = false;
            this.DgvListaFacturas.GridColor = System.Drawing.Color.Indigo;
            this.DgvListaFacturas.Location = new System.Drawing.Point(12, 211);
            this.DgvListaFacturas.Name = "DgvListaFacturas";
            this.DgvListaFacturas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListaFacturas.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DgvListaFacturas.RowHeadersVisible = false;
            this.DgvListaFacturas.RowHeadersWidth = 51;
            this.DgvListaFacturas.Size = new System.Drawing.Size(1032, 429);
            this.DgvListaFacturas.TabIndex = 15;
            this.DgvListaFacturas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvListaFacturas_CellContentClick);
            // 
            // LblListadoFactura
            // 
            this.LblListadoFactura.AutoSize = true;
            this.LblListadoFactura.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblListadoFactura.ForeColor = System.Drawing.Color.Black;
            this.LblListadoFactura.Location = new System.Drawing.Point(12, 21);
            this.LblListadoFactura.Name = "LblListadoFactura";
            this.LblListadoFactura.Size = new System.Drawing.Size(188, 23);
            this.LblListadoFactura.TabIndex = 16;
            this.LblListadoFactura.Text = "Listado de Facturas";
            // 
            // LblHasta
            // 
            this.LblHasta.AutoSize = true;
            this.LblHasta.BackColor = System.Drawing.Color.Transparent;
            this.LblHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHasta.Location = new System.Drawing.Point(255, 56);
            this.LblHasta.Name = "LblHasta";
            this.LblHasta.Size = new System.Drawing.Size(52, 19);
            this.LblHasta.TabIndex = 51;
            this.LblHasta.Text = "Hasta";
            // 
            // Lbldesde
            // 
            this.Lbldesde.AutoSize = true;
            this.Lbldesde.BackColor = System.Drawing.Color.Transparent;
            this.Lbldesde.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbldesde.Location = new System.Drawing.Point(12, 56);
            this.Lbldesde.Name = "Lbldesde";
            this.Lbldesde.Size = new System.Drawing.Size(57, 19);
            this.Lbldesde.TabIndex = 50;
            this.Lbldesde.Text = "Desde";
            // 
            // DtpHasta
            // 
            this.DtpHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpHasta.Location = new System.Drawing.Point(258, 83);
            this.DtpHasta.Name = "DtpHasta";
            this.DtpHasta.Size = new System.Drawing.Size(133, 27);
            this.DtpHasta.TabIndex = 49;
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
            this.Btnlupa.Location = new System.Drawing.Point(454, 80);
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
            // FrmListaFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1173, 749);
            this.Controls.Add(this.txtPagina_Fin);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbPagina_Ini);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Btnlupa);
            this.Controls.Add(this.LblHasta);
            this.Controls.Add(this.Lbldesde);
            this.Controls.Add(this.DtpHasta);
            this.Controls.Add(this.DtpDesde);
            this.Controls.Add(this.LblListadoFactura);
            this.Controls.Add(this.DgvListaFacturas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmListaFactura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ListaFactura";
            this.Load += new System.EventHandler(this.FrmListaFactura_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvListaFacturas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvListaFacturas;
        private System.Windows.Forms.Label LblListadoFactura;
        private System.Windows.Forms.Label LblHasta;
        private System.Windows.Forms.Label Lbldesde;
        private System.Windows.Forms.DateTimePicker DtpHasta;
        private System.Windows.Forms.DateTimePicker DtpDesde;
        public System.Windows.Forms.Button Btnlupa;
        private System.Windows.Forms.TextBox txtPagina_Fin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbPagina_Ini;
        private System.Windows.Forms.Label label11;
    }
}