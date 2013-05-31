using System;
using System.Runtime.InteropServices;
using System.Security;

#if ANY_CPU
namespace PcreSharp
{
	internal static class PcreWrapper
	{

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr LoadLibrary(string lpszLib);

		private static readonly IntPtr iptr_pcre_callout;

		private static readonly IntPtr _dllHandle;

		private static readonly string DllName;

		static PcreWrapper()
		{
			DllName = IntPtr.Size == 4 ? "pcre.dll" : "pcre64.dll";

			_dllHandle = LoadLibrary(DllName);

			if (_dllHandle != IntPtr.Zero)
			{
				IntPtr ptr;
				ptr = GetVarFromDll(_dllHandle, "pcre_free");
				pcre_free = (FreeDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(FreeDelegate));
				ptr = GetVarFromDll(_dllHandle, "pcre_stack_free");
				pcre_stack_free = (FreeDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(FreeDelegate));
				ptr = GetVarFromDll(_dllHandle, "pcre_malloc");
				pcre_malloc = (AllocDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(AllocDelegate));
				ptr = GetVarFromDll(_dllHandle, "pcre_stack_malloc");
				pcre_stack_malloc = (AllocDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(AllocDelegate));
				iptr_pcre_callout = GetProcAddress(_dllHandle, "pcre_callout");


				pcre_compile = (pcre_compile_delegate)GetDelegateFromDll(_dllHandle, "pcre_compile", typeof(pcre_compile_delegate));
				pcre_compile2 = (pcre_compile2_delegate)GetDelegateFromDll(_dllHandle, "pcre_compile2", typeof(pcre_compile2_delegate));
				pcre_study = (pcre_study_delegate)GetDelegateFromDll(_dllHandle, "pcre_study", typeof(pcre_study_delegate));
				pcre_free_study = (pcre_free_study_delegate)GetDelegateFromDll(_dllHandle, "pcre_free_study", typeof(pcre_free_study_delegate));
				pcre_exec = (pcre_exec_delegate)GetDelegateFromDll(_dllHandle, "pcre_exec", typeof(pcre_exec_delegate));
				pcre_dfa_exec = (pcre_dfa_exec_delegate)GetDelegateFromDll(_dllHandle, "pcre_dfa_exec", typeof(pcre_dfa_exec_delegate));


				pcre_copy_named_substring = (pcre_copy_named_substring_delegate)GetDelegateFromDll(_dllHandle, "pcre_copy_named_substring", typeof(pcre_copy_named_substring_delegate));
				pcre_copy_substring = (pcre_copy_substring_delegate)GetDelegateFromDll(_dllHandle, "pcre_copy_substring", typeof(pcre_copy_substring_delegate));
				pcre_get_named_substring = (pcre_get_named_substring_delegate)GetDelegateFromDll(_dllHandle, "pcre_get_named_substring", typeof(pcre_get_named_substring_delegate));
				pcre_get_stringnumber = (pcre_get_stringnumber_delegate)GetDelegateFromDll(_dllHandle, "pcre_get_stringnumber", typeof(pcre_get_stringnumber_delegate));
				pcre_get_stringtable_entries = (pcre_get_stringtable_entries_delegate)GetDelegateFromDll(_dllHandle, "pcre_get_stringtable_entries", typeof(pcre_get_stringtable_entries_delegate));
				pcre_get_substring = (pcre_get_substring_delegate)GetDelegateFromDll(_dllHandle, "pcre_get_substring", typeof(pcre_get_substring_delegate));
				pcre_get_substring_list = (pcre_get_substring_list_delegate)GetDelegateFromDll(_dllHandle, "pcre_get_substring_list", typeof(pcre_get_substring_list_delegate));
				pcre_free_substring = (pcre_free_substring_delegate)GetDelegateFromDll(_dllHandle, "pcre_free_substring", typeof(pcre_free_substring_delegate));
				pcre_free_substring_list = (pcre_free_substring_list_delegate)GetDelegateFromDll(_dllHandle, "pcre_free_substring_list", typeof(pcre_free_substring_list_delegate));


				pcre_jit_exec = (pcre_jit_exec_delegate)GetDelegateFromDll(_dllHandle, "pcre_jit_exec", typeof(pcre_jit_exec_delegate));
				pcre_jit_stack_alloc = (pcre_jit_stack_alloc_delegate)GetDelegateFromDll(_dllHandle, "pcre_jit_stack_alloc", typeof(pcre_jit_stack_alloc_delegate));
				pcre_jit_stack_free = (pcre_jit_stack_free_delegate)GetDelegateFromDll(_dllHandle, "pcre_jit_stack_free", typeof(pcre_jit_stack_free_delegate));
				pcre_assign_jit_stack = (pcre_assign_jit_stack_delegate)GetDelegateFromDll(_dllHandle, "pcre_assign_jit_stack", typeof(pcre_assign_jit_stack_delegate));
				pcre_maketables = (pcre_maketables_delegate)GetDelegateFromDll(_dllHandle, "pcre_maketables", typeof(pcre_maketables_delegate));
				pcre_fullinfo = (pcre_fullinfo_delegate)GetDelegateFromDll(_dllHandle, "pcre_fullinfo", typeof(pcre_fullinfo_delegate));
				pcre_refcount = (pcre_refcount_delegate)GetDelegateFromDll(_dllHandle, "pcre_refcount", typeof(pcre_refcount_delegate));
				pcre_config = (pcre_config_delegate)GetDelegateFromDll(_dllHandle, "pcre_config", typeof(pcre_config_delegate));
				pcre_version = (pcre_version_delegate)GetDelegateFromDll(_dllHandle, "pcre_version", typeof(pcre_version_delegate));
				pcre_pattern_to_host_byte_order = (pcre_pattern_to_host_byte_order_delegate)GetDelegateFromDll(_dllHandle, "pcre_pattern_to_host_byte_order", typeof(pcre_pattern_to_host_byte_order_delegate));
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

		private static Delegate GetDelegateFromDll(IntPtr handle, string name, Type t)
		{
			IntPtr addr = GetProcAddress(handle, name);

			if (addr != IntPtr.Zero)
			{
				return Marshal.GetDelegateForFunctionPointer(addr, t);
			}

			return null;
		}

		#region PCRE NATIVE API INDIRECTED FUNCTIONS
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void FreeDelegate(IntPtr ptr);
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate IntPtr AllocDelegate(int size);
		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate int CalloutDelegate(IntPtr data);


		internal static readonly FreeDelegate pcre_free;
		internal static readonly FreeDelegate pcre_stack_free;
		internal static readonly AllocDelegate pcre_malloc;
		internal static readonly AllocDelegate pcre_stack_malloc;

		internal static CalloutDelegate pcre_callout
		{
			set
			{
				IntPtr ptr = Marshal.GetFunctionPointerForDelegate(value);
				Marshal.WriteIntPtr(iptr_pcre_callout, ptr);
			}
		}

		#endregion

		#region PCRE NATIVE API BASIC FUNCTIONS

		/*
		 pcre *pcre_compile(const char *pattern, int options,
            const char **errptr, int *erroffset,
            const unsigned char *tableptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate IntPtr pcre_compile_delegate(byte* pattern, int options,
		                                                      out IntPtr errptr, ref int erroffset,
		                                                      IntPtr tableptr);

		internal static readonly pcre_compile_delegate pcre_compile;

		/*
		 pcre *pcre_compile2(const char *pattern, int options,
			int *errorcodeptr,
			const char **errptr, int *erroffset,
			const unsigned char *tableptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate IntPtr pcre_compile2_delegate(byte* pattern, int options,
		                                                       ref int errorcodeptr,
		                                                       out IntPtr errptr, ref int erroffset,
		                                                       IntPtr tableptr);

		internal static readonly pcre_compile2_delegate pcre_compile2;

		/*
		 pcre_extra *pcre_study(const pcre *code, int options,
			const char **errptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate IntPtr pcre_study_delegate(IntPtr code, int options, out IntPtr errptr);

		internal static readonly pcre_study_delegate pcre_study;


		/*
		 void pcre_free_study(pcre_extra *extra);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void pcre_free_study_delegate(IntPtr extra);

		internal static readonly pcre_free_study_delegate pcre_free_study;

		/*
		 int pcre_exec(const pcre *code, const pcre_extra *extra,
			const char *subject, int length, int startoffset,
			int options, int *ovector, int ovecsize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_exec_delegate(IntPtr code, IntPtr extra,
		                                                byte* subject, int length, int startoffset,
		                                                int options, int* ovector, int ovecsize);

		internal static readonly pcre_exec_delegate pcre_exec;

		/*
		 int pcre_dfa_exec(const pcre *code, const pcre_extra *extra,
			const char *subject, int length, int startoffset,
			int options, int *ovector, int ovecsize,
			int *workspace, int wscount);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_dfa_exec_delegate(IntPtr code, IntPtr extra,
		                                                    byte* subject, int length, int startoffset,
		                                                    int options, int* ovector, int ovecsize,
		                                                    int* workspace, int wscount);

		internal static readonly pcre_dfa_exec_delegate pcre_dfa_exec;

		#endregion

		#region PCRE NATIVE API STRING EXTRACTION FUNCTIONS

		/*
		 int pcre_copy_named_substring(const pcre *code,
			const char *subject, int *ovector,
			int stringcount, const char *stringname,
			char *buffer, int buffersize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_copy_named_substring_delegate(IntPtr code,
		                                                                byte* subject, int* ovector,
		                                                                int stringcount, byte* stringname,
		                                                                byte* buffer, int buffersize);

		internal static readonly pcre_copy_named_substring_delegate pcre_copy_named_substring;


		/*
		 int pcre_copy_substring(const char *subject, int *ovector,
			int stringcount, int stringnumber, char *buffer,
			int buffersize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_copy_substring_delegate(byte* subject, int* ovector,
		                                                          int stringcount, int stringnumber, byte* buffer,
		                                                          int buffersize);

		internal static readonly pcre_copy_substring_delegate pcre_copy_substring;

		/*
		 int pcre_get_named_substring(const pcre *code,
			const char *subject, int *ovector,
			int stringcount, const char *stringname,
			const char **stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_get_named_substring_delegate(IntPtr code,
		                                                               byte* subject, int* ovector,
		                                                               int stringcount, byte* stringname,
		                                                               out byte* stringptr);

		internal static readonly pcre_get_named_substring_delegate pcre_get_named_substring;

		/*
		 int pcre_get_stringnumber(const pcre *code,
			const char *name);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_get_stringnumber_delegate(IntPtr code, byte* name);

		internal static readonly pcre_get_stringnumber_delegate pcre_get_stringnumber;

		/*
		 int pcre_get_stringtable_entries(const pcre *code,
			const char *name, char **first, char **last);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_get_stringtable_entries_delegate(IntPtr code, byte* name,
		                                                                   out byte* first, out byte* last);

		internal static readonly pcre_get_stringtable_entries_delegate pcre_get_stringtable_entries;


		/*
		 int pcre_get_substring(const char *subject, int *ovector,
			int stringcount, int stringnumber,
			const char **stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_get_substring_delegate(byte* subject, int* ovector,
		                                                         int stringcount, int stringnumber,
		                                                         out IntPtr stringptr);

		internal static readonly pcre_get_substring_delegate pcre_get_substring;


		/*
		 int pcre_get_substring_list(const char *subject,
			int *ovector, int stringcount, const char ***listptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_get_substring_list_delegate(byte* subject, int* ovector,
		                                                              int stringcount, out IntPtr[] listptr);

		internal static readonly pcre_get_substring_list_delegate pcre_get_substring_list;


		/*
		 void pcre_free_substring(const char *stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void pcre_free_substring_delegate(IntPtr stringptr);

		internal static readonly pcre_free_substring_delegate pcre_free_substring;


		/*
		 void pcre_free_substring_list(const char **stringptr);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void pcre_free_substring_list_delegate(IntPtr[] stringptr);

		internal static readonly pcre_free_substring_list_delegate pcre_free_substring_list;

		#endregion

		#region PCRE NATIVE API AUXILIARY FUNCTIONS

		/*
		 int pcre_jit_exec(const pcre *code, const pcre_extra *extra,
			const char *subject, int length, int startoffset,
			int options, int *ovector, int ovecsize,
			pcre_jit_stack *jstack);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate int pcre_jit_exec_delegate(IntPtr code, IntPtr extra,
		                                                    byte* subject, int length, int startoffset,
		                                                    int options, int* ovector, int ovecsize,
		                                                    IntPtr jstack);

		internal static readonly pcre_jit_exec_delegate pcre_jit_exec;


		/*
		 pcre_jit_stack *pcre_jit_stack_alloc(int startsize, int maxsize);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate IntPtr pcre_jit_stack_alloc_delegate(int startsize, int maxsize);

		internal static readonly pcre_jit_stack_alloc_delegate pcre_jit_stack_alloc;

		/*
		 void pcre_jit_stack_free(pcre_jit_stack *stack);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void pcre_jit_stack_free_delegate(IntPtr stack);

		internal static readonly pcre_jit_stack_free_delegate pcre_jit_stack_free;

		/*
		 void pcre_assign_jit_stack(pcre_extra *extra,
			pcre_jit_callback callback, void *data);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void pcre_assign_jit_stack_delegate(IntPtr extra,
		                                                      IntPtr callback, IntPtr data);

		internal static readonly pcre_assign_jit_stack_delegate pcre_assign_jit_stack;

		/*
		 const unsigned char *pcre_maketables(void);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate IntPtr pcre_maketables_delegate();

		internal static readonly pcre_maketables_delegate pcre_maketables;


		/*
		 int pcre_fullinfo(const pcre *code, const pcre_extra *extra,
			int what, void *where);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate int pcre_fullinfo_delegate(IntPtr code, IntPtr extra,
		                                             int what, IntPtr where);

		internal static readonly pcre_fullinfo_delegate pcre_fullinfo;

		/*
		 int pcre_refcount(pcre *code, int adjust);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate int pcre_refcount_delegate(IntPtr code, int adjust);

		internal static readonly pcre_refcount_delegate pcre_refcount;

		/*
		 int pcre_config(int what, void *where);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate int pcre_config_delegate(int what, IntPtr where);

		internal static readonly pcre_config_delegate pcre_config;

		/*
		 const char *pcre_version(void);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate string pcre_version_delegate();

		internal static readonly pcre_version_delegate pcre_version;

		/*
		 int pcre_pattern_to_host_byte_order(pcre *code,
			pcre_extra *extra, const unsigned char *tables);
		 */

		[SuppressUnmanagedCodeSecurity]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate int pcre_pattern_to_host_byte_order_delegate(IntPtr code,
		                                                               IntPtr extra, IntPtr tables);

		internal static readonly pcre_pattern_to_host_byte_order_delegate pcre_pattern_to_host_byte_order;

		#endregion
	}
}
#endif