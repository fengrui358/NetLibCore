using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FrHello.NetLib.Core.Reflection;

namespace FrHello.NetLib.Core.Serialization
{
    /// <summary>
    /// AutoMapper辅助类
    /// </summary>
    public static class AutoMapperHelper
    {
        /// <summary>
        /// 获取默认的映射配置，自动创建所有类的深度映射配置，并且支持类和带Dto后缀的类的转换
        /// </summary>
        /// <param name="assemblies">需要查找自动映射的程序集</param>
        /// <param name="mapperConfigurationExpression">自动创建AutoMapper后的配置，可以在此基础上进行更细致的定制</param>
        /// <returns>用于创建实体的配置</returns>
        public static async Task<MapperConfiguration> GetDefaultMapperConfiguration(Assembly[] assemblies,
            Action<IMapperConfigurationExpression> mapperConfigurationExpression = null)
        {
            if (assemblies == null || !assemblies.Any())
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.ValidateInlineMaps = false;

                    mapperConfigurationExpression?.Invoke(cfg);
                });
            }

            return await Task.Run(() =>
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    //程序集中扫描处的原始类型，Value为其基类
                    var originalCache = new Dictionary<Type, HashSet<Type>>();
                    var originalCacheDto = new Dictionary<Type, HashSet<Type>>();

                    //用类型的名字来缓存类型，方便进行名字查找
                    var nameCache = new Dictionary<string, HashSet<Type>>();

                    //已经映射过的进行记录，防止重复映射，Key作为映射的Source，Value的HashSet为映射时的Dest集合
                    var mapedCache = new Dictionary<Type, HashSet<Type>>();

                    foreach (var type in assemblies.SelectMany(s => s.GetTypes()))
                    {
                        if (IsDtoType(type))
                        {
                            AddToCache(type, ref originalCacheDto, ref nameCache);

                            //将一组相关集合创建映射
                            var types = new Type[originalCacheDto[type].Count + 1];
                            types[0] = type;

                            var index = 1;
                            foreach (var typeItem in originalCacheDto[type])
                            {
                                types[index] = typeItem;
                                index++;
                            }

                            CreateMap(cfg, types, ref mapedCache);
                        }
                        else
                        {
                            AddToCache(type, ref originalCache, ref nameCache);

                            //将一组相关集合创建映射
                            var types = new Type[originalCache[type].Count + 1];
                            types[0] = type;

                            var index = 1;
                            foreach (var typeItem in originalCache[type])
                            {
                                types[index] = typeItem;
                                index++;
                            }

                            CreateMap(cfg, types, ref mapedCache);
                        }
                    }

                    //整理继承关系链条
                    var combineCache = originalCache.ToDictionary(s => s.Key, v => v.Value);

                    foreach (var keyValuePair in originalCacheDto)
                    {
                        combineCache.Add(keyValuePair.Key, keyValuePair.Value);
                    }

                    AddParentRelationship(ref combineCache);

                    //将Dto和非Dto进行映射
                    foreach (var dtoTypes in originalCacheDto)
                    {
                        //查找类型
                        var dtotypes = new List<Type> { dtoTypes.Key };
                        dtotypes.AddRange(dtoTypes.Value.Select(s => s));

                        var noneDtoTypes = new HashSet<Type>();
                        foreach (var dtotype in dtotypes)
                        {
                            var typeName = dtotype.Name.Substring(0, dtotype.Name.Length - 3);
                            if (nameCache.ContainsKey(typeName))
                            {
                                foreach (var type1 in nameCache[typeName])
                                {
                                    if (originalCache.ContainsKey(type1))
                                    {
                                        if (!noneDtoTypes.Contains(type1))
                                        {
                                            noneDtoTypes.Add(type1);
                                        }

                                        foreach (var type2 in originalCache[type1])
                                        {
                                            if (!noneDtoTypes.Contains(type2))
                                            {
                                                noneDtoTypes.Add(type2);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var noneDtoType in noneDtoTypes)
                        {
                            foreach (var dtotype in dtotypes)
                            {
                                CreateMap(cfg, noneDtoType, dtotype, ref mapedCache);
                                CreateMap(cfg, dtotype, noneDtoType, ref mapedCache);
                            }
                        }
                    }

                    cfg.ValidateInlineMaps = false;

                    mapperConfigurationExpression?.Invoke(cfg);
                });

                return configuration;
            });
        }

        /// <summary>
        /// 获取默认的映射实例，自动创建所有类的深度映射配置，并且支持类和带Dto后缀的类的转换
        /// </summary>
        /// <param name="assemblies">需要查找自动映射的程序集</param>
        /// <param name="mapperConfigurationExpression">自动创建AutoMapper后的配置，可以在此基础上进行更细致的定制</param>
        /// <returns>用于创建实体的配置</returns>
        public static async Task<IMapper> GetDefaultMapper(Assembly[] assemblies,
            Action<IMapperConfigurationExpression> mapperConfigurationExpression = null)
        {
            var configuration = await GetDefaultMapperConfiguration(assemblies, mapperConfigurationExpression);
            return configuration.CreateMapper();
        }

        /// <summary>
        /// 判断是否是贫血模型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否为贫血模型</returns>
        private static bool IsDtoType(Type type)
        {
            return type.Name.EndsWith("Dto");
        }

        /// <summary>
        /// 获取某个类型的基类集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>该类型的基类的集合</returns>
        private static List<Type> GetBaseTypes(Type type)
        {
            var result = new List<Type>();
            var baseType = type.BaseType;

            while (baseType != null && baseType != typeof(object))
            {
                result.Add(baseType);
                baseType = baseType.BaseType;
            }

            return result;
        }

        /// <summary>
        /// 将某个类型的相关信息以及基类信息添加到缓存中
        /// </summary>
        /// <param name="type">需要加入缓存的类型</param>
        /// <param name="cache">类型及其子类型的缓存集合</param>
        /// <param name="nameCache">缓存类型的名称</param>
        private static void AddToCache(Type type, ref Dictionary<Type, HashSet<Type>> cache,
            ref Dictionary<string, HashSet<Type>> nameCache)
        {
            if (!cache.ContainsKey(type))
            {
                cache.Add(type, new HashSet<Type>());
            }

            //获取基类
            var baseTypes = GetBaseTypes(type);
            if (baseTypes.Any())
            {
                foreach (var baseType in baseTypes)
                {
                    cache[type].Add(baseType);

                    if (!nameCache.ContainsKey(baseType.Name))
                    {
                        nameCache.Add(baseType.Name, new HashSet<Type> {baseType});
                    }
                    else
                    {
                        nameCache[baseType.Name].Add(baseType);
                    }
                }
            }

            if (!nameCache.ContainsKey(type.Name))
            {
                nameCache.Add(type.Name, new HashSet<Type> {type});
            }
            else
            {
                nameCache[type.Name].Add(type);
            }
        }

        /// <summary>
        /// 对一组相关集合进行两两映射
        /// </summary>
        /// <param name="cfg">Automapper配置</param>
        /// <param name="list">需要创建映射的集合类型</param>
        /// <param name="mappedCache">已映射的类型缓存，防止重复映射</param>
        private static void CreateMap(IMapperConfigurationExpression cfg, IList<Type> list,
            ref Dictionary<Type, HashSet<Type>> mappedCache)
        {
            //进行俩俩遍历映射
            for (var i = 0; i < list.Count; i++)
            {
                for (var j = i; j < list.Count; j++)
                {
                    CreateMap(cfg, list[i], list[j], ref mappedCache);
                    CreateMap(cfg, list[j], list[i], ref mappedCache);
                }
            }
        }

        /// <summary>
        /// 对两个类型创建映射
        /// </summary>
        /// <param name="cfg">Automapper配置</param>
        /// <param name="source">源类型</param>
        /// <param name="dest">目标类型</param>
        /// <param name="mappedCache">已映射的类型缓存，防止重复映射</param>
        private static void CreateMap(IMapperConfigurationExpression cfg, Type source, Type dest,
            ref Dictionary<Type, HashSet<Type>> mappedCache)
        {
            if (mappedCache.ContainsKey(source))
            {
                if (mappedCache[source] != null && mappedCache[source].Contains(dest))
                {
                    //已经映射过了，直接退出
                    return;
                }
            }

            //目标类型要能够构造才创建映射
            if (TypeHelper.HasDefaultConstructor(dest))
            {
                cfg.CreateMap(source, dest);
                if (!mappedCache.ContainsKey(source))
                {
                    mappedCache.Add(source, new HashSet<Type> {dest});
                }
                else
                {
                    mappedCache[source].Add(dest);
                }
            }
        }

        /// <summary>
        /// 完善类型的继承关系，在继承链中加入能识别的子类
        /// </summary>
        /// <param name="typeRelationShip">类型关系</param>
        private static void AddParentRelationship(ref Dictionary<Type, HashSet<Type>> typeRelationShip)
        {
            //便利集合缓存
            foreach (var keyValuePair in typeRelationShip)
            {
                //遍历类型的基类
                foreach (var type in keyValuePair.Value)
                {
                    //如果基类存在于大集合中
                    if (typeRelationShip.ContainsKey(type))
                    {
                        //则将子类的父类向另一个类型进行合并
                        typeRelationShip[type].UnionWith(keyValuePair.Value);

                        //再将顶级子类本身加入
                        if (!typeRelationShip[type].Contains(keyValuePair.Key))
                        {
                            typeRelationShip[type].Add(keyValuePair.Key);
                        }
                    }
                }
            }
        }
    }
}