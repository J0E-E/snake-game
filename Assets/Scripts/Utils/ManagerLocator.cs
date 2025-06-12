using System;
using System.Collections.Generic;

public interface IManager { };

/// <summary>
/// Provides a mechanism for registering, retrieving, and managing service instances by type.
/// </summary>
public static class ManagerLocator
{
    private static Dictionary<Type, object> _managers = new();

    /// <summary>
    /// Registers a service of type T. 
    /// </summary>
    public static void Register<T>(T manager) where T : class
    {
        var type = typeof(T);
        if (manager == null)
            throw new ArgumentNullException(nameof(manager), $"Can't register null for type {type}");

        if (!_managers.TryAdd(type, manager))
            throw new InvalidOperationException($"Service of type {type} is already registered.");
    }

    /// <summary>
    /// Retrieves the registered service of type T.
    /// </summary>
    public static T Get<T>() where T : class
    {
        var type = typeof(T);

        if (_managers.TryGetValue(typeof(T), out var manager))
            return manager as T;

        throw new InvalidOperationException($"Service of type {type} is not registered.");
    }

    /// <summary>
    /// Deregisters the service of type T.
    /// </summary>
    public static bool Deregister<T>() where T : class
    {
        return _managers.Remove(typeof(T));
    }

    /// <summary>
    ///  Clears all registered services.
    /// </summary>
    public static void ClearAll()
    {
        _managers.Clear();
    }
}