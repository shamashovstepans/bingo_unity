using System;
using System.Collections.Generic;

namespace Utils.ReactiveProperty
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private static readonly IEqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;
        public static implicit operator T(ReactiveProperty<T> reactiveProperty) => reactiveProperty.Value;

        public event Action<T> ValueChanged;

        protected T _value = default;

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer.Equals(_value, value))
                {
                    _value = value;
                    ValueChanged?.Invoke(_value);
                }
            }
        }

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}