/*
//////////////////////////////////////////////////////////////////////////
HookApi 0.6

thanks to xIkUg ,sucsor

by 海风月影[RCT] , StrongOD@Safengine.com
2011.06.08

//////////////////////////////////////////////////////////////////////////
//更新内容
2011.06.08	0.6
1，增加cdecl的hook
2，使用malloc申请内存，节约空间
3，新接口InstallHookStub，支持直接传函数地址去hook
4，hook还没完成的时候，不会发生调用hookproc的情况(主要是VirtualProtect函数)

2008.04.15  0.5

1，重新写了Stub，换了一种模式，使hook更加自由，将hookbefore和hookafter合并
HookProc的定义方式与以前有所不同：

HookProc的函数类型和原来的api一样，只是参数比原API多2个
DWORD WINAPI HookProc(DWORD RetAddr ,__pfnXXXX pfnXXXX, ...);

//参数比原始的API多2个参数
RetAddr	//调用api的返回地址
pfnXXX 	//类型为__pfnXXXX，待hook的api的声明类型，用于调用未被hook的api

详见My_LoadLibraryA
原始的LoadLibraryA的声明是：

HMODULE WINAPI LoadLibraryA( LPCSTR lpLibFileName );

那么首先定义一下hook的WINAPI的类型
typedef HMODULE (WINAPI __pfnLoadLibraryA)(LPCTSTR lpFileName);

然后hookproc的函数声明如下：
HMODULE WINAPI My_LoadLibraryA(DWORD RetAddr,
							   __pfnLoadLibraryA pfnLoadLibraryA,
							   LPCTSTR lpFileName
							   );

比原来的多了2个参数，参数位置不能颠倒，在My_LoadLibraryA中可以自由的调用未被hook的pfnLoadLibraryA
也可以调用系统的LoadLibraryA，不过要自己在hookproc中处理好重入问题

另外，也可以在My_LoadLibraryA中使用UnInstallHookApi()函数来卸载hook，用法如下：
将第二个参数__pfnLoadLibraryA pfnLoadLibraryA强制转换成PHOOKENVIRONMENT类型，使用UnInstallHookApi来卸载

例如：
UnInstallHookApi((PHOOKENVIRONMENT)pfnLoadLibraryA);


至于以前版本的HookBefore和HookAfter，完全可以在自己的HookProc里面灵活使用了


2，支持卸载hook
InstallHookApi()调用后会返回一个PHOOKENVIRONMENT类型的指针
需要卸载的时候可以使用UnInstallHookApi(PHOOKENVIRONMENT pHookEnv)来卸载

在HookProc中也可以使用UnInstallHookApi来卸载，参数传入HookProc中的第二个参数

注意：当HookProc中使用UnInstallHookApi卸载完后就不能用第二个参数来调用API了~~，切记！

2008.04.15  0.41
1，前面的deroko的LdeX86 有BUG，678b803412 会算错
	换了一个LDX32，代码更少，更容易理解

2，修复了VirtualProtect的一个小BUG


0.4以前
改动太大了，前面的就不写了
*/


#include "pch.h"
#include "HookApi.h"

#pragma comment(linker, "/SECTION:HookStub,R")

#define ALLOCATE_HookStub ALLOCATE(HookStub)

#pragma code_seg("HookStub")
#pragma optimize("gsy",on)
ALLOCATE_HookStub HOOKENVIRONMENT pEnv = { 0 };
NAKED void StubShell_stdcall()
{
	__asm
	{
		push dword ptr[esp];
		push dword ptr[esp];
		call _next;
	_next:
		xchg dword ptr[esp], eax;
		lea eax, [eax - 0x20];
		mov dword ptr[esp + 0xC], eax;
		pop eax;
		_emit 0xE9;
		_emit 'g';
		_emit 'o';
		_emit 'o';
		_emit 'd';
	}
}

NAKED void StubShell_cdecl()
{
	__asm
	{
		push dword ptr[esp];
		call _next;
	_next:
		xchg dword ptr[esp], eax;
		lea eax, [eax - 0x1D];
		mov dword ptr[esp + 0x8], eax;
		pop eax;
		_emit 0x68;
		_emit 'b';
		_emit 'a';
		_emit 'd';
		_emit 'd';
		_emit 0xE9;
		_emit 'g';
		_emit 'o';
		_emit 'o';
		_emit 'd';
	}
}
#pragma optimize("",off)
#pragma code_seg()

NAKED void cdeclret_stub()
{
	__asm retn 4;
}
DWORD MyInterlockedExchange32(PDWORD Target, DWORD Value)
{
	DWORD retvalue;
	__asm
	{
		mov ecx, Target;
		mov eax, Value;
		xchg dword ptr[ecx], eax;
		mov retvalue, eax;
	}
	return retvalue;
}
WORD MyInterlockedExchange16(PWORD Target, WORD Value)
{
	WORD retvalue;
	__asm
	{
		mov ecx, Target;
		mov ax, Value;
		xchg word ptr[ecx], ax;
		mov retvalue, ax;
	}
	return retvalue;
}

__inline void* _memcpy(void* to, const void* from, int size)
{
	char* dst = (char*)to;
	char* src = (char*)from;

	int new_len = size;

	while (new_len > 0)
	{
		*dst++ = *src++;
		new_len--;
	}
	return to;
}

void MyCopyBytes(void* pTarget, BYTE* pBuf, int size)
{
	/*
	优化 memcpy 字节
	先把stub的前2个字节改成 EB FE，构造一个死循环，然后memcpy剩下字节，再把前2个字节改回去
	*/
	WORD w1;
	w1 = *(WORD*)pBuf;
	MyInterlockedExchange16((PWORD)pTarget, 0xFEEB);
	_memcpy((char*)pTarget + 2, (char*)pBuf + 2, size - 2);
	MyInterlockedExchange16((PWORD)pTarget, w1);
}

DWORD __stdcall GetOpCodeSize(BYTE* iptr0)
{
	BYTE* iptr = iptr0;

	DWORD f = 0;

prefix:
	BYTE b = *iptr++;

	f |= table_1[b];

	if (f & C_FUCKINGTEST)
		if (((*iptr) & 0x38) == 0x00)   // ttt
			f = C_MODRM + C_DATAW0;       // TEST
		else
			f = C_MODRM;                // NOT,NEG,MUL,IMUL,DIV,IDIV

	if (f & C_TABLE_0F)
	{
		b = *iptr++;
		f = table_0F[b];
	}

	if (f == C_ERROR)
	{
		//printf("error in %02X\n",b);
		return C_ERROR;
	}

	if (f & C_PREFIX)
	{
		f &= ~C_PREFIX;
		goto prefix;
	}

	if (f & C_DATAW0) if (b & 0x01) f |= C_DATA66; else f |= C_DATA1;

	if (f & C_MODRM)
	{
		b = *iptr++;
		BYTE mod = b & 0xC0;
		BYTE rm = b & 0x07;
		if (mod != 0xC0)
		{
			if (f & C_67)         // modrm16
			{
				if ((mod == 0x00) && (rm == 0x06)) f |= C_MEM2;
				if (mod == 0x40) f |= C_MEM1;
				if (mod == 0x80) f |= C_MEM2;
			}
			else                // modrm32
			{
				if (mod == 0x40) f |= C_MEM1;
				if (mod == 0x80) f |= C_MEM4;
				if (rm == 0x04) rm = (*iptr++) & 0x07;    // rm<-sib.base
				if ((rm == 0x05) && (mod == 0x00)) f |= C_MEM4;
			}
		}
	} // C_MODRM

	if (f & C_MEM67)  if (f & C_67) f |= C_MEM2;  else f |= C_MEM4;
	if (f & C_DATA66) if (f & C_66) f |= C_DATA2; else f |= C_DATA4;

	if (f & C_MEM1)  iptr++;
	if (f & C_MEM2)  iptr += 2;
	if (f & C_MEM4)  iptr += 4;

	if (f & C_DATA1) iptr++;
	if (f & C_DATA2) iptr += 2;
	if (f & C_DATA4) iptr += 4;

	return iptr - iptr0;
}

HANDLE g_hStupHeap = NULL;

PHOOKENVIRONMENT __stdcall InstallHookStub(PVOID StubAddress, PVOID HookProc, int type)
{
	int ReplaceCodeSize;
	DWORD oldpro;
	DWORD SizeOfStub;

	DWORD SizeOfStubShell = 0;
	DWORD AddrOfStubShell = 0;

	DWORD dwHookStubAddress;
	DWORD RetSize = 0;

	PHOOKENVIRONMENT pHookEnv;

	BYTE JMPGate[5] = { 0xE9, 0x00, 0x00, 0x00, 0x00 };

	if (HookProc == NULL)
	{
		return NULL;
	}

	if (StubAddress == NULL) return NULL;

	if (*(BYTE*)StubAddress == 0xE9 || *(BYTE*)StubAddress == 0xE8) return NULL;

	if (type == e_stdcall)
	{
		SizeOfStubShell = 0x1B;
		AddrOfStubShell = (DWORD)StubShell_stdcall;
	}
	else if (type == e_cdecl)
	{
		SizeOfStubShell = 0x1D;
		AddrOfStubShell = (DWORD)StubShell_cdecl;
	}

#ifdef _DEBUG
	AddrOfStubShell = AddrOfStubShell + 5 + *(DWORD*)(AddrOfStubShell + 1);
#endif

	ReplaceCodeSize = GetOpCodeSize((BYTE*)StubAddress);

	while (ReplaceCodeSize < 5)
	{
		ReplaceCodeSize += GetOpCodeSize((BYTE*)((DWORD)StubAddress + (DWORD)ReplaceCodeSize));
	}

	if (ReplaceCodeSize > 16) return NULL;

	SizeOfStub = SizeOfStubShell + sizeof(HOOKENVIRONMENT);

	if (g_hStupHeap == NULL)
	{
		g_hStupHeap = HeapCreate(HEAP_CREATE_ENABLE_EXECUTE, 0, 0);
		if (g_hStupHeap == NULL)
		{
			return NULL;
		}
	}

	pHookEnv = (PHOOKENVIRONMENT)HeapAlloc(g_hStupHeap, 0, sizeof(HOOKENVIRONMENT));

	if (pHookEnv == NULL)
	{
		return pHookEnv;
	}

	_memcpy(pHookEnv, (PVOID)&pEnv, sizeof(HOOKENVIRONMENT));
	memset((void*)pHookEnv->savebytes, 0x90, sizeof(pHookEnv->savebytes));
	_memcpy((void*)pHookEnv->hookstub, (PVOID)AddrOfStubShell, SizeOfStubShell);
	_memcpy(pHookEnv->savebytes, StubAddress, ReplaceCodeSize);

	pHookEnv->OrgApiAddr = StubAddress;
	pHookEnv->SizeOfReplaceCode = ReplaceCodeSize;

	pHookEnv->jmptoapi[0] = 0xE9;
	*(DWORD*)(&pHookEnv->jmptoapi[1]) = (DWORD)StubAddress + ReplaceCodeSize - ((DWORD)pHookEnv->jmptoapi + 5);

	dwHookStubAddress = (DWORD)pHookEnv->hookstub;

	pHookEnv->jmptostub[0] = 0xE9;
	*(DWORD*)(&pHookEnv->jmptostub[1]) = (DWORD)pHookEnv->savebytes - ((DWORD)pHookEnv->jmptostub + 5);
	//*(DWORD*)(&pHookEnv->jmptostub[1]) = (DWORD)(dwHookStubAddress) - ((DWORD)pHookEnv->jmptostub + 5);

	*(DWORD*)(&JMPGate[1]) = ((DWORD)pHookEnv->jmptostub) - ((DWORD)StubAddress + 5);

	//写入变量，这里要先写变量，否则如果hook VirtualProtect，下面的api调用会出问题
	if (type == e_stdcall)
	{
		*(DWORD*)(dwHookStubAddress + SizeOfStubShell - 4) = (DWORD)HookProc - (dwHookStubAddress + SizeOfStubShell);
	}
	else if (type == e_cdecl)
	{
		*(DWORD*)(dwHookStubAddress + SizeOfStubShell - 4) = (DWORD)HookProc - (dwHookStubAddress + SizeOfStubShell);
		*(DWORD*)(dwHookStubAddress + SizeOfStubShell - 9) = (DWORD)cdeclret_stub;
	}

	//patch api
	if (VirtualProtect(StubAddress, ReplaceCodeSize, PAGE_EXECUTE_READWRITE, &oldpro))
	{
		//memcpy(StubAddress, JMPGate, sizeof(JMPGate));
		MyCopyBytes((void*)StubAddress, (BYTE*)JMPGate, sizeof(JMPGate));
		VirtualProtect(StubAddress, ReplaceCodeSize, oldpro, &oldpro);
		MyInterlockedExchange32((PDWORD)(&pHookEnv->jmptostub[1]), (DWORD)(dwHookStubAddress)-((DWORD)pHookEnv->jmptostub + 5));
	}
	else
	{
		//失败了，无法hook
		HeapFree(g_hStupHeap, 0, (void*)pHookEnv);
		return NULL;
	}
	return pHookEnv;
}

PHOOKENVIRONMENT __stdcall InstallHookApi(PCCH DllName, PCCH ApiName, PVOID HookProc, int type)
{
	return InstallHookStub((PVOID)GetProcAddress(LoadLibraryA(DllName), ApiName), HookProc, type);
}

BOOL __stdcall UnInstallHookApi(PHOOKENVIRONMENT pHookEnv)
{
	DWORD oldpro;

	//如果内存不存在了，则退出
	if (HeapSize(g_hStupHeap, 0, (void*)pHookEnv) <= 0)
		return FALSE;

	if (IsBadReadPtr((const void*)pHookEnv, sizeof(HOOKENVIRONMENT)))
		return FALSE;

	if (!VirtualProtect(pHookEnv->OrgApiAddr, pHookEnv->SizeOfReplaceCode, PAGE_EXECUTE_READWRITE, &oldpro))
		return FALSE;

	//memcpy(pHookEnv->OrgApiAddr, pHookEnv->savebytes, pHookEnv->SizeOfReplaceCode);
	MyCopyBytes((void*)pHookEnv->OrgApiAddr, (BYTE*)pHookEnv->savebytes, pHookEnv->SizeOfReplaceCode);

	VirtualProtect(pHookEnv->OrgApiAddr, pHookEnv->SizeOfReplaceCode, oldpro, &oldpro);

	HeapFree(g_hStupHeap, 0, (void*)pHookEnv);
	return TRUE;
}

