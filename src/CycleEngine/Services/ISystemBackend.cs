namespace CycleEngine.Services
{
    public interface ISystemBackend : IService
    {
        public void UpdateWindowTitle(string text);
    }
}