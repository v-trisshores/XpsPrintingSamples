using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace Wpf_Xps_PrintDialog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InvokePrint(null, null);
        }

        private void InvokePrint(object sender, RoutedEventArgs e)
        {
            // Create the print dialog object and set options
            var printDialog = new PrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.AllPages;
            printDialog.UserPageRangeEnabled = true;

            // Display the dialog. This returns true if the user presses the Print button.
            bool? print = printDialog.ShowDialog();
            if (print == true)
            {
                var xpsDocument = new XpsDocument(@"C:\Users\Tris Shores\Desktop\test.xps", FileAccess.ReadWrite);   // ReadWrite mode generates a System.IO.IOException: 'Entries cannot be opened multiple times in Update mode'.
                FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
                printDialog.PrintDocument(fixedDocSeq.DocumentPaginator, "Test print job");
            }
        }
    }
}
