using System;
using System.IO;
using System.Printing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace XpsWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            RunXps2();
        }

        private void RunXps2()
        {
            // Create the print server and print queue objects.
            var defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

            // Call AddJob
            PrintSystemJobInfo myPrintJob = defaultPrintQueue.AddJob();

            // Write a Byte buffer to the JobStream and close the stream
            Stream myStream = myPrintJob.JobStream;
            Byte[] myByteBuffer = UnicodeEncoding.Unicode.GetBytes("This is a test string for the print job stream.");
            myStream.Write(myByteBuffer, 0, myByteBuffer.Length);
            myStream.Close();
        }

        private void RunXps1()
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
                PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

                // Prompt user to identify the directory, and then create the directory object.
                Console.Write("Enter the directory containing the XPS files: ");
                string directoryPath = @"C:\Users\Tris Shores\Desktop";
                DirectoryInfo dir = new DirectoryInfo(directoryPath);

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
                foreach (FileInfo f in dir.GetFiles("*.xps"))
                {
                    string nextFile = directoryPath + "\\" + f.Name;
                    Console.WriteLine("Adding {0} to queue.", nextFile);

                    try
                    {
                        // Print the XPS file while providing XPS validation and progress notifications.
                        PrintSystemJobInfo xpsPrintJob = defaultPrintQueue.AddJob(f.Name, nextFile, false);

                        // why does it always prompt for an output filename, couldn't that be an optional input param?
                    }
                    catch (PrintJobException e)
                    {
                        Console.WriteLine("\n\t{0} could not be added to the print queue.", f.Name);
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