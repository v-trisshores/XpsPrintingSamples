using System;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Wpf_Xps_CreateXpsDocWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GetPrintXpsDocumentWriter(null, null);
        }

        // -------------------- GetPrintXpsDocumentWriter() -------------------
        /// <summary>
        ///   Returns an XpsDocumentWriter for the default print queue.</summary>
        /// <returns>
        ///   An XpsDocumentWriter for the default print queue.</returns>
        private XpsDocumentWriter GetPrintXpsDocumentWriter(object sender, RoutedEventArgs e)
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