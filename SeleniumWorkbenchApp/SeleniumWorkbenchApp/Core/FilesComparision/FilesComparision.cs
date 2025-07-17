using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Generals;

namespace SeleniumGendKS.Core.FilesComparision
{
    internal class FilesComparision
    {
        public static bool IsFilesEqual(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);
            if (file1.Length == file2.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool PDFIsFilesEqual(string fileNameBaseline, string downloadedFileName)
        {
            bool result = false;

            var pdfBaseline = ExtractTextFromPdf(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\PDF files\" + fileNameBaseline));
            var downloadedFile = ExtractTextFromPdf(Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads" + @"\" + downloadedFileName);

            IEnumerable<string> onlyB = pdfBaseline.Except(downloadedFile).ToList();

            if (onlyB.Count() == 0) result = true;
            else
            {
                foreach (var lin in onlyB)
                {
                    Console.WriteLine(lin); //difference line by line
                }

                // Save as the downloaded filename and put it in the 'TestResult' folder
                string fileName = "download.pdf";
                string sourceFileToCopy = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads" + @"\" + fileName;
                string destinationDirectory = System.IO.Path.GetFullPath(@"../../../../TestResults/");
                File.Copy(sourceFileToCopy, destinationDirectory + "New_" + fileNameBaseline);
            }

            return result;
        }

        // Where ExtractTextFromPDF function is as below or check "read pdf file in C#"
        public static string[] ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString().Split(new[] { '\r', '\n' });
            }
        }
    }
}
