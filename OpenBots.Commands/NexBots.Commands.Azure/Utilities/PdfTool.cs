using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;


namespace NexBots.Commands.Azure.Utilities
{
    public class PdfTool
    {
        private bool FileIsPdf(string filePath)
        {
            return PdfReader.TestPdfFile(filePath) > 0;
        }


        private PdfDocument LoadPdf(string filePath)
        {

            if (!FileIsPdf(filePath))
            {
                throw new InvalidDataException("This file is not a PDF");
            }
            return PdfReader.Open(filePath, PdfDocumentOpenMode.Modify);
        }

        private PdfDocument LoadPdf(string filePath, bool modify)
        {
            if (modify)
            {
                return LoadPdf(filePath);
            }

            if (!FileIsPdf(filePath))
            {
                throw new InvalidDataException("This file is not a PDF");
            }
            return PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
        }

        public int GetTotalPages(string filePath)
        {
            var pdf = LoadPdf(filePath);
            return pdf.PageCount;
        }

        public string SplitPages(string targetPdfPath, string saveToFolderPath)
        {
            try
            {
                var pdf = LoadPdf(targetPdfPath, false);
                var pdfFileName = Path.GetFileName(targetPdfPath).ToLower().Replace(".pdf", "");
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                for (int pdfPage = 0; pdfPage < pdf.PageCount; pdfPage++)
                {
                    var page = new PdfDocument();
                    var destinationPath = Path.Combine(saveToFolderPath, $"{pdfFileName}-SPLITPAGE{pdfPage + 1}.pdf");
                    page.AddPage(pdf.Pages[pdfPage]);
                    page.Save(destinationPath);
                }
                return "OK";
            }
            catch
            {
                throw;
            }
        }

        public string SavePdfAsSplitPNGPages(string targetPdfPath, string destinationPath)
        {
            try
            {
                var pdf = LoadPdf(targetPdfPath);
                var cachePath = @"C:\ProgramData\NexBots\PdfSplitPages";
                if (!System.IO.Directory.Exists(cachePath))
                {
                    System.IO.Directory.CreateDirectory(cachePath);
                }
                foreach (var file in System.IO.Directory.GetFiles(cachePath))
                {
                    System.IO.File.Delete(file);
                }
                var splitPages = SplitPages(targetPdfPath, cachePath);
                var pageList = System.IO.Directory.GetFiles(cachePath).ToList();
                pageList.ForEach(x =>
                {
                    cs_pdf_to_image.Pdf2Image.PrintQuality = 5000;
                    cs_pdf_to_image.Pdf2Image.Convert(x, $"{destinationPath}-PAGE{pageList.IndexOf(x) + 1}.png");
                });

                return "OK";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        public string SavePdfAsImage(string targetPdfPath, string destinationPath)
        {
            try
            {
                var pdf = LoadPdf(targetPdfPath);
                pdf.Save(destinationPath);
                return "OK";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }
    }
}
