using System.Linq.Expressions;

namespace AutoMapperClone
{
    public class ExpressionMapper
    {

        public static Dictionary<(Type, Type), Delegate> _cache = new Dictionary<(Type, Type), Delegate>();

        public static T To<T>(object o)
        {
            var inType = o.GetType();
            var outType = typeof(T);

            var key = (inType, outType);
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = CreateDelegate(inType, outType);
            }

            return (T)_cache[key].DynamicInvoke(o);
        }

        private static Delegate CreateDelegate(Type inType, Type outType) {
            var param = Expression.Parameter(inType);
            var newExpression = Expression.New(outType.GetConstructor(Type.EmptyTypes));

            List<MemberBinding> bindings = new();
            foreach (var prop in inType.GetProperties())
            {
                var typeBMemberInfo = outType.GetProperty(prop.Name);
                if (typeBMemberInfo == null)
                {
                    continue;
                }

                var propertyMemberAccess = Expression.MakeMemberAccess(param, prop);

                var binding = Expression.Bind(typeBMemberInfo, propertyMemberAccess);
                bindings.Add(binding);
            }

            var body = Expression.MemberInit(newExpression, bindings);


            return Expression.Lambda(body, false, param)
                .Compile();
        }
    }

    public class A
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }

    }

    public class B
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
    }
}