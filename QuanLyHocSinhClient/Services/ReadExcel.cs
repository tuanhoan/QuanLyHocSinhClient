using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using OfficeOpenXml;


namespace QuanLyHocSinhClient.Services
{
    public class ReadExcel
    {
        public async Task<T> ReadFromExcel<T>(IBrowserFile file, bool hasHeader = true)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            MemoryStream ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); 
            using (var excelPack = new ExcelPackage())
            {
                //Load excel stream
                await excelPack.LoadAsync(ms);

                //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                var ws = excelPack.Workbook.Worksheets[0];

                //Get all details as DataTable -because Datatable make life easy :)
                DataTable excelasTable = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    //Get colummn details
                    if (!string.IsNullOrEmpty(firstRowCell.Text))
                    {
                        string firstColumn = string.Format("Column {0}", firstRowCell.Start.Column);
                        excelasTable.Columns.Add(hasHeader ? firstRowCell.Text : firstColumn);
                    }
                }
                var startRow = hasHeader ? 2 : 1;
                //Get row details
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
                    DataRow row = excelasTable.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                //Get everything as generics and let end user decides on casting to required type
                var generatedType = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(excelasTable)); 
                return (T)Convert.ChangeType(generatedType, typeof(T));
            }
        }
    }
}
