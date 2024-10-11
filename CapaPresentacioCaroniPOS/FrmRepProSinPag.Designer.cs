
namespace CapaVisual_Login
{
    partial class FrmRepProSinPag
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.TxtOrden = new System.Windows.Forms.TextBox();
            this.dsRepProSinPago = new CapaVisual_Login.Reportes.DsRepProSinPago();
            this.dsRepProSinPagoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPCPOSReporProSinPagoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sP_CPOS_ReporProSinPagoTableAdapter = new CapaVisual_Login.Reportes.DsRepProSinPagoTableAdapters.SP_CPOS_ReporProSinPagoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepProSinPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepProSinPagoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPCPOSReporProSinPagoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DtsProSinPag";
            reportDataSource1.Value = this.sPCPOSReporProSinPagoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RptProSinPag.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // TxtOrden
            // 
            this.TxtOrden.Location = new System.Drawing.Point(395, 81);
            this.TxtOrden.Name = "TxtOrden";
            this.TxtOrden.Size = new System.Drawing.Size(100, 20);
            this.TxtOrden.TabIndex = 1;
            this.TxtOrden.Visible = false;
            this.TxtOrden.TextChanged += new System.EventHandler(this.TxtOrden_TextChanged);
            // 
            // dsRepProSinPago
            // 
            this.dsRepProSinPago.DataSetName = "DsRepProSinPago";
            this.dsRepProSinPago.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dsRepProSinPagoBindingSource
            // 
            this.dsRepProSinPagoBindingSource.DataSource = this.dsRepProSinPago;
            this.dsRepProSinPagoBindingSource.Position = 0;
            // 
            // sPCPOSReporProSinPagoBindingSource
            // 
            this.sPCPOSReporProSinPagoBindingSource.DataMember = "SP_CPOS_ReporProSinPago";
            this.sPCPOSReporProSinPagoBindingSource.DataSource = this.dsRepProSinPago;
            // 
            // sP_CPOS_ReporProSinPagoTableAdapter
            // 
            this.sP_CPOS_ReporProSinPagoTableAdapter.ClearBeforeFill = true;
            // 
            // FrmRepProSinPag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TxtOrden);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmRepProSinPag";
            this.Text = "FrmRepProSinPag";
            this.Load += new System.EventHandler(this.FrmRepProSinPag_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsRepProSinPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepProSinPagoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPCPOSReporProSinPagoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.TextBox TxtOrden;
        private System.Windows.Forms.BindingSource sPCPOSReporProSinPagoBindingSource;
        private Reportes.DsRepProSinPago dsRepProSinPago;
        private System.Windows.Forms.BindingSource dsRepProSinPagoBindingSource;
        private Reportes.DsRepProSinPagoTableAdapters.SP_CPOS_ReporProSinPagoTableAdapter sP_CPOS_ReporProSinPagoTableAdapter;
    }
}