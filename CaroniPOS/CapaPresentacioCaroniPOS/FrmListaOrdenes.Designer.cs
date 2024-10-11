
namespace CapaVisual_Login
{
    public partial class FrmListaOrdenes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmListaOrdenes));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Btnlupa = new System.Windows.Forms.Button();
            this.CbxUltimosTesD = new System.Windows.Forms.ComboBox();
            this.LblListadoOrdenes = new System.Windows.Forms.Label();
            this.DgvListadoOrdenes = new System.Windows.Forms.DataGridView();
            this.txtNumeroOrden = new System.Windows.Forms.TextBox();
            this.CbxEstatus = new System.Windows.Forms.ComboBox();
            this.PnlLSecundario = new System.Windows.Forms.Panel();
            this.BtnCancelar = new System.Windows.Forms.Button();
            this.DtpDesde = new System.Windows.Forms.DateTimePicker();
            this.DtpHasta = new System.Windows.Forms.DateTimePicker();
            this.Lbldesde = new System.Windows.Forms.Label();
            this.LblHasta = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListadoOrdenes)).BeginInit();
            this.SuspendLayout();
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
            this.Btnlupa.Location = new System.Drawing.Point(654, 88);
            this.Btnlupa.Name = "Btnlupa";
            this.Btnlupa.Size = new System.Drawing.Size(53, 38);
            this.Btnlupa.TabIndex = 16;
            this.Btnlupa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Btnlupa.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Btnlupa.UseVisualStyleBackColor = false;
            this.Btnlupa.Click += new System.EventHandler(this.btnlupa_Click_1);
            // 
            // CbxUltimosTesD
            // 
            this.CbxUltimosTesD.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxUltimosTesD.ForeColor = System.Drawing.Color.Black;
            this.CbxUltimosTesD.FormattingEnabled = true;
            this.CbxUltimosTesD.Location = new System.Drawing.Point(19, 96);
            this.CbxUltimosTesD.Name = "CbxUltimosTesD";
            this.CbxUltimosTesD.Size = new System.Drawing.Size(314, 30);
            this.CbxUltimosTesD.TabIndex = 12;
            this.CbxUltimosTesD.Text = "Ultimos Treinta días";
            this.CbxUltimosTesD.SelectionChangeCommitted += new System.EventHandler(this.CbxUltimosTesD_SelectionChangeCommitted);
            this.CbxUltimosTesD.Enter += new System.EventHandler(this.CbxUltimosTesD_Enter);
            // 
            // LblListadoOrdenes
            // 
            this.LblListadoOrdenes.AutoSize = true;
            this.LblListadoOrdenes.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblListadoOrdenes.ForeColor = System.Drawing.Color.Black;
            this.LblListadoOrdenes.Location = new System.Drawing.Point(12, 21);
            this.LblListadoOrdenes.Name = "LblListadoOrdenes";
            this.LblListadoOrdenes.Size = new System.Drawing.Size(208, 25);
            this.LblListadoOrdenes.TabIndex = 11;
            this.LblListadoOrdenes.Text = "Listado de Ordenes";
            // 
            // DgvListadoOrdenes
            // 
            this.DgvListadoOrdenes.AllowUserToAddRows = false;
            this.DgvListadoOrdenes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvListadoOrdenes.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvListadoOrdenes.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DgvListadoOrdenes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvListadoOrdenes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DgvListadoOrdenes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListadoOrdenes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.DgvListadoOrdenes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvListadoOrdenes.DefaultCellStyle = dataGridViewCellStyle8;
            this.DgvListadoOrdenes.EnableHeadersVisualStyles = false;
            this.DgvListadoOrdenes.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgvListadoOrdenes.Location = new System.Drawing.Point(12, 211);
            this.DgvListadoOrdenes.Name = "DgvListadoOrdenes";
            this.DgvListadoOrdenes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListadoOrdenes.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DgvListadoOrdenes.RowHeadersVisible = false;
            this.DgvListadoOrdenes.Size = new System.Drawing.Size(1032, 429);
            this.DgvListadoOrdenes.TabIndex = 14;
            this.DgvListadoOrdenes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvListadoOrdenes_CellContentClick);
            this.DgvListadoOrdenes.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvListadoOrdenes_CellPainting);
            // 
            // txtNumeroOrden
            // 
            this.txtNumeroOrden.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroOrden.ForeColor = System.Drawing.Color.Black;
            this.txtNumeroOrden.Location = new System.Drawing.Point(483, 94);
            this.txtNumeroOrden.MaxLength = 7;
            this.txtNumeroOrden.Name = "txtNumeroOrden";
            this.txtNumeroOrden.Size = new System.Drawing.Size(154, 31);
            this.txtNumeroOrden.TabIndex = 15;
            this.txtNumeroOrden.Enter += new System.EventHandler(this.txtNumeroOrden_Enter);
            this.txtNumeroOrden.Leave += new System.EventHandler(this.txtNumeroOrden_Leave);
            // 
            // CbxEstatus
            // 
            this.CbxEstatus.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxEstatus.ForeColor = System.Drawing.Color.LightGray;
            this.CbxEstatus.FormattingEnabled = true;
            this.CbxEstatus.Location = new System.Drawing.Point(339, 96);
            this.CbxEstatus.Name = "CbxEstatus";
            this.CbxEstatus.Size = new System.Drawing.Size(137, 30);
            this.CbxEstatus.TabIndex = 13;
            this.CbxEstatus.Text = "Estatus";
            this.CbxEstatus.SelectionChangeCommitted += new System.EventHandler(this.CbxEstatus_SelectionChangeCommitted);
            this.CbxEstatus.Enter += new System.EventHandler(this.CbxEstatus_Enter);
            // 
            // PnlLSecundario
            // 
            this.PnlLSecundario.BackColor = System.Drawing.Color.Transparent;
            this.PnlLSecundario.Location = new System.Drawing.Point(20, 326);
            this.PnlLSecundario.Name = "PnlLSecundario";
            this.PnlLSecundario.Size = new System.Drawing.Size(1024, 949);
            this.PnlLSecundario.TabIndex = 30;
            this.PnlLSecundario.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlLSecundario_Paint);
            // 
            // BtnCancelar
            // 
            this.BtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(44)))), ((int)(((byte)(59)))));
            this.BtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancelar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelar.ForeColor = System.Drawing.SystemColors.Window;
            this.BtnCancelar.Location = new System.Drawing.Point(919, 290);
            this.BtnCancelar.Name = "BtnCancelar";
            this.BtnCancelar.Size = new System.Drawing.Size(125, 30);
            this.BtnCancelar.TabIndex = 43;
            this.BtnCancelar.Text = "Cancelar";
            this.BtnCancelar.UseVisualStyleBackColor = false;
            this.BtnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // DtpDesde
            // 
            this.DtpDesde.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpDesde.Location = new System.Drawing.Point(20, 171);
            this.DtpDesde.Name = "DtpDesde";
            this.DtpDesde.Size = new System.Drawing.Size(174, 31);
            this.DtpDesde.TabIndex = 44;
            // 
            // DtpHasta
            // 
            this.DtpHasta.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpHasta.Location = new System.Drawing.Point(262, 171);
            this.DtpHasta.Name = "DtpHasta";
            this.DtpHasta.Size = new System.Drawing.Size(152, 31);
            this.DtpHasta.TabIndex = 45;
            // 
            // Lbldesde
            // 
            this.Lbldesde.AutoSize = true;
            this.Lbldesde.BackColor = System.Drawing.Color.Transparent;
            this.Lbldesde.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbldesde.Location = new System.Drawing.Point(16, 144);
            this.Lbldesde.Name = "Lbldesde";
            this.Lbldesde.Size = new System.Drawing.Size(57, 19);
            this.Lbldesde.TabIndex = 46;
            this.Lbldesde.Text = "Desde";
            this.Lbldesde.Click += new System.EventHandler(this.Lbldesde_Click);
            // 
            // LblHasta
            // 
            this.LblHasta.AutoSize = true;
            this.LblHasta.BackColor = System.Drawing.Color.Transparent;
            this.LblHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHasta.Location = new System.Drawing.Point(259, 144);
            this.LblHasta.Name = "LblHasta";
            this.LblHasta.Size = new System.Drawing.Size(52, 19);
            this.LblHasta.TabIndex = 47;
            this.LblHasta.Text = "Hasta";
            // 
            // FrmListaOrdenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1189, 788);
            this.Controls.Add(this.BtnCancelar);
            this.Controls.Add(this.LblHasta);
            this.Controls.Add(this.Lbldesde);
            this.Controls.Add(this.DtpHasta);
            this.Controls.Add(this.DtpDesde);
            this.Controls.Add(this.PnlLSecundario);
            this.Controls.Add(this.Btnlupa);
            this.Controls.Add(this.CbxUltimosTesD);
            this.Controls.Add(this.CbxEstatus);
            this.Controls.Add(this.LblListadoOrdenes);
            this.Controls.Add(this.txtNumeroOrden);
            this.Controls.Add(this.DgvListadoOrdenes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmListaOrdenes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ListaOrdenes";
            this.Load += new System.EventHandler(this.FrmListaOrdenes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvListadoOrdenes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CbxUltimosTesD;
        private System.Windows.Forms.Label LblListadoOrdenes;
        private System.Windows.Forms.DataGridView DgvListadoOrdenes;
        private System.Windows.Forms.TextBox txtNumeroOrden;
        private System.Windows.Forms.ComboBox CbxEstatus;
        private System.Windows.Forms.Panel PnlLSecundario;
        private System.Windows.Forms.Button BtnCancelar;
        private System.Windows.Forms.Button Btnlupa;
        private System.Windows.Forms.DateTimePicker DtpDesde;
        private System.Windows.Forms.DateTimePicker DtpHasta;
        private System.Windows.Forms.Label Lbldesde;
        private System.Windows.Forms.Label LblHasta;
    }
}