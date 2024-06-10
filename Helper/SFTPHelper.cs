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
        public static void UploadFile(string hostServerName, short port, string userName, string password,string localPath,string uploadPath)
        {
            using SftpClient client = new(hostServerName, port, userName, password);

            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory("CTS");
                    client.UploadFile(File.OpenRead(localPath), uploadPath);
                    client.Disconnect();
                }
            }
            catch (Exception e) when (e is SshConnectionException || e is SocketException || e is ProxyException)
            {
               throw new Exception($"Error connecting to server: {e.Message}");
            }
            catch (SshAuthenticationException e)
            {
                 throw new Exception($"Failed to authenticate: {e.Message}");
            }
            catch (SftpPermissionDeniedException e)
            {
                 throw new Exception($"Operation denied by the server: {e.Message}");
            }
            catch (SshException e)
            {
                 throw new Exception($"Sftp Error: {e.Message}");
            }
        }
    }
}