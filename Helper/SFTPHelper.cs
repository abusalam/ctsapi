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
        public static void UploadFile(string hostServerName, short port, string userName, string password, string localPath, string uploadPath)
        {
            using SftpClient client = new(hostServerName, port, userName, password);

            try
            {
                if (!File.Exists(localPath))
                {
                    throw new Exception("Local file not found!");
                }
                client.Connect();
                if (client.IsConnected)
                {
                    //client.ChangeDirectory("CTS");
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
        public static void DownloadFile(string hostServerName, short port, string userName, string password, string remotePath, string localPath)
        {
            using SftpClient client = new(hostServerName, port, userName, password);

            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    if (client.Exists(remotePath))
                    {
                        using (var fileStream = File.Create(localPath))
                        {
                            client.DownloadFile(remotePath, fileStream);
                        }
                    }
                    else
                    {
                        throw new Exception($"File '{remotePath}' does not exist on the remote server.");
                    }
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
        public static void MoveFile(string hostServerName, short port, string userName, string password, string sourcePath, string destinationPath, string fileName)
        {
            using SftpClient client = new(hostServerName, port, userName, password);

            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    string sourceFilePath = Path.Combine(sourcePath, fileName);
                    string destinationFilePath = Path.Combine(destinationPath, fileName);

                    if (client.Exists(sourceFilePath))
                    {
                        client.RenameFile(sourceFilePath, destinationFilePath);
                    }
                    else
                    {
                        throw new Exception($"File '{fileName}' does not exist in the source folder.");
                    }

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
        public static void RenameFile(string hostServerName, short port, string userName, string password, string oldFileName, string newFileName,string remotePath)
        {
            using SftpClient client = new(hostServerName, port, userName, password);

            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    if (client.Exists(Path.Combine(remotePath, oldFileName)))
                    {
                        client.RenameFile(Path.Combine(remotePath, oldFileName), Path.Combine(remotePath, newFileName));
                    }
                    else
                    {
                        throw new Exception($"File '{oldFileName}' does not exist on the remote server.");
                    }

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