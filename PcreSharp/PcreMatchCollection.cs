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

        internal PcreMatchCollection(PcreRegex parent, byte[] data, int options)
        {
            _options = options;
            _parent = parent;
            _data = data;
            _matches = new List<PcreMatch>();
        }

        public IEnumerator<PcreMatch> GetEnumerator()
        {
            throw new NotImplementedException();
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

        private PcreMatch GetMatch(int i)
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
                match = new PcreMatch(_parent, _data, 0,0,_options);
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
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(PcreMatch item)
        {
            return _matches.Contains(item);
        }

        public void CopyTo(PcreMatch[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(PcreMatch item)
        {
            throw new NotImplementedException();
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
