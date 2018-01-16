using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model.OutExcel
{
    public class OutExcel
    {

        private string RetHtmlHeard() {
            string htmlHeard = "<html><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />" + "<style type='text/css'>tbody tr td{text-align:center;border:solid .5pt Black; } tfoot tr td{text-align:center;border:solid .5pt Black;} tr{ height:23.5px} .xh{width:30px;} .bai_{width:69px;} .wu_{width:45.5px;} .liu_{width:53.5px;} .qi_{width:61px;} .jiu_{width:77.5px;} </style>" + "<body><table >";
            return htmlHeard;
        }
        public void outss(){
            #region
            //string excelHtml = context.Request["OFtable"];
            //string bxguishu = context.Request["bxguishu"] == null ? "总汇" : context.Request["bxguishu"].ToString();
            //string bms = context.Request["bumen"].ToString(); ;
            //context.Response.Buffer = true;
            ////输出的应用类型 
            //context.Response.ContentType = "application/vnd.ms-excel";
            ////context.Response.Charset = "gb2312";
            //bms = bms == "" ? "总汇表" : bms;
            ////filenames是自定义的文件名
            //context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + bms + "（" + bxguishu + "）" + ".xls");
            ////设定编码方式，若输出的excel有乱码，可优先从编码方面解决
            //context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //context.Response.Charset = "utf-8";

            ////content是步骤1的html，注意是string类型
            //context.Response.Write(excelHtml);
            //context.Response.End();
            #endregion
        }


        public  ICellStyle Cellstyle() {

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            IFont font = book.CreateFont();
            font.FontName = "宋体";
            //font.setFontHeightInPoints((short)10);
            font.FontHeightInPoints = 10;
            // 普通单元格样式  
            ICellStyle style = book.CreateCellStyle();
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

            return style;
        }
        

    }
}
