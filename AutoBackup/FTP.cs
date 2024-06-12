using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBackup
{
    internal class FTP
    {
        public static void FtpEnvio(string bancoNome, string BancoSenha, string instanciaNome, string UrlFtp, string UsuarioFtp, string SenhaFtp, string arquivoLocal)
        {
            string destinoRemoto = $"/database/{bancoNome}.bak";

            try
            {
                FtpWebRequest ftpAcesso = (FtpWebRequest)WebRequest.Create(UrlFtp + destinoRemoto);
                ftpAcesso.Method = WebRequestMethods.Ftp.UploadFile;
                ftpAcesso.Credentials = new NetworkCredential(UsuarioFtp, SenhaFtp);


                using (Stream stream = ftpAcesso.GetRequestStream())
                {
                    byte[] conteudoArquivo = File.ReadAllBytes(arquivoLocal);
                    stream.Write(conteudoArquivo, 0, conteudoArquivo.Length);

                }

                FtpWebResponse response = (FtpWebResponse)ftpAcesso.GetResponse();
                MessageBox.Show($"Upload concluído. Status: {response.StatusDescription}");
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao fazer upload: {ex.Message}");
            }
        }
    }
}
