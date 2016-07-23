using System.Collections.Generic;
using System.Linq;

namespace livemenu.SignalR
{
    public interface IConnectionMapping<in T>
    {
        int Count { get; }
        void Add(T key, string connectionId);
        IEnumerable<string> GetConnections(T key);
        void Remove(T key, string connectionId);
        void RemoveByConnectionId(string connectionId);
    }

    public class AccountComparer : IEqualityComparer<ConnectionUserAccount>
    {
        public bool Equals(ConnectionUserAccount x, ConnectionUserAccount y)
        {
            return x.Account.Login == y.Account.Login;
        }

        public int GetHashCode(ConnectionUserAccount obj)
        {
            return obj.Account.Login.GetHashCode();
        }
    }

    public class ConnectionMapping<T> : IConnectionMapping<T> where T : ConnectionUserAccount
    {
        private readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>(new AccountComparer());

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

        public void RemoveByConnectionId(string connectionId)
        {
            lock (_connections)
            {
                var connectionUser = _connections.FirstOrDefault(x => x.Value.Contains(connectionId));
                if(connectionUser.Value == null)
                    return;

                connectionUser.Value.Remove(connectionId);
                if (!connectionUser.Value.Any())
                    _connections.Remove(connectionUser.Key);

            }
        }
    }
}