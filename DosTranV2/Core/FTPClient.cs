using System;
using System.IO;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;

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
            _ftpRequest.UseBinary = false;//false
            _ftpRequest.UsePassive = true;
            _ftpRequest.KeepAlive = true;
        }

        public string Download(string remoteFile, string localFile, string fileType, char? seperator)
        {
            try
            {
                CreateFTPRequest(remoteFile);

                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                try
                {
                    if (fileType == "Text")
                    {
                        FileStream localFileStream = new FileStream(localFile, FileMode.Create);

                        byte[] byteBuffer = new byte[_bufferSize];
                        int bytesRead = _ftpStream.Read(byteBuffer, 0, _bufferSize);

                        while (bytesRead > 0)
                        {
                            localFileStream.Write(byteBuffer, 0, bytesRead);
                            bytesRead = _ftpStream.Read(byteBuffer, 0, _bufferSize);
                        }
                        localFileStream.Close();
                    }
                    else
                    {

                        Excel.Application excel = new Excel.Application();
                        Excel.Workbook workBook = excel.Workbooks.Add();
                        Excel.Worksheet sheet = (Excel.Worksheet)workBook.ActiveSheet;
                        StreamReader reader = new StreamReader(_ftpStream, System.Text.Encoding.UTF8);
                        string line;
                        int lineNumber = 1;
                        while (reader.Peek() > -1)
                        {
                            line = reader.ReadLine();
                            string[] cells = line.Split((char)seperator);
                            for (int i = 0; i < cells.Length; i++)
                            {
                                sheet.Cells[lineNumber,i+1] = cells[i];
                            }
                            lineNumber++;
                        }

                        workBook.SaveAs(localFile);
                        workBook.Close();
                    }
                }

                catch (Exception ex)
                {
                    return "FTP indirme hatası: " + ex.Message;
                }
                //Resource Cleanup
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (IOException ex)
            {
                return "Dosya oluşturma hatası: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "FTP Hata: " + ex.Message;
            }
            return "İşlem Başarılı";
        }



        public string Upload(string remoteFile, string localFile)
        {
            try
            {
                CreateFTPRequest(remoteFile);

                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                _ftpStream = _ftpRequest.GetRequestStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Open);
                var buffLenght = Convert.ToInt32(localFileStream.Length);
                byte[] byteBuffer = new byte[buffLenght];
                int bytesSent = localFileStream.Read(byteBuffer, 0, buffLenght);
                try
                {
                    _ftpStream.Write(byteBuffer, 0, bytesSent);
                }
                catch (Exception ex)
                {
                    return "FTP yükleme hatası: " + ex.Message;
                }
                localFileStream.Close();
                _ftpStream.Close();
                _ftpRequest = null;
            }
            catch (FileNotFoundException ex)
            {
                return "Dosya oluşturma hatası: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "FTP Hata: " + ex.Message;
            }
            return "İşlem Başarılı";
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