using System;
using System.IO;
using System.Printing;
using System.Threading;
using System.Windows;

namespace trisshores
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BatchPrintXps(null, null);
        }

        public void BatchPrintXps(object sender, RoutedEventArgs e)
        {
            // Create the secondary thread and pass the printing method for 
            // the constructor's ThreadStart delegate parameter. The BatchXPSPrinter
            // class is defined below.
            var printingThread = new Thread(BatchXPSPrinter.PrintXPS);

            // Set the thread that will use PrintQueue.AddJob to single threading.
            printingThread.SetApartmentState(ApartmentState.STA);

            // Start the printing thread. The method passed to the Thread 
            // constructor will execute.
            printingThread.Start();
        }

        public class BatchXPSPrinter
        {
            public static void PrintXPS()
            {
                // Create print server and print queue.
                //LocalPrintServer localPrintServer = new LocalPrintServer(); // not needed.
                PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

                // Prompt user to identify the directory, and then create the directory object.
                Console.Write("Enter the directory containing the XPS files: ");
                var directoryPath = @"C:\Users\Tris Shores\Desktop";    // Console.ReadLine(); 
                var dir = new DirectoryInfo(directoryPath);

                // If the user mistyped, end the thread and return to the Main thread.
                if (!dir.Exists)
                {
                    Console.WriteLine("There is no such directory.");
                    return;
                }

                // If there are no XPS files in the directory, end the thread 
                // and return to the Main thread.
                if (dir.GetFiles("*.xps").Length == 0)
                {
                    Console.WriteLine("There are no XPS files in the directory.");
                    return;
                }

                Console.WriteLine("\nJobs will now be added to the print queue.");
                Console.WriteLine("If the queue is not paused and the printer is working, jobs will begin printing.");

                // Batch process all XPS files in the directory.
                foreach (FileInfo file in dir.GetFiles("*.xps"))
                {
                    var nextFile = directoryPath + "\\" + file.Name;
                    Console.WriteLine($"Adding {nextFile} to queue.");

                    try
                    {
                        // Print the Xps file while providing XPS validation and progress notifications.
                        PrintSystemJobInfo xpsPrintJob = defaultPrintQueue.AddJob(file.Name, nextFile, fastCopy: false);
                    }
                    catch (PrintJobException e)
                    {
                        Console.WriteLine("\n\t{0} could not be added to the print queue.", file.Name);
                        if (e.InnerException.Message == "File contains corrupted data.")
                        {
                            Console.WriteLine("\tIt is not a valid XPS file. Use the isXPS Conformance Tool to debug it.");
                        }
                        Console.WriteLine("\tContinuing with next XPS file.\n");
                    }
                }
            }
        }
    }
}