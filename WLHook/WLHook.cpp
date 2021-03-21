// DllMain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"

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
#define MAX_SOCKET_SERVER_COUNT 20

#pragma data_seg("WLShared")
DWORD			g_dwTarget = 0;
PROXYMAPPING	g_pmList[MAX_SOCKET_SERVER_COUNT] = { 0 };
DWORD			g_dwConnectionCount = 0;
#pragma data_seg()

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

		DisableThreadLibraryCalls(static_cast<HMODULE>(hModule));
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
	char str[INET_ADDRSTRLEN];
	inet_ntop(AF_INET, &addrIn->sin_addr, str, sizeof str);
	for (int i = 0; i < 1; i++) {
		if (g_pmList[i].isEnabled && strcmp(str, g_pmList[i].remoteIp) == 0 && addrIn->sin_port == g_pmList[i].remotePort) {
			inet_pton(AF_INET, g_pmList[i].localIp, &addrIn->sin_addr);
			addrIn->sin_port = g_pmList[i].localPort;
			g_dwConnectionCount++;
			break;
		}
	}
}

void WINAPI SetTargetPid(const DWORD dwPid)
{
	g_dwTarget = dwPid;
}

void WINAPI SetProxyMapping(PROXYMAPPING pmList[20])
{
	for (int i = 0; i < 20; i++) {
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
