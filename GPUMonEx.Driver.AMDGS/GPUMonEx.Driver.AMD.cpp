// GPUMonEx.Driver.AMD.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <iostream>
#include "debug.h"
#include "GPUMonEx.Driver.AMD.h"


/* Logging file */
std::ofstream logfi;


GPUMONEXDRIVERAMD_API int Drv_Initialize()
{
	_LOG( __FUNCTION__ << "(): " << "AMD driver initialization started...\n" );
	return 0;
}

GPUMONEXDRIVERAMD_API void Drv_Uninitialize()
{
	_LOG( __FUNCTION__ << "AMD driver uninitialization completed.\n" );
}

GPUMONEXDRIVERAMD_API int Drv_GetGpuDetails( int AdapterNumber, GPUDETAILS* pGpuDetails )
{
	_LOG( __FUNCTION__ << "TODO: Implement...\n" );

	return 0;
}

GPUMONEXDRIVERAMD_API int Drv_GetOverallGpuLoad()
{
	return 0;
}

GPUMONEXDRIVERAMD_API int Drv_GetGpuTemperature()
{
	return 0;
}
