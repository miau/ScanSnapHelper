namespace ScanSnapHelper
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSsMon = new System.Windows.Forms.Label();
            this.txtSsMon = new System.Windows.Forms.TextBox();
            this.lblSsMff = new System.Windows.Forms.Label();
            this.txtSsMff = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSsMon
            // 
            this.lblSsMon.AutoSize = true;
            this.lblSsMon.Location = new System.Drawing.Point(15, 16);
            this.lblSsMon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSsMon.Name = "lblSsMon";
            this.lblSsMon.Size = new System.Drawing.Size(387, 25);
            this.lblSsMon.TabIndex = 1;
            this.lblSsMon.Text = "ScanSnap Manager(PfuSsMon.exe):";
            // 
            // txtSsMon
            // 
            this.txtSsMon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSsMon.Enabled = false;
            this.txtSsMon.Location = new System.Drawing.Point(607, 10);
            this.txtSsMon.Margin = new System.Windows.Forms.Padding(4);
            this.txtSsMon.Name = "txtSsMon";
            this.txtSsMon.Size = new System.Drawing.Size(236, 31);
            this.txtSsMon.TabIndex = 2;
            // 
            // lblSsMff
            // 
            this.lblSsMff.AutoSize = true;
            this.lblSsMff.Location = new System.Drawing.Point(15, 56);
            this.lblSsMff.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSsMff.Name = "lblSsMff";
            this.lblSsMff.Size = new System.Drawing.Size(511, 25);
            this.lblSsMff.TabIndex = 3;
            this.lblSsMff.Text = "ScanSnap Manager for fi Series(PfuSsMff.exe):";
            // 
            // txtSsMff
            // 
            this.txtSsMff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSsMff.Enabled = false;
            this.txtSsMff.Location = new System.Drawing.Point(607, 53);
            this.txtSsMff.Margin = new System.Windows.Forms.Padding(4);
            this.txtSsMff.Name = "txtSsMff";
            this.txtSsMff.Size = new System.Drawing.Size(236, 31);
            this.txtSsMff.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 102);
            this.Controls.Add(this.txtSsMff);
            this.Controls.Add(this.lblSsMff);
            this.Controls.Add(this.txtSsMon);
            this.Controls.Add(this.lblSsMon);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "MainForm";
            this.Text = "ScanSnapHelper";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSsMon;
        private System.Windows.Forms.TextBox txtSsMon;
        private System.Windows.Forms.Label lblSsMff;
        private System.Windows.Forms.TextBox txtSsMff;
    }
}

