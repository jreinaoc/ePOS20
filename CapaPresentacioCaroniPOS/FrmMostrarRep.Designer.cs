
namespace CapaVisual_Login
{
    partial class FrmMostrarRep
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dsCambio = new CapaVisual_Login.Reportes.DsReppCambio();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.TxtOrden = new System.Windows.Forms.TextBox();
            this.SP_CPOS_RepCambioTableAdapter = new CapaVisual_Login.Reportes.DsReppCambioTableAdapters.SP_CPOS_RepCambioTableAdapter();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.dsRepPagosTranferencia1 = new CapaVisual_Login.Reportes.DsRepPagosTranferencia();
            this.cpos_PagoTransferenciaTableAdapter = new CapaVisual_Login.Reportes.DsRepPagosTranferenciaTableAdapters.Cpos_PagoTransferenciaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCambio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepPagosTranferencia1)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingSource1
            // 
            this.bindingSource1.AllowNew = true;
            this.bindingSource1.DataMember = "SP_CPOS_RepCambio";
            this.bindingSource1.DataSource = this.dsCambio;
            // 
            // dsCambio
            // 
            this.dsCambio.DataSetName = "DsCambio";
            this.dsCambio.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource3.Name = "DsRepCambio";
            reportDataSource3.Value = this.bindingSource1;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepCambio.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(822, 475);
            this.reportViewer1.TabIndex = 0;
            // 
            // TxtOrden
            // 
            this.TxtOrden.AcceptsTab = true;
            this.TxtOrden.Location = new System.Drawing.Point(379, 56);
            this.TxtOrden.Name = "TxtOrden";
            this.TxtOrden.Size = new System.Drawing.Size(92, 20);
            this.TxtOrden.TabIndex = 2;
            this.TxtOrden.Visible = false;
            // 
            // SP_CPOS_RepCambioTableAdapter
            // 
            this.SP_CPOS_RepCambioTableAdapter.ClearBeforeFill = true;
            // 
            // bindingSource2
            // 
            this.bindingSource2.DataMember = "Cpos_PagoTransferencia";
            this.bindingSource2.DataSource = this.dsRepPagosTranferencia1;
            // 
            // dsRepPagosTranferencia1
            // 
            this.dsRepPagosTranferencia1.DataSetName = "DsRepPagosTranferencia";
            this.dsRepPagosTranferencia1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cpos_PagoTransferenciaTableAdapter
            // 
            this.cpos_PagoTransferenciaTableAdapter.ClearBeforeFill = true;
            // 
            // FrmMostrarRep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 475);
            this.Controls.Add(this.TxtOrden);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmMostrarRep";
            this.Text = "FrmMostrarRep";
            this.Load += new System.EventHandler(this.FrmMostrarRep_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCambio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRepPagosTranferencia1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TextBox TxtOrden;
        private Reportes.DsReppCambio dsCambio;
        private Reportes.DsReppCambioTableAdapters.SP_CPOS_RepCambioTableAdapter SP_CPOS_RepCambioTableAdapter;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.BindingSource bindingSource3;
        private Reportes.DsRepPagosTranferencia dsRepPagosTranferencia1;
        private Reportes.DsRepPagosTranferenciaTableAdapters.Cpos_PagoTransferenciaTableAdapter cpos_PagoTransferenciaTableAdapter;
    }
}