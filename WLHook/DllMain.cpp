// DllMain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"

#include <WinSock2.h>

#include "HookApi.h"

#pragma comment(linker,"/SECTION:BBShared,RWS")

#pragma data_seg("BBShared")	//共享段必须初始化！
HWND	g_hWndClient = nullptr; // SetClientHWnd
DWORD	g_dwTarget = 0; //由于主程序也会加载，为避免混淆多一个这个
CHAR	g_sendName[20] = { 0 };
#pragma data_seg()

CRITICAL_SECTION g_CriticalSection;

PHOOKENVIRONMENT g_pHookConnect = nullptr;
PHOOKENVIRONMENT g_pHookSend = nullptr;
PHOOKENVIRONMENT g_pHookRecv = nullptr;
PHOOKENVIRONMENT g_pHookWSARecv = nullptr;

extern void SwitchAddress(SOCKADDR_IN* addrIn);

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
	SwitchAddress(addrIn);

	const auto iRet = pFnConnect(s, addr, len);
	return iRet;
}

//-------------------------------------connect------------------------------------

//---------------------------------------recv-------------------------------------

typedef int (WSAAPI __pFnRecv)(
	SOCKET s,
	char FAR* buf,
	int len,
	int flags
);

int WSAAPI HookRecv(
	DWORD retAddr,
	__pFnRecv pFnRecv,
	SOCKET s,
	char FAR* buf,
	int len,
	int flags
)
{
	const auto iRet = pFnRecv(
		s,
		buf,
		len,
		flags
	);
	return iRet;
}

typedef int (WSAAPI __pFnWSARecv)(
	SOCKET s,
	char FAR* buf,
	int len,
	int flags
);

int WSAAPI HookWSARecv(
	DWORD retAddr,
	__pFnWSARecv pFnWSARecv,
	SOCKET s,
	char FAR* buf,
	int len,
	int flags
)
{
	const auto iRet = pFnWSARecv(
		s,
		buf,
		len,
		flags
	);
	return iRet;
}

//---------------------------------------recv---------------------------------------

//---------------------------------------send-------------------------------------
typedef int (WSAAPI __pFnSend)(
	SOCKET s,
	const char FAR* buf,
	int len,
	int flags
);

int WSAAPI HookSend(
	DWORD retAddr,
	__pFnSend pFnSend,
	SOCKET s,
	char FAR* buf,
	int len,
	int flags
)
{
	return pFnSend(s,
	               buf,
	               len,
	               flags
	);
}

//-------------------------------------send--------------------------------------

BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:

		DisableThreadLibraryCalls(static_cast<HMODULE>(hModule)); //屏蔽 DLL_THREAD_ATTACH 和 DLL_THREAD_DETACH
		if (GetCurrentProcessId() == g_dwTarget) //必须在loadlibary之前设置g_dwTarget
		{
			g_pHookConnect = InstallHookApi("ws2_32.dll", "connect", HookConnect);
			g_pHookSend = InstallHookApi("ws2_32.dll", "send", HookSend);
			g_pHookRecv = InstallHookApi("ws2_32.dll", "recv", HookRecv);
			g_pHookWSARecv = InstallHookApi("wsock32.dll", "recv", HookWSARecv);
			InitializeCriticalSection(&g_CriticalSection);
		}
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		if (g_pHookConnect)
			UnInstallHookApi(g_pHookConnect);
		if (g_pHookSend)
			UnInstallHookApi(g_pHookSend);
		if (g_pHookRecv)
			UnInstallHookApi(g_pHookRecv);
		if (g_pHookWSARecv)
			UnInstallHookApi(g_pHookWSARecv);
		DeleteCriticalSection(&g_CriticalSection);
		break;
	default:
		break;
	}
	return TRUE;
}
