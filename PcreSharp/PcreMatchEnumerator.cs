using System;
using System.Collections;
using System.Collections.Generic;

namespace PcreSharp
{
	class PcreMatchEnumerator : IEnumerator<PcreMatch>
	{
		private PcreMatchCollection _collection;
		private PcreMatch _match;
		private int _index;
		private bool _finished;

		internal PcreMatchEnumerator(PcreMatchCollection collection)
		{
			_collection = collection;
		}

		public void Dispose()
		{
			_collection = null;
			_match = null;
		}

		public bool MoveNext()
		{
			if (_finished) return false;

			_match = _collection.GetMatch(_index);
			_index++;

			if (_match == null)
			{
				_finished = true;
				return false;
			}

			return true;
		}

		public void Reset()
		{
			_match = null;
			_index = 0;
			_finished = false;
		}

		public PcreMatch Current {
			get
			{
				if (_match == null)
				{
					throw new InvalidOperationException();
				}

				return _match;
			}
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}
	}
}
