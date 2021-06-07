using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SharpMonoInjector;

namespace Muck_Cheat_Injector.Forms
{
    public partial class MainForm : Form
    {
        private Thread injectThread;

        private int clickCount = 0;

        private string @namespace = "Muck_Cheat",
                       @class = "CheatLoader";
        public MainForm()
        {
            InitializeComponent();

            #region Auto-Update Cheat
            //Check for Internet Connection
            if (Utils.CheckForInternetConnection())
            {

                //Download latest Cheat DLL
                Directory.CreateDirectory("temp");
                string fileName = Application.StartupPath + @"\temp\Muck_Cheat.dll";
                Utils.DownloadFile("https://github.com/DasJNNJ/Muck-Cheat/releases/download/v1.2/Muck_Cheat.dll", fileName);

                if(!File.Exists(GetCheatDLL()))
                {
                    File.Copy(fileName, Application.StartupPath + @"\Muck_Cheat.dll");
                } else
                {
                    Version githubVersion = Version.Parse(Utils.GetVersionStringFromAssemblyDLL(fileName));
                    Version localVersion = Version.Parse(Utils.GetVersionStringFromAssemblyDLL(GetCheatDLL()));

                    //Check if local version is newer
                    int result = githubVersion.CompareTo(localVersion);
                    if (result > 0)
                        File.Copy(fileName, Application.StartupPath + @"\Muck_Cheat.dll");
                }
            }
            #endregion
        }

        private string GetCheatDLL()
        {
            foreach(string dll in Directory.GetFiles(Application.StartupPath))
            {
                if(dll.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) &&
                    dll.Contains("Muck") &&
                    dll.Contains("Cheat"))
                {
                    return dll;
                }
            }
            return "null";
        }

        #region Buttons
        private void injectButton_Click(object sender, EventArgs e)
        {
            if(injectThread == null)
            {
                Process.Start("steam://rungameid/1625450");
                injectThread = new Thread(() => {
                    bool muckRunning = false;
                    while(!muckRunning)
                    {
                        Thread.Sleep(1000);
                        Process[] pname = Process.GetProcessesByName("Muck");
                        if (pname.Length > 0)
                            muckRunning = true;
                    }

                    Thread.Sleep(2500);

                    try
                    {
                        Injector injector = new Injector("Muck");
                        byte[] assembly = File.ReadAllBytes(GetCheatDLL());
                        using (injector)
                        {
                            injector.Inject(assembly, @namespace, @class, "main");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(null, "The Injection Failed: " + ex.Message,
                            "Injection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    injectThread = null;
                });

                injectThread.Start();
                
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            clickCount++;
            if(clickCount == 5)
            {
                MessageBox.Show(null, "secret go brrrrrr - Baum 2021",
                    "Very Secret", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/KweH53ADtW");
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            new CreditsForm(this);
            Hide();
        }
        #endregion
    }
}
