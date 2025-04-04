using System;

namespace Utils.ReactiveProperty
{
    public interface IReadonlyReactiveProperty<T>
    {
        event Action<T> ValueChanged;
        T Value { get; }
    }
}