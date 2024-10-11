
namespace CapaVisual_Login.reportes
{
    partial class Form1
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
            this.Orden_txt = new System.Windows.Forms.TextBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SP_CPOS_ReporAbonoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DsAbono = new CapaVisual_Login.reportes.DsAbono();
            this.SP_CPOS_ReporAbonoTableAdapter = new CapaVisual_Login.reportes.DsAbonoTableAdapters.SP_CPOS_ReporAbonoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.SP_CPOS_ReporAbonoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DsAbono)).BeginInit();
            this.SuspendLayout();
            // 
            // Orden_txt
            // 
            this.Orden_txt.Location = new System.Drawing.Point(223, 116);
            this.Orden_txt.Name = "Orden_txt";
            this.Orden_txt.Size = new System.Drawing.Size(131, 20);
            this.Orden_txt.TabIndex = 2;
            this.Orden_txt.Visible = false;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DsRpAbono";
            reportDataSource1.Value = this.SP_CPOS_ReporAbonoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.reportes.ReportAbono.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(766, 705);
            this.reportViewer1.TabIndex = 3;
            // 
            // SP_CPOS_ReporAbonoBindingSource
            // 
            this.SP_CPOS_ReporAbonoBindingSource.DataMember = "SP_CPOS_ReporAbono";
            this.SP_CPOS_ReporAbonoBindingSource.DataSource = this.DsAbono;
            // 
            // DsAbono
            // 
            this.DsAbono.DataSetName = "DsAbono";
            this.DsAbono.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // SP_CPOS_ReporAbonoTableAdapter
            // 
            this.SP_CPOS_ReporAbonoTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 705);
            this.Controls.Add(this.Orden_txt);
            this.Controls.Add(this.reportViewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SP_CPOS_ReporAbonoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DsAbono)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Orden_txt;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource SP_CPOS_ReporAbonoBindingSource;
        private DsAbono DsAbono;
        private DsAbonoTableAdapters.SP_CPOS_ReporAbonoTableAdapter SP_CPOS_ReporAbonoTableAdapter;
    }
}