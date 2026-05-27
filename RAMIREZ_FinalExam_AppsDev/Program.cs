using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using RAMIREZ_FinalExam_AppsDev.Models;
using RAMIREZ_FinalExam_AppsDev.Services;

namespace RAMIREZ_FinalExam_AppsDev
{
    class Program
    {
        static FileRepository repository = new FileRepository();

        static void Main(string[] args)
        {
            InitializeStorage();

            while (true)
            {
                Console.WriteLine("\n=== CLINIC APPOINTMENT SYSTEM ===");
                Console.WriteLine("1. Add Appointment");
                Console.WriteLine("2. View Appointments");
                Console.WriteLine("3. Update Appointment");
                Console.WriteLine("4. Soft Delete Appointment");
                Console.WriteLine("5. Hard Delete Appointment");
                Console.WriteLine("6. Generate Report");
                Console.WriteLine("7. Exit");

                Console.Write("Choose: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAppointment();
                        break;

                    case "2":
                        ViewAppointments();
                        break;

                    case "3":
                        UpdateAppointment();
                        break;

                    case "4":
                        SoftDelete();
                        break;

                    case "5":
                        HardDelete();
                        break;

                    case "6":
                        GenerateReport();
                        break;

                    case "7":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void InitializeStorage()
        {
            Directory.CreateDirectory("Data");

            if (!File.Exists("Data/appointments.json"))
            {
                File.WriteAllText("Data/appointments.json", "[]");
            }

            if (!File.Exists("Data/audit.log"))
            {
                File.Create("Data/audit.log").Close();
            }
        }

        static void AddAppointment()
        {
            try
            {
                List<Appointment> appointments =
                    repository.LoadAppointments();

                Appointment appointment = new Appointment();

                appointment.RecordId = appointments.Count == 0
                    ? 1
                    : appointments.Max(a => a.RecordId) + 1;

                Console.Write("Patient Name: ");
                appointment.PatientName = Console.ReadLine();

                Console.Write("Age: ");
                appointment.Age = int.Parse(Console.ReadLine());

                Console.Write("Doctor Name: ");
                appointment.DoctorName = Console.ReadLine();

                Console.Write("Appointment Date (yyyy-mm-dd): ");
                appointment.AppointmentDate =
                    DateTime.Parse(Console.ReadLine());

                Console.Write("Reason: ");
                appointment.Reason = Console.ReadLine();

                appointment.CreatedAt = DateTime.Now;
                appointment.UpdatedAt = DateTime.Now;
                appointment.IsActive = true;

                if (!ValidationService.ValidateAppointment(
                    appointment, out string error))
                {
                    Console.WriteLine(error);
                    return;
                }

                appointment.Checksum =
                    repository.GenerateChecksum(appointment);

                appointments.Add(appointment);

                repository.SaveAppointments(appointments);

                AuditLogger.Log(
                    "ADD",
                    $"Added appointment {appointment.RecordId}");

                Console.WriteLine(
                    $"Appointment added with ID: {appointment.RecordId}");
            }
            catch (Exception ex)
            {
                AuditLogger.Log("ERROR", ex.Message);
                Console.WriteLine("Error occurred.");
            }
        }

        static void ViewAppointments()
        {
            try
            {
                List<Appointment> appointments =
                    repository.LoadAppointments();

                if (appointments.Count == 0)
                {
                    Console.WriteLine("No appointments found.");
                    return;
                }

                Console.WriteLine("\n=== APPOINTMENT LIST ===");

                foreach (var a in appointments.Where(a => a.IsActive))
                {
                    Console.WriteLine($"\nRecord ID: {a.RecordId}");
                    Console.WriteLine($"Patient Name: {a.PatientName}");
                    Console.WriteLine($"Age: {a.Age}");
                    Console.WriteLine($"Doctor Name: {a.DoctorName}");
                    Console.WriteLine($"Appointment Date: {a.AppointmentDate}");
                    Console.WriteLine($"Reason: {a.Reason}");
                }

                AuditLogger.Log("READ", "Viewed all appointments.");
            }
            catch (Exception ex)
            {
                AuditLogger.Log("ERROR", ex.Message);
                Console.WriteLine("Error viewing appointments.");
            }
        }

        static void UpdateAppointment()
        {
            List<Appointment> appointments =
                repository.LoadAppointments();

            Console.Write("Enter Record ID: ");
            int id = int.Parse(Console.ReadLine());

            Appointment appointment =
                appointments.FirstOrDefault(
                    a => a.RecordId == id);

            if (appointment == null)
            {
                Console.WriteLine("Record not found.");
                return;
            }

            Console.Write("New Doctor Name: ");
            appointment.DoctorName = Console.ReadLine();

            appointment.UpdatedAt = DateTime.Now;

            appointment.Checksum =
                repository.GenerateChecksum(appointment);

            repository.SaveAppointments(appointments);

            AuditLogger.Log(
                "UPDATE",
                $"Updated {appointment.RecordId}");

            Console.WriteLine("Updated successfully.");
        }


        static void SoftDelete()
        {
            try
            {
                List<Appointment> appointments =
                    repository.LoadAppointments();

                Console.Write("Enter Record ID: ");

                int id = int.Parse(Console.ReadLine());

                Appointment appointment =
                    appointments.FirstOrDefault(
                        a => a.RecordId == id);

                if (appointment == null)
                {
                    Console.WriteLine("Appointment not found.");
                    return;
                }

                appointment.IsActive = false;
                appointment.UpdatedAt = DateTime.Now;

                repository.SaveAppointments(appointments);

                AuditLogger.Log(
                    "SOFT_DELETE",
                    $"Soft deleted appointment {id}");

                Console.WriteLine("Appointment soft deleted.");
            }
            catch (Exception ex)
            {
                AuditLogger.Log("ERROR", ex.Message);
                Console.WriteLine("Error deleting appointment.");
            }
        }

        static void HardDelete()
        {
            try
            {
                List<Appointment> appointments =
                    repository.LoadAppointments();

                Console.Write("Enter Record ID: ");

                int id = int.Parse(Console.ReadLine());

                Appointment appointment =
                    appointments.FirstOrDefault(
                        a => a.RecordId == id);

                if (appointment == null)
                {
                    Console.WriteLine("Appointment not found.");
                    return;
                }

                appointments.Remove(appointment);

                repository.SaveAppointments(appointments);

                AuditLogger.Log(
                    "HARD_DELETE",
                    $"Hard deleted appointment {id}");

                Console.WriteLine("Appointment permanently deleted.");
            }
            catch (Exception ex)
            {
                AuditLogger.Log("ERROR", ex.Message);
                Console.WriteLine("Error deleting appointment.");
            }
        }

        static void GenerateReport()
        {
            List<Appointment> appointments =
                repository.LoadAppointments();

            ReportGenerator.GenerateDoctorReport(appointments);

            AuditLogger.Log(
                "REPORT",
                "Generated doctor report.");
        }
    }
}