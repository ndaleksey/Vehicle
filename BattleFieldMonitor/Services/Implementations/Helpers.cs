namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal static class Helpers
    {
        #region Methods
        public static T Exchange<T>(ref T field, T value)
        {
            var oldValue = field;
            field = value;
            return oldValue;
        }
        #endregion
    }
}
