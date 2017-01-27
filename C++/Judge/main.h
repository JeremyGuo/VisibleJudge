#ifndef __MAIN_H__
#define __MAIN_H__

#include <windows.h>
#include <cstdio>
#include <psapi.h>
#include <cctype>

#define UNABLEOPENEXE -2
#define COMPILE_ERROR -1
#define ACCEPT 0
#define WRONGANSWER 1
#define RUNTIMEERROR 2
#define PERMUTATIONERROR 3
/*  To use this exported function of dll, include this header
 *  in your project.
 */

#ifdef BUILD_DLL
    #define DLL_EXPORT __declspec(dllexport)
#else
    #define DLL_EXPORT __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C"
{
#endif

void DLL_EXPORT Compile(const LPCSTR name, const LPCSTR args);\
void DLL_EXPORT SomeFunction(const LPCSTR sometext);\
void DLL_EXPORT Judge(const LPCSTR exe, const LPCSTR indata, const LPCSTR outdata, const LPCSTR userdata, const LPCSTR result, int& ans, int& tm, int& mem);\

#ifdef __cplusplus
}
#endif

#endif // __MAIN_H__
