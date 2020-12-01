using BarcodeLib;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarcodeStampDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Generate Barcode Image
            Barcode barcode = new Barcode();
            barcode.LabelFont = new Font("Arial", 10 * 96f / 72, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
            barcode.IncludeLabel = true;
            barcode.Alignment = AlignmentPositions.CENTER;
            var barcodeImage = barcode.Encode(TYPE.CODE128A, "1234567890", System.Drawing.Color.Black, System.Drawing.Color.White, 160, 50);

            //Open PDf
            var pdfPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Dummy.pdf");
            PdfDocument document = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Modify);
            PdfPage page = document.Pages[0];
            XGraphics gfx = XGraphics.FromPdfPage(page);
            barcodeImage.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            barcodeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

            // Stamp barcode at bottom
            XImage image = XImage.FromGdiPlusImage(barcodeImage);
            var x = (page.Width / 2) - 80;
            var y = (page.Height - 50);
            gfx.DrawImage(image, x, y);
            document.Save(pdfPath);
        }
    }
}
