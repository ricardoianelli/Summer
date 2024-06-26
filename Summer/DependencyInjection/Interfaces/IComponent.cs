﻿namespace Summer.DependencyInjection.Interfaces;

/// <summary>
/// Defines a class that needs to be instantiated and be stored as a Singleton in the ComponentStore.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Method used to do any object initialization after all dependencies were injected.
    /// </summary>
    void Initialize()
    {
    }
}