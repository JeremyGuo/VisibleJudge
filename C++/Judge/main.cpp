#include "main.h"

// a sample exported function
void DLL_EXPORT SomeFunction(const LPCSTR sometext)
{
    MessageBoxA(0, sometext, "DLL Message", MB_OK | MB_ICONINFORMATION);
}

void completeClose(STARTUPINFO& si, PROCESS_INFORMATION& pi){
    TerminateProcess(pi.hProcess, 0);
    CloseHandle(si.hStdError);
    CloseHandle(si.hStdOutput);
    CloseHandle(si.hStdInput);
}

void Find_Char(int & c1, int & c2, FILE *& f1, FILE *& f2, int & ret){
    while ((isspace(c1)) || (isspace(c2)))
    {
        if (c1 != c2)
        {
            if (c2 == EOF) {
                do {
                    c1 = fgetc(f1);
                } while (isspace(c1));
                continue;
            }
            else if (c1 == EOF) {
                do {
                    c2 = fgetc(f2);
                } while (isspace(c2));
                continue;
            }
            else if ((c1 == '\r' && c2 == '\n')) c1 = fgetc(f1);
            else if ((c2 == '\r' && c1 == '\n')) c2 = fgetc(f2);
            else ret = PERMUTATIONERROR;
        }
        if (isspace(c1)) c1 = fgetc(f1);
        if (isspace(c2)) c2 = fgetc(f2);
    }
}

int Compare(LPCSTR Std, LPCSTR User){
    FILE* stf = fopen((char*)Std, "r");
    FILE* usf = fopen((char*)User, "r");
    if(!stf || !usf)
        return RUNTIMEERROR;
    int ret = ACCEPT, c1=-1, c2=-1;
    while(true){
        c1 = fgetc(stf);
        c2 = fgetc(usf);
        Find_Char(c1, c2, stf, usf, ret);
        while(true) {
            while ((!isspace(c1) && c1) || (!isspace(c2) && c2)) {
                if (c1 == EOF && c2 == EOF){
                    fclose(stf);
                    fclose(usf);
                    return ret;
                }
                if (c1 == EOF || c2 == EOF)
                    break;
                if (c1 != c2){
                    fclose(stf);
                    fclose(usf);
                    return WRONGANSWER;
                }
                c1 = fgetc(stf);
                c2 = fgetc(usf);
            }
            Find_Char(c1, c2, stf, usf, ret);
            if (c1 == EOF && c2 == EOF){
                fclose(stf);
                fclose(usf);
                return ret;
            }
            if (c1 == EOF || c2 == EOF){
                fclose(stf);
                fclose(usf);
                return WRONGANSWER;
            }
            if ((c1 == '\n' || !c1) && (c2 == '\n' || !c2)) break;
        }
    }
    return ret;
}

void write_info(LPCSTR path){
    FILE* fp = fopen((char*)path, "w");
    if(!fp) return ;
    fprintf(fp, "Testing");
    fclose(fp);
}

void DLL_EXPORT Compile(const LPCSTR name, const LPCSTR args){
    SetErrorMode(SEM_NOGPFAULTERRORBOX);

    char cmd[1024];
    sprintf(cmd, "g++ %s.cpp -o -Wall %s %s", (char*)name, (char*)name, (char*)args);
    system(cmd);
}

void DLL_EXPORT Judge(const LPCSTR exe, const LPCSTR indata, const LPCSTR outdata, const LPCSTR userdata, const LPCSTR result, int& ans, int &tm, int &mem){
    SetErrorMode(SEM_NOGPFAULTERRORBOX);

    STARTUPINFO si;
    PROCESS_INFORMATION pi;
    SECURITY_ATTRIBUTES sa;

    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    si.dwFlags = STARTF_USESTDHANDLES;
    ZeroMemory(&pi, sizeof(pi));
    ZeroMemory(&sa, sizeof(sa));
    sa.bInheritHandle = TRUE;

    si.hStdInput = CreateFile(indata, GENERIC_READ,
                                  FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE, &sa,
                                  OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

    si.hStdOutput = CreateFile(userdata, GENERIC_WRITE,
                                   FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE, &sa,
                                   CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

    if(!CreateProcess(exe, NULL, NULL, &sa, TRUE, HIGH_PRIORITY_CLASS | CREATE_NO_WINDOW, NULL, NULL, &si, &pi)){
        ans = UNABLEOPENEXE;
        return ;
    }
    WaitForSingleObject(pi.hProcess, INFINITE);
    unsigned long exitCode;
    GetExitCodeProcess(pi.hProcess, &exitCode);
    if(exitCode != 0){
        completeClose(si, pi);
        ans = RUNTIMEERROR;
        return ;
    }
    FILETIME creationTime, exitTime, kernelTime, userTime;
    GetProcessTimes(pi.hProcess, &creationTime, &exitTime, &kernelTime, &userTime);

    SYSTEMTIME realTime;
    FileTimeToSystemTime(&userTime, &realTime);
    tm = realTime.wMilliseconds
               + realTime.wSecond * 1000
               + realTime.wMinute * 60 * 1000
               + realTime.wHour * 60 * 60 * 1000;

    PROCESS_MEMORY_COUNTERS_EX info;
    GetProcessMemoryInfo(pi.hProcess, (PROCESS_MEMORY_COUNTERS*)&info, sizeof(info));
    mem = info.PeakWorkingSetSize;

    if((ans = Compare(outdata, userdata)) == WRONGANSWER)
        write_info(result);
    completeClose(si, pi);
}

extern "C" DLL_EXPORT BOOL APIENTRY DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
    switch (fdwReason)
    {
        case DLL_PROCESS_ATTACH:
            // attach to process
            // return FALSE to fail DLL load
            break;

        case DLL_PROCESS_DETACH:
            // detach from process
            break;

        case DLL_THREAD_ATTACH:
            // attach to thread
            break;

        case DLL_THREAD_DETACH:
            // detach from thread
            break;
    }
    return TRUE; // succesful
}
