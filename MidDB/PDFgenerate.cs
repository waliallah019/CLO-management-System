using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace MidDB
{
    public class PDFgenerate
    {
       
            public static void GeneratePdf(DataTable dataTable, string titlePdf)
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Generated PDF";
                document.Info.Author = "Laiba Khan";
                document.Info.Subject = "PDF Generation ";
                document.Info.Keywords = "PDF, Report, DataTable";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont titleFont = new XFont("Arial", 18, XFontStyle.Bold);
                XFont headingFont = new XFont("Arial", 14, XFontStyle.Bold);
                XFont contentFont = new XFont("Arial", 12);
                XFont tableHeaderFont = new XFont("Arial", 12, XFontStyle.Bold);
                XFont tableFont = new XFont("Arial", 10);
                XColor titleColor = XColors.Brown;
                XColor tableHeaderColor = XColors.LightGray;

                double marginLeft = 20;
                double marginRight = 20;
                double marginTop = 40;

                double contentWidth = page.Width - marginLeft - marginRight;

                DrawCenteredText(gfx, titlePdf, titleFont, titleColor, page.Width, marginTop);
                DrawCenteredText(gfx, "Test PDF developed by Laiba Khan", contentFont, XColors.Black, page.Width, marginTop + 30);

                double tableY = DrawTableHeaders(gfx, dataTable, tableHeaderFont, tableHeaderColor, tableFont, marginLeft, marginTop + 60, contentWidth);
                DrawTableData(gfx, dataTable, tableFont, marginLeft, tableY, contentWidth);

                string fileName = string.Empty;
                using (SaveFileDialog fdb = new SaveFileDialog())
                {
                    fdb.Filter = "pdf files (.pdf)|.pdf|All files(.) | . ";
                    fdb.DefaultExt = "pdf";
                    if (fdb.ShowDialog() == DialogResult.OK)
                    {
                        fileName = fdb.FileName;
                    }
                }

                document.Save(fileName);
            }

            private static void DrawCenteredText(XGraphics gfx, string text, XFont font, XColor color, double pageWidth, double yPos)
            {
                XSize textSize = gfx.MeasureString(text, font);
                double xPos = (pageWidth - textSize.Width) / 2;
                gfx.DrawString(text, font, new XSolidBrush(color), new XPoint(xPos, yPos));
            }

            private static double DrawTableHeaders(XGraphics gfx, DataTable dataTable, XFont headerFont, XColor headerColor, XFont cellFont, double x, double y, double contentWidth)
            {
                double xPos = x;
                double yPos = y;

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string columnName = dataTable.Columns[i].ColumnName;
                    double columnWidth = GetColumnWidth(gfx, dataTable, i, headerFont, cellFont);
                    XRect rect = new XRect(xPos, yPos, columnWidth, 20);

                    gfx.DrawRectangle(new XSolidBrush(headerColor), rect);
                    gfx.DrawString(columnName, headerFont, XBrushes.Black, rect, XStringFormats.Center);
                    xPos += columnWidth;
                }

                return yPos + 20;
            }

            private static void DrawTableData(XGraphics gfx, DataTable dataTable, XFont cellFont, double x, double y, double contentWidth)
            {
                double xPos = x;
                double yPos = y;

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        string cellValue = dataTable.Rows[i][j].ToString();
                        double columnWidth = GetColumnWidth(gfx, dataTable, j, cellFont, cellFont);
                        XRect rect = new XRect(xPos, yPos, columnWidth, 20);

                        XBrush cellBrush = GetCellBrush(dataTable.Rows[i][j]);
                        gfx.DrawRectangle(cellBrush, rect);
                        gfx.DrawString(cellValue, cellFont, XBrushes.Black, rect, XStringFormats.Center);
                        xPos += columnWidth;
                    }

                    xPos = x;
                    yPos += 20;
                }
            }

            private static double GetColumnWidth(XGraphics gfx, DataTable dataTable, int columnIndex, XFont headerFont, XFont cellFont)
            {
                double headerWidth = gfx.MeasureString(dataTable.Columns[columnIndex].ColumnName, headerFont).Width;

                double maxCellWidth = headerWidth;

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    double cellWidth = gfx.MeasureString(dataTable.Rows[j][columnIndex].ToString(), cellFont).Width;
                    if (cellWidth > maxCellWidth)
                    {
                        maxCellWidth = cellWidth;
                    }
                }

                return Math.Max(headerWidth, maxCellWidth) + 10;
            }

            private static XBrush GetCellBrush(object cellValue)
            {
                if (cellValue is int intValue && intValue > 50)
                {
                    return new XSolidBrush(XColors.LightGreen);
                }
                else
                {
                    return XBrushes.White;
                }
            }
        }
    }
