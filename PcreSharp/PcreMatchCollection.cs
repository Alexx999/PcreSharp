using System;
using System.Collections;
using System.Collections.Generic;
namespace PcreSharp
{
	public class PcreMatchCollection : ICollection<PcreMatch>
	{
		private bool _foundAll;
		private readonly List<PcreMatch> _matches;
		private readonly PcreRegex _parent;
		private readonly byte[] _data;
		private readonly int _options;
		private readonly int _start;

		internal PcreMatchCollection(PcreRegex parent, byte[] data, int start, int options)
		{
			_options = options;
			_parent = parent;
			_data = data;
			_start = start;
			_matches = new List<PcreMatch>();
		}

		public IEnumerator<PcreMatch> GetEnumerator()
		{
			return new PcreMatchEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public PcreMatch this[int i]
		{
			get
			{
				PcreMatch match = GetMatch(i);
				if (match == null)
					throw new ArgumentOutOfRangeException("i");

				return match;
			}
		}

		internal PcreMatch GetMatch(int i)
		{
			if (i < 0) return null;

			if (i < _matches.Count)
			{
				return _matches[i];
			}

			if (_foundAll) return null;

			PcreMatch match;

			if (_matches.Count == 0)
			{
				match = new PcreMatch(_parent, _data, _start, 0, _options);
			}
			else
			{
				match = _matches[_matches.Count - 1];
			}

			do
			{
				match = match.NextMatch();

				if (!match.Success)
				{
					_foundAll = true;
					return null;
				}

				_matches.Add(match);

			} while (_matches.Count <= i);

			_foundAll = true;

			return match;
		}

		public void Add(PcreMatch item)
		{
			throw new NotSupportedException();
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public bool Contains(PcreMatch item)
		{
			GetMatch(int.MaxValue);
			return _matches.Contains(item);
		}

		public void CopyTo(PcreMatch[] array, int arrayIndex)
		{
			if ((array != null) && (array.Rank != 1))
			{
				throw new ArgumentException("Multi-dimmensional arrays not supported");
			}

			GetMatch(int.MaxValue);

			_matches.CopyTo(array, arrayIndex);
		}

		public bool Remove(PcreMatch item)
		{
			throw new NotSupportedException();
		}

		public int Count
		{
			get
			{
				if (_foundAll)
				{
					return _matches.Count;
				}

				GetMatch(int.MaxValue);

				return _matches.Count;
			}
		}

		public bool IsReadOnly
		{
			get { return true; }
		}
	}
}
