using System;
using System.Text;

namespace PcreSharp
{
    public class PcreMatch
    {
        private readonly PcreRegex _parent;
        private readonly byte[] _input;
        private readonly int _start;
        private readonly int _end;
        private readonly int _options;
        private readonly bool _success;
        private string _value;

        internal PcreMatch(PcreRegex parent, byte[] input, int start, int end, int options)
        {
            _success = true;
            _parent = parent;
            _input = input;
            _start = start;
            _end = end;
            _options = options;
        }

        internal PcreMatch()
        {
            _success = false;
        }

        public PcreMatch NextMatch()
        {
            if (!_success) return new PcreMatch();

            return _parent.GetMatch(_input, _end, _options);
        }

        public int Index
        {
            get { return _start; }
        }

        public int Length
        {
            get { return _end - _start; }
        }

        public bool Success
        {
            get { return _success; }
        }

        public string Value
        {
            get
            {
                if (_value == null)
                {
                    _value = Encoding.UTF8.GetString(_input, _start, Length);
                }
                return _value;
            }
        }
    }
}
