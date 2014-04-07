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
    [Serializable]
    public struct ApiCall
    {
        public string api;
        public string[] parameters;
        public ApiCall(string api, params string[] parameters)
        {
            this.api = api;
            this.parameters = parameters;
        }
    }

    public partial class MainForm : Form
    {
        private static MainForm _instance;

        private Timer timer = new Timer();

        private Int32 SsMonPID = 0;

        public static MainForm Instance
        {
            get {
                return _instance;
            }
        }
        public MainForm()
        {
            _instance = this;

            InitializeComponent();

            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new EventHandler(this.timer_Tick);

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
            LogFileName(item);
        }

        String ChannelName = null;

        protected void InitEasyHook()
        {
            Int32 TargetPID = 0;

            System.Diagnostics.Process[] ps =
                System.Diagnostics.Process.GetProcessesByName("PfuSsMon");

            string pids = string.Join(",", ps.Select(p => p.Id.ToString()).ToArray());

            if (ps.Length != 1)
            {
                txtSsMon.Text = "Not hooked: " + pids;
                return;
            }
            TargetPID = ps.First().Id;
            if (TargetPID == SsMonPID)
            {
                return;
            }

            try
            {
                if (ChannelName == null)
                {
                    try
                    {
                        Config.Register(
                            "A helper tool for ScanSnapManager.",
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
                }
                txtSsMon.Text = "Hooked: " + pids;
                SsMonPID = TargetPID;
            }
            catch (Exception ExtInfo)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n{0}", ExtInfo.ToString());
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            InitEasyHook();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        public void OnApiCall(ApiCall Call)
        {
            if (Call.api == "CreateFileW" || Call.api == "CreateFileA") {
                string[] item = {Call.parameters.First(), Call.api};
                lvFiles.Items.Add(new ListViewItem(item));
            } else {
                MessageBox.Show("unexpected api", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LogFileName(string[] item)
        {
            lvFiles.Items.Add(new ListViewItem(item));
        }

    }

    public class ScanSnapHelperInterface : MarshalByRefObject
    {
        private delegate void MyDelegate(ApiCall Call);

        public void IsInstalled(Int32 InClientPID)
        {
            Console.WriteLine("ScanSnapHelper has been installed in target {0}.\r\n", InClientPID);
        }

        public void OnApiCall(Int32 InClientPID, ApiCall[] Calls)
        {
            foreach (ApiCall Call in Calls) {
                // 方法 : Windows フォーム コントロールのスレッド セーフな呼び出しを行う
                // http://msdn.microsoft.com/ja-jp/library/ms171728%28VS.80%29.aspx
                MainForm.Instance.Invoke(new MyDelegate(ProxyApiCall), Call);
            }
        }

        private void ProxyApiCall(ApiCall Call)
        {
            MainForm.Instance.OnApiCall(Call);
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
