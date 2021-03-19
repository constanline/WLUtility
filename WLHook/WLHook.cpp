#include "pch.h"

#include <map>
#include <WinSock2.h>
#include <WS2tcpip.h>

#pragma comment(lib,"ws2_32.lib")

extern CHAR		g_sendName[20];
extern HWND		g_hWndClient;
extern DWORD	g_dwTarget;

const WORD	POS_NOT_FOUND = 0xFFFF;
const BYTE	ANY_CHAR = 0xFF;
const BYTE	XOR_BYTE = 0x6E;
const WORD	XOR_WORD = 0x6E6E;
const DWORD	XOR_DWORD = 0x6E6E6E6E;

typedef struct tagSOCKETINFO
{
	DWORD id;
	BOOL needWaiting;
	BOOL interruptTask;
	BOOL needOffsetSkill;
	BOOL needOffsetPet;
	CHAR chip[50000];
	WORD lenChip;
	WORD lenPacket;
	BOOL ackMapMonster;
	BOOL allowMove;
} SOCKETINFO, *PSOCKETINFO;

void PrintLog(const CHAR* logInfo, int len = NULL, const DWORD id = NULL, const BYTE printLen = 1)
{
	const auto dwAttribute = GetFileAttributes("IsDebug");
	if (INVALID_FILE_ATTRIBUTES == dwAttribute) return;

	CreateDirectory("Log", nullptr);

	auto* const logFileName = new CHAR[20];
	if (id != NULL)
	{
		if (id == 999)
		{
			sprintf_s(logFileName, sizeof logFileName, "Log\\Adjust.log");
		}
		else
		{
			sprintf_s(logFileName, sizeof logFileName, "Log\\%d.log", id);
		}
	}
	else
	{
		sprintf_s(logFileName, sizeof logFileName, "Log\\Default.log");
	}
	auto* const hFile = CreateFile(logFileName, GENERIC_WRITE, FILE_SHARE_WRITE, 
	                               nullptr, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, nullptr);
	delete[] logFileName;
	if (INVALID_HANDLE_VALUE == hFile)
		return;

	if (len == NULL)
		len = strlen(logInfo);

	DWORD out;
	SetFilePointer(hFile, 0, nullptr, FILE_END);

	if (printLen)
	{
		auto* cLen = new char[7];
		sprintf_s(cLen, sizeof cLen, "%d|", len);
		WriteFile(hFile, cLen, strlen(cLen), &out, nullptr);
		delete[] cLen;
	}
	WriteFile(hFile, logInfo, len, &out, nullptr);
	WriteFile(hFile, "\r\n", 2, &out, nullptr);
	
	if (hFile)
		CloseHandle(hFile);
}

void SwitchAddress(SOCKADDR_IN* addrIn)
{
	char str[INET_ADDRSTRLEN];
	inet_ntop(AF_INET, &addrIn->sin_addr, str, sizeof str);
	if(strcmp(str, "123.123.123.123") == 0)
	{
		inet_pton(AF_INET, "127.0.0.1", &addrIn->sin_addr);
	}
}

void WINAPI SetTargetPid(const DWORD dwPid)
{
	g_dwTarget = dwPid;
}
DWORD WINAPI GetTargetPid()		//为多开补丁特别设置的
{
	return g_dwTarget;
}
