using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using model;

namespace bll
{
    public class FtpClient
    {
        string strFTPIP = "";
        string strFTPUser = "";
        string strFTPPSW = "";
        public delegate void WriteLogDelegate(string txt);
        public event WriteLogDelegate WriteLog;
        public FtpClient()
        {
            WriteLog += (x) => { };
            FtpConfig ftpcon = new FtpConfig();
            strFTPIP = ftpcon.IP + ":" + ftpcon.Port;
            strFTPUser = ftpcon.LoginName;
            strFTPPSW = ftpcon.Password;
        }


        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="ftpfilePath">ftp服务器路径</param>
        /// <param name="ftpfileName">ftp保存的文件名</param>
        /// <param name="localfile">本地文件完整路径</param>
        /// <returns></returns>
        public bool Upload(string ftpfilePath, string ftpfileName, string localfile)
        {
            FileInfo fileInf = new FileInfo(localfile);
            string uri = "ftp://" + strFTPIP + "/" + ftpfilePath + "/" + ftpfileName;
            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided  
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strFTPIP + "/" + ftpfilePath + "/" + ftpfileName));

            // Provide the WebPermission Credintials  
            reqFTP.Credentials = new NetworkCredential(strFTPUser, strFTPPSW);

            // By default KeepAlive is true, where the control connection is not closed  
            // after a command is executed.  
            reqFTP.KeepAlive = false;

            // Specify the command to be executed.  
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.  
            reqFTP.UseBinary = true;

            // Notify the server about the size of the uploaded file  
            reqFTP.ContentLength = fileInf.Length;

            // The buffer size is set to 2kb  
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded  
            using (FileStream fs = fileInf.OpenRead())
            {

                try
                {
                    // Stream to which the file to be upload is written  
                    using (Stream strm = reqFTP.GetRequestStream())
                    {

                        // Read from the file stream 2kb at a time  
                        contentLen = fs.Read(buff, 0, buffLength);

                        // Till Stream content ends  
                        while (contentLen != 0)
                        {
                            // Write Content from the file stream to the FTP Upload Stream  
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }

                        // Close the file stream and the Request Stream  
                        //strm.Close();  
                        //fs.Close();  
                    }
                }
                catch (Exception ex)
                {
                    //fs.Close();  
                    //LogAPI.WriteLog("Error in getting the Upload!" + ex.Message);
                    return false;
                }
            }

            return true;

        }


        public bool FileCheckExist(string ftpPath, string ftpName)
        {
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;

            try
            {
                string url = "ftp://" + strFTPIP + "/" + ftpPath;
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                ftpWebRequest.Credentials = new NetworkCredential(strFTPUser, strFTPPSW);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    WriteLog(line);
                    if (line.Contains(ftpName))
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                success = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
            return success;
        }


        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool DirectoryExist(string dirName)
        {

            try
            {
                string uri = "ftp://" + strFTPIP + "/" + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(strFTPUser, strFTPPSW);
                reqFTP.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
                reqFTP.UseBinary = true;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();              
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return false;
            }
            
        }



        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="pFtpServerIP"></param>
        /// <returns>false不存在，true存在</returns>
        public bool DirectoryIsExist(string dirName)
        {
            string uri = "ftp://" + strFTPIP + "/" + dirName;

            string[] value = GetFileList(new Uri(uri));
            if (value == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public   string[] GetFileList(Uri uri)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strFTPUser, strFTPPSW);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch
            {
                return null;
            }
        }






        /// <summary>
        /// 创建目录  
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool MakeDir(string dirName)
        {
            string uri = "ftp://" + strFTPIP + "/" + dirName;
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(uri);
            req.Credentials = new NetworkCredential(strFTPUser, strFTPPSW);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool DelDir(string dirName)
        {
            try
            {
                string uri = "ftp://" + strFTPIP + "/" + dirName;
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(uri);
                req.Credentials = new NetworkCredential(strFTPUser, strFTPPSW);
                req.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
                req.Abort();
                return true;
            }

            catch (Exception ex)
            {

            }
            return false;
        }







        /// <summary>  
        /// 下载文件  
        /// </summary>  
        /// <param name="filePath">文件下载后存放路径</param>  
        /// <param name="SaveFileName">文件下载后的名称</param>  
        /// <param name="parFileName">要下载的文件名</param>  
        /// <returns></returns>  
        private int Download(string filePath, string SaveFileName, string ftpDownPath, string ftpFileName)
        {
            FtpWebRequest reqFTP;
            FileStream outputStream = new FileStream(filePath + "\\" + SaveFileName, FileMode.Create);

            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strFTPIP + "/" + ftpDownPath + "/" + ftpFileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strFTPUser, strFTPPSW);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ////下载成功后，删除服务器上的文件。  
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + strFTPIP + "/" + strFtpPath + "/" + fileName));  
                //request.Method = WebRequestMethods.Ftp.DeleteFile;  

                //FtpWebResponse responseD = (FtpWebResponse)request.GetResponse();  


                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return readCount;
            }
            catch (Exception ex)
            {
                outputStream.Close();
                //MessageBox.Show(ex.Message);  
                return -1;
            }
        }


    }//end
}//end
