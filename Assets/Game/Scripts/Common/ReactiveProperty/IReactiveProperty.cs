namespace Utils.ReactiveProperty
{
    public interface IReactiveProperty<T> : IReadonlyReactiveProperty<T>
    {
        new T Value { get; set; }
    }
}