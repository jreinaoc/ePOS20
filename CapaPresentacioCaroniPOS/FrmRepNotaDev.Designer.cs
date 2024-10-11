
namespace CapaVisual_Login
{
    partial class FrmRepNotaDev
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
            this.dsNotaDevolucion = new CapaVisual_Login.Reportes.DsNotaDevolucion();
            this.dsNotaDevolucionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPCPOSREPNOTDEVBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sP_CPOS_REP_NOT_DEVTableAdapter = new CapaVisual_Login.Reportes.DsNotaDevolucionTableAdapters.SP_CPOS_REP_NOT_DEVTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dsNotaDevolucion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsNotaDevolucionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPCPOSREPNOTDEVBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DsNotaDevolucion";
            reportDataSource1.Value = this.sPCPOSREPNOTDEVBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepNotaDevolucion.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // TxtOrden
            // 
            this.TxtOrden.Location = new System.Drawing.Point(418, 59);
            this.TxtOrden.Name = "TxtOrden";
            this.TxtOrden.Size = new System.Drawing.Size(100, 20);
            this.TxtOrden.TabIndex = 1;
            this.TxtOrden.Visible = false;
            // 
            // dsNotaDevolucion
            // 
            this.dsNotaDevolucion.DataSetName = "DsNotaDevolucion";
            this.dsNotaDevolucion.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dsNotaDevolucionBindingSource
            // 
            this.dsNotaDevolucionBindingSource.DataSource = this.dsNotaDevolucion;
            this.dsNotaDevolucionBindingSource.Position = 0;
            // 
            // sPCPOSREPNOTDEVBindingSource
            // 
            this.sPCPOSREPNOTDEVBindingSource.DataMember = "SP_CPOS_REP_NOT_DEV";
            this.sPCPOSREPNOTDEVBindingSource.DataSource = this.dsNotaDevolucionBindingSource;
            // 
            // sP_CPOS_REP_NOT_DEVTableAdapter
            // 
            this.sP_CPOS_REP_NOT_DEVTableAdapter.ClearBeforeFill = true;
            // 
            // FrmRepNotaDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TxtOrden);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmRepNotaDev";
            this.Text = "FrmRepNotaDev";
            this.Load += new System.EventHandler(this.FrmRepNotaDev_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsNotaDevolucion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsNotaDevolucionBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPCPOSREPNOTDEVBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.TextBox TxtOrden;
        private System.Windows.Forms.BindingSource sPCPOSREPNOTDEVBindingSource;
        private System.Windows.Forms.BindingSource dsNotaDevolucionBindingSource;
        private Reportes.DsNotaDevolucion dsNotaDevolucion;
        private Reportes.DsNotaDevolucionTableAdapters.SP_CPOS_REP_NOT_DEVTableAdapter sP_CPOS_REP_NOT_DEVTableAdapter;
    }
}