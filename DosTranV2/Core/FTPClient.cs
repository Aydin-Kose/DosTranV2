using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DosTranV2.Core
{
    internal class FTPClient
    {
        private string _host = null;
        private string _user = null;
        private string _pass = null;
        private FtpWebRequest _ftpRequest = null;
        private FtpWebResponse _ftpResponse = null;
        private Stream _ftpStream = null;
        private int _bufferSize = 2048;

        public FTPClient(string hostIP, string userName, string password)
        {
            _host = hostIP;
            _user = userName;
            _pass = password;
        }

        private void CreateFTPRequest(string remoteFile)
        {
            _ftpRequest = (FtpWebRequest)FtpWebRequest.Create($"{_host}/'{remoteFile}'");
            _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
            _ftpRequest.UseBinary = true;//false
            _ftpRequest.UsePassive = true;
            _ftpRequest.KeepAlive = true;
        }

        public void Download(string remoteFile, string localFile)
        {
            try
            {
                CreateFTPRequest(remoteFile);

                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.CreateNew);
                byte[] byteBuffer = new byte[_bufferSize];
                int bytesRead = _ftpStream.Read(byteBuffer, 0, _bufferSize);
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = _ftpStream.Read(byteBuffer, 0, _bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    ///FTP Download Error
                }
                //Resource Cleanup
                localFileStream.Close();
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (IOException ex)
            {
                //File Create Error
            }
            catch (Exception ex)
            {
                //General Error
            }
        }

        

        public void Upload(string remoteFile, string localFile)
        {
            try
            {
                CreateFTPRequest(remoteFile);

                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                _ftpStream = _ftpRequest.GetRequestStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Open);
                byte[] byteBuffer = new byte[_bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, _bufferSize);
                int i = 1;
                try
                {
                    while (bytesSent != 0)
                    {
                        _ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, _bufferSize);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    //Upload Error
                }
                localFileStream.Close();
                _ftpStream.Close();
                _ftpRequest = null;
            }
            catch (FileNotFoundException ex)
            {
                //File error
            }
            catch (Exception ex)
            {
                //General Error
            }
        }

        public FtpWebResponse GetResponse(FtpWebRequest request)
        {
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //Log.LogWrite("Append status: " + response.StatusDescription);
                //response.Close();
                return response;

            }
            catch (Exception ex)
            {
                //Log.LogWrite("response err : " + ex.Message);
                return null;
            }
        }

        public string GetFileSize(string fileName)
        {
            try
            {
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create($"{_host}/'{fileName}'");
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;

                _ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                StreamReader ftpReader = new StreamReader(_ftpStream);
                string fileInfo = null;
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                ftpReader.Close();
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return "";
        }
    }
}