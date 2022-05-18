#pragma once

#include <vcclr.h>
#include <gcroot.h>
#include <libraw.h>
#include <libraw_datastream.h>
#include "LibRawWrapper.h"

using namespace System::IO;

class LibRaw_managed_datastream : public LibRaw_abstract_datastream
{
public:
	LibRaw_managed_datastream(Stream^ stream)
	{
		m_stream = stream;
	}

	~LibRaw_managed_datastream() 
	{
		delete m_stream; 
	}

	virtual int valid()
	{
		if (System::Object::ReferenceEquals(m_stream, nullptr))
			return 0;

		if (!m_stream->CanRead)
			return 0;

		return 1;
	}

#ifdef LIBRAW_OLD_VIDEO_SUPPORT
	virtual void* make_jas_stream() 
	{
		return NULL;
	}
#endif

	virtual int jpeg_src(void* jpegdata) 
	{ 
		return -1; 
	}	

	virtual int read(void* ptr, size_t sz, size_t nmemb)
	{
		size_t to_read = sz * nmemb;
		array<byte>^ buffer = gcnew array<byte>(to_read); // TODO: check whether we can construct managed array from ptr
		int read = m_stream->Read(buffer, 0, to_read);
		pin_ptr<byte> pBuffer = &buffer[0];
#ifndef WIN32SECURECALLS
		memcpy(ptr, pBuffer, read);
#else
		memcpy_s(ptr, to_read, pBuffer, to_read);
#endif
		return int((read + sz - 1) / (sz > 0 ? sz : 1));
	}

	virtual int eof() { return m_stream->Position >= m_stream->Length; }
	virtual int seek(INT64 o, int whence)
	{
		switch (whence)
		{
		case SEEK_SET:
			m_stream->Seek(o, SeekOrigin::Begin);
			return 0;
		case SEEK_CUR:
			m_stream->Seek(o, SeekOrigin::Current);
			return 0;
		case SEEK_END:
			m_stream->Seek(o, SeekOrigin::End);
			return 0;
		default:
			return 0;
		}
	}	
	virtual INT64 tell() 
	{
		return m_stream->Position; 
	}
	virtual INT64 size()
	{
		return m_stream->Length;
	}

	virtual char* gets(char* s, int sz)
	{
		if (sz < 1) return NULL;
		if (eof()) return NULL;
		array<byte>^ buffer = gcnew array<byte>(sz); // LibRaw_buffer_datastream sets rest to zeroes; last one is trailing
		pin_ptr<byte> pBuffer = &buffer[0];

		int i = 0;
		while (i < (sz-1)) // we are supposed to read sz-1 bytes, sz is trailing null
		{
			int ch = m_stream->ReadByte();
			if (ch < 0) 
				break;
			
			pBuffer[i++] = ch;
			
			if (ch == '\n')
				break;
		}

#ifndef WIN32SECURECALLS
		memcpy(s, pBuffer, sz);
#else
		memcpy_s(s, sz, pBuffer, sz);
#endif
		return s;
	}

	virtual int scanf_one(const char* fmt, void* val)
	{
		if (eof()) return 0;

		// LibRaw_buffer_datastream calls sscanf directly on buffer and then tries to advance the stream
		// until eof, \0, \t, \n, or a space, up to maximum of 24 characters.
		
		// However, we should not advance past parsing errors. One option is to add char by char and call
		// sscanf repeatedly until it fails. Another is to assume the stream is seekable and revert the position,
		// but sscanf does not tell us where the error occurs.

		// Let's be optimistic and assume the input is well formated.
		
		INT64 position = m_stream->Position;
		char buffer[24] = { 0 };
		int i = 0;
		while (i < 24)
		{
			int c = m_stream->ReadByte();
			if (c < 0) break;
			if (c == 0 || c == ' ' || c == '\t' || c == '\n') break;
			buffer[i++] = c;
		}

#ifndef WIN32SECURECALLS
		int count = sscanf(buffer, fmt, val);
#else
		int count = sscanf_s(buffer, fmt, val);
#endif

		if (count < 1 && i > 0) // there is data in buffer but parsing failed
			throw LIBRAW_EXCEPTION_IO_BADFILE;

		return count;
	}

	virtual int get_char() 
	{
		return m_stream->ReadByte();
	}

	virtual const char* fname()
	{
		FileStream^ fileStream = dynamic_cast<FileStream^>((Stream^)m_stream);
		if (fileStream)
			return StringToChar(fileStream->Name);

		return NULL;
	}

#ifdef LIBRAW_WIN32_UNICODEPATHS
	virtual const wchar_t* wfname()
	{
		FileStream^ fileStream = dynamic_cast<FileStream^>((Stream^)m_stream);
		if (fileStream)
		{
			pin_ptr<const wchar_t> str = PtrToStringChars(fileStream->Name);
			int bytelen = (fileStream->Name->Length + 1) * sizeof(wchar_t);
			wchar_t* pBytes = (wchar_t*)malloc(bytelen);
			if (pBytes)
			{
#ifndef WIN32SECURECALLS
				memcpy(pBytes, &str[0], bytelen - sizeof(wchar_t));
#else
				memcpy_s(pBytes, bytelen, &str[0], bytelen - sizeof(wchar_t));
#endif
				return pBytes;
			}
		}

		return NULL;
	}
#endif

private:
	gcroot<Stream^> m_stream;
};

