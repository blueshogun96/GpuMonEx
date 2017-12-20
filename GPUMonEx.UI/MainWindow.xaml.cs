using System;
using System.Collections.Generic;
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
using System.Threading;
using System.Windows.Threading;
using System.Timers;
using GPUMonEx;


namespace GPUMonEx.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* Available driver types */
        public const int NVAPI = 0;
        public const int D3DKMT = 1;

        /* A handle to the primary display adapter's driver (TODO: Enumerate) */
        private GPUDriverBase[] GPUDriver = { new GPUDriverNVAPI(), new GPUDriverD3DKMT() };   
        private bool[] ValidDrivers = { false, false };
        private int CurrentDriver;

        /*
         * Do initialization of the GPU drivers prior to launch.
         * TODO: Enumerate all available GPUs and properly detect them all in order as well as initialize multiple
         * device drivers per GPU as necessary.
         */
        private bool InitializeGpuDrivers()
        {
            bool DriverSelected = false;
         
            /*
             * Attempt to initialize the NVAPI driver first.  Without a valid NVIDIA GPU driver installed,
             * this should fail in one way or another.
             * 
             * Known Issues:
             * - The GPU temperature feature doesn't seem to work for the Quadro K1000M
             */
            try
            {
                if (GPUDriver[NVAPI].Initialize() != 0)
                {
                    ValidDrivers[NVAPI] = true;

                    if (!DriverSelected)
                    {
                        DriverSelected = true;
                        CurrentDriver = NVAPI;
                    }
                }
            }
            catch (System.DllNotFoundException e)
            {
                Console.WriteLine(e.Message + "\n");
                MessageBox.Show("The following exception has occurred: \n" + e.Message, "GPUMonEx");
            }
            catch (System.EntryPointNotFoundException e)
            {
                Console.WriteLine(e.Message + "\n");
                MessageBox.Show("The following exception has occurred: \n" + e.Message, "GPUMonEx");
            }

            /*
             * The D3DKMT driver is our fallback in case any vendor specific drivers aren't working.
             * It's theoretically supposed to (but not guaranteed to) work on every GPU.
             * 
             * Known Issues:
             * - Can't seem to get GPU usage from the Quadro K1000M
             * - The current code for the D3DKMT driver is hard coded to check for Intel GPUs.
             */
            try
            {
                if (GPUDriver[D3DKMT].Initialize() != 0)
                {
                    ValidDrivers[D3DKMT] = true;

                    if (!DriverSelected)
                    {
                        DriverSelected = true;
                        CurrentDriver = D3DKMT;
                    }
                }
            }
            catch (System.DllNotFoundException e)
            {
                Console.WriteLine(e.Message + "\n");
                MessageBox.Show("The following exception has occurred: \n" + e.Message, "GPUMonEx");
            }
            catch (System.EntryPointNotFoundException e)
            {
                Console.WriteLine(e.Message + "\n");
                MessageBox.Show("The following exception has occurred: \n" + e.Message, "GPUMonEx");
            }

            /*
             * Okay, let's see what we got.
             */
            int Found = 0;
            foreach (bool vd in ValidDrivers)
            {
                if (vd) Found++;
            }

            return DriverSelected;
        }

        /*
         * Handles uninitialization of GPU drivers after the user chooses to quit the application
         */
        private void UninitializeGpuDrivers()
        {
            GPUDriver[D3DKMT].Uninitialize();
            GPUDriver[NVAPI].Uninitialize();
        }

        /*
         * Updates the current overall GPU usage and displays it to the user
         */
        private void UpdateOverallGpuUsage( object sender, ElapsedEventArgs e )
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                int d = CurrentDriver;
                txtOverallUsage.Text = "GPU Usage: " + GPUDriver[d].GetOverallGpuLoad() + "%";
            });
        }

        public MainWindow()
        {
            InitializeComponent();

            /* Initialize the GPU drivers */
            if( !InitializeGpuDrivers() )
            {
                txtOverallUsage.Text = "Driver not initialized...";
                return;
            }

            /*
             * Display GPU details
             * TODO: a command line option for this
             */
            GPUDETAILS GpuDetails = new GPUDETAILS();
            GPUDriver[CurrentDriver].GetGpuDetails(0, ref GpuDetails);

            /* Display information about this display adapter */
            string strDeviceID = String.Format("0x{0:X4}", GpuDetails.DeviceID);
            string strVendorID = String.Format("0x{0:X4}", GpuDetails.VendorID);
            txtAdapterInformation.Text = "Adapter ID: " + GpuDetails.DeviceDesc + "\n" +
                "Device ID: " + strDeviceID + "\n" +
                "Vendor ID: " + strVendorID + "\n";

            /* Initialize a timer that upates the GPU usage every 1000 milliseconds */
            System.Timers.Timer timerOverallUsage = new System.Timers.Timer();
            timerOverallUsage.Elapsed += new System.Timers.ElapsedEventHandler(this.UpdateOverallGpuUsage);
            timerOverallUsage.Interval = 1000;
            timerOverallUsage.Enabled = true;
        }
    
        ~MainWindow()
        {
            UninitializeGpuDrivers();
        }

        private void GPUConsumingProcesses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
