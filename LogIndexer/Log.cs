using System.Collections.Generic;
using System.Linq;

namespace LogIndexer {
  public class Log {
    public int Id { get; set; }
    public string Path { get; set; }
    public List<Line> Lines { get; set; }

    public int LastLineNumber {
      get {
        if (!Lines.Any()) {
          return 0;
        }
        return Lines.Max(line => line.Number);
      }
    }

    public Log(string path) {
      Id = path.GetHashCode();
      Path = path;
      Lines = new List<Line>();
    }

    public void AddLine(Line line) {
      Lines.Add(line);
    }
  }
}