using System;

namespace PcreSharp
{
	[Flags]
	public enum PcreStudyOptions
	{
		NONE = 0,
		PCRE_STUDY_JIT_COMPILE = 0x0001,
		PCRE_STUDY_JIT_PARTIAL_SOFT_COMPILE = 0x0002,
		PCRE_STUDY_JIT_PARTIAL_HARD_COMPILE = 0x0004,
		PCRE_STUDY_EXTRA_NEEDED = 0x0008
	}
}
