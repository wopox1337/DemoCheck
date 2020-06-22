using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DemoCheck.Tools
{
    public class LimitedStack<T> : IEnumerable<T>
    {
        private int _maxCount;
        public LimitedStack(int maxCount)
        {
            _maxCount = maxCount;
        }
        private LinkedList<T> _values = new LinkedList<T>();
        public void Push(T value)
        {
            _values.AddFirst(value);
            while (_values.Count > _maxCount)
            {
                _values.RemoveLast();
            }
        }
        public T Pop()
        {
            var value = _values.First.Value;
            _values.RemoveFirst();
            return value;
        }
        public T[] ToArray() => _values.ToArray();

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
