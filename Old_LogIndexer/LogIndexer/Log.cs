using System;
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

    class Ejemplo {
        public IEnumerable<EventsApplication> Logs { get; set; }

        public void a() {
            var a = 
            from log in Logs
            where log.AgentUniqueId == TargetAgent.UniqueId
                  && (log.Action.Contains("Sending") || log.Action.Contains("Publishing"))
                  && log.State.AgentState == AgentState.AgentReady
            select log;
        }

        internal enum AgentState {
            AgentReady = 0
        }

        public Agent TargetAgent { get; set; }

        internal class Agent {
            public Guid UniqueId { get; set; }
        }
    }

    class WebExceptionLog {
        public DateTime DateTime { get; set; }
        public string Level { get; set; }
        public string Controller { get; set; }
        public string Message { get; set; }
        public WebException Exception { get; set; }

    }

    internal class WebException
    {
        public string ExceptionMessage { get; set; }
        public StackTrace StackTrace { get; set; }
    }

    internal class StackTrace {}


    class EventsApplication {
        public DateTime DateTime { get; set; }
        public string Level { get; set; }
        public string Source { get; set; }
        public string Action { get; set; }
        public Guid AgentUniqueId { get; set; }
        public StateDTO State { get; set; } 
    }

    internal class StateDTO {
        public Ejemplo.AgentState AgentState { get; set; }
    }
}