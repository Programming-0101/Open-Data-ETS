using System;
using System.Collections.Generic;
using System.Linq;

namespace EdmontonTransit.Common
{
    public interface ITextFileAdapter<T>
    {
        FileFormat FileFormat { get; }
        string FilePath { get; }
        IList<T> Load();
    }
}
