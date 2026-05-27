using RAMIREZ_FinalExam_AppsDev.Models;
using RAMIREZ_FinalExam_AppsDev.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace RAMIREZ_FinalExam_AppsDev.Services
{
    public class FileRepository
    {
        private readonly string filePath = "Data/appointments.json";

        public List<Appointment> LoadAppointments()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<Appointment>();
                }

                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Appointment>();
                }

                return JsonSerializer.Deserialize<List<Appointment>>(json)
                       ?? new List<Appointment>();
            }
            catch
            {
                AuditLogger.Log("ERROR", "Failed to load appointments.");
                return new List<Appointment>();
            }
        }

        public void SaveAppointments(List<Appointment> appointments)
        {
            string json = JsonSerializer.Serialize(
                appointments,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, json);
        }

        public string GenerateChecksum(Appointment appointment)
        {
            string rawData =
                appointment.RecordId +
                appointment.PatientName +
                appointment.Age +
                appointment.DoctorName +
                appointment.AppointmentDate +
                appointment.Reason;

            using SHA256 sha = SHA256.Create();

            byte[] bytes = sha.ComputeHash(
                Encoding.UTF8.GetBytes(rawData));

            return Convert.ToBase64String(bytes);
        }
    }
}