
namespace CapaVisual_Login
{
    partial class FrmPagoMovil
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPagoMovil));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LblHasta = new System.Windows.Forms.Label();
            this.Lbldesde = new System.Windows.Forms.Label();
            this.DtpHasta = new System.Windows.Forms.DateTimePicker();
            this.DtpDesde = new System.Windows.Forms.DateTimePicker();
            this.Btnlupa = new System.Windows.Forms.Button();
            this.DgvListadoOrdenes = new System.Windows.Forms.DataGridView();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.LblListadoOrdenes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListadoOrdenes)).BeginInit();
            this.SuspendLayout();
            // 
            // LblHasta
            // 
            this.LblHasta.AutoSize = true;
            this.LblHasta.BackColor = System.Drawing.Color.Transparent;
            this.LblHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHasta.Location = new System.Drawing.Point(245, 50);
            this.LblHasta.Name = "LblHasta";
            this.LblHasta.Size = new System.Drawing.Size(52, 19);
            this.LblHasta.TabIndex = 58;
            this.LblHasta.Text = "Hasta";
            // 
            // Lbldesde
            // 
            this.Lbldesde.AutoSize = true;
            this.Lbldesde.BackColor = System.Drawing.Color.Transparent;
            this.Lbldesde.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbldesde.Location = new System.Drawing.Point(2, 50);
            this.Lbldesde.Name = "Lbldesde";
            this.Lbldesde.Size = new System.Drawing.Size(57, 19);
            this.Lbldesde.TabIndex = 57;
            this.Lbldesde.Text = "Desde";
            // 
            // DtpHasta
            // 
            this.DtpHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpHasta.Location = new System.Drawing.Point(248, 77);
            this.DtpHasta.Name = "DtpHasta";
            this.DtpHasta.Size = new System.Drawing.Size(133, 27);
            this.DtpHasta.TabIndex = 56;
            // 
            // DtpDesde
            // 
            this.DtpDesde.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDesde.Location = new System.Drawing.Point(6, 77);
            this.DtpDesde.Name = "DtpDesde";
            this.DtpDesde.Size = new System.Drawing.Size(143, 27);
            this.DtpDesde.TabIndex = 55;
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
            this.Btnlupa.Location = new System.Drawing.Point(400, 74);
            this.Btnlupa.Name = "Btnlupa";
            this.Btnlupa.Size = new System.Drawing.Size(50, 30);
            this.Btnlupa.TabIndex = 54;
            this.Btnlupa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Btnlupa.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Btnlupa.UseVisualStyleBackColor = false;
            this.Btnlupa.Click += new System.EventHandler(this.btnlupa_Click_1);
            // 
            // DgvListadoOrdenes
            // 
            this.DgvListadoOrdenes.AllowUserToAddRows = false;
            this.DgvListadoOrdenes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvListadoOrdenes.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvListadoOrdenes.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DgvListadoOrdenes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvListadoOrdenes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.DgvListadoOrdenes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListadoOrdenes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DgvListadoOrdenes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvListadoOrdenes.DefaultCellStyle = dataGridViewCellStyle5;
            this.DgvListadoOrdenes.EnableHeadersVisualStyles = false;
            this.DgvListadoOrdenes.GridColor = System.Drawing.Color.Indigo;
            this.DgvListadoOrdenes.Location = new System.Drawing.Point(6, 140);
            this.DgvListadoOrdenes.Name = "DgvListadoOrdenes";
            this.DgvListadoOrdenes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListadoOrdenes.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DgvListadoOrdenes.RowHeadersVisible = false;
            this.DgvListadoOrdenes.RowHeadersWidth = 51;
            this.DgvListadoOrdenes.Size = new System.Drawing.Size(1112, 429);
            this.DgvListadoOrdenes.TabIndex = 60;
            this.DgvListadoOrdenes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvListadoOrdenes_CellContentClick);
            this.DgvListadoOrdenes.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvListadoOrdenes_CellFormatting);
            this.DgvListadoOrdenes.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvListadoOrdenes_CellPainting);
            // 
            // lblMensaje
            // 
            this.lblMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensaje.Font = new System.Drawing.Font("Century Gothic", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.Color.Black;
            this.lblMensaje.Location = new System.Drawing.Point(12, 621);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(960, 16);
            this.lblMensaje.TabIndex = 61;
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblListadoOrdenes
            // 
            this.LblListadoOrdenes.AutoSize = true;
            this.LblListadoOrdenes.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblListadoOrdenes.ForeColor = System.Drawing.Color.Black;
            this.LblListadoOrdenes.Location = new System.Drawing.Point(2, 9);
            this.LblListadoOrdenes.Name = "LblListadoOrdenes";
            this.LblListadoOrdenes.Size = new System.Drawing.Size(177, 23);
            this.LblListadoOrdenes.TabIndex = 62;
            this.LblListadoOrdenes.Text = "Listado de Vueltos";
            this.LblListadoOrdenes.Click += new System.EventHandler(this.LblListadoOrdenes_Click);
            // 
            // FrmPagoMovil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1173, 749);
            this.Controls.Add(this.LblListadoOrdenes);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.DgvListadoOrdenes);
            this.Controls.Add(this.LblHasta);
            this.Controls.Add(this.Lbldesde);
            this.Controls.Add(this.DtpHasta);
            this.Controls.Add(this.DtpDesde);
            this.Controls.Add(this.Btnlupa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPagoMovil";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmPagoMovil";
            this.Load += new System.EventHandler(this.FrmPagoMovil_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvListadoOrdenes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LblHasta;
        private System.Windows.Forms.Label Lbldesde;
        private System.Windows.Forms.DateTimePicker DtpHasta;
        private System.Windows.Forms.DateTimePicker DtpDesde;
        public System.Windows.Forms.Button Btnlupa;
        private System.Windows.Forms.DataGridView DgvListadoOrdenes;
        public System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Label LblListadoOrdenes;
    }
}