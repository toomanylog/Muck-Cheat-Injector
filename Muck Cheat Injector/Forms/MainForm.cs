using System;
using System.IO;
using System.Windows.Forms;
using SharpMonoInjector;

namespace Muck_Cheat_Injector.Forms
{
    public partial class MainForm : Form
    {
        private Injector injector;
        private byte[] assembly;
        private IntPtr remoteAssembly = IntPtr.Zero;
        public MainForm()
        {
            InitializeComponent();
            flatButton2.Hide();
            if(File.Exists("Muck Cheat.dll")) assembly = File.ReadAllBytes("Muck Cheat.dll");
        }

        private void flatButton1_Click(object sender, EventArgs e)
        {
            try
            {
                injector = new Injector("Muck");
                remoteAssembly = injector.Inject(assembly, "Muck_Cheat", "CheatLoader", "Main");

                if (remoteAssembly != IntPtr.Zero) flatButton2.Show();
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
