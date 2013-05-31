using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PcreSharp
{
	public delegate string PcreMatchEvaluator(PcreMatch match);

	public class PcreRegex : IDisposable
	{
		private bool _disposed;
		private readonly IntPtr _code;
		private readonly IntPtr _extra;
		[ThreadStatic]
		private static IntPtr _stack;
		private readonly string _pattern;

		private const int OvecSize = 10 * 3;
		private readonly int[] _ovectorArr = new int[OvecSize];
		private readonly PcreOptions _options;
		private readonly PcreStudyOptions _studyOptions;

		public unsafe PcreRegex(string pattern, PcreOptions options, PcreStudyOptions studyOptions)
		{
			_pattern = pattern;
			_options = options;
			_studyOptions = studyOptions;

			int errorCode = 0;
			IntPtr error;
			int errorOffset = 0;

			byte[] patternBytes = Encoding.UTF8.GetBytes(pattern);
			fixed (byte* bytes = patternBytes)
			{
				_code = PcreWrapper.pcre_compile2(bytes, (int)_options, ref errorCode, out error, ref errorOffset, IntPtr.Zero);
			}
			if (errorCode != 0)
			{
				throw new Exception(Marshal.PtrToStringAuto(error));
			}

			_extra = PcreWrapper.pcre_study(_code, (int)studyOptions, out error);
			if ((studyOptions & PcreStudyOptions.PCRE_STUDY_JIT_COMPILE) != 0)
			{
				if (_stack == IntPtr.Zero)
				{
					_stack = PcreWrapper.pcre_jit_stack_alloc(64*1024, 16*1024*1024);
				}
			}
		}

		#region Constructor overloads
		public PcreRegex(string pattern, PcreOptions options)
			: this(pattern, options, PcreStudyOptions.NONE)
		{
		}

		public PcreRegex(string pattern)
			: this(pattern, PcreOptions.NONE, PcreStudyOptions.NONE)
		{
		}
		#endregion

		public override string ToString()
		{
			return _pattern;
		}

		public bool IsMatch(string input, int startat, PcreOptions options)
		{
			byte[] tgtBytes = Encoding.UTF8.GetBytes(input);

			PcreMatch match = GetMatch(tgtBytes, startat, (int)options);
			return match.Success;
		}

		#region IsMatch() overloads
		public bool IsMatch(string input, int startat)
		{
			return IsMatch(input, startat, _options);
		}

		public bool IsMatch(string input, PcreOptions options)
		{
			return IsMatch(input, 0, options);
		}

		public bool IsMatch(string input)
		{
			return IsMatch(input, 0, _options);
		}
		#endregion

		public PcreMatch Match(string input, int beginning, int length, PcreOptions options)
		{
			if (length != -1 || beginning > 0)
			{
				if (length == -1)
				{
					length = input.Length - beginning;
				}

				input = input.Substring(beginning, length);
			}
			byte[] tgtBytes = Encoding.UTF8.GetBytes(input);
			return GetMatch(tgtBytes, 0, (int)_options);
		}

		#region Match() overloads
		public PcreMatch Match(string input, int beginning, int length)
		{
			return Match(input, beginning, length, _options);
		}

		public PcreMatch Match(string input, int beginning, PcreOptions options)
		{
			return Match(input, beginning, -1, options);
		}

		public PcreMatch Match(string input, int beginning)
		{
			return Match(input, beginning, -1, _options);
		}

		public PcreMatch Match(string input, PcreOptions options)
		{
			return Match(input, 0, -1, options);
		}

		public PcreMatch Match(string input)
		{
			return Match(input, 0, -1, _options);
		}
		#endregion

		public PcreMatchCollection Matches(string input, int start, PcreOptions options)
		{
			byte[] tgtBytes = Encoding.UTF8.GetBytes(input);
			int opts = (int) options;

			return new PcreMatchCollection(this, tgtBytes, start, opts);
		}

		#region Matches() overloads
		public PcreMatchCollection Matches(string input)
		{
			return Matches(input, 0, PcreOptions.NONE);
		}

		public PcreMatchCollection Matches(string input, int start)
		{
			return Matches(input, start, PcreOptions.NONE);
		}

		public PcreMatchCollection Matches(string input, PcreOptions options)
		{
			return Matches(input, 0, options);
		}
		#endregion

		public string Replace(string input, PcreMatchEvaluator evaluator, int maxCount, int start, PcreOptions options)
		{
			var collection = Matches(input, start, options);
			int count;
			if (maxCount == -1)
			{
				count = collection.Count;
			}
			else
			{
				count = collection.GetMatch(maxCount) != null ? maxCount : collection.Count;
			}

			if (count == 0) return input;

			throw new NotImplementedException();
		}

		public string Replace(string input, string replacement, int maxCount, int start, PcreOptions options)
		{
			byte[] srcBytes = Encoding.UTF8.GetBytes(input);
			int opts = (int)options;
			var collection = new PcreMatchCollection(this, srcBytes, start, opts);
			int count;
			if (maxCount == -1)
			{
				count = collection.Count;
			}
			else
			{
				count = collection.GetMatch(maxCount) != null ? maxCount : collection.Count;
			}

			if (count == 0) return input;

			int totalLen = 0;

			for (int i = 0; i < count; i++)
			{
				var currMatch = collection[i];
				totalLen += currMatch.Length;
			}

			byte[] replacementBytes = Encoding.UTF8.GetBytes(replacement);
			var replacementLen = replacementBytes.Length;
			var resultLen = srcBytes.Length - totalLen + replacementLen * count;

			var resultBytes = new byte[resultLen];

			int currIndex = 0;
			int currSrcPos = 0;
			int currDstPos = 0;

			while (currIndex < count)
			{
				var currMatch = collection[currIndex];
				var copyCount = currMatch._start - currSrcPos;
				Buffer.BlockCopy(srcBytes, currSrcPos, resultBytes, currDstPos, copyCount);
				currSrcPos += copyCount;
				currDstPos += copyCount;

				Buffer.BlockCopy(replacementBytes, 0, resultBytes, currDstPos, replacementLen);
				currSrcPos += currMatch.Length;
				currDstPos += replacementLen;

				currIndex++;
			}

			Buffer.BlockCopy(srcBytes, currSrcPos, resultBytes, currDstPos, srcBytes.Length - currSrcPos);

			return Encoding.UTF8.GetString(resultBytes, 0, resultBytes.Length);
		}

		#region Replace() overloads
		public string Replace(string input, PcreMatchEvaluator evaluator, int maxCount, int start)
		{
			return Replace(input, evaluator, maxCount, start, PcreOptions.NONE);
		}

		public string Replace(string input, PcreMatchEvaluator evaluator, int maxCount)
		{
			return Replace(input, evaluator, maxCount, 0, PcreOptions.NONE);
		}

		public string Replace(string input, PcreMatchEvaluator evaluator)
		{
			return Replace(input, evaluator, -1, 0, PcreOptions.NONE);
		}

		public string Replace(string input, PcreMatchEvaluator evaluator, int maxCount, PcreOptions options)
		{
			return Replace(input, evaluator, maxCount, 0, options);
		}

		public string Replace(string input, PcreMatchEvaluator evaluator, PcreOptions options)
		{
			return Replace(input, evaluator, -1, 0, options);
		}

		public string Replace(string input, string replacement, int maxCount, int start)
		{
			return Replace(input, replacement, maxCount, start, PcreOptions.NONE);
		}

		public string Replace(string input, string replacement, int maxCount)
		{
			return Replace(input, replacement, maxCount, 0, PcreOptions.NONE);
		}

		public string Replace(string input, string replacement)
		{
			return Replace(input, replacement, -1, 0, PcreOptions.NONE);
		}

		public string Replace(string input, string replacement, int maxCount, PcreOptions options)
		{
			return Replace(input, replacement, maxCount, 0, options);
		}

		public string Replace(string input, string replacement, PcreOptions options)
		{
			return Replace(input, replacement, -1, 0, options);
		}
		#endregion

		public unsafe int MatchCount(string input, PcreOptions options)
		{
			int count = -1;
			int pos = 0;
			byte[] tgtBytes = Encoding.UTF8.GetBytes(input);
			int bytesLen = tgtBytes.Length;
			int opts = (int) options;
			fixed (byte* bytes = tgtBytes)
			{
				fixed (int* ovector = _ovectorArr)
				{
					int res;
					do
					{
						res = Exec(bytes, bytesLen, pos, opts, ovector);
						pos = _ovectorArr[1];
						count++;
					} while (res > 0);
				}
			}
			return count;
		}

		#region MatchCount() overloads
		public int MatchCount(string input)
		{
			return MatchCount(input, PcreOptions.NONE);
		}
		#endregion

		internal unsafe PcreMatch GetMatch(byte[] data, int pos, int options)
		{
			int res;
			fixed (byte* bytes = data)
			{
				fixed (int* ovector = _ovectorArr)
				{
					res = Exec(bytes, data.Length, pos, options, ovector);
				}
			}
			if (res <= 0)
			{
				return new PcreMatch();
			}
			return new PcreMatch(this, data, _ovectorArr[0], _ovectorArr[1], options);

		}

		private unsafe int Exec(byte* subject, int length, int startoffset, int options, int* ovector)
		{
			if ((_studyOptions & PcreStudyOptions.PCRE_STUDY_JIT_COMPILE) != 0)
			{
				return PcreWrapper.pcre_jit_exec(_code, _extra, subject, length, startoffset, options, ovector, OvecSize, _stack);
			}
			else
			{
				return PcreWrapper.pcre_exec(_code, _extra, subject, length, startoffset, options, ovector, OvecSize);
			}
		}

		~PcreRegex()
		{
			if (!_disposed)
			{
				Dispose();
			}
		}

		public void Dispose()
		{
			if (_disposed) return;
			_disposed = true;

			if (_code != IntPtr.Zero)
			{
				PcreWrapper.pcre_free(_code);
			}
			if (_extra != IntPtr.Zero)
			{
				PcreWrapper.pcre_free_study(_extra);
			}
		}
	}
}
