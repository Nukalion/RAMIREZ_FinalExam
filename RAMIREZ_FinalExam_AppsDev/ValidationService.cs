using RAMIREZ_FinalExam_AppsDev.Models;
using RAMIREZ_FinalExam_AppsDev;
using System;

namespace RAMIREZ_FinalExam_AppsDev.Services
{
    public static class ValidationService
    {
        public static bool ValidateAppointment(Appointment appointment, out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(appointment.PatientName))
            {
                error = "Patient name is required.";
                return false;
            }

            if (appointment.Age <= 0)
            {
                error = "Invalid age.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(appointment.DoctorName))
            {
                error = "Doctor name is required.";
                return false;
            }

            if (appointment.AppointmentDate < DateTime.Now)
            {
                error = "Appointment date cannot be in the past.";
                return false;
            }

            return true;
        }
    }
}