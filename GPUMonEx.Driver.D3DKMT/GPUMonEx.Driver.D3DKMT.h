#pragma once

#include "drvdefs.h"


// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the GPUMONEXDRIVERD3DKMT_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// GPUMONEXDRIVERD3DKMT_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef GPUMONEXDRIVERD3DKMT_EXPORTS
#define GPUMONEXDRIVERD3DKMT_API __declspec(dllexport)
#else
#define GPUMONEXDRIVERD3DKMT_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

GPUMONEXDRIVERD3DKMT_API int Drv_Initialize();
GPUMONEXDRIVERD3DKMT_API void Drv_Uninitialize();
GPUMONEXDRIVERD3DKMT_API int Drv_GetGpuDetails( int AdapterNumber, GPUDETAILS* pGpuDetails );
GPUMONEXDRIVERD3DKMT_API int Drv_GetOverallGpuLoad();
GPUMONEXDRIVERD3DKMT_API int Drv_GetGpuTemperature();

#ifdef __cplusplus
}
#endif