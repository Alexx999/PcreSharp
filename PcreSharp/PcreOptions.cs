﻿using System;

namespace PcreSharp
{
	[Flags]
	public enum PcreOptions
	{
		NONE = 0,
		PCRE_CASELESS = 0x00000001,
		PCRE_MULTILINE = 0x00000002,
		PCRE_DOTALL = 0x00000004,
		PCRE_EXTENDED = 0x00000008,
		PCRE_ANCHORED = 0x00000010,
		PCRE_DOLLAR_ENDONLY = 0x00000020,
		PCRE_EXTRA = 0x00000040,
		PCRE_NOTBOL = 0x00000080,
		PCRE_NOTEOL = 0x00000100,
		PCRE_UNGREEDY = 0x00000200,
		PCRE_NOTEMPTY = 0x00000400,
		PCRE_UTF8 = 0x00000800,
		//PCRE_UTF16             = 0x00000800,
		//PCRE_UTF32             = 0x00000800,
		PCRE_NO_AUTO_CAPTURE = 0x00001000,
		PCRE_NO_UTF8_CHECK = 0x00002000,
		//PCRE_NO_UTF16_CHECK    = 0x00002000,
		//PCRE_NO_UTF32_CHECK    = 0x00002000,
		PCRE_AUTO_CALLOUT = 0x00004000,
		PCRE_PARTIAL_SOFT = 0x00008000,
		//PCRE_PARTIAL           = 0x00008000,
		PCRE_DFA_SHORTEST = 0x00010000,
		PCRE_DFA_RESTART = 0x00020000,
		PCRE_FIRSTLINE = 0x00040000,
		PCRE_DUPNAMES = 0x00080000,
		PCRE_NEWLINE_CR = 0x00100000,
		PCRE_NEWLINE_LF = 0x00200000,
		PCRE_NEWLINE_CRLF = 0x00300000,
		PCRE_NEWLINE_ANY = 0x00400000,
		PCRE_NEWLINE_ANYCRLF = 0x00500000,
		PCRE_BSR_ANYCRLF = 0x00800000,
		PCRE_BSR_UNICODE = 0x01000000,
		PCRE_JAVASCRIPT_COMPAT = 0x02000000,
		PCRE_NO_START_OPTIMIZE = 0x04000000,
		//PCRE_NO_START_OPTIMISE = 0x04000000,
		PCRE_PARTIAL_HARD = 0x08000000,
		PCRE_NOTEMPTY_ATSTART = 0x10000000,
		PCRE_UCP = 0x20000000
	}
}
