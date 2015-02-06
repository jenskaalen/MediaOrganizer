namespace MediaOrganizer.Model.Disk
{
    public interface IFilenameChanger
    {
        string ChangedName(string originalFilename);
    }
}
