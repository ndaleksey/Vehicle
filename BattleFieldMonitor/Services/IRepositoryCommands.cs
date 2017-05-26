namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IRepositoryCommands<T, TState>
    {
        #region Methods
        void Create(TState state);

        void Delete(T obj);

        void Update(T obj, TState state);
        #endregion
    }
}
