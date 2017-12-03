using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    internal interface IContainer : IServiceProvider, IDisposable
    {
    }

    internal class IocContainer : IContainer
    {
        private static readonly Lazy<IocContainer> _defaultContainer = new Lazy<IocContainer>(() => new IocContainer());

        private readonly ConcurrentDictionary<Type, object> _servicesDictionary = new ConcurrentDictionary<Type, object>();

        public static IocContainer DefaultContainer => _defaultContainer.Value;

        public object GetService(Type serviceType)
        => _servicesDictionary[serviceType];

        public bool TryGetService(Type serviceType, out object service)
            => _servicesDictionary.TryGetValue(serviceType, out service);

        public TService GetService<TService>()
        {
            var serviceType = typeof(TService);
            if (_servicesDictionary.ContainsKey(serviceType))
            {
                return (TService)_servicesDictionary[serviceType];
            }
            throw new ArgumentException($"service has not been registered,service type:{serviceType.FullName}");
        }

        public bool TryGetService<TService>(out TService service)
        {
            var result = _servicesDictionary.TryGetValue(typeof(TService), out var value);
            service = (TService)value;
            return result;
        }

        public bool Register<TService>()
            => _servicesDictionary.TryAdd(typeof(TService), ActivatorUtils.CreateInstance<TService>());

        public bool Register<TService, TImplement>() where TImplement : TService
            => _servicesDictionary.TryAdd(typeof(TService), ActivatorUtils.CreateInstance<TImplement>());

        public bool RegisterInstance<TService>(TService service)
        => _servicesDictionary.TryAdd(typeof(TService), service);

        public bool RegisterInstance<TService>(Func<TService> serviceFunc)
        {
            if (_servicesDictionary.ContainsKey(typeof(TService)))
            {
                return false;
            }
            _servicesDictionary.GetOrAdd(typeof(TService), serviceFunc);
            return true;
        }

        public bool UnRegister<TService>()
        => _servicesDictionary.TryRemove(typeof(TService), out var service);

        public bool UnRegister<TService>(out object service)
            => _servicesDictionary.TryRemove(typeof(TService), out service);

        public void Dispose()
        {
            _servicesDictionary.Clear();
        }
    }

    internal static class ActivatorUtils
    {
        public static TService CreateInstance<TService>(params object[] parameters)
        {
            var serviceType = typeof(TService);
            if (serviceType.IsAbstract)
            {
                throw new InvalidOperationException("can not create instance of abstract class");
            }

            int bestLength = -1;
            ConstructorMatcher bestMatcher = null;

            var constructors = serviceType.GetConstructors();
            foreach (var matcher in constructors.Where(c => !c.IsStatic && c.IsPublic).Select(c => new ConstructorMatcher(c)))
            {
                var length = matcher.Match(parameters);
                if (length == -1)
                {
                    continue;
                }
                if (bestLength < length)
                {
                    bestLength = length;
                    bestMatcher = matcher;
                }
            }

            if (bestMatcher == null)
            {
                var message = $"A suitable constructor for type {serviceType.FullName} could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.";
                throw new InvalidOperationException(message);
            }

            return (TService)bestMatcher.CreateInstance(IocContainer.DefaultContainer);
        }

        private class ConstructorMatcher
        {
            private readonly ConstructorInfo _constructor;
            private readonly ParameterInfo[] _parameters;
            private readonly object[] _parameterValues;
            private readonly bool[] _parameterValuesSet;

            public ConstructorMatcher(ConstructorInfo constructor)
            {
                _constructor = constructor;
                _parameters = _constructor.GetParameters();
                _parameterValuesSet = new bool[_parameters.Length];
                _parameterValues = new object[_parameters.Length];
            }

            public int Match(object[] givenParameters)
            {
                var applyIndexStart = 0;
                var applyExactLength = 0;
                for (var givenIndex = 0; givenIndex != givenParameters.Length; givenIndex++)
                {
                    var givenType = givenParameters[givenIndex]?.GetType().GetTypeInfo();
                    var givenMatched = false;

                    for (var applyIndex = applyIndexStart; givenMatched == false && applyIndex != _parameters.Length; ++applyIndex)
                    {
                        if (_parameterValuesSet[applyIndex] == false &&
                            _parameters[applyIndex].ParameterType.GetTypeInfo().IsAssignableFrom(givenType))
                        {
                            givenMatched = true;
                            _parameterValuesSet[applyIndex] = true;
                            _parameterValues[applyIndex] = givenParameters[givenIndex];
                            if (applyIndexStart == applyIndex)
                            {
                                applyIndexStart++;
                                if (applyIndex == givenIndex)
                                {
                                    applyExactLength = applyIndex;
                                }
                            }
                        }
                    }

                    if (givenMatched == false)
                    {
                        return -1;
                    }
                }
                return applyExactLength;
            }

            public object CreateInstance(IServiceProvider provider)
            {
                for (var index = 0; index != _parameters.Length; index++)
                {
                    if (_parameterValuesSet[index] == false)
                    {
                        var value = provider.GetService(_parameters[index].ParameterType);
                        if (value == null)
                        {
                            if (!ParameterDefaultValue.TryGetDefaultValue(_parameters[index], out var defaultValue))
                            {
                                throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[index].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
                            }
                            else
                            {
                                _parameterValues[index] = defaultValue;
                            }
                        }
                        else
                        {
                            _parameterValues[index] = value;
                        }
                    }
                }

                try
                {
                    return _constructor.Invoke(_parameterValues);
                }
                catch (TargetInvocationException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    // The above line will always throw, but the compiler requires we throw explicitly.
                    throw;
                }
            }
        }
    }

    internal class ParameterDefaultValue
    {
        public static bool TryGetDefaultValue(ParameterInfo parameter, out object defaultValue)
        {
            bool hasDefaultValue;
            var tryToGetDefaultValue = true;
            defaultValue = null;

            try
            {
                hasDefaultValue = parameter.HasDefaultValue;
            }
            catch (FormatException) when (parameter.ParameterType == typeof(DateTime))
            {
                // Workaround for https://github.com/dotnet/corefx/issues/12338
                // If HasDefaultValue throws FormatException for DateTime
                // we expect it to have default value
                hasDefaultValue = true;
                tryToGetDefaultValue = false;
            }

            if (hasDefaultValue)
            {
                if (tryToGetDefaultValue)
                {
                    defaultValue = parameter.DefaultValue;
                }

                // Workaround for https://github.com/dotnet/corefx/issues/11797
                if (defaultValue == null && parameter.ParameterType.IsValueType)
                {
                    defaultValue = Activator.CreateInstance(parameter.ParameterType);
                }
            }

            return hasDefaultValue;
        }
    }
}