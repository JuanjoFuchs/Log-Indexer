using System.Collections.Generic;
using System.Linq;

namespace LogIndexer {
  public class Log {
    public int Id { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public int LastLineNumber { get; set; }

    public Log(string path) {
      Id = path.GetHashCode();
      Path = path;
      Name = System.IO.Path.GetFileNameWithoutExtension(path);
    }

    public Line CreateLine(string line, int index) {
      LastLineNumber = index;
      return new Line { Data = line, Number = index, LogId = Id };
    }
  }
}