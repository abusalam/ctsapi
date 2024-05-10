using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace CTS_BE.Helper
{
    public static class SFTPHelper
    {
        public static void UploadFile(string hostServerName, short port, string userName, string password,string localPath, string localDirectoryName,string uploadPath,string uploadDirectoryName)
        {
            using SftpClient client = new(hostServerName, port, userName, password);

            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.UploadFile(File.OpenRead(localPath), uploadPath);
                    client.Disconnect();
                }
            }
            catch (Exception e) when (e is SshConnectionException || e is SocketException || e is ProxyException)
            {
                Console.WriteLine($"Error connecting to server: {e.Message}");
            }
            catch (SshAuthenticationException e)
            {
                Console.WriteLine($"Failed to authenticate: {e.Message}");
            }
            catch (SftpPermissionDeniedException e)
            {
                Console.WriteLine($"Operation denied by the server: {e.Message}");
            }
            catch (SshException e)
            {
                Console.WriteLine($"Sftp Error: {e.Message}");
            }
        }
    }
}