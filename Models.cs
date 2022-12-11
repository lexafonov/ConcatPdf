using Aspose.Pdf.Facades;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        async public Task<(int res, string str)> SaveConcatPdf(Window own)
        {
            SaveFileDialog opf = new SaveFileDialog
            {
                Filter = @"pdf|*.pdf",
                FileName = "Concat",
            };
            if (opf.ShowDialog() ?? false)
            {
                Window ojid = new Window()
                {
                    Title = "Подождите... Идет объединение файлов.",
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Content = new TextBlock()
                    {
                        Text = "Подождите... Идет объединение файлов.",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5),
                    },
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = own
                };
                ojid.Show();
                var file = opf.FileName;
                await Task.Run(() =>
                {
                    PdfFileEditor pdfEditor = new PdfFileEditor();

                    pdfEditor.Concatenate(pathPdfFiles, file);
                });
                ojid.Close();
                return ValueTuple.Create(0, string.Empty);
            }
            return ValueTuple.Create(-1, "Выходной путь не выбран.");
        }
    }
}
