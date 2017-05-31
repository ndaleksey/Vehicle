namespace Swsu.BattleFieldMonitor.Protocol
{
    internal static class ArrayHelpers
    {
        #region Methods
        public static void EnsureLength<T>(ref T[] array, int minLength)
        {
            if (array == null || array.Length < minLength)
            {
                array = new T[minLength];
            }
        }
        #endregion
    }
}
