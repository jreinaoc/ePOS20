
namespace CapaVisual_Login
{
    partial class FrmRepContratGart
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
            this.TxtOrden = new System.Windows.Forms.TextBox();
            this.SP_CPOS_ReporGarantiaExtendidaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CGarantiaDataSet = new CapaVisual_Login.CGarantiaDataSet();
            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter = new CapaVisual_Login.CGarantiaDataSetTableAdapters.SP_CPOS_ReporGarantiaExtendidaTableAdapter();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.SP_CPOS_ReporGarantiaExtendidaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CGarantiaDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtOrden
            // 
            this.TxtOrden.AcceptsTab = true;
            this.TxtOrden.Location = new System.Drawing.Point(329, 49);
            this.TxtOrden.Name = "TxtOrden";
            this.TxtOrden.Size = new System.Drawing.Size(92, 20);
            this.TxtOrden.TabIndex = 1;
            this.TxtOrden.Visible = false;
            // 
            // SP_CPOS_ReporGarantiaExtendidaBindingSource
            // 
            this.SP_CPOS_ReporGarantiaExtendidaBindingSource.DataMember = "SP_CPOS_ReporGarantiaExtendida";
            this.SP_CPOS_ReporGarantiaExtendidaBindingSource.DataSource = this.CGarantiaDataSet;
            // 
            // CGarantiaDataSet
            // 
            this.CGarantiaDataSet.DataSetName = "CGarantiaDataSet";
            this.CGarantiaDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // SP_CPOS_ReporGarantiaExtendidaTableAdapter
            // 
            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter.ClearBeforeFill = true;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSetContGarantia";
            reportDataSource1.Value = this.SP_CPOS_ReporGarantiaExtendidaBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepContratoGarantia.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // FrmRepContratGart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TxtOrden);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmRepContratGart";
            this.Text = "FrmRepContratGart";
            this.Load += new System.EventHandler(this.FrmRepContratGart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SP_CPOS_ReporGarantiaExtendidaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CGarantiaDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource SP_CPOS_ReporGarantiaExtendidaBindingSource;
        private CGarantiaDataSet CGarantiaDataSet;
        private CGarantiaDataSetTableAdapters.SP_CPOS_ReporGarantiaExtendidaTableAdapter SP_CPOS_ReporGarantiaExtendidaTableAdapter;
        private System.Windows.Forms.TextBox TxtOrden;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}