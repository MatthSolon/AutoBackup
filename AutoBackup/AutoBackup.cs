using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBackup
{
    public partial class autoBackup : Form
    {
        public autoBackup()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void bunifuLabel7_Click(object sender, EventArgs e)
        {

        }

        private void BotaoProcurarL_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog localPasta = new FolderBrowserDialog();
            

            DialogResult dr = localPasta.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LocalPasta.Text = localPasta.SelectedPath;
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Erro.\n\n" +
                                    "Mensagem : " + ex.Message);
                }

            }
        }

        private void botaoBackup_Click(object sender, EventArgs e)
        {
            BackupSql.Backup(Instancia.Text, Banco.Text, Usuario.Text, Senha.Text, LocalPasta.Text, bkpDiferencial.Checked, EmpresaNome.Text);
        }

        private void EnviarFtp_Click(object sender, EventArgs e)
        {

            FTP.FtpEnvio(Banco.Text, Senha.Text, Instancia.Text, UrlFtp.Text, UserFtp.Text, SenhaFtp.Text, LocalArquivo.Text);
        }

        private void BotaoProcurarA_Click(object sender, EventArgs e)
        {
            OpenFileDialog localArquivo = new OpenFileDialog();
            localArquivo.Multiselect = false;
            localArquivo.Title = "Selecionar Arquivos";
            localArquivo.InitialDirectory = LocalPasta.SelectedText;
            localArquivo.Filter = "All files (*.*)|*.*";
            localArquivo.CheckFileExists = true;
            localArquivo.CheckPathExists = true;
            localArquivo.RestoreDirectory = true;


            DialogResult lr = localArquivo.ShowDialog();

            if (lr == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LocalArquivo.Text = localArquivo.FileName;
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Erro.\n\n" +
                                    "Mensagem : " + ex.Message);
                }

            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();

            this.WindowState = FormWindowState.Normal;

            this.notifyIcon1.Visible = false;
        }
        private void autoBackup_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)

            {

                this.Hide();

                notifyIcon1.Visible = true;

            }
        }
    }
}
