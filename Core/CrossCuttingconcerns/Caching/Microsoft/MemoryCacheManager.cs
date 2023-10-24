using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingconcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly List<string> _cacheKeys;
        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
            _cacheKeys = new List<string>();
        }


        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);

        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            _cacheKeys.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
 //bu kodda çalışıyor fakat Bu kodda birden fazla iş parçacığı aynı _cacheKeys listesine erişim sağlayabilir. Eğer birden fazla iş parçacığı aynı anda bu listeye erişirse, uygun senkronizasyon olmadan sorunlar oluşabilir. Bu nedenle, eğer çoklu iş parçacığına dayalı bir senaryo söz konusu ise uygun senkronizasyonu sağlamamız önemlidir.

            //var cacheentriescollectiondefinition = typeof(memorycache).getproperty("entriescollection", system.reflection.bindingflags.nonpublic | system.reflection.bindingflags.ınstance);
            ////  var cacheentriescollection = cacheentriescollectiondefinition.getvalue(_memorycache) as dynamic;
            //var cacheentriescollection = _cachekeys.findall(key => (dynamic)cacheentriescollectiondefinition.getvalue(_memorycache));
            //list<ıcacheentry> cachecollectionvalues = new list<ıcacheentry>();
            //foreach (var cacheıtem in cacheentriescollection)
            //{
            //    ıcacheentry cacheıtemvalue = (ıcacheentry)cacheıtem.gettype().getproperty("value").getvalue(cacheıtem, null);
            //    cachecollectionvalues.add(cacheıtemvalue);
            //}

            //var regex = new regex(pattern, regexoptions.singleline | regexoptions.compiled | regexoptions.ıgnorecase);
            //var keystoremove = cachecollectionvalues.where(d => regex.ısmatch(d.key.tostring())).select(d => d.key).tolist();

            //foreach (var key in keystoremove)
            //{
            //    _memorycache.remove(key);
            //}

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = _cacheKeys.Where(key => regex.IsMatch(key)).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _cacheKeys.Remove(key);
            }

            /*
             Bu kod, Add metodunda _cacheKeys listesine anahtarları ekler ve Remove metodunda anahtarları hem _memoryCache'den hem de
            _cacheKeys listesinden kaldırır. RemoveByPattern metodunda da _cacheKeys listesini kullanarak desene uyan anahtarları
            bulur ve kaldırır. Bu şekilde, _cacheKeys listesi _memoryCache'deki anahtarları takip etmek için kullanılır.
             */
        }
    }
}
