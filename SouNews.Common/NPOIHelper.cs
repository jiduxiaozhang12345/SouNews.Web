using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace SouNews.Common {
    /// <summary>
    /// Excel操作
    /// </summary>
    public class NpoiHelper {
        #region DataTable To Excel

        /// <summary>   
        /// DataTable导出到Excel文件   
        /// </summary>   
        /// <param name="dtSource">源DataTable</param>   
        /// <param name="strHeaderText">表头文本</param>   
        /// <param name="strFileName">保存位置</param>
        /// <param name="strSheetName">工作表名称</param>
        /// <param name="oldColumnNames">被替换的列名</param>
        /// <param name="newColumnNames">新列名</param>
        public static void DataTableToExcel(DataTable dtSource, string strFileName,
            string[] oldColumnNames, string[] newColumnNames, string strHeaderText = null, string strSheetName = null) {
            using (
                MemoryStream ms = DataTableToExcel(dtSource, oldColumnNames, newColumnNames, strHeaderText, strSheetName)
                ) {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write)) {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>   
        /// DataTable导出到Excel文件
        /// </summary>   
        /// <param name="dtSource">源DataTable</param>   
        /// <param name="strHeaderText">表头文本</param>   
        /// <param name="strSheetName">工作表名称</param>
        /// <param name="oldColumnNames"></param>
        /// <param name="newColumnNames"></param> 
        public static MemoryStream DataTableToExcel(DataTable dtSource,
            string[] oldColumnNames, string[] newColumnNames, string strHeaderText = null, string strSheetName = null) {
            if (oldColumnNames.Length != newColumnNames.Length) {
                return new MemoryStream();
            }
            if (string.IsNullOrWhiteSpace(strSheetName)) {
                strSheetName = "Sheet";
            }
            if (string.IsNullOrWhiteSpace(strHeaderText)) {
                strHeaderText = string.Empty;
            }
            HSSFWorkbook workbook = new HSSFWorkbook();
            //HSSFSheet sheet = workbook.CreateSheet();// workbook.CreateSheet();
            ISheet sheet = workbook.CreateSheet(strSheetName);

            #region 右击文件 属性信息

                {
                    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                    dsi.Company = "http://....../";
                    workbook.DocumentSummaryInformation = dsi;

                    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                    if (HttpContext.Current.Session["realname"] != null) {
                        si.Author = HttpContext.Current.Session["realname"].ToString();
                    }
                    else {
                        if (HttpContext.Current.Session["username"] != null) {
                            si.Author = HttpContext.Current.Session["username"].ToString();
                        }
                    } //填加xls文件作者信息   
                    si.ApplicationName = "NPOI"; //填加xls文件创建程序信息   
                    si.LastAuthor = ""; //填加xls文件最后保存者信息   
                    si.Comments = ""; //填加xls文件作者信息   
                    si.Title = strHeaderText; //填加xls文件标题信息   
                    si.Subject = strHeaderText; //填加文件主题信息   
                    si.CreateDateTime = DateTime.Now;
                    workbook.SummaryInformation = si;
                }

            #endregion

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            #region 取得列宽

            int[] arrColWidth = new int[oldColumnNames.Length];
            for (int i = 0; i < oldColumnNames.Length; i++) {
                arrColWidth[i] = Encoding.GetEncoding(936).GetBytes(newColumnNames[i]).Length;
            }
            /*
        foreach (DataColumn item in dtSource.Columns)
        {
            arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
        }
         * */

            for (int i = 0; i < dtSource.Rows.Count; i++) {
                for (int j = 0; j < oldColumnNames.Length; j++) {
                    int intTemp =
                        Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][oldColumnNames[j]].ToString()).Length;
                    if (intTemp > arrColWidth[j]) {
                        arrColWidth[j] = intTemp;
                    }
                }
                /*
            for (int j = 0; j < dtSource.Columns.Count; j++)
            {
                int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                if (intTemp > arrColWidth[j])
                {
                    arrColWidth[j] = intTemp;
                }
            }
             * */
            }

            #endregion

            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows) {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0) {
                    if (rowIndex != 0) {
                        sheet = workbook.CreateSheet(strSheetName + (rowIndex/65535).ToString());
                    }
                    rowIndex = 0;

                    #region 表头及样式

                    if (!string.IsNullOrWhiteSpace(strHeaderText)) {
                        IRow headerRow = sheet.CreateRow(rowIndex);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                        rowIndex++;
                    }

                    #endregion

                    #region 列头及样式

                    {
                        //HSSFRow headerRow = sheet.CreateRow(1);
                        IRow headerRow = sheet.CreateRow(rowIndex);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        for (int i = 0; i < oldColumnNames.Length; i++) {
                            headerRow.CreateCell(i).SetCellValue(newColumnNames[i]);
                            headerRow.GetCell(i).CellStyle = headStyle;
                            //设置列宽
                            sheet.SetColumnWidth(i, (arrColWidth[i] + 1)*256);
                        }
                        /*
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                        //设置列宽   
                        sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                    }
                     * */
                    }

                    #endregion

                    rowIndex++;
                }

                #endregion

                #region 填充内容

                IRow dataRow = sheet.CreateRow(rowIndex);
                //foreach (DataColumn column in dtSource.Columns)
                for (int i = 0; i < oldColumnNames.Length; i++) {
                    ICell newCell = dataRow.CreateCell(i);

                    string drValue = row[oldColumnNames[i]].ToString();

                    switch (dtSource.Columns[oldColumnNames[i]].DataType.ToString()) {
                        case "System.String": //字符串类型   
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime": //日期类型   
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示   
                            break;
                        case "System.Boolean": //布尔型   
                            bool boolV;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型   
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            var intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型   
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理   
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }

                #endregion

                rowIndex++;
            }


            using (MemoryStream ms = new MemoryStream()) {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                sheet = null;
                workbook = null;
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet   
                return ms;
            }
        }

        /// <summary>
        ///  Web DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">输出文件名</param>
        /// <param name="strSheetName">工作表名称</param>
        public static void DataTableToExcelForWeb(DataTable dtSource, string strFileName
            , string strHeaderText = null, string strSheetName = null) {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式   
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            //生成列
            string columns = "";
            for (int i = 0; i < dtSource.Columns.Count; i++) {
                if (i > 0) {
                    columns += ",";
                }
                columns += dtSource.Columns[i].ColumnName;
            }

            curContext.Response.BinaryWrite(
                DataTableToExcel(dtSource, columns.Split(','), columns.Split(','), strHeaderText, strSheetName)
                    .GetBuffer());
            curContext.Response.Flush();
            curContext.Response.End();
        }

        /// <summary>
        ///  Web DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">要导出的DataTable</param>
        /// <param name="strHeaderText">标题文字</param>
        /// <param name="strFileName">文件名，包含扩展名</param>
        /// <param name="strSheetName">工作表名</param>
        /// <param name="oldColumnNames">要导出的DataTable列数组</param>
        /// <param name="newColumnNames">导出后的对应列名</param>
        public static void DataTableToExcelForWeb(DataTable dtSource, string strFileName,
            string[] oldColumnNames, string[] newColumnNames, string strHeaderText = null, string strSheetName = null) {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式   
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(
                DataTableToExcel(dtSource, oldColumnNames, newColumnNames, strHeaderText, strSheetName).GetBuffer());
            curContext.Response.End();
        }

        #endregion

        #region List<T> To Excel

        /// <summary>   
        /// List导出到Excel文件
        /// </summary>   
        /// <param name="dtSource">源DataTable</param>   
        /// <param name="strHeaderText">表头文本</param>   
        /// <param name="strFileName">保存位置</param>
        /// <param name="strSheetName">工作表名称</param>
        /// <param name="oldColumnNames"></param>
        /// <param name="newColumnNames"></param>
        public static void ListToExcel<T>(IList<T> dtSource, string strFileName,
            string[] oldColumnNames, string[] newColumnNames, string strHeaderText = null,
            string strSheetName = null) {
            DataTableToExcel(DataTableHelper.ConvertTo(dtSource), strFileName,
                oldColumnNames,
                newColumnNames, strHeaderText, strSheetName);
        }

        /// <summary>   
        /// List导出到Excel文件
        /// </summary>   
        /// <param name="dtSource">源DataTable</param>   
        /// <param name="strHeaderText">表头文本</param>   
        /// <param name="strSheetName">工作表名称</param>
        /// <param name="oldColumnNames"></param>
        /// <param name="newColumnNames"></param> 
        public static MemoryStream ListToExcel<T>(IList<T> dtSource,
            string[] oldColumnNames, string[] newColumnNames, string strHeaderText = null, string strSheetName = null) {
            return DataTableToExcel(DataTableHelper.ConvertTo(dtSource), oldColumnNames,
                newColumnNames, strHeaderText, strSheetName);
        }

        /// <summary>
        ///  Web  List导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">输出文件名</param>
        /// <param name="strSheetName">工作表名称</param>
        public static void ListToExcelForWeb<T>(IList<T> dtSource, string strFileName, string strHeaderText = null,
            string strSheetName = null) {
            DataTableToExcelForWeb(DataTableHelper.ConvertTo(dtSource), strFileName, strHeaderText, strSheetName);
        }

        /// <summary>
        /// Web List导出到Excel文件
        /// </summary>
        /// <param name="dtSource">要导出的DataTable</param>
        /// <param name="strHeaderText">标题文字</param>
        /// <param name="strFileName">文件名，包含扩展名</param>
        /// <param name="strSheetName">工作表名</param>
        /// <param name="oldColumnNames">要导出的DataTable列数组</param>
        /// <param name="newColumnNames">导出后的对应列名</param>
        public static void ListToExcelForWeb<T>(IList<T> dtSource, string strFileName, string[] oldColumnNames,
            string[] newColumnNames, string strHeaderText = null,
            string strSheetName = null) {
            DataTableToExcelForWeb(DataTableHelper.ConvertTo(dtSource), strFileName,
                oldColumnNames,
                newColumnNames, strHeaderText, strSheetName);
        }

        #endregion

        #region Excel To Table

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string strFileName, string sheetName = null, int? headerRowIndex = null) {
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read)) {
                IWorkbook workbook = new HSSFWorkbook(file);
                return ExcelToDataTable(workbook, sheetName, headerRowIndex);
            }
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(Stream excelFileStream, string sheetName = null,
            int? headerRowIndex = null) {
            IWorkbook workbook = new HSSFWorkbook(excelFileStream);
            excelFileStream.Close();
            return ExcelToDataTable(workbook, sheetName, headerRowIndex);
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="workbook">要处理的工作薄</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(IWorkbook workbook, string sheetName = null, int? headerRowIndex = null) {
            if (string.IsNullOrWhiteSpace(sheetName)) {
                sheetName = workbook.GetSheetName(0);
            }

            if (headerRowIndex == null) {
                headerRowIndex = 0;
            }

            ISheet sheet = workbook.GetSheet(sheetName);
            DataTable table = new DataTable();
            try {
                IRow headerRow = sheet.GetRow(headerRowIndex.Value);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++) {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;

                #region 循环各行各列,写入数据到DataTable

                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++) {
                    IRow row = sheet.GetRow(i);
                    if (row == null) {
                        continue;
                    }
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++) {
                        ICell cell = row.GetCell(j);
                        if (cell == null) {
                            dataRow[j] = null;
                        }
                        else {
                            //dataRow[j] = cell.ToString();
                            switch (cell.CellType) {
                                case CellType.Blank:
                                    dataRow[j] = null;
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                    dataRow[j] = cell.ToString();
                                    break;
                                case CellType.String:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                                case CellType.Error:
                                    dataRow[j] = cell.ErrorCellValue;
                                    break;
                                case CellType.Formula:
                                default:
                                    dataRow[j] = "=" + cell.CellFormula;
                                    break;
                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                    //dataRow[j] = row.GetCell(j).ToString();
                }

                #endregion
            }
            catch (Exception ex) {
                table.Clear();
                table.Columns.Clear();
                table.Columns.Add("出错了");
                DataRow dr = table.NewRow();
                dr[0] = ex.Message;
                table.Rows.Add(dr);
                return table;
            }
            finally {
                //sheet.Dispose();
                workbook = null;
                sheet = null;
            }

            #region 清除最后的空行

            for (int i = table.Rows.Count - 1; i > 0; i--) {
                bool isnull = true;
                for (int j = 0; j < table.Columns.Count; j++) {
                    if (table.Rows[i][j] != null) {
                        if (table.Rows[i][j].ToString() != "") {
                            isnull = false;
                            break;
                        }
                    }
                }
                if (isnull) {
                    table.Rows[i].Delete();
                }
            }

            #endregion

            return table;
        }

        #endregion

        #region Excel To List<T>

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static IList<T> ExcelToList<T>(string strFileName, string sheetName = null, int? headerRowIndex = null) {
            return DataTableHelper.ConvertTo<T>(ExcelToDataTable(strFileName, sheetName, headerRowIndex));
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="oldColumnNames">被替换 table 列表</param>   
        /// <param name="newColumnNames">新table 列名</param>   
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static IList<T> ExcelToList<T>(string strFileName, string[] oldColumnNames, string[] newColumnNames,
            string sheetName = null, int? headerRowIndex = null) {
            var table = ExcelToDataTable(strFileName, sheetName, headerRowIndex);
            return DataTableHelper.ConvertTo<T>(ConvertColumnNames(table, oldColumnNames, newColumnNames));
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static IList<T> ExcelToList<T>(Stream excelFileStream, string sheetName = null,
            int? headerRowIndex = null) {
            return DataTableHelper.ConvertTo<T>(ExcelToDataTable(excelFileStream, sheetName, headerRowIndex));
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="oldColumnNames">被替换 table 列表</param>   
        /// <param name="newColumnNames">新table 列名</param>   
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static IList<T> ExcelToList<T>(Stream excelFileStream, string[] oldColumnNames, string[] newColumnNames,
            string sheetName = null, int? headerRowIndex = null) {
            var table = ExcelToDataTable(excelFileStream, sheetName, headerRowIndex);
            return DataTableHelper.ConvertTo<T>(ConvertColumnNames(table, oldColumnNames, newColumnNames));
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="workbook">要处理的工作薄</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static IList<T> ExcelToList<T>(IWorkbook workbook, string sheetName = null, int? headerRowIndex = null) {
            return DataTableHelper.ConvertTo<T>(ExcelToDataTable(workbook, sheetName, headerRowIndex));
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="workbook">要处理的工作薄</param>
        /// <param name="oldColumnNames">被替换 table 列表</param>   
        /// <param name="newColumnNames">新table 列名</param>   
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static IList<T> ExcelToList<T>(IWorkbook workbook, string[] oldColumnNames, string[] newColumnNames,
            string sheetName = null, int? headerRowIndex = null) {
            var table = ExcelToDataTable(workbook, sheetName, headerRowIndex);
            return DataTableHelper.ConvertTo<T>(ConvertColumnNames(table, oldColumnNames, newColumnNames));
        }

        private static DataTable ConvertColumnNames(DataTable dataTable, string[] oldColumnNames,
            string[] newColumnNames) {
            if (oldColumnNames.Length != newColumnNames.Length) {
                throw new Exception("列名不一致");
            }
            var oldList = oldColumnNames.ToList();
            foreach (DataColumn col in dataTable.Columns) {
                col.ColumnName = newColumnNames[oldList.IndexOf(col.ColumnName)];
            }
            return dataTable;
        }

        #endregion
    }
}