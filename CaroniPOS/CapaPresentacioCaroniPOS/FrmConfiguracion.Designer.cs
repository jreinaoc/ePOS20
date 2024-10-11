
namespace CapaVisual_Login
{
    partial class FrmConfiguracion
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
            this.GbxConfDashBoard = new System.Windows.Forms.GroupBox();
            this.LblHasta = new System.Windows.Forms.Label();
            this.Lbldesde = new System.Windows.Forms.Label();
            this.DtpHasta = new System.Windows.Forms.DateTimePicker();
            this.DtpDesde = new System.Windows.Forms.DateTimePicker();
            this.CbxColaborador = new System.Windows.Forms.ComboBox();
            this.RbColaborador = new System.Windows.Forms.RadioButton();
            this.RbTienda = new System.Windows.Forms.RadioButton();
            this.Rbmiinfo = new System.Windows.Forms.RadioButton();
            this.CbxPeriodoDash = new System.Windows.Forms.ComboBox();
            this.LblPeriodoDashboard = new System.Windows.Forms.Label();
            this.LblConfigDashboard = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CbMostrarMetas = new System.Windows.Forms.CheckBox();
            this.TxtIngresosBs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtUnidadesMetas = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CbxPeriodoMetas = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RbEurosTasa = new System.Windows.Forms.RadioButton();
            this.RbDolTasa = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RbBsVentas = new System.Windows.Forms.RadioButton();
            this.RbDolVentas = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnCancelar = new System.Windows.Forms.Button();
            this.BtnGuardar = new System.Windows.Forms.Button();
            this.GbxConfDashBoard.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbxConfDashBoard
            // 
            this.GbxConfDashBoard.Controls.Add(this.LblHasta);
            this.GbxConfDashBoard.Controls.Add(this.Lbldesde);
            this.GbxConfDashBoard.Controls.Add(this.DtpHasta);
            this.GbxConfDashBoard.Controls.Add(this.DtpDesde);
            this.GbxConfDashBoard.Controls.Add(this.CbxColaborador);
            this.GbxConfDashBoard.Controls.Add(this.RbColaborador);
            this.GbxConfDashBoard.Controls.Add(this.RbTienda);
            this.GbxConfDashBoard.Controls.Add(this.Rbmiinfo);
            this.GbxConfDashBoard.Controls.Add(this.CbxPeriodoDash);
            this.GbxConfDashBoard.Controls.Add(this.LblPeriodoDashboard);
            this.GbxConfDashBoard.Controls.Add(this.LblConfigDashboard);
            this.GbxConfDashBoard.Location = new System.Drawing.Point(3, 5);
            this.GbxConfDashBoard.Name = "GbxConfDashBoard";
            this.GbxConfDashBoard.Size = new System.Drawing.Size(1072, 232);
            this.GbxConfDashBoard.TabIndex = 0;
            this.GbxConfDashBoard.TabStop = false;
            this.GbxConfDashBoard.Enter += new System.EventHandler(this.GbxConfDashBoard_Enter);
            // 
            // LblHasta
            // 
            this.LblHasta.AutoSize = true;
            this.LblHasta.BackColor = System.Drawing.Color.Transparent;
            this.LblHasta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHasta.Location = new System.Drawing.Point(888, 50);
            this.LblHasta.Name = "LblHasta";
            this.LblHasta.Size = new System.Drawing.Size(52, 19);
            this.LblHasta.TabIndex = 51;
            this.LblHasta.Text = "Hasta";
            this.LblHasta.Click += new System.EventHandler(this.LblHasta_Click);
            // 
            // Lbldesde
            // 
            this.Lbldesde.AutoSize = true;
            this.Lbldesde.BackColor = System.Drawing.Color.Transparent;
            this.Lbldesde.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbldesde.Location = new System.Drawing.Point(691, 50);
            this.Lbldesde.Name = "Lbldesde";
            this.Lbldesde.Size = new System.Drawing.Size(57, 19);
            this.Lbldesde.TabIndex = 50;
            this.Lbldesde.Text = "Desde";
            // 
            // DtpHasta
            // 
            this.DtpHasta.CalendarFont = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpHasta.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpHasta.Location = new System.Drawing.Point(892, 77);
            this.DtpHasta.Name = "DtpHasta";
            this.DtpHasta.Size = new System.Drawing.Size(125, 31);
            this.DtpHasta.TabIndex = 49;
            // 
            // DtpDesde
            // 
            this.DtpDesde.CalendarFont = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDesde.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpDesde.Location = new System.Drawing.Point(695, 77);
            this.DtpDesde.Name = "DtpDesde";
            this.DtpDesde.Size = new System.Drawing.Size(128, 31);
            this.DtpDesde.TabIndex = 48;
            // 
            // CbxColaborador
            // 
            this.CbxColaborador.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxColaborador.FormattingEnabled = true;
            this.CbxColaborador.Location = new System.Drawing.Point(795, 178);
            this.CbxColaborador.Name = "CbxColaborador";
            this.CbxColaborador.Size = new System.Drawing.Size(222, 27);
            this.CbxColaborador.TabIndex = 6;
            // 
            // RbColaborador
            // 
            this.RbColaborador.AutoSize = true;
            this.RbColaborador.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbColaborador.Location = new System.Drawing.Point(643, 178);
            this.RbColaborador.Name = "RbColaborador";
            this.RbColaborador.Size = new System.Drawing.Size(127, 23);
            this.RbColaborador.TabIndex = 5;
            this.RbColaborador.TabStop = true;
            this.RbColaborador.Text = "Colaborador";
            this.RbColaborador.UseVisualStyleBackColor = true;
            // 
            // RbTienda
            // 
            this.RbTienda.AutoSize = true;
            this.RbTienda.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbTienda.Location = new System.Drawing.Point(375, 178);
            this.RbTienda.Name = "RbTienda";
            this.RbTienda.Size = new System.Drawing.Size(80, 23);
            this.RbTienda.TabIndex = 4;
            this.RbTienda.TabStop = true;
            this.RbTienda.Text = "Tienda";
            this.RbTienda.UseVisualStyleBackColor = true;
            this.RbTienda.CheckedChanged += new System.EventHandler(this.RbTienda_CheckedChanged);
            // 
            // Rbmiinfo
            // 
            this.Rbmiinfo.AutoSize = true;
            this.Rbmiinfo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rbmiinfo.Location = new System.Drawing.Point(9, 178);
            this.Rbmiinfo.Name = "Rbmiinfo";
            this.Rbmiinfo.Size = new System.Drawing.Size(143, 23);
            this.Rbmiinfo.TabIndex = 3;
            this.Rbmiinfo.TabStop = true;
            this.Rbmiinfo.Text = "Mi información";
            this.Rbmiinfo.UseVisualStyleBackColor = true;
            this.Rbmiinfo.CheckedChanged += new System.EventHandler(this.Rbmiinfo_CheckedChanged);
            // 
            // CbxPeriodoDash
            // 
            this.CbxPeriodoDash.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxPeriodoDash.FormattingEnabled = true;
            this.CbxPeriodoDash.Location = new System.Drawing.Point(438, 54);
            this.CbxPeriodoDash.Name = "CbxPeriodoDash";
            this.CbxPeriodoDash.Size = new System.Drawing.Size(222, 27);
            this.CbxPeriodoDash.TabIndex = 2;
            this.CbxPeriodoDash.SelectionChangeCommitted += new System.EventHandler(this.CbxPeriodoDash_SelectionChangeCommitted);
            // 
            // LblPeriodoDashboard
            // 
            this.LblPeriodoDashboard.AutoSize = true;
            this.LblPeriodoDashboard.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPeriodoDashboard.Location = new System.Drawing.Point(217, 54);
            this.LblPeriodoDashboard.Name = "LblPeriodoDashboard";
            this.LblPeriodoDashboard.Size = new System.Drawing.Size(191, 19);
            this.LblPeriodoDashboard.TabIndex = 1;
            this.LblPeriodoDashboard.Text = "Período del dashboard ";
            this.LblPeriodoDashboard.Click += new System.EventHandler(this.LblPeriodoDashboard_Click);
            // 
            // LblConfigDashboard
            // 
            this.LblConfigDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.LblConfigDashboard.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblConfigDashboard.Location = new System.Drawing.Point(0, 0);
            this.LblConfigDashboard.Name = "LblConfigDashboard";
            this.LblConfigDashboard.Size = new System.Drawing.Size(1072, 36);
            this.LblConfigDashboard.TabIndex = 0;
            this.LblConfigDashboard.Text = "Configuración del Dashboard";
            this.LblConfigDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CbMostrarMetas);
            this.groupBox1.Controls.Add(this.TxtIngresosBs);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.TxtUnidadesMetas);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.CbxPeriodoMetas);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(3, 243);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1072, 178);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // CbMostrarMetas
            // 
            this.CbMostrarMetas.AutoSize = true;
            this.CbMostrarMetas.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbMostrarMetas.Location = new System.Drawing.Point(903, 118);
            this.CbMostrarMetas.Name = "CbMostrarMetas";
            this.CbMostrarMetas.Size = new System.Drawing.Size(132, 23);
            this.CbMostrarMetas.TabIndex = 11;
            this.CbMostrarMetas.Text = "Mostrar Metas";
            this.CbMostrarMetas.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.CbMostrarMetas.UseVisualStyleBackColor = true;
            // 
            // TxtIngresosBs
            // 
            this.TxtIngresosBs.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtIngresosBs.Location = new System.Drawing.Point(875, 60);
            this.TxtIngresosBs.Name = "TxtIngresosBs";
            this.TxtIngresosBs.Size = new System.Drawing.Size(173, 27);
            this.TxtIngresosBs.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(732, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ingresos en Bs. ";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // TxtUnidadesMetas
            // 
            this.TxtUnidadesMetas.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtUnidadesMetas.Location = new System.Drawing.Point(125, 119);
            this.TxtUnidadesMetas.Name = "TxtUnidadesMetas";
            this.TxtUnidadesMetas.Size = new System.Drawing.Size(100, 27);
            this.TxtUnidadesMetas.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 19);
            this.label3.TabIndex = 7;
            this.label3.Text = "Unidades";
            // 
            // CbxPeriodoMetas
            // 
            this.CbxPeriodoMetas.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxPeriodoMetas.FormattingEnabled = true;
            this.CbxPeriodoMetas.Location = new System.Drawing.Point(125, 59);
            this.CbxPeriodoMetas.Name = "CbxPeriodoMetas";
            this.CbxPeriodoMetas.Size = new System.Drawing.Size(222, 27);
            this.CbxPeriodoMetas.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mes / Año";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label2.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1072, 36);
            this.label2.TabIndex = 0;
            this.label2.Text = "Metas";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RbEurosTasa);
            this.groupBox2.Controls.Add(this.RbDolTasa);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(7, 437);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(525, 176);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // RbEurosTasa
            // 
            this.RbEurosTasa.AutoSize = true;
            this.RbEurosTasa.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbEurosTasa.Location = new System.Drawing.Point(390, 91);
            this.RbEurosTasa.Name = "RbEurosTasa";
            this.RbEurosTasa.Size = new System.Drawing.Size(66, 23);
            this.RbEurosTasa.TabIndex = 2;
            this.RbEurosTasa.TabStop = true;
            this.RbEurosTasa.Text = "Euros";
            this.RbEurosTasa.UseVisualStyleBackColor = true;
            this.RbEurosTasa.CheckedChanged += new System.EventHandler(this.RbEurosTasa_CheckedChanged);
            // 
            // RbDolTasa
            // 
            this.RbDolTasa.AutoSize = true;
            this.RbDolTasa.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbDolTasa.Location = new System.Drawing.Point(25, 91);
            this.RbDolTasa.Name = "RbDolTasa";
            this.RbDolTasa.Size = new System.Drawing.Size(84, 23);
            this.RbDolTasa.TabIndex = 1;
            this.RbDolTasa.TabStop = true;
            this.RbDolTasa.Text = "Dolares";
            this.RbDolTasa.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label8.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(525, 36);
            this.label8.TabIndex = 0;
            this.label8.Text = "Por defecto tasa del día";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.RbBsVentas);
            this.groupBox3.Controls.Add(this.RbDolVentas);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(550, 437);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(525, 176);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // RbBsVentas
            // 
            this.RbBsVentas.AutoSize = true;
            this.RbBsVentas.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbBsVentas.Location = new System.Drawing.Point(390, 91);
            this.RbBsVentas.Name = "RbBsVentas";
            this.RbBsVentas.Size = new System.Drawing.Size(95, 23);
            this.RbBsVentas.TabIndex = 2;
            this.RbBsVentas.TabStop = true;
            this.RbBsVentas.Text = "Bolivares";
            this.RbBsVentas.UseVisualStyleBackColor = true;
            // 
            // RbDolVentas
            // 
            this.RbDolVentas.AutoSize = true;
            this.RbDolVentas.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbDolVentas.Location = new System.Drawing.Point(25, 91);
            this.RbDolVentas.Name = "RbDolVentas";
            this.RbDolVentas.Size = new System.Drawing.Size(84, 23);
            this.RbDolVentas.TabIndex = 1;
            this.RbDolVentas.TabStop = true;
            this.RbDolVentas.Text = "Dolares";
            this.RbDolVentas.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label5.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(525, 36);
            this.label5.TabIndex = 0;
            this.label5.Text = "Por defecto ventas del día";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnCancelar
            // 
            this.BtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancelar.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelar.ForeColor = System.Drawing.Color.White;
            this.BtnCancelar.Location = new System.Drawing.Point(817, 619);
            this.BtnCancelar.Name = "BtnCancelar";
            this.BtnCancelar.Size = new System.Drawing.Size(104, 23);
            this.BtnCancelar.TabIndex = 4;
            this.BtnCancelar.Text = "Cancelar";
            this.BtnCancelar.UseVisualStyleBackColor = false;
            // 
            // BtnGuardar
            // 
            this.BtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGuardar.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGuardar.ForeColor = System.Drawing.Color.White;
            this.BtnGuardar.Location = new System.Drawing.Point(947, 619);
            this.BtnGuardar.Name = "BtnGuardar";
            this.BtnGuardar.Size = new System.Drawing.Size(104, 23);
            this.BtnGuardar.TabIndex = 5;
            this.BtnGuardar.Text = "Guardar";
            this.BtnGuardar.UseVisualStyleBackColor = false;
            this.BtnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // FrmConfiguracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1076, 646);
            this.Controls.Add(this.BtnGuardar);
            this.Controls.Add(this.BtnCancelar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GbxConfDashBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "FrmConfiguracion";
            this.Text = "FrmConfiguracion";
            this.Load += new System.EventHandler(this.FrmConfiguracion_Load);
            this.GbxConfDashBoard.ResumeLayout(false);
            this.GbxConfDashBoard.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbxConfDashBoard;
        private System.Windows.Forms.Label LblPeriodoDashboard;
        private System.Windows.Forms.Label LblConfigDashboard;
        private System.Windows.Forms.RadioButton RbTienda;
        private System.Windows.Forms.RadioButton Rbmiinfo;
        private System.Windows.Forms.ComboBox CbxPeriodoDash;
        private System.Windows.Forms.RadioButton RbColaborador;
        private System.Windows.Forms.ComboBox CbxColaborador;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TxtIngresosBs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtUnidadesMetas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CbxPeriodoMetas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton RbEurosTasa;
        private System.Windows.Forms.RadioButton RbDolTasa;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton RbBsVentas;
        private System.Windows.Forms.RadioButton RbDolVentas;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnCancelar;
        private System.Windows.Forms.Button BtnGuardar;
        private System.Windows.Forms.CheckBox CbMostrarMetas;
        private System.Windows.Forms.Label LblHasta;
        private System.Windows.Forms.Label Lbldesde;
        private System.Windows.Forms.DateTimePicker DtpHasta;
        private System.Windows.Forms.DateTimePicker DtpDesde;
    }
}