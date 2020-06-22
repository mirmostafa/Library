namespace Mohammad.DesignPatterns.Creational {
    public interface ISingleton<TSingletone>
        where TSingletone : class, ISingleton<TSingletone> { }
}