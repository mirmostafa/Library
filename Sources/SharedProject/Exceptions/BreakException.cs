#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class BreakException : Exception
    {
        public static void Throw()
        {
            throw new BreakException();
        }
    }
}