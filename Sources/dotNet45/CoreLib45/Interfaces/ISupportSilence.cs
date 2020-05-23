#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.Interfaces
{
    /// <summary>
    /// </summary>
    public interface ISupportSilence
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [enable raising events].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [enable raising events]; otherwise, <c>false</c>.
        /// </value>
        bool EnableRaisingEvents { get; set; }
    }
}