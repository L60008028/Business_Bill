using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace bll
{
    public class MyFileOptions
    {

        public MyFileOptions()
        {
           
        }


        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 判断文件是否打开
        /// </summary>
        /// <param name="filePath">文件</param>
        /// <returns>打开:true,没打开:false</returns>
        public static bool FileIsOpen(string filePath)
        {
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {                 
                return true;
            }
            CloseHandle(vHandle);
            return false;
        } 


        /// <summary>
        /// 读文件txt\csv类型的文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public List<string> ReadFileTXT(string filePath)
        {
            List<string> countlist = null;
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return null;
                }

                string extend = filePath.Substring(filePath.LastIndexOf('.')).ToLower();//取扩展名
                if (extend.Equals(".txt") || extend.Equals(".csv"))//判断是否是文本文件
                {
                    FileStream fs = new FileStream(filePath, FileMode.Open);//打开文件               
                    StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gbk"));//读流    
                    countlist = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        string strline = sr.ReadLine().Trim();
                        strline = strline.Replace('，', ',');
                        countlist.Add(strline);//保存到list中                   

                    }
                    sr.Close();
                    fs.Close();

                }
            }catch(Exception ex)
            {
                //log.WriteLog("FileOptions.ReadFileTXT()=>Exception:" + ex.Message);
            }
            return countlist;
        }



        /// <summary>
        /// 读文件txt\csv类型的文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public List<string> ReadFileTXT2(string filePath)
        {
            List<string> countlist = null;
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return null;
                }

                string extend = filePath.Substring(filePath.LastIndexOf('.')).ToLower();//取扩展名
                if (extend.Equals(".txt") || extend.Equals(".csv"))//判断是否是文本文件
                {
                    FileStream fs = new FileStream(filePath, FileMode.Open);//打开文件               
                    StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8"));//读流    
                    countlist = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        string strline = sr.ReadLine().Trim();
                        strline = strline.Replace('，', ',');
                        string[] tmp = strline.Split(',');
                        foreach (string str in tmp)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                countlist.Add(str.Trim());//保存到list中 
                            }
                        }

                    }
                    sr.Close();
                    fs.Close();

                }
            }catch(Exception ex)
            {
                
            }
            return countlist;
        }



        /// <summary>
        /// 写文件txt\csv类型
        /// </summary>
        /// <param name="contList">内容</param>
        /// <param name="filepath">路径</param>
        /// <returns></returns>
        public static bool WriteFileTXT(List<string> contList, string filepath)
        {

            FileStream fs = null;//文件流
            StreamWriter sw = null;//读流
            try
            {
                if (!string.IsNullOrEmpty(filepath) && contList!=null)
                {
 
                    fs = new FileStream(filepath, FileMode.Create);//创建文件
                    sw = new StreamWriter(fs, Encoding.GetEncoding("GBK"));
                    //写文件
                    for (int j = 0; j < contList.Count; j++)
                    {
                        string put =  contList[j] + "\t";
                        sw.WriteLine(put);
                    } 
                }
 
            }
            catch (Exception ex)
            {
                Console.WriteLine("写文件txt,csv类型" + ex);
                return false;
            }
            finally
            {
                if(sw!=null)
                {
                sw.Close();
                }
                if(fs!=null)
                {
                fs.Close();
                }
            }

            return true;

        }


        /// <summary>
        /// 使用流,执行导出2,EXCEL格式
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="fileName">路径</param>
        public bool DoExportUseStream(DataSet ds, string fileName)
        {

            FileStream fs = null;//文件流
            StreamWriter sw = null;//读流

            try
            {
                if (fileName != string.Empty)
                {
                    fs = new FileStream(fileName, FileMode.Create);//创建文件
                    sw = new StreamWriter(fs, Encoding.GetEncoding("GBK"));
                    DataTable dt = ds.Tables[0];
                    //写表头
                    string head = "";
                    foreach (DataColumn col in dt.Columns)
                    {
                        head += col.ColumnName + "\t";
                    }
                    sw.WriteLine(head);
                    //写内容
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataTable colTable = dt.Rows[j].Table;
                        string put = "";
                        for (int i = 0; i < colTable.Columns.Count; i++)
                        {
                            put += colTable.Rows[j][i] + "\t";
                        }

                        sw.WriteLine(put);
                    }
                    sw.Close();
                    fs.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("使用流,执行导出2,EXCEL格式" + ex.Message);
                return false;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
            return true;
        }

         
        /// <summary>
        /// 复制本地文件
        /// </summary>
        /// <param name="sourceFileName">源文件</param>
        /// <param name="destFileName">目标文件</param>
        public void DownLocalFile(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName, true);
        }

         
    }//end
}//end
