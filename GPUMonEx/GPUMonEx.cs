using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPUMonEx
{
    class GPUMonEx
    {
        /* Available driver types */
        public const int NVAPI = 0;
        public const int AMDGS = 1;
        public const int D3DKMT = 2;

        static void Main(string[] args)
        {
            int i = 0;
            foreach( string s in args )
            {
                Console.WriteLine("Arg {0}:= {1}", i, s);
                i++;
            }

            /* 
             * Let's start off with a little driver enumeration. Each driver has to go through a series
             * of tests to determine whether we can actually use it's API or not.
             */
            GPUDriverBase[] Drivers = { new GPUDriverNVAPI(), new GPUDriverAMDGS(), new GPUDriverD3DKMT() };
            bool[] ValidDrivers = { false, false, false };

            /*
             * Attempt to initialize the NVAPI driver first.  Without a valid NVIDIA GPU driver installed,
             * this should fail in one way or another.
             * 
             * Known Issues:
             * - The GPU temperature feature doesn't seem to work for the Quadro K1000M
             */
            try
            {
                if (Drivers[NVAPI].Initialize() != 0)
                    ValidDrivers[NVAPI] = true;
            }
            catch( System.DllNotFoundException e )
            {
                Console.WriteLine(e.Message+"\n");
            }
            catch( System.EntryPointNotFoundException e )
            {
                Console.WriteLine(e.Message + "\n");
            }

            /*
             * Attempt to initialize the AMDGS driver first.  Without a valid AMD GPU driver installed,
             * this should fail in one way or another.
             * 
             * Known Issues:
             * - This needs to be implemented/tested on AMD hardware (I don't have an AMD card).
             */
            try
            {
                if (Drivers[AMDGS].Initialize() != 0)
                    ValidDrivers[AMDGS] = true;
            }
            catch (System.DllNotFoundException e)
            {
                Console.WriteLine(e.Message + "\n");
            }
            catch (System.EntryPointNotFoundException e)
            {
                Console.WriteLine(e.Message + "\n");
            }

            /*
             * The D3DKMT driver is our fallback in case any vendor specific drivers aren't working.
             * It's theoretically supposed to (but not guaranteed to) work on every GPU.
             * 
             * Known Issues:
             * - Can't seem to get GPU usage from the Quadro K1000M
             */
            try
            {
                if (Drivers[D3DKMT].Initialize() != 0)
                    ValidDrivers[D3DKMT] = true;
            }
            catch( System.DllNotFoundException e )
            {
                Console.WriteLine(e.Message + "\n");
            }
            catch (System.EntryPointNotFoundException e)
            {
                Console.WriteLine(e.Message + "\n");
            }

            /*
             * Okay, let's see what we got.
             */
            int Found = 0;
            foreach( bool vd in ValidDrivers )
            {
                if (vd) Found++;
            }

            Console.WriteLine("{0} valid drivers found...\n", Found);

            if( Found == 0 )
            {
                Console.WriteLine("Exiting...\n");
                return;
            }

            /*
             * Let's choose the most appropriate driver.
             * TODO: Allow the user to force D3DKMT if desired.
             */
            int d = 0;

            if (ValidDrivers[NVAPI])
            {
                d = NVAPI;
                Console.WriteLine("NVAPI driver selected...");
            }
            else if (ValidDrivers[D3DKMT])
            {
                d = D3DKMT;
                Console.WriteLine("D3DKMT driver selected...");
            }

            GPUDriverBase Driver = Drivers[d];

            /*
             * Display GPU details
             * TODO: a command line option for this
             */
            GPUDETAILS GpuDetails = new GPUDETAILS();
            Drivers[d].GetGpuDetails(0, ref GpuDetails);

            Console.WriteLine("\nGPU Name: {0}", GpuDetails.DeviceDesc);
            Console.WriteLine("Device ID: {0}\nVendor ID: {1}", GpuDetails.DeviceID, GpuDetails.VendorID);

            /*
             * End it... for now
             */
            foreach (GPUDriverBase drv in Drivers)
                drv.Uninitialize();

            Console.WriteLine( "That's all folks!" );
        }
    }
}
