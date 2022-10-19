using System.Reflection;

namespace AutoMapperClone
{
    public class ReflectionMapper
    {
        public static Dictionary<(Type from, Type to), List<(MethodInfo Get, MethodInfo Set)>> _cache = new();

        public static T Map<T>(object from) where T : class, new()
        {
            var key = (from: from.GetType(), to: typeof(T));
            if (!_cache.ContainsKey(key))
            {
                CacheGettersAndSetters(key);
            }

            var result = new T();
            var entry = _cache[key];

            foreach (var e in entry)
            {
                var val = e.Get.Invoke(from, null);
                e.Set.Invoke(result, new[] { val });
            }

            return result;
        }

        private static void CacheGettersAndSetters((Type from, Type to) key)
        {
            var fromProps = key.from.GetProperties();
            var toProps = key.to.GetProperties();

            List<(MethodInfo, MethodInfo)> entry = new();
            foreach (var prop in fromProps)
            {
                var to = toProps.FirstOrDefault(x => x.Name == prop.Name);
                if (to == null)
                {
                    continue; 
                }
                entry.Add((prop.GetMethod, to.SetMethod));
            }
            _cache[key] = entry;
        }
    }
}
