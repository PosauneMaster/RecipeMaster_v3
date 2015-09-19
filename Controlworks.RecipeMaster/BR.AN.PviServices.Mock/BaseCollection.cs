using System;
using System.Collections;
using System.Reflection;

namespace BR.AN.PviServices
{
    public abstract class BaseCollection : IDisposable, ICollection, IEnumerable
    {
        public virtual int Count { get; }
        public bool HasError { get; }
        public virtual bool IsSynchronized { get; }
        public virtual ICollection Keys { get; }
        public virtual object Name { get; }
        public virtual object Parent { get; }
        public virtual Service Service { get; }
        public virtual object SyncRoot { get; }
        public object UserData { get; set; }
        public virtual ICollection Values { get; }

        public object this[object indexer] { get; }

        public event CollectionEventHandler CollectionConnected;
        public event CollectionEventHandler CollectionDisconnected;
        public event CollectionEventHandler CollectionError;
        public event PviEventHandler Uploaded;

        public virtual void Add(object key, object value);
        public virtual void Add(object primKey, object secKey, object value);
        public virtual void Clear();
        public virtual bool Contains(object valObj);
        public virtual bool Contains(string key);
        public virtual bool ContainsKey(object key);
        public virtual void CopyTo(Array array, int count);
        public void Dispose();
        public virtual IEnumerator GetEnumerator();
        protected internal virtual void OnCollectionConnected(CollectionEventArgs e);
        protected internal virtual void OnCollectionDisconnected(CollectionEventArgs e);
        protected internal virtual void OnCollectionError(CollectionEventArgs e);
        protected internal virtual void OnUploaded(PviEventArgs e);
        public virtual void Remove(object key);
        public virtual void Remove(string key);
    }
}
