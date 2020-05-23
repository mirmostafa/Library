#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.DesignPatterns.Creational
{
    public interface ISingleton<TSingletone>
        where TSingletone : class, ISingleton<TSingletone>
    {
    }
}