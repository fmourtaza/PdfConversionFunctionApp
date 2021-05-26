using System;
using System.IO;
using System.Net;
using PdfConversionFunctionApp;

namespace PdfConversionConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePathWord = @"C:\Temp\TestDocument.docx";
            string filePathOutWord = @"C:\Temp\TestDocument.pdf";

            string filePathExcel = @"C:\Temp\TestExcel.xlsx";
            string filePathOutExcel = @"C:\Temp\TestExcel.pdf";

            bool IsSuccessWord = ConverToPdf(filePathWord, filePathOutWord);
            bool IsSuccessExcel = ConverToPdf(filePathExcel, filePathOutExcel);
        }

        private static bool ConverToPdf(String filePath, String filePathOut)
        {
            try
            {
                //string urlLocal = "http://localhost:7071/api/ConvertToPdf";
                string urlAzure = "https://graphpdfconverter.azurewebsites.net/api/ConvertToPdf";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(urlAzure);
                req.Method = "POST";

                string fileExtension = Path.GetExtension(filePath);
                switch (fileExtension)
                {
                    case ".doc":
                        req.ContentType = "application/msword";
                        break;
                    case ".docx":
                        req.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".xls":
                        req.ContentType = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        req.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; ;
                        break;
                    default:
                        throw new Exception("Only Word & Excel documents are supported by the Converter");
                }

                Stream fileStream = System.IO.File.Open(filePath, FileMode.Open);
                MemoryStream inputStream = new MemoryStream();
                fileStream.CopyTo(inputStream);
                fileStream.Dispose();
                Stream stream = req.GetRequestStream();
                stream.Write(inputStream.ToArray(), 0, inputStream.ToArray().Length);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                //Create file stream to save the output PDF file
                FileStream outStream = System.IO.File.Create(filePathOut);
                //Copy the responce stream into file stream
                res.GetResponseStream().CopyTo(outStream);
                //Dispose the input stream
                inputStream.Dispose();
                //Dispose the file stream
                outStream.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;


        }
    }
}
