using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SharpMonoInjector;

namespace Muck_Cheat_Injector.Forms
{
    public partial class MainForm : Form
    {

        private Injector injector;
        private IntPtr remoteAssembly = IntPtr.Zero;
        public MainForm()
        {
            InitializeComponent();
            flatButton2.Hide();

            if(Utils.CheckForInternetConnection())
            {
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

                    int result = githubVersion.CompareTo(localVersion);
                    if (result > 0)
                        File.Copy(fileName, Application.StartupPath + @"\Muck_Cheat.dll");
                }
            }
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

        private void flatButton1_Click(object sender, EventArgs e)
        {
            try
            {
                injector = new Injector("Muck");
                byte[] assembly = File.ReadAllBytes(GetCheatDLL());
                using (injector)
                {
                    remoteAssembly = injector.Inject(assembly, "Muck_Cheat", "CheatLoader", "main");

                    if (remoteAssembly != IntPtr.Zero) flatButton2.Show();
                }
                
            } catch(Exception ex)
            {
                MessageBox.Show(null, "The Injection Failed: " + ex.Message,
                    "Injection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void flatButton2_Click(object sender, EventArgs e)
        {
            if(remoteAssembly == IntPtr.Zero)
            {
                flatButton2.Hide();
                return;
            }
            try
            {
                injector.Eject(remoteAssembly, "Muck_Cheat", "ClassLoader", "Eject");
            } catch(Exception ex)
            {
                MessageBox.Show(null, "The Injection Failed: " + ex.Message,
                    "Injection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new CreditsForm(this);
            Hide();
        }
    }
}
