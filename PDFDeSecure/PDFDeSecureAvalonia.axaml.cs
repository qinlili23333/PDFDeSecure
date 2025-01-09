using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System.IO;
using System;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace PDFDeSecure;

public partial class PDFDeSecureAvalonia : Window
{
    public PDFDeSecureAvalonia()
    {
        InitializeComponent();
    }
    PdfDocument pdf;

    PdfDocument outpdf;

    public async void BrowseFile(object sender, RoutedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select PDF File",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.Pdf]
        });

        if (files.Count >= 1)
        {
            pdffile.Text = files[0].Name;
            btnunlock.IsEnabled = false;
            btnbrowse.IsEnabled = false;
            progressBar1.Value = 0;
            var progress = new Progress<int>(report => progressBar1.Value = report);
            btnunlock.Content = "Processing...";
            pdf?.Dispose();
            outpdf?.Dispose();
            outpdf = new PdfDocument();
            Stream fileStream = await files[0].OpenReadAsync();
            pdf = PdfReader.Open(fileStream, PdfDocumentOpenMode.Import);
            int current = 0;
            foreach (PdfPage page in pdf.Pages)
            {
                outpdf.AddPage(page);
                current++;
                IProgress<int> iprog = progress;
                iprog.Report(current * 100 / pdf.PageCount);
            }
            fileStream.Close();
            btnunlock.IsEnabled = true;
            btnbrowse.IsEnabled = true;
            btnunlock.Content = "Unlock PDF";
        }
    }

    public async void SaveFile(object sender, RoutedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Unlocked PDF File",
            DefaultExtension="pdf",
            FileTypeChoices = [FilePickerFileTypes.Pdf],
            SuggestedFileName= pdffile.Text.Replace(".pdf","_unlock.pdf")
        });

        if (file is not null)
        {
            btnunlock.Content = "Saving...";
            btnunlock.IsEnabled = false;
            outpdf.Save(await file.OpenWriteAsync(), true);
            outpdf.Dispose();
            pdf.Dispose();
            await MessageBoxManager.GetMessageBoxStandard("Unlocked & Saved", "PDF file Unlocked! and Saved!", ButtonEnum.Ok).ShowAsync();
            pdffile.Text = "";
            btnunlock.Content = "Unlock PDF";
        }
    }
}