﻿using System.Threading.Tasks;

namespace Mohammad.Helpers
{
    public static class AsyncHelper
    {
        public static async void CatchAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // ignored
            }
        }
    }
}