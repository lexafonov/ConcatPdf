using Aspose.Pdf.Facades;
using Microsoft.Win32;
using System;
using System.IO;

namespace ConcatPdf
{
    public class ConcatModel
    {
        public int kolFiles = 2;

        public string[] pathPdfFiles;

        public bool IsCorretPaths()
        {
            if (pathPdfFiles == null) { return false; }

            if (pathPdfFiles.Length < 2) { return false; }

            bool res = true;
            foreach(string file in pathPdfFiles)
            {
                res &= File.Exists(file);
            }
            return res;
        }
        /// <summary>
        /// Открытие и занесение путей к pdf файлам
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public (int res, string str) OpenAndReadPdf(int num)
        {
            var ofd = new OpenFileDialog
            {
                Filter = @"pdf|*.pdf"
            };
            if (ofd.ShowDialog() ?? false)
            {
                pathPdfFiles[num] = ofd.FileName;
                return ValueTuple.Create(0, string.Empty);
            }
            return ValueTuple.Create(-1, "Файл не выбран.");
        }

        public (int res, string str) SaveConcatPdf()
        {
            SaveFileDialog opf = new SaveFileDialog
            {
                Filter = @"pdf|*.pdf"
            };
            if (opf.ShowDialog() ?? false)
            {
                var file = opf.FileName;

                PdfFileEditor pdfEditor = new PdfFileEditor();

                pdfEditor.Concatenate(pathPdfFiles, file);

                return ValueTuple.Create(0, string.Empty);
            }
            return ValueTuple.Create(-1, "Выходной путь не выбран.");
        }
    }
}
