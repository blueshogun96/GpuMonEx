using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GPUMonEx
{
    /*
	 * GPU Details structure
	 * NOTE: Subject to change
	 */
    [StructLayout(LayoutKind.Sequential, Size = 136), Serializable]
    public struct GPUDETAILS
    {
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceDesc;

        [MarshalAsAttribute(UnmanagedType.U4, SizeConst = 1)]
        public UInt32 DeviceID;

        [MarshalAsAttribute(UnmanagedType.U4, SizeConst = 1)]
        public UInt32 VendorID;
    }


    /*
	 * Driver importer classes for the following APIs under Windows
	 * TODO: Get ahold of Intel's SDK as well as implement AMD's equivalent for their hardware.
	 * 
	 * NVAPI - NVIDIA Driver Specific functionality 
     * AMD - Radeon Driver specific functionality
	 * D3DKMT - Direct3D internal driver functions.  Should work for all GPUs, but currently needed for Intel.
	 */
    static class DrvD3DKMT
    {
        [DllImport("GPUMonEx.Driver.D3DKMT.dll")]
        public static extern int Drv_Initialize();

        [DllImport("GPUMonEx.Driver.D3DKMT.dll")]
        public static extern void Drv_Uninitialize();

        [DllImport("GPUMonEx.Driver.D3DKMT.dll")]
        public static extern unsafe int Drv_GetGpuDetails(int Adapter, ref GPUDETAILS pGpuDetails);

        [DllImport("GPUMonEx.Driver.D3DKMT.dll")]
        public static extern int Drv_GetOverallGpuLoad();

        [DllImport("GPUMonEx.Driver.D3DKMT.dll")]
        public static extern int Drv_GetGpuTemperature();
    }

    static class DrvNVAPI
    {
        [DllImport("GPUMonEx.Driver.NVAPI.dll")]
        public static extern int Drv_Initialize();

        [DllImport("GPUMonEx.Driver.NVAPI.dll")]
        public static extern void Drv_Uninitialize();

        [DllImport("GPUMonEx.Driver.NVAPI.dll")]
        public static extern unsafe int Drv_GetGpuDetails(int Adapter, ref GPUDETAILS pGpuDetails);

        [DllImport("GPUMonEx.Driver.NVAPI.dll")]
        public static extern int Drv_GetOverallGpuLoad();

        [DllImport("GPUMonEx.Driver.NVAPI.dll")]
        public static extern int Drv_GetGpuTemperature();
    }


    static class DrvAMDGS
    {
        [DllImport("GPUMonEx.Driver.AMDGS.dll")]
        public static extern int Drv_Initialize();

        [DllImport("GPUMonEx.Driver.AMDGS.dll")]
        public static extern void Drv_Uninitialize();

        [DllImport("GPUMonEx.Driver.AMDGS.dll")]
        public static extern unsafe int Drv_GetGpuDetails(int Adapter, ref GPUDETAILS pGpuDetails);

        [DllImport("GPUMonEx.Driver.AMDGS.dll")]
        public static extern int Drv_GetOverallGpuLoad();

        [DllImport("GPUMonEx.Driver.AMDGS.dll")]
        public static extern int Drv_GetGpuTemperature();
    }


    /*
	 * GPU Driver interfacing classes (the ones you actually call in user mode)
	 */
    public abstract class GPUDriverBase
    {
        public abstract int Initialize();
        public abstract void Uninitialize();
        public abstract int GetGpuDetails(int Adapter, ref GPUDETAILS pGpuDetails);
        public abstract int GetOverallGpuLoad();
        public abstract int GetGpuTemperature();
    }

    public class GPUDriverD3DKMT : GPUDriverBase
    {
        public override int Initialize()
        {
            return DrvD3DKMT.Drv_Initialize();
        }

        public override void Uninitialize()
        {
            DrvD3DKMT.Drv_Uninitialize();
        }

        public override int GetGpuDetails(int Adapter, ref GPUDETAILS pGpuDetails)
        {
            return DrvD3DKMT.Drv_GetGpuDetails(Adapter, ref pGpuDetails);
        }

        public override int GetOverallGpuLoad()
        {
            return DrvD3DKMT.Drv_GetOverallGpuLoad();
        }

        public override int GetGpuTemperature()
        {
            return DrvD3DKMT.Drv_GetGpuTemperature();
        }
    }

    public class GPUDriverNVAPI : GPUDriverBase
    {
        public override int Initialize()
        {
            return DrvNVAPI.Drv_Initialize();
        }

        public override void Uninitialize()
        {
            DrvNVAPI.Drv_Uninitialize();
        }

        public override int GetGpuDetails(int Adapter, ref GPUDETAILS pGpuDetails)
        {
            return DrvNVAPI.Drv_GetGpuDetails(Adapter, ref pGpuDetails);
        }

        public override int GetOverallGpuLoad()
        {
            return DrvNVAPI.Drv_GetOverallGpuLoad();
        }

        public override int GetGpuTemperature()
        {
            return DrvNVAPI.Drv_GetGpuTemperature();
        }
    }

    public class GPUDriverAMDGS : GPUDriverBase
    {
        public override int Initialize()
        {
            return DrvAMDGS.Drv_Initialize();
        }

        public override void Uninitialize()
        {
            DrvAMDGS.Drv_Uninitialize();
        }

        public override int GetGpuDetails(int Adapter, ref GPUDETAILS pGpuDetails)
        {
            return DrvAMDGS.Drv_GetGpuDetails(Adapter, ref pGpuDetails);
        }

        public override int GetOverallGpuLoad()
        {
            return DrvAMDGS.Drv_GetOverallGpuLoad();
        }

        public override int GetGpuTemperature()
        {
            return DrvAMDGS.Drv_GetGpuTemperature();
        }
    }
}
