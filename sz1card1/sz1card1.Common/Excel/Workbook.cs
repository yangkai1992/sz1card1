///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2009-12-01 Created by pq
///功能说明：提供创建Excel工作簿和导出功能
///注意事项：
///</summary>
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using sz1card1.Common.Enum;
using sz1card1.Common.FileServer;
using Biff8Excel;
using System.Configuration;

namespace sz1card1.Common.Excel
{
    public class Workbook : IDisposable
    {
        private string author;
        private string company;
        private List<WorkSheet> workSheets;
        private ExcelFormat formatType = ExcelFormat.Binary;// ExcelFormat.Binary; //ExcelFormat.Xml
        private StreamWriter writer;

        public Workbook()
        {
            workSheets = new List<WorkSheet>();
        }

        public Workbook(ExcelFormat formatType)
            : this()
        {
            this.formatType = formatType;
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
            }
        }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                company = value;
            }
        }

        internal StreamWriter Writer
        {
            get
            {
                return writer;
            }
        }

        public List<WorkSheet> WorkSheets
        {
            get
            {
                return workSheets;
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        public void AddWorkSheet(WorkSheet workSheet)
        {
            workSheet.Parent = this;
            workSheets.Add(workSheet);
        }

        private void InitHeader()
        {
            writer.Write("<?xml version=\"1.0\"?>");
            writer.Write("<?mso-application progid=\"Excel.Sheet\"?>");
            writer.Write("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
            //文件属性
            writer.Write(string.Format(@"<DocumentProperties xmlns={0}urn:schemas-microsoft-com:office:office{0}>
                                            <Author>{1}</Author>
                                            <Company>{2}</Company>
                                            <Created>{3}</Created>
                                            <Version>11.5606</Version>
                                        </DocumentProperties>", "\"", Author, Company, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")));

            writer.Write(string.Format(@"<OfficeDocumentSettings xmlns={0}urn:schemas-microsoft-com:office:office{0}>
                                            <RemovePersonalInformation/>
                                        </OfficeDocumentSettings>", "\""));
            //Excel大小位置
            writer.Write(string.Format(@"<ExcelWorkbook xmlns={0}urn:schemas-microsoft-com:office:excel{0}>
                                            <WindowHeight>4530</WindowHeight>
                                            <WindowWidth>8505</WindowWidth>
                                            <WindowTopX>480</WindowTopX>
                                            <WindowTopY>120</WindowTopY>
                                            <ActiveSheet>0</ActiveSheet>
                                            <AcceptLabelsInFormulas />
                                            <ProtectStructure>False</ProtectStructure>
                                            <ProtectWindows>False</ProtectWindows>
                                        </ExcelWorkbook>", "\""));
            //格式
            writer.Write(string.Format(@"<Styles>
                                            <Style ss:ID={0}Default{0} ss:Name={0}Normal{0}>
                                                <Alignment ss:Vertical={0}Bottom{0}/>
                                                <Borders/>
                                                <Font ss:FontName={0}宋体{0} x:CharSet={0}134{0} ss:Size={0}12{0}/>
                                                <Interior/>
                                                <NumberFormat/>
                                                <Protection/>
                                            </Style>
                                            <Style ss:ID={0}s21{0}>
                                                <NumberFormat ss:Format={0}@{0}/>
                                            </Style>
                                            <Style ss:ID={0}s22{0}>
                                                <NumberFormat ss:Format={0}0.00_ {0}/>
                                            </Style>
                                            <Style ss:ID={0}s23{0}>
                                                <Alignment ss:Horizontal={0}Center{0} ss:Vertical={0}Center{0}/>
                                                <Font ss:FontName={0}宋体{0} x:CharSet={0}134{0} ss:Size={0}12{0} ss:Color={0}#FF0000{0} ss:Bold={0}1{0}/>
                                            </Style>
                                            <Style ss:ID={0}s24{0}>
                                                <NumberFormat ss:Format={0}yyyy/m/d;@{0}/>
                                            </Style>
                                        </Styles>", "\""));
        }

        private void InitFooter()
        {
            writer.Write("</Workbook>");
        }

        /// <summary>
        /// 下载Excel文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Download(string fileName)
        {
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpContext.Current.Server.UrlEncode(fileName)));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            if (formatType == ExcelFormat.Xml)
            {
                writer = new StreamWriter(HttpContext.Current.Response.OutputStream);
                ExcelProcess();
                //HttpContext.Current.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (formatType == ExcelFormat.Binary)
            {
                Random random = new Random(DateTime.Now.Millisecond);
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1, 9999).ToString("D4") + fileName;
                byte[] buffer = BiffExcelProcess();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Expires = 0;
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Accept-Language", "zh-tw");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                HttpContext.Current.Response.ContentType = "application/octet-stream;charset=gbk";
                HttpContext.Current.Response.BinaryWrite(buffer);
                //HttpContext.Current.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private void ExcelProcess()
        {
            InitHeader();
            foreach (WorkSheet workSheet in workSheets)
            {
                workSheet.InitHeader();
                workSheet.InitRows();
                workSheet.InitFooter();
            }
            InitFooter();
            writer.Flush();
            writer.Close();
        }


        // Excel 转为 二进制
        private byte[] BiffExcelProcess()
        {
            Biff8Excel.Excel.ExcelWorkbook wbook;
            Biff8Excel.Excel.ExcelWorksheet wsheet;
            wbook = new Biff8Excel.Excel.ExcelWorkbook();
            wbook.SetDefaultFont("Arial", 10);     // default to Arial,10

            string content = "";
            Biff8Excel.Excel.ExcelCellStyle style = wbook.CreateStyle();
            style.Font.Size = 13;
            style.Font.Name = "Tahoma";
            style.Font.Bold = true;
            style.Font.Colour = EnumColours.Red;
            style.PatternBackColour = EnumColours.Red;
            foreach (WorkSheet workSheet in workSheets)
            {
                wbook.CreateSheet(workSheet.SheetName);
                wsheet = wbook.GetSheet(workSheet.SheetName);
                wbook.SetActiveSheet = workSheet.SheetName;
                int columnsCount = 1;
                foreach (WorkSheetColumn column in workSheet.Columns)
                {
                    wsheet.AddCell((ushort)columnsCount, 1, column.ColumnName, style);
                    columnsCount++;
                }
                style = wbook.CreateStyle();
                style.Font.Size = 12;
                for (int i = 0; i < workSheet.rowsCount; i++)
                {
                    for (int j = 0; j < workSheet.columns.Count; j++)
                    {
                        if (workSheet.rows[i][j] is decimal)
                        {
                            wsheet.AddCell((ushort)(j + 1), (ushort)(i + 2), workSheet.rows[i][j], style);
                        }
                        else
                        {
                            content = workSheet.rows[i][j].ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("&", "&amp;");
                            wsheet.AddCell((ushort)(j + 1), (ushort)(i + 2), content, style);
                        }
                    }
                }
            }

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDirectory[baseDirectory.Length - 1] == '\\')
            {
                baseDirectory = baseDirectory.Remove(baseDirectory.Length - 1);
            }
            string filePath = string.Format("{0}\\Temp", baseDirectory);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            Random random = new Random(DateTime.Now.Millisecond);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1, 9999).ToString("D4") + ".xls";
            filePath = filePath + string.Format("\\{0}", fileName);
            wbook.Save(filePath);
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            File.Delete(filePath);
            return buffer;
        }
        /// <summary>
        /// 保存Excel文件到服务器磁盘
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            Stream stream = new FileStream(fileName, FileMode.Create);
            writer = new StreamWriter(stream);
            ExcelProcess();
            if (formatType == ExcelFormat.Binary)
            {
                ConvertStandardExcel(fileName);
            }
        }

        /// <summary>
        /// 获取Excel文件的二进制流
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            if (formatType == ExcelFormat.Binary)
            {
                //throw new NotSupportedException("该方法不支持二进制格式的Excel文件");
                Random random = new Random(DateTime.Now.Millisecond);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1, 9999).ToString("D4") + ".xls";
                byte[] buffer = BiffExcelProcess();
                return buffer;
            }
            else
            {
                using (Stream stream = new MemoryStream())
                {
                    writer = new StreamWriter(stream);
                    InitHeader();
                    foreach (WorkSheet workSheet in workSheets)
                    {
                        workSheet.InitHeader();
                        workSheet.InitRows();
                        workSheet.InitFooter();
                    }
                    InitFooter();
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
        }

        public void Dispose()
        {
            writer.Close();
        }

        private void ConvertStandardExcel(string fileName)
        {
            //将xml文件转换为标准的Excel格式 
            Object nothing = Missing.Value;//由于用COM组件很多值需要用Missing.Value代替   
            Microsoft.Office.Interop.Excel.Application exclApp = new Microsoft.Office.Interop.Excel.ApplicationClass();// 初始化
            Microsoft.Office.Interop.Excel.Workbook exclDoc = exclApp.Workbooks.Open(fileName, nothing, nothing, nothing, nothing, nothing, nothing, nothing, nothing, nothing, nothing, nothing, nothing, nothing, nothing);//打开Excl工作薄   
            Object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal;//获取Excl 2007文件格式   
            exclApp.DisplayAlerts = false;
            exclDoc.SaveAs(fileName, format, nothing, nothing, nothing, nothing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, nothing, nothing, nothing, nothing, nothing);//保存为Excl 2007格式   
            exclDoc.Close(nothing, nothing, nothing);
            KillSpecialExcel(exclApp);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        /// <summary>
        /// 强制退出Excel进程
        /// </summary>
        private void KillSpecialExcel(Microsoft.Office.Interop.Excel.Application exclApp)
        {
            try
            {
                if (exclApp != null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId(new IntPtr(exclApp.Hwnd), out lpdwProcessId);
                    System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                }
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn(this, ex);
            }
        }
    }
}