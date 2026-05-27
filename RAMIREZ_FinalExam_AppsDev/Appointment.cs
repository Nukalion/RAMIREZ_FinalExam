using System;

namespace RAMIREZ_FinalExam_AppsDev.Models
{
    public class Appointment
    {
        public int RecordId { get; set; }

        public string PatientName { get; set; }
        public int Age { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string Checksum { get; set; }
    }
}