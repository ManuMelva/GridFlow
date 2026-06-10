using System.Drawing;
using System.Windows.Forms;

namespace GridFlow.Controls
{
    partial class PaginationControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.paginationPanel = new System.Windows.Forms.Panel();
            this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.pnlPageNumbers = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cmbPageSize = new System.Windows.Forms.ComboBox();
            this.paginationPanel.SuspendLayout();
            this.flowLayout.SuspendLayout();
            this.SuspendLayout();

            this.paginationPanel.BackColor = System.Drawing.Color.White;
            this.paginationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paginationPanel.Controls.Add(this.flowLayout);
            this.paginationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paginationPanel.Location = new System.Drawing.Point(0, 0);
            this.paginationPanel.Name = "paginationPanel";
            this.paginationPanel.Size = new System.Drawing.Size(800, 36);
            this.paginationPanel.TabIndex = 0;

            this.flowLayout.Controls.Add(this.btnFirst);
            this.flowLayout.Controls.Add(this.btnPrev);
            this.flowLayout.Controls.Add(this.pnlPageNumbers);
            this.flowLayout.Controls.Add(this.btnNext);
            this.flowLayout.Controls.Add(this.btnLast);
            this.flowLayout.Controls.Add(this.lblInfo);
            this.flowLayout.Controls.Add(this.cmbPageSize);
            this.flowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayout.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowLayout.Location = new System.Drawing.Point(0, 0);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.flowLayout.Size = new System.Drawing.Size(798, 34);
            this.flowLayout.TabIndex = 0;
            this.flowLayout.WrapContents = false;

            this.btnFirst.FlatAppearance.BorderSize = 0;
            this.btnFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirst.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirst.ForeColor = Color.FromArgb(0x00, 0x78, 0xD4);
            this.btnFirst.Location = new System.Drawing.Point(8, 5);
            this.btnFirst.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(28, 24);
            this.btnFirst.TabIndex = 0;
            this.btnFirst.Text = "\u00AB";
            this.btnFirst.UseVisualStyleBackColor = false;
            this.btnFirst.Click += new System.EventHandler(this.BtnFirst_Click);

            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnPrev.ForeColor = Color.FromArgb(0x00, 0x78, 0xD4);
            this.btnPrev.Location = new System.Drawing.Point(38, 5);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(28, 24);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "\u2039";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.BtnPrev_Click);

            this.pnlPageNumbers.AutoSize = true;
            this.pnlPageNumbers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPageNumbers.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPageNumbers.Name = "pnlPageNumbers";
            this.pnlPageNumbers.Size = new System.Drawing.Size(0, 24);
            this.pnlPageNumbers.WrapContents = false;

            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNext.ForeColor = Color.FromArgb(0x00, 0x78, 0xD4);
            this.btnNext.Location = new System.Drawing.Point(70, 5);
            this.btnNext.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(28, 24);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "\u203A";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);

            this.btnLast.FlatAppearance.BorderSize = 0;
            this.btnLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLast.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLast.ForeColor = Color.FromArgb(0x00, 0x78, 0xD4);
            this.btnLast.Location = new System.Drawing.Point(100, 5);
            this.btnLast.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(28, 24);
            this.btnLast.TabIndex = 3;
            this.btnLast.Text = "\u00BB";
            this.btnLast.UseVisualStyleBackColor = false;
            this.btnLast.Click += new System.EventHandler(this.BtnLast_Click);

            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInfo.ForeColor = Color.FromArgb(0x32, 0x31, 0x30);
            this.lblInfo.Location = new System.Drawing.Point(136, 5);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(28, 15);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "info";

            this.cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPageSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPageSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbPageSize.Items.AddRange(new object[] { "10", "20", "50", "100" });
            this.cmbPageSize.Location = new System.Drawing.Point(168, 5);
            this.cmbPageSize.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.cmbPageSize.Name = "cmbPageSize";
            this.cmbPageSize.Size = new System.Drawing.Size(60, 23);
            this.cmbPageSize.TabIndex = 5;
            this.cmbPageSize.SelectedIndexChanged += new System.EventHandler(this.CmbPageSize_SelectedIndexChanged);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.paginationPanel);
            this.Name = "PaginationControl";
            this.Size = new System.Drawing.Size(800, 36);
            this.paginationPanel.ResumeLayout(false);
            this.flowLayout.ResumeLayout(false);
            this.flowLayout.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel paginationPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayout;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.FlowLayoutPanel pnlPageNumbers;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox cmbPageSize;
    }
}
