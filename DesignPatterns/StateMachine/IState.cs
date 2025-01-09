namespace KuroNeko.Utilities.DesignPattern
{
    public interface IState
    {
        void StateEnter(params dynamic[] args);
        void StateUpdate(params dynamic[] args);
        void StateExit(params dynamic[] args);
    }
}