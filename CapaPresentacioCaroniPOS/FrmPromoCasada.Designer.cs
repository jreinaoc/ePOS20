
namespace CapaVisual_Login
{
    partial class FrmPromoCasada
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
            this.Cbx_Promo = new System.Windows.Forms.ComboBox();
            this.Lbl_Tap1_DatosPersonal = new System.Windows.Forms.Label();
            this.Dgv_ListOsCasadas = new System.Windows.Forms.DataGridView();
            this.Btn_Pnl3_Cancelar = new System.Windows.Forms.Button();
            this.btAplicarPromo = new System.Windows.Forms.Button();
            this.Lbl_Promociones = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_ListOsCasadas)).BeginInit();
            this.SuspendLayout();
            // 
            // Cbx_Promo
            // 
            this.Cbx_Promo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbx_Promo.DropDownWidth = 235;
            this.Cbx_Promo.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cbx_Promo.ForeColor = System.Drawing.Color.Black;
            this.Cbx_Promo.FormattingEnabled = true;
            this.Cbx_Promo.Location = new System.Drawing.Point(187, 44);
            this.Cbx_Promo.Name = "Cbx_Promo";
            this.Cbx_Promo.Size = new System.Drawing.Size(253, 24);
            this.Cbx_Promo.TabIndex = 315;
            this.Cbx_Promo.Visible = false;
            this.Cbx_Promo.SelectedValueChanged += new System.EventHandler(this.Cbx_Promociones_SelectedValueChanged_1);
            // 
            // Lbl_Tap1_DatosPersonal
            // 
            this.Lbl_Tap1_DatosPersonal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(156)))));
            this.Lbl_Tap1_DatosPersonal.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Tap1_DatosPersonal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Lbl_Tap1_DatosPersonal.Location = new System.Drawing.Point(108, 77);
            this.Lbl_Tap1_DatosPersonal.Name = "Lbl_Tap1_DatosPersonal";
            this.Lbl_Tap1_DatosPersonal.Size = new System.Drawing.Size(704, 38);
            this.Lbl_Tap1_DatosPersonal.TabIndex = 87;
            this.Lbl_Tap1_DatosPersonal.Text = "Ordenes Casadas";
            this.Lbl_Tap1_DatosPersonal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Dgv_ListOsCasadas
            // 
            this.Dgv_ListOsCasadas.AllowUserToAddRows = false;
            this.Dgv_ListOsCasadas.AllowUserToResizeColumns = false;
            this.Dgv_ListOsCasadas.AllowUserToResizeRows = false;
            this.Dgv_ListOsCasadas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dgv_ListOsCasadas.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Dgv_ListOsCasadas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Dgv_ListOsCasadas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.Dgv_ListOsCasadas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(185)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Dgv_ListOsCasadas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Dgv_ListOsCasadas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Dgv_ListOsCasadas.DefaultCellStyle = dataGridViewCellStyle2;
            this.Dgv_ListOsCasadas.EnableHeadersVisualStyles = false;
            this.Dgv_ListOsCasadas.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.Dgv_ListOsCasadas.Location = new System.Drawing.Point(108, 118);
            this.Dgv_ListOsCasadas.Name = "Dgv_ListOsCasadas";
            this.Dgv_ListOsCasadas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Dgv_ListOsCasadas.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Dgv_ListOsCasadas.RowHeadersVisible = false;
            this.Dgv_ListOsCasadas.RowHeadersWidth = 51;
            this.Dgv_ListOsCasadas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Dgv_ListOsCasadas.Size = new System.Drawing.Size(704, 298);
            this.Dgv_ListOsCasadas.TabIndex = 310;
            this.Dgv_ListOsCasadas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_ListOsCasadas_CellContentClick);
            this.Dgv_ListOsCasadas.CurrentCellDirtyStateChanged += new System.EventHandler(this.Dgv_ListOsCasadas_CurrentCellDirtyStateChanged);
            // 
            // Btn_Pnl3_Cancelar
            // 
            this.Btn_Pnl3_Cancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(44)))), ((int)(((byte)(59)))));
            this.Btn_Pnl3_Cancelar.FlatAppearance.BorderSize = 0;
            this.Btn_Pnl3_Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Pnl3_Cancelar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Pnl3_Cancelar.ForeColor = System.Drawing.SystemColors.Window;
            this.Btn_Pnl3_Cancelar.Location = new System.Drawing.Point(596, 426);
            this.Btn_Pnl3_Cancelar.Name = "Btn_Pnl3_Cancelar";
            this.Btn_Pnl3_Cancelar.Size = new System.Drawing.Size(105, 44);
            this.Btn_Pnl3_Cancelar.TabIndex = 312;
            this.Btn_Pnl3_Cancelar.Text = "Cancelar";
            this.Btn_Pnl3_Cancelar.UseVisualStyleBackColor = false;
            // 
            // btAplicarPromo
            // 
            this.btAplicarPromo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(184)))), ((int)(((byte)(52)))));
            this.btAplicarPromo.FlatAppearance.BorderSize = 0;
            this.btAplicarPromo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAplicarPromo.Font = new System.Drawing.Font("Century Gothic", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btAplicarPromo.ForeColor = System.Drawing.SystemColors.Window;
            this.btAplicarPromo.Location = new System.Drawing.Point(707, 426);
            this.btAplicarPromo.Name = "btAplicarPromo";
            this.btAplicarPromo.Size = new System.Drawing.Size(105, 44);
            this.btAplicarPromo.TabIndex = 311;
            this.btAplicarPromo.Text = "Aplicar promoción";
            this.btAplicarPromo.UseVisualStyleBackColor = false;
            this.btAplicarPromo.Click += new System.EventHandler(this.btAplicarPromo_Click);
            // 
            // Lbl_Promociones
            // 
            this.Lbl_Promociones.AutoSize = true;
            this.Lbl_Promociones.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Promociones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Lbl_Promociones.Location = new System.Drawing.Point(105, 46);
            this.Lbl_Promociones.Name = "Lbl_Promociones";
            this.Lbl_Promociones.Size = new System.Drawing.Size(76, 16);
            this.Lbl_Promociones.TabIndex = 313;
            this.Lbl_Promociones.Text = "Promoción";
            this.Lbl_Promociones.Visible = false;
            // 
            // FrmPromoCasada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(952, 486);
            this.Controls.Add(this.Cbx_Promo);
            this.Controls.Add(this.Lbl_Promociones);
            this.Controls.Add(this.Btn_Pnl3_Cancelar);
            this.Controls.Add(this.btAplicarPromo);
            this.Controls.Add(this.Dgv_ListOsCasadas);
            this.Controls.Add(this.Lbl_Tap1_DatosPersonal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPromoCasada";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmPromoCasada";
            this.Load += new System.EventHandler(this.FrmPromoCasada_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_ListOsCasadas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Lbl_Tap1_DatosPersonal;
        public System.Windows.Forms.DataGridView Dgv_ListOsCasadas;
        public System.Windows.Forms.Button Btn_Pnl3_Cancelar;
        private System.Windows.Forms.Button btAplicarPromo;
        private System.Windows.Forms.Label Lbl_Promociones;
        private System.Windows.Forms.ComboBox Cbx_Promo;
    }
}