using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScanSnapHelper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // initialize this form
            this.Size = new Size(600, 600);

            // initialize file list
            ColumnHeader[] columnHeaders = {
                new ColumnHeader() { Text = "File", Width = 300 },
                new ColumnHeader() { Text = "Status" }
            };
            lvFiles.Columns.AddRange(columnHeaders);

            lvFiles.FullRowSelect = true;
            lvFiles.GridLines = true;
            lvFiles.View = View.Details;

            // add a item to file list
            string[] item = { Environment.GetEnvironmentVariable("TEMP"), "Open" };
            lvFiles.Items.Add(
                new ListViewItem(item)
            );
        }
    }
}
