namespace MetaEx
{
    using System.Collections.Generic;

    using ServiceStack.Text;

    public static class MetaExtensions
    {
        public static T GetMeta<T>(this IMeta meta)
        {
            string item;
            meta.GetMetaDictionary().TryGetValue(typeof(T).Name, out item);
            return item == null ? default(T) : TypeSerializer.DeserializeFromString<T>(item);
        }

        public static void SetMeta<T>(this IMeta meta, T value)
        {
            var metaDictionary = meta.GetMetaDictionary();
            metaDictionary[typeof(T).Name] = TypeSerializer.SerializeToString(value);
            meta.SetMetaDictionary(metaDictionary);
        }

        internal static Dictionary<string, string> GetMetaDictionary(this IMeta meta)
        {
            if (string.IsNullOrEmpty(meta.Meta))
            {
                var dictionary = new Dictionary<string, string>();
                meta.Meta = TypeSerializer.SerializeToString(dictionary);
                return dictionary;
            }
            return TypeSerializer.DeserializeFromString<Dictionary<string, string>>(meta.Meta);
        }

        internal static void SetMetaDictionary(this IMeta meta, Dictionary<string, string> dictionary)
        {
            meta.Meta = TypeSerializer.SerializeToString(dictionary);
        }
    }
}