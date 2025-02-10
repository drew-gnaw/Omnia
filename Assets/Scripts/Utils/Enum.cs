using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

public abstract class Enum<T> where T : Enum<T> {
    private static readonly Dictionary<Type, T> valuesMap = new();

    static Enum() {
        RegisterValues();
    }

    private static void RegisterValues() {
        Type baseType = typeof(T);

        var subTypes = Assembly.GetAssembly(baseType)!
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t));

        foreach (Type subType in subTypes) {
            if (Activator.CreateInstance(subType) is T instance) {
                valuesMap[subType] = instance;
            } else {
                throw new InvalidOperationException($"Could not create instance of {subType}");
            }
        }
    }

    protected static T ParseFromType(Type type) {
        if (valuesMap.TryGetValue(type, out T instance))
            return instance;

        throw new ArgumentException($"No enum value found for {type}");
    }

    protected static IEnumerable<T> Values => valuesMap.Values;
}
