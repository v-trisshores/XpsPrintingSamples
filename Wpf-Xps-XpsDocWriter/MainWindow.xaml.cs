using System;
using System.Printing;
using System.Windows;
using System.Windows.Xps;

namespace Wpf_Xps_XpsDocWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            btnPrint.Click += BtnPrint_Click;
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            var xpsdw1 = GetPrintXpsDocumentWriter();

            xpsdw1.Write(@"C:\Users\Tris Shores\Desktop\test.xps");  // include XpsDocumentWriter.Write() example?
            Console.WriteLine("Done synchronously printing");
        }

        // -------------------- GetPrintXpsDocumentWriter() -------------------
        /// <summary>
        ///   Returns an XpsDocumentWriter for the default print queue.</summary>
        /// <returns>
        ///   An XpsDocumentWriter for the default print queue.</returns>
        private XpsDocumentWriter GetPrintXpsDocumentWriter()
        {
            // Create a local print server
            LocalPrintServer ps = new LocalPrintServer();

            // Get the default print queue
            PrintQueue pq = ps.DefaultPrintQueue;

            // Get an XpsDocumentWriter for the default print queue
            XpsDocumentWriter xpsdw = PrintQueue.CreateXpsDocumentWriter(pq);
            return xpsdw;
        }// end:GetPrintXpsDocumentWriter()
    }
}
