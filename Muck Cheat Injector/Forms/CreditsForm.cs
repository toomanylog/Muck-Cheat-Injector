using System.Windows.Forms;

namespace Muck_Cheat_Injector.Forms
{
    public partial class CreditsForm : Form
    {
        private MainForm parent;
        public CreditsForm(MainForm parent)
        {
            InitializeComponent();
            this.parent = parent;
            Show();
        }

        private void flatButton1_Click(object sender, System.EventArgs e)
        {
            Hide();
            parent.Show();
            Dispose(true);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/DasJNNJ/Muck-Cheat");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/DasJNNJ/Muck-Cheat-Injector");
        }
    }
}
