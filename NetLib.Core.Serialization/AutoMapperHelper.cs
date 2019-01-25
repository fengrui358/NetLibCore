using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using AutoMapper.Mappers;

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
        public static MapperConfiguration GetDefaultMapperConfiguration(Assembly[] assemblies,
            Action<IMapperConfigurationExpression> mapperConfigurationExpression = null)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                if (assemblies != null)
                {
                    var types = new List<Type>();

                    foreach (var assembly in assemblies)
                    {
                        types.AddRange(assembly.GetTypes());
                    }

                    foreach (var type in types)
                    {
                        cfg.CreateMap(type, type);
                    }

                    cfg.AddConditionalObjectMapper().Where((s, d) => s.Name == $"{d.Name}Dto");
                    cfg.AddConditionalObjectMapper().Where((s, d) => $"{s.Name}Dto" == d.Name);
                }

                mapperConfigurationExpression?.Invoke(cfg);
            });
            return configuration;
        }

        /// <summary>
        /// 获取默认的映射实例，自动创建所有类的深度映射配置，并且支持类和带Dto后缀的类的转换
        /// </summary>
        /// <param name="assemblies">需要查找自动映射的程序集</param>
        /// <param name="mapperConfigurationExpression">自动创建AutoMapper后的配置，可以在此基础上进行更细致的定制</param>
        /// <returns>用于创建实体的配置</returns>
        public static IMapper GetDefaultMapper(Assembly[] assemblies,
            Action<IMapperConfigurationExpression> mapperConfigurationExpression = null)
        {
            var configuration = GetDefaultMapperConfiguration(assemblies, mapperConfigurationExpression);
            return configuration.CreateMapper();
        }
    }
}