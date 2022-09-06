using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DosTranV2.Core
{
    internal class FTP
    {
        private string _host = null;
        private string _user = null;
        private string _pass = null;
        private FtpWebRequest _ftpRequest = null;
        private FtpWebResponse _ftpResponse = null;
        private Stream _ftpStream = null;
        private int _bufferSize = 2048;

        public FTP(string hostIP, string userName, string password)
        {
            _host = hostIP;
            _user = userName;
            _pass = password;
        }

        public void Download(string remoteFile, string localFile)
        {
            try
            {
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create($"{_host}/'{remoteFile}'");
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;//false
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;

                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
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
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                //Resource Cleanup
                localFileStream.Close();
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        public void Upload(string remoteFile, string localFile)
        {
            try
            {
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create($"{_host}/'{remoteFile}'");
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;//false
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                _ftpStream = _ftpRequest.GetRequestStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                byte[] byteBuffer = new byte[_bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, _bufferSize);
                try
                {
                    while (bytesSent != 0)
                    {
                        _ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, _bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                localFileStream.Close();
                _ftpStream.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
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