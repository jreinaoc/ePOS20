
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Btnlupa = new System.Windows.Forms.Button();
            this.CbxUltimosTesD = new System.Windows.Forms.ComboBox();
            this.LblListadoOrdenes = new System.Windows.Forms.Label();
            this.txtNumeroOrden = new System.Windows.Forms.TextBox();
            this.CbxEstatus = new System.Windows.Forms.ComboBox();
            this.PnlLSecundario = new System.Windows.Forms.Panel();
            this.BtnCancelar = new System.Windows.Forms.Button();
            this.DtpDesde = new System.Windows.Forms.DateTimePicker();
            this.DtpHasta = new System.Windows.Forms.DateTimePicker();
            this.Lbldesde = new System.Windows.Forms.Label();
            this.LblHasta = new System.Windows.Forms.Label();
            this.LblOpciones = new System.Windows.Forms.Label();
            this.PnlNotaCreditoManual = new System.Windows.Forms.Panel();
            this.TxtNumeroCorrelativo = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnGuardar = new System.Windows.Forms.Button();
            this.TxtNumeroNC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.LblNCManual = new System.Windows.Forms.Label();
            this.DgvListadoOrdenes = new System.Windows.Forms.DataGridView();
            this.TxtCedula = new System.Windows.Forms.TextBox();
            this.PnlComprobanteRetencion = new System.Windows.Forms.Panel();
            this.TxtRetencionFactura = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtRetencionISRL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnCancelarRetencion = new System.Windows.Forms.Button();
            this.BtnGuardarRetencion = new System.Windows.Forms.Button();
            this.TxtRetencionIVA = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbPagina_Ini = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPagina_Fin = new System.Windows.Forms.TextBox();
            this.PnlNotaCreditoManual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListadoOrdenes)).BeginInit();
            this.PnlComprobanteRetencion.SuspendLayout();
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
            this.Btnlupa.Location = new System.Drawing.Point(814, 98);
            this.Btnlupa.Name = "Btnlupa";
            this.Btnlupa.Size = new System.Drawing.Size(50, 30);
            this.Btnlupa.TabIndex = 16;
            this.Btnlupa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Btnlupa.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Btnlupa.UseVisualStyleBackColor = false;
            this.Btnlupa.Click += new System.EventHandler(this.btnlupa_Click_1);
            // 
            // CbxUltimosTesD
            // 
            this.CbxUltimosTesD.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxUltimosTesD.ForeColor = System.Drawing.Color.Black;
            this.CbxUltimosTesD.FormattingEnabled = true;
            this.CbxUltimosTesD.Location = new System.Drawing.Point(19, 96);
            this.CbxUltimosTesD.Name = "CbxUltimosTesD";
            this.CbxUltimosTesD.Size = new System.Drawing.Size(292, 29);
            this.CbxUltimosTesD.TabIndex = 12;
            this.CbxUltimosTesD.Text = "Ultimos Treinta días";
            this.CbxUltimosTesD.SelectionChangeCommitted += new System.EventHandler(this.CbxUltimosTesD_SelectionChangeCommitted);
            this.CbxUltimosTesD.Enter += new System.EventHandler(this.CbxUltimosTesD_Enter);
            // 
            // LblListadoOrdenes
            // 
            this.LblListadoOrdenes.AutoSize = true;
            this.LblListadoOrdenes.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblListadoOrdenes.ForeColor = System.Drawing.Color.Black;
            this.LblListadoOrdenes.Location = new System.Drawing.Point(12, 21);
            this.LblListadoOrdenes.Name = "LblListadoOrdenes";
            this.LblListadoOrdenes.Size = new System.Drawing.Size(188, 23);
            this.LblListadoOrdenes.TabIndex = 11;
            this.LblListadoOrdenes.Text = "Listado de Ordenes";
            // 
            // txtNumeroOrden
            // 
            this.txtNumeroOrden.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroOrden.ForeColor = System.Drawing.Color.Black;
            this.txtNumeroOrden.Location = new System.Drawing.Point(499, 98);
            this.txtNumeroOrden.MaxLength = 7;
            this.txtNumeroOrden.Name = "txtNumeroOrden";
            this.txtNumeroOrden.Size = new System.Drawing.Size(131, 27);
            this.txtNumeroOrden.TabIndex = 15;
            this.txtNumeroOrden.Enter += new System.EventHandler(this.txtNumeroOrden_Enter);
            this.txtNumeroOrden.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumeroOrden_KeyPress);
            this.txtNumeroOrden.Leave += new System.EventHandler(this.txtNumeroOrden_Leave);
            // 
            // CbxEstatus
            // 
            this.CbxEstatus.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxEstatus.ForeColor = System.Drawing.Color.Black;
            this.CbxEstatus.FormattingEnabled = true;
            this.CbxEstatus.Location = new System.Drawing.Point(322, 96);
            this.CbxEstatus.Name = "CbxEstatus";
            this.CbxEstatus.Size = new System.Drawing.Size(162, 29);
            this.CbxEstatus.TabIndex = 13;
            this.CbxEstatus.Text = "Estatus";
            this.CbxEstatus.SelectionChangeCommitted += new System.EventHandler(this.CbxEstatus_SelectionChangeCommitted);
            this.CbxEstatus.Enter += new System.EventHandler(this.CbxEstatus_Enter);
            // 
            // PnlLSecundario
            // 
            this.PnlLSecundario.BackColor = System.Drawing.Color.Transparent;
            this.PnlLSecundario.Location = new System.Drawing.Point(242, 708);
            this.PnlLSecundario.Name = "PnlLSecundario";
            this.PnlLSecundario.Size = new System.Drawing.Size(496, 448);
            this.PnlLSecundario.TabIndex = 30;
            // 
            // BtnCancelar
            // 
            this.BtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(44)))), ((int)(((byte)(59)))));
            this.BtnCancelar.FlatAppearance.BorderSize = 0;
            this.BtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancelar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelar.ForeColor = System.Drawing.SystemColors.Window;
            this.BtnCancelar.Location = new System.Drawing.Point(919, 290);
            this.BtnCancelar.Name = "BtnCancelar";
            this.BtnCancelar.Size = new System.Drawing.Size(105, 30);
            this.BtnCancelar.TabIndex = 43;
            this.BtnCancelar.Text = "Cancelar";
            this.BtnCancelar.UseVisualStyleBackColor = false;
            this.BtnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // DtpDesde
            // 
            this.DtpDesde.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpDesde.Location = new System.Drawing.Point(20, 171);
            this.DtpDesde.Name = "DtpDesde";
            this.DtpDesde.Size = new System.Drawing.Size(143, 27);
            this.DtpDesde.TabIndex = 44;
            // 
            // DtpHasta
            // 
            this.DtpHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpHasta.Location = new System.Drawing.Point(262, 171);
            this.DtpHasta.Name = "DtpHasta";
            this.DtpHasta.Size = new System.Drawing.Size(133, 27);
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
            // LblOpciones
            // 
            this.LblOpciones.AutoSize = true;
            this.LblOpciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            this.LblOpciones.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOpciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.LblOpciones.Location = new System.Drawing.Point(943, 236);
            this.LblOpciones.Name = "LblOpciones";
            this.LblOpciones.Size = new System.Drawing.Size(83, 19);
            this.LblOpciones.TabIndex = 48;
            this.LblOpciones.Text = "Opciones";
            // 
            // PnlNotaCreditoManual
            // 
            this.PnlNotaCreditoManual.BackColor = System.Drawing.Color.White;
            this.PnlNotaCreditoManual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlNotaCreditoManual.Controls.Add(this.TxtNumeroCorrelativo);
            this.PnlNotaCreditoManual.Controls.Add(this.label6);
            this.PnlNotaCreditoManual.Controls.Add(this.label7);
            this.PnlNotaCreditoManual.Controls.Add(this.button1);
            this.PnlNotaCreditoManual.Controls.Add(this.BtnGuardar);
            this.PnlNotaCreditoManual.Controls.Add(this.TxtNumeroNC);
            this.PnlNotaCreditoManual.Controls.Add(this.label1);
            this.PnlNotaCreditoManual.Controls.Add(this.label35);
            this.PnlNotaCreditoManual.Controls.Add(this.LblNCManual);
            this.PnlNotaCreditoManual.Location = new System.Drawing.Point(287, 229);
            this.PnlNotaCreditoManual.Name = "PnlNotaCreditoManual";
            this.PnlNotaCreditoManual.Size = new System.Drawing.Size(531, 227);
            this.PnlNotaCreditoManual.TabIndex = 49;
            this.PnlNotaCreditoManual.Visible = false;
            // 
            // TxtNumeroCorrelativo
            // 
            this.TxtNumeroCorrelativo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNumeroCorrelativo.ForeColor = System.Drawing.Color.Black;
            this.TxtNumeroCorrelativo.Location = new System.Drawing.Point(302, 101);
            this.TxtNumeroCorrelativo.Mask = "00-00000000";
            this.TxtNumeroCorrelativo.Name = "TxtNumeroCorrelativo";
            this.TxtNumeroCorrelativo.Size = new System.Drawing.Size(211, 27);
            this.TxtNumeroCorrelativo.TabIndex = 110;
            this.TxtNumeroCorrelativo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(22, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 19);
            this.label6.TabIndex = 107;
            this.label6.Text = "Número de control:";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(135)))), ((int)(((byte)(129)))));
            this.label7.Location = new System.Drawing.Point(301, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(213, 29);
            this.label7.TabIndex = 109;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(44)))), ((int)(((byte)(59)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(171, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 28);
            this.button1.TabIndex = 106;
            this.button1.Text = "Cancelar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnGuardar
            // 
            this.BtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.BtnGuardar.FlatAppearance.BorderSize = 0;
            this.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGuardar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGuardar.ForeColor = System.Drawing.Color.White;
            this.BtnGuardar.Location = new System.Drawing.Point(293, 168);
            this.BtnGuardar.Name = "BtnGuardar";
            this.BtnGuardar.Size = new System.Drawing.Size(80, 28);
            this.BtnGuardar.TabIndex = 108;
            this.BtnGuardar.Text = "Guardar";
            this.BtnGuardar.UseVisualStyleBackColor = false;
            this.BtnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // TxtNumeroNC
            // 
            this.TxtNumeroNC.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNumeroNC.Location = new System.Drawing.Point(302, 63);
            this.TxtNumeroNC.MaxLength = 7;
            this.TxtNumeroNC.Name = "TxtNumeroNC";
            this.TxtNumeroNC.Size = new System.Drawing.Size(211, 27);
            this.TxtNumeroNC.TabIndex = 100;
            this.TxtNumeroNC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 19);
            this.label1.TabIndex = 99;
            this.label1.Text = "Número de Nota de Crédito:";
            // 
            // label35
            // 
            this.label35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(135)))), ((int)(((byte)(129)))));
            this.label35.Location = new System.Drawing.Point(301, 62);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(213, 29);
            this.label35.TabIndex = 103;
            // 
            // LblNCManual
            // 
            this.LblNCManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(125)))), ((int)(((byte)(123)))));
            this.LblNCManual.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNCManual.ForeColor = System.Drawing.Color.White;
            this.LblNCManual.Location = new System.Drawing.Point(0, 0);
            this.LblNCManual.Name = "LblNCManual";
            this.LblNCManual.Size = new System.Drawing.Size(530, 37);
            this.LblNCManual.TabIndex = 25;
            this.LblNCManual.Text = "Nota de Crédito Manual";
            this.LblNCManual.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListadoOrdenes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvListadoOrdenes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvListadoOrdenes.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgvListadoOrdenes.EnableHeadersVisualStyles = false;
            this.DgvListadoOrdenes.GridColor = System.Drawing.Color.Indigo;
            this.DgvListadoOrdenes.Location = new System.Drawing.Point(12, 211);
            this.DgvListadoOrdenes.Name = "DgvListadoOrdenes";
            this.DgvListadoOrdenes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvListadoOrdenes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DgvListadoOrdenes.RowHeadersVisible = false;
            this.DgvListadoOrdenes.RowHeadersWidth = 51;
            this.DgvListadoOrdenes.Size = new System.Drawing.Size(1032, 429);
            this.DgvListadoOrdenes.TabIndex = 14;
            this.DgvListadoOrdenes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvListadoOrdenes_CellContentClick);
            this.DgvListadoOrdenes.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvListadoOrdenes_CellFormatting);
            this.DgvListadoOrdenes.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvListadoOrdenes_CellPainting);
            // 
            // TxtCedula
            // 
            this.TxtCedula.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCedula.ForeColor = System.Drawing.Color.Black;
            this.TxtCedula.Location = new System.Drawing.Point(647, 98);
            this.TxtCedula.MaxLength = 10;
            this.TxtCedula.Name = "TxtCedula";
            this.TxtCedula.Size = new System.Drawing.Size(156, 27);
            this.TxtCedula.TabIndex = 50;
            this.TxtCedula.Enter += new System.EventHandler(this.TxtCedula_Enter);
            this.TxtCedula.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCedula_KeyPress);
            this.TxtCedula.Leave += new System.EventHandler(this.TxtCedula_Leave);
            // 
            // PnlComprobanteRetencion
            // 
            this.PnlComprobanteRetencion.BackColor = System.Drawing.Color.White;
            this.PnlComprobanteRetencion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlComprobanteRetencion.Controls.Add(this.TxtRetencionFactura);
            this.PnlComprobanteRetencion.Controls.Add(this.label10);
            this.PnlComprobanteRetencion.Controls.Add(this.TxtRetencionISRL);
            this.PnlComprobanteRetencion.Controls.Add(this.label3);
            this.PnlComprobanteRetencion.Controls.Add(this.label9);
            this.PnlComprobanteRetencion.Controls.Add(this.label2);
            this.PnlComprobanteRetencion.Controls.Add(this.BtnCancelarRetencion);
            this.PnlComprobanteRetencion.Controls.Add(this.BtnGuardarRetencion);
            this.PnlComprobanteRetencion.Controls.Add(this.TxtRetencionIVA);
            this.PnlComprobanteRetencion.Controls.Add(this.label4);
            this.PnlComprobanteRetencion.Controls.Add(this.label5);
            this.PnlComprobanteRetencion.Controls.Add(this.label8);
            this.PnlComprobanteRetencion.Location = new System.Drawing.Point(432, 171);
            this.PnlComprobanteRetencion.Name = "PnlComprobanteRetencion";
            this.PnlComprobanteRetencion.Size = new System.Drawing.Size(397, 269);
            this.PnlComprobanteRetencion.TabIndex = 51;
            this.PnlComprobanteRetencion.Visible = false;
            // 
            // TxtRetencionFactura
            // 
            this.TxtRetencionFactura.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TxtRetencionFactura.Enabled = false;
            this.TxtRetencionFactura.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRetencionFactura.Location = new System.Drawing.Point(216, 149);
            this.TxtRetencionFactura.MaxLength = 15;
            this.TxtRetencionFactura.Name = "TxtRetencionFactura";
            this.TxtRetencionFactura.Size = new System.Drawing.Size(152, 27);
            this.TxtRetencionFactura.TabIndex = 114;
            this.TxtRetencionFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(135)))), ((int)(((byte)(129)))));
            this.label10.Location = new System.Drawing.Point(215, 148);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(154, 29);
            this.label10.TabIndex = 115;
            // 
            // TxtRetencionISRL
            // 
            this.TxtRetencionISRL.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRetencionISRL.Location = new System.Drawing.Point(216, 109);
            this.TxtRetencionISRL.MaxLength = 15;
            this.TxtRetencionISRL.Name = "TxtRetencionISRL";
            this.TxtRetencionISRL.Size = new System.Drawing.Size(152, 27);
            this.TxtRetencionISRL.TabIndex = 112;
            this.TxtRetencionISRL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtRetencionISRL_KeyPress);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(135)))), ((int)(((byte)(129)))));
            this.label3.Location = new System.Drawing.Point(215, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 29);
            this.label3.TabIndex = 113;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(22, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 19);
            this.label9.TabIndex = 111;
            this.label9.Text = "Factura";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 19);
            this.label2.TabIndex = 107;
            this.label2.Text = "Retención ISRL";
            // 
            // BtnCancelarRetencion
            // 
            this.BtnCancelarRetencion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(44)))), ((int)(((byte)(59)))));
            this.BtnCancelarRetencion.FlatAppearance.BorderSize = 0;
            this.BtnCancelarRetencion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancelarRetencion.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelarRetencion.ForeColor = System.Drawing.Color.White;
            this.BtnCancelarRetencion.Location = new System.Drawing.Point(104, 217);
            this.BtnCancelarRetencion.Name = "BtnCancelarRetencion";
            this.BtnCancelarRetencion.Size = new System.Drawing.Size(87, 28);
            this.BtnCancelarRetencion.TabIndex = 106;
            this.BtnCancelarRetencion.Text = "Cancelar";
            this.BtnCancelarRetencion.UseVisualStyleBackColor = false;
            this.BtnCancelarRetencion.Click += new System.EventHandler(this.BtnCancelarRetencion_Click);
            // 
            // BtnGuardarRetencion
            // 
            this.BtnGuardarRetencion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.BtnGuardarRetencion.FlatAppearance.BorderSize = 0;
            this.BtnGuardarRetencion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGuardarRetencion.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGuardarRetencion.ForeColor = System.Drawing.Color.White;
            this.BtnGuardarRetencion.Location = new System.Drawing.Point(226, 217);
            this.BtnGuardarRetencion.Name = "BtnGuardarRetencion";
            this.BtnGuardarRetencion.Size = new System.Drawing.Size(80, 28);
            this.BtnGuardarRetencion.TabIndex = 108;
            this.BtnGuardarRetencion.Text = "Guardar";
            this.BtnGuardarRetencion.UseVisualStyleBackColor = false;
            this.BtnGuardarRetencion.Click += new System.EventHandler(this.BtnGuardarRetencion_Click);
            // 
            // TxtRetencionIVA
            // 
            this.TxtRetencionIVA.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtRetencionIVA.Location = new System.Drawing.Point(216, 67);
            this.TxtRetencionIVA.MaxLength = 15;
            this.TxtRetencionIVA.Name = "TxtRetencionIVA";
            this.TxtRetencionIVA.Size = new System.Drawing.Size(152, 27);
            this.TxtRetencionIVA.TabIndex = 100;
            this.TxtRetencionIVA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtRetencionIVA_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 19);
            this.label4.TabIndex = 99;
            this.label4.Text = "Retención IVA";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(135)))), ((int)(((byte)(129)))));
            this.label5.Location = new System.Drawing.Point(215, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 29);
            this.label5.TabIndex = 103;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(125)))), ((int)(((byte)(123)))));
            this.label8.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(412, 37);
            this.label8.TabIndex = 25;
            this.label8.Text = "Comprobante de Retención";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(454, 644);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 15);
            this.label11.TabIndex = 52;
            this.label11.Text = "Página";
            this.label11.Visible = false;
            // 
            // cbPagina_Ini
            // 
            this.cbPagina_Ini.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPagina_Ini.FormattingEnabled = true;
            this.cbPagina_Ini.Location = new System.Drawing.Point(502, 641);
            this.cbPagina_Ini.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbPagina_Ini.Name = "cbPagina_Ini";
            this.cbPagina_Ini.Size = new System.Drawing.Size(43, 23);
            this.cbPagina_Ini.TabIndex = 53;
            this.cbPagina_Ini.Visible = false;
            this.cbPagina_Ini.SelectedIndexChanged += new System.EventHandler(this.cbPagina_Ini_SelectedIndexChanged);
            this.cbPagina_Ini.SelectionChangeCommitted += new System.EventHandler(this.cbPagina_Ini_SelectionChangeCommitted);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(547, 644);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 15);
            this.label12.TabIndex = 54;
            this.label12.Text = "de";
            this.label12.Visible = false;
            // 
            // txtPagina_Fin
            // 
            this.txtPagina_Fin.Enabled = false;
            this.txtPagina_Fin.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPagina_Fin.Location = new System.Drawing.Point(569, 640);
            this.txtPagina_Fin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPagina_Fin.Name = "txtPagina_Fin";
            this.txtPagina_Fin.Size = new System.Drawing.Size(38, 22);
            this.txtPagina_Fin.TabIndex = 55;
            this.txtPagina_Fin.WordWrap = false;
            // 
            // FrmListaOrdenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1040, 640);
            this.Controls.Add(this.txtPagina_Fin);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbPagina_Ini);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.PnlComprobanteRetencion);
            this.Controls.Add(this.TxtCedula);
            this.Controls.Add(this.PnlNotaCreditoManual);
            this.Controls.Add(this.LblOpciones);
            this.Controls.Add(this.LblHasta);
            this.Controls.Add(this.Lbldesde);
            this.Controls.Add(this.DtpHasta);
            this.Controls.Add(this.DtpDesde);
            this.Controls.Add(this.BtnCancelar);
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
            this.PnlNotaCreditoManual.ResumeLayout(false);
            this.PnlNotaCreditoManual.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListadoOrdenes)).EndInit();
            this.PnlComprobanteRetencion.ResumeLayout(false);
            this.PnlComprobanteRetencion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CbxUltimosTesD;
        private System.Windows.Forms.Label LblListadoOrdenes;
        private System.Windows.Forms.TextBox txtNumeroOrden;
        private System.Windows.Forms.ComboBox CbxEstatus;
        private System.Windows.Forms.Panel PnlLSecundario;
        private System.Windows.Forms.DateTimePicker DtpDesde;
        private System.Windows.Forms.DateTimePicker DtpHasta;
        private System.Windows.Forms.Label Lbldesde;
        private System.Windows.Forms.Label LblHasta;
        public System.Windows.Forms.Button BtnCancelar;
        public System.Windows.Forms.Button Btnlupa;
        private System.Windows.Forms.Label LblOpciones;
        public System.Windows.Forms.Panel PnlNotaCreditoManual;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button BtnGuardar;
        private System.Windows.Forms.TextBox TxtNumeroNC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label LblNCManual;
        private System.Windows.Forms.DataGridView DgvListadoOrdenes;
        private System.Windows.Forms.TextBox TxtCedula;
        private System.Windows.Forms.MaskedTextBox TxtNumeroCorrelativo;
        public System.Windows.Forms.Panel PnlComprobanteRetencion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnCancelarRetencion;
        public System.Windows.Forms.Button BtnGuardarRetencion;
        private System.Windows.Forms.TextBox TxtRetencionIVA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtRetencionFactura;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TxtRetencionISRL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbPagina_Ini;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtPagina_Fin;
    }
}