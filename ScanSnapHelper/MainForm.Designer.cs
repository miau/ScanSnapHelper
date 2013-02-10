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
            this.lblAcrobat = new System.Windows.Forms.Label();
            this.txtAcrobat = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.Location = new System.Drawing.Point(10, 100);
            this.lvFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(556, 428);
            this.lvFiles.TabIndex = 0;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            // 
            // lblAcrobat
            // 
            this.lblAcrobat.AutoSize = true;
            this.lblAcrobat.Location = new System.Drawing.Point(13, 13);
            this.lblAcrobat.Name = "lblAcrobat";
            this.lblAcrobat.Size = new System.Drawing.Size(119, 21);
            this.lblAcrobat.TabIndex = 1;
            this.lblAcrobat.Text = "Acrobat.exe:";
            // 
            // txtAcrobat
            // 
            this.txtAcrobat.Enabled = false;
            this.txtAcrobat.Location = new System.Drawing.Point(152, 10);
            this.txtAcrobat.Name = "txtAcrobat";
            this.txtAcrobat.Size = new System.Drawing.Size(200, 27);
            this.txtAcrobat.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 538);
            this.Controls.Add(this.txtAcrobat);
            this.Controls.Add(this.lblAcrobat);
            this.Controls.Add(this.lvFiles);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "ScanSnapHelper";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.Label lblAcrobat;
        private System.Windows.Forms.TextBox txtAcrobat;
    }
}

