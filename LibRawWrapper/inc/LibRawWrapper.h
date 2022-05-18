#pragma once
#include <libraw.h>
#include <string.h>
#include <vcclr.h>

#define FixedCharToString(c) CharToString(c, sizeof(c))

inline System::String^ CharToString(char* c, int maxlen) 
{
	if (!c[0]) return nullptr;
	int len = (int)strnlen(c, maxlen);
	return System::Text::Encoding::UTF8->GetString((unsigned char*)&c[0], len);
}

inline char* StringToChar(System::String^ s)
{
	if (s == nullptr)
		return 0;

	pin_ptr<const wchar_t> str = PtrToStringChars(s);

	int bytelen = System::Text::Encoding::Default->GetByteCount(s) + 1;
	unsigned char* pBytes = (unsigned char*)malloc(bytelen);
	if (pBytes)
	{
		memset(pBytes, 0, bytelen);
		System::Text::Encoding::Default->GetBytes((wchar_t*)str, s->Length, pBytes, bytelen);
	}
	return (char*)pBytes;
}

template <typename T>
inline array<T, 2>^ FixedToArray(T* t, int len1, int len2, bool returnZeroes = false)
{
	size_t bytelen = len1 * len2 * sizeof(T);
	array<T, 2>^ arr = gcnew array<T, 2>(len1, len2);

	if (arr->Length > 0)
	{
		pin_ptr<T> pArr = &arr[0, 0];

		if (returnZeroes || memcmp(pArr, t, bytelen))  // compare with zero-initialized arr
		{
			memcpy(pArr, t, bytelen);
			return arr;
		}
	}

	return nullptr;
}

template <typename T>
inline array<T>^ FixedToArray(T* t, int bytelen, bool returnZeroes = false)
{
	array<T>^ arr = gcnew array<T>(bytelen / sizeof(T));

	if (arr->Length > 0)
	{
		pin_ptr<T> pArr = &arr[0];
		
		if (returnZeroes || memcmp(pArr, t, bytelen))  // compare with zero-initialized arr
		{
			memcpy(pArr, t, bytelen);
			return arr;
		}
	}

	return nullptr;
}