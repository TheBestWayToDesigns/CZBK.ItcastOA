using CZBK.ItcastOA.Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {
            int a = 2;
            int b = 0;
            int c = a / b;
            return Content(c.ToString());
        }

   
        public ActionResult excelPrint()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();// 创建一个Excel文件  
           // HSSFSheet sheet = workbook.CreateSheet();// 创建一个Excel的Sheet 
            ISheet sheet = workbook.CreateSheet();
            sheet.CreateFreezePane(1, 3);// 冻结  
                                         // 设置列宽  
            sheet.SetColumnWidth(0, 1000);
            sheet.SetColumnWidth(1, 3500);
            sheet.SetColumnWidth(2, 3500);
            sheet.SetColumnWidth(3, 6500);
            sheet.SetColumnWidth(4, 6500);
            sheet.SetColumnWidth(5, 6500);
            sheet.SetColumnWidth(6, 6500);
            sheet.SetColumnWidth(7, 2500);
            // Sheet样式  
            ICellStyle sheetStyle = workbook.CreateCellStyle();
            // 背景色的设定  
            HSSFPalette XlPalette = workbook.GetCustomPalette();
           // sheetStyle.FillBackgroundColor = 0;
            sheetStyle.FillPattern = FillPattern.FineDots;
           // sheetStyle.setFillBackgroundColor(HSSFColor.GREY_25_PERCENT.index);
           // 前景色的设定  
           // sheetStyle.setFillForegroundColor(HSSFColor.GREY_25_PERCENT.index);
           // 填充模式  
           // sheetStyle.setFillPattern(HSSFCellStyle.Equals)
            // 设置列的样式  
            for (int i = 0; i <= 14; i++)
            {
                sheet.SetDefaultColumnStyle((short)i, sheetStyle);
            }
            // 设置字体  
            IFont headfont = workbook.CreateFont(); 
            //headfont.setFontName("黑体");
            //headfont.setFontHeightInPoints((short)22);// 字体大小  
            //headfont.setBoldweight(HSSFFont.BOLDWEIGHT_BOLD);// 加粗  
            headfont.FontName = "黑体";
            headfont.FontHeightInPoints = (short)22;
            headfont.Boldweight = (short)10;
            headfont.IsBold = true;
            // 另一个样式  
            ICellStyle headstyle = workbook.CreateCellStyle();
            //headstyle.setFont(headfont);
            //headstyle.setAlignment(HSSFCellStyle.ALIGN_CENTER);// 左右居中  
            //headstyle.setVerticalAlignment(HSSFCellStyle.VERTICAL_CENTER);// 上下居中  
            //headstyle.setLocked(true);
            //headstyle.setWrapText(true);// 自动换行  
            headstyle.SetFont(headfont);
            headstyle.Alignment = HorizontalAlignment.Center;
            headstyle.VerticalAlignment = VerticalAlignment.Center;
            headstyle.IsLocked = true;
            headstyle.WrapText = true;
                                        // 另一个字体样式  
            IFont columnHeadFont = workbook.CreateFont();
            //columnHeadFont.setFontName("宋体");
            //columnHeadFont.setFontHeightInPoints((short)10);
            //columnHeadFont.setBoldweight(HSSFFont.BOLDWEIGHT_BOLD);
            columnHeadFont.FontName = "宋体";
            columnHeadFont.FontHeightInPoints = (short)10;
            columnHeadFont.Boldweight = (short)10;
            // 列头的样式  
            ICellStyle columnHeadStyle = workbook.CreateCellStyle(); 
            //columnHeadStyle.setFont(columnHeadFont);
            //columnHeadStyle.setAlignment(HSSFCellStyle.ALIGN_CENTER);// 左右居中  
            //columnHeadStyle.setVerticalAlignment(HSSFCellStyle.VERTICAL_CENTER);// 上下居中  
            //columnHeadStyle.setLocked(true);
            //columnHeadStyle.setWrapText(true);
            //columnHeadStyle.setLeftBorderColor(HSSFColor.BLACK.index);// 左边框的颜色  
            //columnHeadStyle.setBorderLeft((short)1);// 边框的大小  
            //columnHeadStyle.setRightBorderColor(HSSFColor.BLACK.index);// 右边框的颜色  
            //columnHeadStyle.setBorderRight((short)1);// 边框的大小  
            //columnHeadStyle.setBorderBottom(HSSFCellStyle.BORDER_THIN); // 设置单元格的边框为粗体  
            //columnHeadStyle.setBottomBorderColor(HSSFColor.BLACK.index); // 设置单元格的边框颜色  
            columnHeadStyle.SetFont(columnHeadFont);
            columnHeadStyle.Alignment = HorizontalAlignment.Center;
            columnHeadStyle.VerticalAlignment = VerticalAlignment.Center;
            columnHeadStyle.IsLocked = true;
            columnHeadStyle.WrapText = true;
            columnHeadStyle.LeftBorderColor = HSSFColor.Black.Index;
            columnHeadStyle.BorderLeft = BorderStyle.Thin ;
            columnHeadStyle.RightBorderColor = HSSFColor.Black.Index;
            columnHeadStyle.BorderRight = BorderStyle.Thin;

            // 设置单元格的背景颜色（单元格的样式会覆盖列或行的样式）  
            columnHeadStyle.FillForegroundColor=HSSFColor.White.Index;

            IFont font = workbook.CreateFont();
            font.FontName="宋体";
            //font.setFontHeightInPoints((short)10);
            font.FontHeightInPoints = 10;
            // 普通单元格样式  
            ICellStyle style = workbook.CreateCellStyle();
            style.SetFont(font);
            //style.setAlignment(HSSFCellStyle.ALIGN_LEFT);// 左右居中  
            //style.setVerticalAlignment(HSSFCellStyle.VERTICAL_TOP);// 上下居中  
            //style.setWrapText(true);
            //style.setLeftBorderColor(HSSFColor.BLACK.index);
            //style.setBorderLeft((short)1);
            //style.setRightBorderColor(HSSFColor.BLACK.index);
            //style.setBorderRight((short)1);
            //style.setBorderBottom(HSSFCellStyle.BORDER_THIN); // 设置单元格的边框为粗体  
            //style.setBottomBorderColor(HSSFColor.BLACK.index); // 设置单元格的边框颜色．  
            //style.setFillForegroundColor(HSSFColor.WHITE.index);// 设置单元格的背景颜色．  
            // 另一个样式  
            ICellStyle centerstyle = workbook.CreateCellStyle();
            centerstyle.SetFont(font);
            centerstyle.Alignment=HorizontalAlignment.Center;// 左右居中  
            centerstyle.VerticalAlignment=VerticalAlignment.Center;// 上下居中  
            centerstyle.WrapText=true;
            //centerstyle.setLeftBorderColor(HSSFColor.BLACK.index);
            //centerstyle.setBorderLeft((short)1);
            //centerstyle.setRightBorderColor(HSSFColor.BLACK.index);
            //centerstyle.setBorderRight((short)1);
            //centerstyle.setBorderBottom(HSSFCellStyle.BORDER_THIN); // 设置单元格的边框为粗体  
            //centerstyle.setBottomBorderColor(HSSFColor.BLACK.index); // 设置单元格的边框颜色．  
            //centerstyle.setFillForegroundColor(HSSFColor.WHITE.index);// 设置单元格的背景颜色．  

            try
            {
                // 创建第一行  
                IRow row0 = sheet.CreateRow(0);
                // 设置行高  
                row0.Height = 900;
                //row0.setHeight((short)900);
                // 创建第一列  
                ICell cell0 = row0.CreateCell(0);
                cell0.SetCellValue(new HSSFRichTextString("中非发展基金投资项目调度会工作落实情况对照表"));
                cell0.CellStyle = headstyle;
                //cell0.SetCellStyle()=headstyle;
                /** 
                 * 合并单元格 
                 *    第一个参数：第一个单元格的行数（从0开始） 
                 *    第二个参数：第二个单元格的行数（从0开始） 
                 *    第三个参数：第一个单元格的列数（从0开始） 
                 *    第四个参数：第二个单元格的列数（从0开始） 
                 */
                CellRangeAddress range = new CellRangeAddress(0, 0, 0, 7);
                sheet.AddMergedRegion(range);
                // 创建第二行  
                IRow row1 = sheet.CreateRow(1);
                ICell cell1 = row1.CreateCell(0);
                cell1.SetCellValue(new HSSFRichTextString("本次会议时间：2009年8月31日       前次会议时间：2009年8月24日"));
                //cell1.SetCellStyle(centerstyle);
                cell1.CellStyle = centerstyle;
                // 合并单元格  
                range = new CellRangeAddress(1, 2, 0, 7);
                sheet.AddMergedRegion(range);
                // 第三行  
                IRow row2 = sheet.CreateRow(3);
                //row2.setHeight((short)750);
                row2.Height = 750;
                ICell cell = row2.CreateCell(0);
                cell.SetCellValue(new HSSFRichTextString("责任者"));
                cell.CellStyle=columnHeadStyle;
                cell = row2.CreateCell(1);
                cell.SetCellValue(new HSSFRichTextString("成熟度排序"));
                cell.CellStyle = columnHeadStyle;
                cell = row2.CreateCell(2);
                cell.SetCellValue(new HSSFRichTextString("事项"));
                cell.CellStyle = columnHeadStyle;
                cell = row2.CreateCell(3);
                cell.SetCellValue(new HSSFRichTextString("前次会议要求\n/新项目的项目概要"));
                cell.CellStyle = columnHeadStyle;
                cell = row2.CreateCell(4);
                cell.SetCellValue(new HSSFRichTextString("上周工作进展"));
                cell.CellStyle = columnHeadStyle;
                cell = row2.CreateCell(5);
                cell.SetCellValue(new HSSFRichTextString("本周工作计划"));
                cell.CellStyle = columnHeadStyle;
                cell = row2.CreateCell(6);
                cell.SetCellValue(new HSSFRichTextString("问题和建议"));
                cell.CellStyle = columnHeadStyle;
                cell = row2.CreateCell(7);
                cell.SetCellValue(new HSSFRichTextString("备 注"));
                cell.CellStyle = columnHeadStyle;
                // 访问数据库，得到数据集  
                List<UserInfo> deitelVOList = null;
                //int m = 4;
                //int k = 4;
                for (int i = 0; i < deitelVOList.Count(); i++)
                {
                }
                   // UserInfo vo = deitelVOList[i];
                    //String dname = vo.PerSonName;
                    //List<Workinfo> workList = vo.getWorkInfoList();
                    //IRow row = sheet.CreateRow(m);
                    //cell = row.createCell(0);
                    //cell.SetCellValue(new HSSFRichTextString(dname));
                    //cell.CellStyle=centerstyle;
                    //// 合并单元格  
                    //range = new CellRangeAddress(m, m + workList.size() - 1, 0, 0);
                    //sheet.addMergedRegion(range);
                    //m = m + workList.size();

                //    for (int j = 0; j < workList.size(); j++)
                //    {
                //        Workinfo w = workList.get(j);
                //        // 遍历数据集创建Excel的行  
                //        row = sheet.getRow(k + j);
                //        if (null == row)
                //        {
                //            row = sheet.createRow(k + j);
                //        }
                //        cell = row.createCell(1);
                //        cell.setCellValue(w.getWnumber());
                //        cell.setCellStyle(centerstyle);
                //        cell = row.createCell(2);
                //        cell.setCellValue(new HSSFRichTextString(w.getWitem()));
                //        cell.setCellStyle(style);
                //        cell = row.createCell(3);
                //        cell.setCellValue(new HSSFRichTextString(w.getWmeting()));
                //        cell.setCellStyle(style);
                //        cell = row.createCell(4);
                //        cell.setCellValue(new HSSFRichTextString(w.getWbweek()));
                //        cell.setCellStyle(style);
                //        cell = row.createCell(5);
                //        cell.setCellValue(new HSSFRichTextString(w.getWtweek()));
                //        cell.setCellStyle(style);
                //        cell = row.createCell(6);
                //        cell.setCellValue(new HSSFRichTextString(w.getWproblem()));
                //        cell.setCellStyle(style);
                //        cell = row.createCell(7);
                //        cell.setCellValue(new HSSFRichTextString(w.getWremark()));
                //        cell.setCellStyle(style);
                //    }
                //    k = k + workList.size();
                //}
                //// 列尾  
                //int footRownumber = sheet.getLastRowNum();
                //HSSFRow footRow = sheet.createRow(footRownumber + 1);
                //HSSFCell footRowcell = footRow.createCell(0);
                //footRowcell.setCellValue(new HSSFRichTextString("                    审  定：XXX      审  核：XXX     汇  总：XX"));
                //footRowcell.setCellStyle(centerstyle);
                //range = new CellRangeAddress(footRownumber + 1, footRownumber + 1, 0, 7);
                //sheet.addMergedRegion(range);

                //HttpServletResponse response = getResponse();
                //HttpServletRequest request = getRequest();
                //String filename = "未命名.xls";//设置下载时客户端Excel的名称  
                //                            // 请见：http://zmx.iteye.com/blog/622529  
                //filename = Util.encodeFilename(filename, request);
                //response.setContentType("application/vnd.ms-excel");
                //response.setHeader("Content-disposition", "attachment;filename=" + filename);
                //OutputStream ouputStream = response.getOutputStream();
                //workbook.write(ouputStream);
                //ouputStream.flush();
                //ouputStream.close();

            }
            catch (Exception e)
            {
                string s=e.ToString();
            }
            return null;
        }

    }
}
