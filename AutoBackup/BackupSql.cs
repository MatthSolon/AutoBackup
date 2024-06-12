using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBackup
{
    internal class BackupSql
    {
        public static void Backup(string instanciaNome, string bancoNome, string bancoUsuario, string BancoSenha, string localpasta, bool bkpDiferencial, string nomeEmpresa)
        {
            string connectionString = $"Server={instanciaNome};Database={bancoNome};User ID={bancoUsuario};Password={BancoSenha};";
            string backupConteudo;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {

                    if (bkpDiferencial)
                    {



                        connection.Open();
                        SqlCommand cmd = new SqlCommand(


                           backupConteudo = $@"BACKUP DATABASE {bancoNome} 
                            TO DISK = '{localpasta}\{nomeEmpresa}{bancoNome}DF.bak'
                            WITH DIFFERENTIAL
                            ", connection
                            );


                        cmd.ExecuteNonQuery();


                        MessageBox.Show("Backup Diferencial feito com Sucesso");

                    }
                    else
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(
                           backupConteudo = $@"BACKUP DATABASE {bancoNome} 
                            TO DISK = '{localpasta}\{nomeEmpresa}{bancoNome}.bak'", connection
                            );
                        cmd.ExecuteNonQuery();


                        MessageBox.Show("Backup feito com Sucesso");

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
