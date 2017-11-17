using System;
using System.Collections;
using System.Collections.Generic;

namespace Polly
{
    /// <summary>
    /// Context that carries with a single execution through a Policy.   Commonly-used properties are directly on the class.  Backed by a dictionary of string key / object value pairs, to which user-defined values may be added.
    /// <remarks>Do not re-use an instance of <see cref="Context"/> across more than one execution.</remarks>
    /// </summary>
    public class Context : IDictionary<string, object>
    {
        // For an individual execution through a policy or policywrap, it is expected that all execution steps (for example executing the user delegate, invoking policy-activity delegates such as onRetry, onBreak, onTimeout etc) execute sequentially.  
        // Therefore, this class is intentionally not constructed to be safe for concurrent access from multiple threads.


        private static Lazy<Dictionary<string, object>> _contextData = new Lazy<Dictionary<string, object>>();
        internal static readonly Context None = new Context();

        private Guid? _executionGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class, with the specified <paramref name="executionKey"/>.
        /// </summary>
        /// <param name="executionKey">The execution key.</param>
        public Context(String executionKey)
        {
            ExecutionKey = executionKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class, with the specified <paramref name="executionKey" /> and the supplied <paramref name="contextData"/>.
        /// </summary>
        /// <param name="executionKey">The execution key.</param>
        /// <param name="contextData">The context data.</param>
        public Context(String executionKey, IDictionary<string, object> contextData) : this(contextData)
        {
            ExecutionKey = executionKey;
        }

        internal Context()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class, with the specified <paramref name="contextData"/>.
        /// </summary>
        /// <param name="contextData">The context data</param>
        public Context(IDictionary<string, object> contextData)
        {
            if (contextData == null) throw new ArgumentNullException(nameof(contextData));
            _contextData = new Lazy<Dictionary<string, object>>(() => new Dictionary<string, object>(contextData));
        }

        /// <summary>
        /// A key unique to the outermost <see cref="Polly.Wrap.PolicyWrap"/> instance involved in the current PolicyWrap execution.
        /// </summary>
        public String PolicyWrapKey { get; internal set; }

        /// <summary>
        /// A key unique to the <see cref="Policy"/> instance executing the current delegate.
        /// </summary>
        public String PolicyKey { get; internal set; }

        /// <summary>
        /// A key unique to the call site of the current execution. 
        /// <remarks>The value is set </remarks>
        /// </summary>
        public String ExecutionKey { get; }

        /// <summary>
        /// A Guid guaranteed to be unique to each execution.
        /// </summary>
        public Guid ExecutionGuid
        {
            get
            {
                if (!_executionGuid.HasValue) { _executionGuid = Guid.NewGuid(); }
                return _executionGuid.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> Keys => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        public ICollection<object> Values => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        public int Count => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _contextData.Value.ContainsKey(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            _contextData.Value.Add(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _contextData.Value.Remove(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value)
        {
            return _contextData.Value.TryGetValue(key, out value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, object> item)
        {
            _contextData.Value.Add(item.Key, item.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _contextData.Value.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item)
        {

            return ((IDictionary<string, object>)_contextData.Value).Contains(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((IDictionary<string, object>)_contextData.Value).CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            return _contextData.Value.Remove(item.Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _contextData.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();

        }
    }
}