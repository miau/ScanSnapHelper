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
            this.lvFiles = new System.Windows.Forms.ListView();
            this.lblSsMon = new System.Windows.Forms.Label();
            this.txtSsMon = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.Location = new System.Drawing.Point(12, 120);
            this.lvFiles.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(656, 513);
            this.lvFiles.TabIndex = 0;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            // 
            // lblSsMon
            // 
            this.lblSsMon.AutoSize = true;
            this.lblSsMon.Location = new System.Drawing.Point(15, 16);
            this.lblSsMon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSsMon.Name = "lblSsMon";
            this.lblSsMon.Size = new System.Drawing.Size(168, 25);
            this.lblSsMon.TabIndex = 1;
            this.lblSsMon.Text = "PfuSsMon.exe:";
            // 
            // txtSsMon
            // 
            this.txtSsMon.Enabled = false;
            this.txtSsMon.Location = new System.Drawing.Point(180, 12);
            this.txtSsMon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSsMon.Name = "txtSsMon";
            this.txtSsMon.Size = new System.Drawing.Size(236, 31);
            this.txtSsMon.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 646);
            this.Controls.Add(this.txtSsMon);
            this.Controls.Add(this.lblSsMon);
            this.Controls.Add(this.lvFiles);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "MainForm";
            this.Text = "ScanSnapHelper";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.Label lblSsMon;
        private System.Windows.Forms.TextBox txtSsMon;
    }
}

