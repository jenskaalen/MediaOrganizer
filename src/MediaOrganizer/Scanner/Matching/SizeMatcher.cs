using System.IO;

namespace MediaOrganizer.Scanner.Matching
{
    public class SizeMatcher : IContentMatcher
    {
        private readonly long _minBytes;

        public SizeMatcher(long minKbSize)
        {
            _minBytes = minKbSize*1024;
        }

        public bool Match(string fullFilename)
        {
            var fileinfo = new FileInfo(fullFilename);
            return fileinfo.Length > _minBytes;
        }
    }
}
