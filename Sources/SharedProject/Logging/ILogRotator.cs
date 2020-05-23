#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.Logging
{
    public interface ILogRotator
    {
        bool IsLogRotationEnabled { get; set; }
    }
}