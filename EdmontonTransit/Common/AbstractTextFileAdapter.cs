using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmontonTransit.Common
{
    public abstract class AbstractTextFileAdapter<T> : ITextFileAdapter<T> where T:class
    {
        public bool SkipFirstLine { get; set; }
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTextFileAdapter"/> class.
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <param name="filePath"></param>
        public AbstractTextFileAdapter(FileFormat fileFormat, string filePath, bool skipFirstLine)
        {
            FileFormat = fileFormat;
            FilePath = filePath;
            SkipFirstLine = skipFirstLine;
        }
        #endregion

        #region ITextFileAdapter Implementations
        public FileFormat FileFormat { get; private set; }
        public string FilePath { get; private set; }

        public IList<T> Load()
        {
            var result = new List<T>();
            switch (FileFormat)
            {
                case FileFormat.CSV:
                    result = LoadList(new CSVFileIO(FilePath));
                    break;
                // TODO: Handle case FileFormat.XML
                // TODO: Handle case FileFormat.JSON
                default:
                    throw new NotSupportedException($"The file format {FileFormat} is not supported by this adapter");
            }
            return result;
        }
        #endregion

        #region Private methods
        private List<T> LoadList(CSVFileIO reader)
        {
            List<T> data = new List<T>();
            List<string> lines = reader.ReadAllLines();
            if (SkipFirstLine)
                lines = lines.Skip(1).ToList();
            foreach (string individualLine in lines)
            {
                data.Add(ParseRow(individualLine));
            }
            return data;
        }

        // TODO: XMLFileIO version of LoadList
        //private static List<PhoneNumber> LoadList(XMLFileIO<PhoneNumber> reader)
        //{
        //    return reader.LoadAll();
        //}
        #endregion

        protected abstract T ParseRow(string row);
    }
}
