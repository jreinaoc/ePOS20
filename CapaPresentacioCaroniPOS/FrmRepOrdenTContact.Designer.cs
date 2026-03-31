
namespace CapaVisual_Login
{
    partial class FrmRepOrdenTContact
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.sPCPOSREPORDENBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsRepOrdenBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsRepOrden = new CapaVisual_Login.Reportes.DsRepOrden();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.sP_CPOS_REP_ORDENTableAdapter = new CapaVisual_Login.Reportes.DsRepOrdenTableAdapters.SP_CPOS_REP_ORDENTableAdapter();
            this.TxtOrden = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.sPCPOSREPORDENBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepOrdenBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepOrden)).BeginInit();
            this.SuspendLayout();
            // 
            // sPCPOSREPORDENBindingSource
            // 
            this.sPCPOSREPORDENBindingSource.DataMember = "SP_CPOS_REP_ORDEN";
            this.sPCPOSREPORDENBindingSource.DataSource = this.dsRepOrdenBindingSource;
            this.sPCPOSREPORDENBindingSource.CurrentChanged += new System.EventHandler(this.sPCPOSREPORDENBindingSource_CurrentChanged);
            // 
            // dsRepOrdenBindingSource
            // 
            this.dsRepOrdenBindingSource.DataSource = this.dsRepOrden;
            this.dsRepOrdenBindingSource.Position = 0;
            this.dsRepOrdenBindingSource.CurrentChanged += new System.EventHandler(this.dsRepOrdenBindingSource_CurrentChanged);
            // 
            // dsRepOrden
            // 
            this.dsRepOrden.DataSetName = "DsRepOrden";
            this.dsRepOrden.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DsRepOrden";
            reportDataSource1.Value = this.sPCPOSREPORDENBindingSource;
            reportDataSource2.Name = "DsRepProSinPago";
            reportDataSource2.Value = null;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrdenTContacto.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // sP_CPOS_REP_ORDENTableAdapter
            // 
            this.sP_CPOS_REP_ORDENTableAdapter.ClearBeforeFill = true;
            // 
            // TxtOrden
            // 
            this.TxtOrden.Location = new System.Drawing.Point(280, 61);
            this.TxtOrden.Name = "TxtOrden";
            this.TxtOrden.Size = new System.Drawing.Size(211, 20);
            this.TxtOrden.TabIndex = 1;
            this.TxtOrden.Visible = false;
            // 
            // FrmRepOrdenTContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TxtOrden);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmRepOrdenTContact";
            this.Text = "FrmRepOrdenTContact";
            this.Load += new System.EventHandler(this.FrmRepOrdenTContact_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sPCPOSREPORDENBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepOrdenBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepOrden)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource sPCPOSREPORDENBindingSource;
        private System.Windows.Forms.BindingSource dsRepOrdenBindingSource;
        private Reportes.DsRepOrden dsRepOrden;
        private Reportes.DsRepOrdenTableAdapters.SP_CPOS_REP_ORDENTableAdapter sP_CPOS_REP_ORDENTableAdapter;
        private System.Windows.Forms.TextBox TxtOrden;
    }
}