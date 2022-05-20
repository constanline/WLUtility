// DllMain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"


#include <cstdio>
#include <WinSock2.h>
#include <WS2tcpip.h>

#include "HookApi.h"

#pragma comment(lib,"ws2_32.lib")
#pragma comment(linker,"/SECTION:WLShared,RWS")

typedef struct tagProxyMapping {
	char remoteIp[16];
	USHORT remotePort;
	char localIp[16];
	USHORT localPort;
	int isEnabled;
} PROXYMAPPING, *PPROXYMAPPING;

constexpr auto MAX_SOCKET_SERVER_COUNT = 4;

#pragma data_seg("WLShared")
DWORD			g_dwTarget = 0;
PROXYMAPPING	g_pmList[MAX_SOCKET_SERVER_COUNT] = { 0 };
DWORD			g_dwConnectionCount = 0;
#pragma data_seg()


void SimpleLog(const CHAR* logInfo)
{
	const auto dwAttribute = GetFileAttributes("IsDebug");
	if (INVALID_FILE_ATTRIBUTES == dwAttribute) return;

	CreateDirectory("Log", nullptr);
	
	auto* const hFile = CreateFile("Log\\Default.log", GENERIC_WRITE, FILE_SHARE_WRITE,
		nullptr, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, nullptr);
	
	if (INVALID_HANDLE_VALUE == hFile)
		return;

	const auto len = strlen(logInfo);

	DWORD out;
	SetFilePointer(hFile, 0, nullptr, FILE_END);
	
	WriteFile(hFile, logInfo, len, &out, nullptr);
	WriteFile(hFile, "\r\n", 2, &out, nullptr);

	if (hFile)
		CloseHandle(hFile);
}

void SimpleLog(const int i)
{
	char logInfo[16] = { 0 };
	sprintf_s(logInfo, "%d", i);
	SimpleLog(logInfo);
}

PHOOKENVIRONMENT g_pHookConnect = nullptr;

extern void ProxyConnect(SOCKADDR_IN* addrIn);

//-------------------------------------connect------------------------------------
typedef int (WSAAPI __pFnConnect)(
	SOCKET s,
	sockaddr FAR* addr,
	int len
);

int WSAAPI HookConnect(
	DWORD retAddr,
	__pFnConnect pFnConnect,
	const SOCKET s,
	sockaddr FAR* addr,
	const int len
)
{
	auto* const addrIn = reinterpret_cast<SOCKADDR_IN*>(addr);
	ProxyConnect(addrIn);

	const auto iRet = pFnConnect(s, addr, len);
	return iRet;
}

//-------------------------------------connect------------------------------------

BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:

		DisableThreadLibraryCalls(hModule);
		if (GetCurrentProcessId() == g_dwTarget) //必须在loadlibary之前设置g_dwTarget
		{
			g_pHookConnect = InstallHookApi("ws2_32.dll", "connect", HookConnect);
		}
		break;
	case DLL_PROCESS_DETACH:
		if (g_pHookConnect)
			UnInstallHookApi(g_pHookConnect);
		break;
	default:
		break;
	}
	return TRUE;
}

void ProxyConnect(SOCKADDR_IN* addrIn)
{
	char connIp[INET_ADDRSTRLEN];
	inet_ntop(AF_INET, &addrIn->sin_addr, connIp, sizeof connIp);
	
	for (auto& i : g_pmList)
	{
		if (i.isEnabled)
		{
			const auto connPort = ntohs(addrIn->sin_port);

			if (strcmp(connIp, i.remoteIp) == 0 && connPort == i.remotePort) {
				inet_pton(AF_INET, i.localIp, &addrIn->sin_addr);
				addrIn->sin_port = htons(i.localPort);
				g_dwConnectionCount++;
				break;
			}
		}
	}
}

void WINAPI SetTargetPid(const DWORD dwPid)
{
	g_dwTarget = dwPid;
}

void WINAPI SetProxyMapping(PROXYMAPPING pmList[])
{
	for (auto i = 0; i < MAX_SOCKET_SERVER_COUNT; i++) {
		if (pmList[i].isEnabled) {
			memcpy(&g_pmList[i], &pmList[i], sizeof(PROXYMAPPING));
		}
	}
}

void WINAPI DecConnectionCount()
{
	g_dwConnectionCount--;
}

DWORD WINAPI GetConnectionCount()
{
	return g_dwConnectionCount;
}
