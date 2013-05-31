using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

#if X86 || AMD64
namespace PcreSharp
{
	internal static class PcreWrapper
	{
		#region PCRE NATIVE API INDIRECTED FUNCTIONS
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void FreeDelegate(IntPtr ptr);
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate IntPtr AllocDelegate(int size);
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
		internal delegate int CalloutDelegate(IntPtr data);

		internal static CalloutDelegate pcre_callout
		{
			set
			{
				IntPtr ptr = Marshal.GetFunctionPointerForDelegate(value);
				Marshal.WriteIntPtr(iptr_pcre_callout, ptr);
			}
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr LoadLibrary(string lpszLib);

		internal static readonly FreeDelegate pcre_free;
		internal static readonly FreeDelegate pcre_stack_free;
		internal static readonly AllocDelegate pcre_malloc;
		internal static readonly AllocDelegate pcre_stack_malloc;

		private static IntPtr iptr_pcre_callout;

#if X86
        private const string DllName = "pcre.dll";
#elif AMD64
		private const string DllName = "pcre64.dll";
#endif

		static PcreWrapper()
		{
			IntPtr dllHandle = LoadLibrary(DllName);

			if (dllHandle != IntPtr.Zero)
			{
				IntPtr ptr;
				ptr = GetVarFromDll(dllHandle, "pcre_free");
				pcre_free = (FreeDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(FreeDelegate));
				ptr = GetVarFromDll(dllHandle, "pcre_stack_free");
				pcre_stack_free = (FreeDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(FreeDelegate));
				ptr = GetVarFromDll(dllHandle, "pcre_malloc");
				pcre_malloc = (AllocDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(AllocDelegate));
				ptr = GetVarFromDll(dllHandle, "pcre_stack_malloc");
				pcre_stack_malloc = (AllocDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(AllocDelegate));
				iptr_pcre_callout = GetProcAddress(dllHandle, "pcre_callout");
			}
		}

		private static IntPtr GetVarFromDll(IntPtr handle, string name)
		{
			IntPtr addr = GetProcAddress(handle, name);

			if (addr != IntPtr.Zero)
			{
				return Marshal.ReadIntPtr(addr);
			}

			return IntPtr.Zero;
		}

		#endregion

		#region PCRE NATIVE API BASIC FUNCTIONS

		/*
		 pcre *pcre_compile(const char *pattern, int options,
            const char **errptr, int *erroffset,
            const unsigned char *tableptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe IntPtr pcre_compile(byte* pattern, int options,
														  out IntPtr errptr, ref int erroffset,
														  IntPtr tableptr);

		/*
		 pcre *pcre_compile2(const char *pattern, int options,
			int *errorcodeptr,
			const char **errptr, int *erroffset,
			const unsigned char *tableptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe IntPtr pcre_compile2(byte* pattern, int options,
														   ref int errorcodeptr,
														   out IntPtr errptr, ref int erroffset,
														   IntPtr tableptr);


		/*
		 pcre_extra *pcre_study(const pcre *code, int options,
			const char **errptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr pcre_study(IntPtr code, int options,
												 out IntPtr errptr);


		/*
		 void pcre_free_study(pcre_extra *extra);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void pcre_free_study(IntPtr extra);


		/*
		 int pcre_exec(const pcre *code, const pcre_extra *extra,
			const char *subject, int length, int startoffset,
			int options, int *ovector, int ovecsize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_exec(IntPtr code, IntPtr extra,
													byte* subject, int length, int startoffset,
													int options, int* ovector, int ovecsize);


		/*
		 int pcre_dfa_exec(const pcre *code, const pcre_extra *extra,
			const char *subject, int length, int startoffset,
			int options, int *ovector, int ovecsize,
			int *workspace, int wscount);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_dfa_exec(IntPtr code, IntPtr extra,
														byte* subject, int length, int startoffset,
														int options, int* ovector, int ovecsize,
														int* workspace, int wscount);

		#endregion

		#region PCRE NATIVE API STRING EXTRACTION FUNCTIONS

		/*
		 int pcre_copy_named_substring(const pcre *code,
            const char *subject, int *ovector,
            int stringcount, const char *stringname,
            char *buffer, int buffersize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_copy_named_substring(IntPtr code,
																	byte* subject, int* ovector,
																	int stringcount, byte* stringname,
																	byte* buffer, int buffersize);


		/*
		 int pcre_copy_substring(const char *subject, int *ovector,
			int stringcount, int stringnumber, char *buffer,
			int buffersize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_copy_substring(byte* subject, int* ovector,
															  int stringcount, int stringnumber, byte* buffer,
															  int buffersize);


		/*
		 int pcre_get_named_substring(const pcre *code,
			const char *subject, int *ovector,
			int stringcount, const char *stringname,
			const char **stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_get_named_substring(IntPtr code,
																   byte* subject, int* ovector,
																   int stringcount, byte* stringname,
																   out byte* stringptr);


		/*
		 int pcre_get_stringnumber(const pcre *code,
			const char *name);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_get_stringnumber(IntPtr code,
																byte* name);


		/*
		 int pcre_get_stringtable_entries(const pcre *code,
			const char *name, char **first, char **last);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_get_stringtable_entries(IntPtr code,
																	   byte* name, out byte* first, out byte* last);


		/*
		 int pcre_get_substring(const char *subject, int *ovector,
			int stringcount, int stringnumber,
			const char **stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_get_substring(byte* subject, int* ovector,
															 int stringcount, int stringnumber,
															 out IntPtr stringptr);


		/*
		 int pcre_get_substring_list(const char *subject,
			int *ovector, int stringcount, const char ***listptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_get_substring_list(byte* subject,
																  int* ovector, int stringcount, out IntPtr[] listptr);


		/*
		 void pcre_free_substring(const char *stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void pcre_free_substring(IntPtr stringptr);


		/*
		 void pcre_free_substring_list(const char **stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void pcre_free_substring_list(IntPtr[] stringptr);

		#endregion

		#region PCRE NATIVE API AUXILIARY FUNCTIONS

		/*
		 int pcre_jit_exec(const pcre *code, const pcre_extra *extra,
            const char *subject, int length, int startoffset,
            int options, int *ovector, int ovecsize,
            pcre_jit_stack *jstack);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe int pcre_jit_exec(IntPtr code, IntPtr extra,
														byte* subject, int length, int startoffset,
														int options, int* ovector, int ovecsize,
														IntPtr jstack);


		/*
		 pcre_jit_stack *pcre_jit_stack_alloc(int startsize, int maxsize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr pcre_jit_stack_alloc(int startsize, int maxsize);


		/*
		 void pcre_jit_stack_free(pcre_jit_stack *stack);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void pcre_jit_stack_free(IntPtr stack);


		/*
		 void pcre_assign_jit_stack(pcre_extra *extra,
			pcre_jit_callback callback, void *data);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void pcre_assign_jit_stack(IntPtr extra,
														  IntPtr callback, IntPtr data);


		/*
		 const unsigned char *pcre_maketables(void);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr pcre_maketables();


		/*
		 int pcre_fullinfo(const pcre *code, const pcre_extra *extra,
			int what, void *where);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int pcre_fullinfo(IntPtr code, IntPtr extra,
												 int what, IntPtr where);


		/*
		 int pcre_refcount(pcre *code, int adjust);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int pcre_refcount(IntPtr code, int adjust);


		/*
		 int pcre_config(int what, void *where);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int pcre_config(int what, IntPtr where);


		/*
		 const char *pcre_version(void);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern string pcre_version();


		/*
		 int pcre_pattern_to_host_byte_order(pcre *code,
			pcre_extra *extra, const unsigned char *tables);
		 */

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int pcre_pattern_to_host_byte_order(IntPtr code,
																   IntPtr extra, IntPtr tables);

		#endregion
	}
}
#endif