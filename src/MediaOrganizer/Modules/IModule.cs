namespace MediaOrganizer.Modules
{
    public interface IModule
    {
        string Name { get; set; }
        void Start();
        void End();
    }
}
