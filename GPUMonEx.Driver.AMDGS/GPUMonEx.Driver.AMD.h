#pragma once

#include "drvdefs.h"


// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the GPUMONEXDRIVERAMD_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// GPUMONEXDRIVERAMD_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef GPUMONEXDRIVERAMD_EXPORTS
#define GPUMONEXDRIVERAMD_API __declspec(dllexport)
#else
#define GPUMONEXDRIVERAMD_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

GPUMONEXDRIVERAMD_API int Drv_Initialize();
GPUMONEXDRIVERAMD_API void Drv_Uninitialize();
GPUMONEXDRIVERAMD_API int Drv_GetGpuDetails( int AdapterNumber, GPUDETAILS* pGpuDetails );
GPUMONEXDRIVERAMD_API int Drv_GetOverallGpuLoad();
GPUMONEXDRIVERAMD_API int Drv_GetGpuTemperature();

#ifdef __cplusplus
}
#endif