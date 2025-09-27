using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTracker.Models
{
    public class Break
    {
        
            public int Id { get; set; }
            public int WorkSessionId { get; set; }
            public WorkSession WorkSession { get; set; }

            public DateTime BreakStartTime { get; set; }
            public DateTime? BreakEndTime { get; set; }

            // stored as minutes
            public double BreakDurationMinutes { get; set; }
        }

    }

