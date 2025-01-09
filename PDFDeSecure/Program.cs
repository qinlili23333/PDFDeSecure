using Avalonia;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PDFDeSecure
{
    static class Program
    {

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int FreeConsole();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var Args = Environment.GetCommandLineArgs();
            if (Args.Length > 2)
            {
                //Auto Processing Mode
                PdfDocument pdf;
                PdfDocument outpdf;
                //First Argu Is INPUT Dir, Second Is OUTPUT Dir
                var Input = Args[1];
                var Output = Args[2];
                DirectoryInfo di = new(Input);
                var aryFi = di.GetFiles("*.pdf");
                var counter = 0;
                var error = 0;
                foreach (FileInfo fi in aryFi)
                {
                    Console.WriteLine("Processing " + fi.Name);
                    //Skip file with errors
                    try
                    {
                        outpdf = new PdfDocument();
                        FileStream fileStream = fi.OpenRead();
                        pdf = PdfReader.Open(fileStream, PdfDocumentOpenMode.Import);
                        foreach (PdfPage page in pdf.Pages)
                        {
                            outpdf.AddPage(page);
                        }
                        outpdf.Save(new FileInfo(Output + "\\" + fi.Name).OpenWrite(), true);
                        counter++;
                        pdf.Dispose();
                        fileStream.Close();
                        outpdf.Dispose();
                    }
                    catch (Exception ex)
                    {
                        error++;
                        Console.Write(ex.ToString());
                    }
                }
                Console.WriteLine("Unlocked " + counter + " files" + Environment.NewLine + "Failed " + error + " files" + Environment.NewLine + "Percentage " + counter + "/" + (counter + error) + " = " + (counter / (float)(counter + error) * 100).ToString("f2") + "%, Cheers!", "PDF file Unlocked! and Saved!");
                Environment.Exit(0);
            }
            else
            {
                FreeConsole();
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(Args);
                var Window = new PDFDeSecureAvalonia();
                Window.Show();
            }
        }
    }
}
