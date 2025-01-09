using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System.IO;
using System.Threading.Tasks;
using System;

namespace PDFDeSecure;

public partial class PDFDeSecureAvalonia : Window
{
    public PDFDeSecureAvalonia()
    {
        InitializeComponent();
    }
    PdfDocument pdf;

    PdfDocument outpdf;


}