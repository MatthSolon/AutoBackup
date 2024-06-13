using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace AutoBackup
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new autoBackup());
        }
        public static void BackupCont(string instanciaNome, string bancoNome, string bancoUsuario, string BancoSenha, string localpasta, string nomeEmpresa)
        {
            string connectionString = $"Server={instanciaNome};Database={bancoNome};User ID={bancoUsuario};Password={BancoSenha};";
            bool horabackup = DateTime.Parse("10:00:00") == DateTime.Now;
             
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {

                    while (true)
                    {
                        if(!horabackup) 
                        {

                            connection.Open();
                            SqlCommand cmd = new SqlCommand(


                                $@"BACKUP DATABASE {bancoNome} 
                            TO DISK = '{localpasta}\{nomeEmpresa}{bancoNome}DF{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}.bak'
                            WITH DIFFERENTIAL
                            ", connection
                                );


                            cmd.ExecuteNonQuery();


                            MessageBox.Show("Backup Diferencial feito com Sucesso");

                        }
                            
                        {
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(
                                $@"BACKUP DATABASE {bancoNome} 
                            TO DISK = '{localpasta}\{nomeEmpresa}{bancoNome}.bak'", connection
                                );
                            cmd.ExecuteNonQuery();


                            MessageBox.Show("Backup feito com Sucesso");

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
                }
            }
        }


    }
}
