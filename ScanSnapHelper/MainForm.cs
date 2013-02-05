using EasyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
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

            // initialize EasyHook
            InitEasyHook();
        }

        String ChannelName = null;

        protected void InitEasyHook()
        {
            Int32 TargetPID = 0;

            System.Diagnostics.Process[] ps =
                System.Diagnostics.Process.GetProcessesByName("Acrobat");

            if (ps.Length != 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: FileMon %PID%");
                Console.WriteLine();

                return;
            }
            TargetPID = ps.First().Id;
            Console.WriteLine(TargetPID);

            try
            {
                try
                {
                    Config.Register(
                        "A helper tool for ScanSnapManager and Acrobat.",
                        "ScanSnapHelper.exe",
                        "ScanSnapHelperInject.dll");
                }
                catch (ApplicationException)
                {
                    MessageBox.Show("This is an administrative task!", "Permission denied...", MessageBoxButtons.OK);

                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }

                RemoteHooking.IpcCreateServer<ScanSnapHelperInterface>(ref ChannelName, WellKnownObjectMode.SingleCall);

                RemoteHooking.Inject(
                    TargetPID,
                    "ScanSnapHelperInject.dll",
                    "ScanSnapHelperInject.dll",
                    ChannelName);

                Console.ReadLine();
            }
            catch (Exception ExtInfo)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n{0}", ExtInfo.ToString());
            }
        }

    }

    public class ScanSnapHelperInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 InClientPID)
        {
            Console.WriteLine("FileMon has been installed in target {0}.\r\n", InClientPID);
        }

        public void OnCreateFile(Int32 InClientPID, String[] InFileNames)
        {
            for (int i = 0; i < InFileNames.Length; i++)
            {
                Console.WriteLine(InFileNames[i]);
            }
        }

        public void ReportException(Exception InInfo)
        {
            Console.WriteLine("The target process has reported an error:\r\n" + InInfo.ToString());
        }

        public void Ping()
        {
        }
    }
}
