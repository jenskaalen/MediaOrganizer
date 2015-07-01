namespace MediaOrganizer.Tests
{
    public interface IMediaLookup
    {
        string ShowExists(string show);
        string MovieExists(string movie);
    }

    public class TheMovieDbLookup : IMediaLookup
    {
        public string ShowExists(string show)
        {
            throw new System.NotImplementedException();
        }

        public string MovieExists(string movie)
        {
            throw new System.NotImplementedException();
        }
    }
}