using System;
using System.Threading.Tasks;

namespace Lazy.Authentication.Utilities.Helpers
{
    public static class TaskHelper
    {
        public static void ContinueWithNoWait<TType>(this Task<TType> updateAllReferences, Action<Task<TType>> logUpdate)
        {
            updateAllReferences.ContinueWith(logUpdate);
        }
    }
}