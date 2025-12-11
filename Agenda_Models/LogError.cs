using System.ComponentModel.DataAnnotations;

namespace Agenda_Models
{
    public class LogError
    // Model toegevoegd om fouten te loggen in de database
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string Application { get; set; }
        public string LogLevel { get; set; }
        public int ThreadId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }

        [DataType(DataType.MultilineText)]
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }

        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
