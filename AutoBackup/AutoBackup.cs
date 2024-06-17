using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            localPasta.RootFolder = Environment.SpecialFolder.MyComputer;

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
            localArquivo.InitialDirectory = LocalPasta.Text;
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

        private void autoBackup_Resize_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)

            {

                this.Hide();

                notifyIcon1.Visible = true;

            }
        }


        private void backupTab_Click(object sender, EventArgs e)
        {
            paginaProg.PageName = "backupPage";
        }

        private void ftpTab_Click(object sender, EventArgs e)
        {
            paginaProg.PageName = "ftpPage";
        }

        private void googleTab_Click(object sender, EventArgs e)
        {
            paginaProg.PageName = "googlePage";
        }

        private void oneDriveTab_Click(object sender, EventArgs e)
        {
            paginaProg.PageName = "oneDrivePage";
        }

        private void iniciarAutoBackup_Click(object sender, EventArgs e)
        {
            if (iniciarAutoBackup.Text == "Parar")
            {
                iniciarAutoBackup.Text = "Iniciar";

                timer1.Enabled = false;
                

            }
            else
            {
                iniciarAutoBackup.Text = "Parar";
            
                timer1.Enabled = true;
                timer1.Tick += new EventHandler(timer1_Tick);
                //timer1.Interval = 18000000; 5 horas
                timer1.Interval = 180000; // 3 minutos
                timer1.Start();
                try
                {
                    string connectionString = $"Server={Instancia.Text};Database={Banco.Text};User ID={Usuario.Text};Password={Senha.Text};";
                    string backupConteudo;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(
                        backupConteudo = $@"BACKUP DATABASE {Banco.Text} 
                        TO DISK = '{LocalPasta.Text}\{EmpresaNome.Text}{Banco.Text}.bak'
                    ", connection
                        );
                        cmd.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string connectionString = $"Server={Instancia.Text};Database={Banco.Text};User ID={Usuario.Text};Password={Senha.Text};";
                string backupConteudo;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(
                    backupConteudo = $@"BACKUP DATABASE {Banco.Text} 
                    TO DISK = '{LocalPasta.Text}\{EmpresaNome.Text}{Banco.Text}DF{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}.bak'
                    WITH DIFFERENTIAL
                    ", connection
                    );
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);

            }
        
        }
        }
    }

