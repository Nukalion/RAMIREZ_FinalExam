using RAMIREZ_FinalExam_AppsDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RAMIREZ_FinalExam_AppsDev.Services
{
    public static class ReportGenerator
    {
        public static void GenerateDoctorReport(List<Appointment> appointments)
        {
            var grouped = appointments
                .Where(a => a.IsActive)
                .GroupBy(a => a.DoctorName);

            Console.WriteLine("\nAppointments Per Doctor\n");

            foreach (var group in grouped)
            {
                Console.WriteLine(
                    $"Doctor: {group.Key} | Total Appointments: {group.Count()}");
            }
        }
    }
}