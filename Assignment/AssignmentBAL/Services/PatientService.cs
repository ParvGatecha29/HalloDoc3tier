using AssignmentBAL.Interfaces;
using AssignmentDAL.Data;
using AssignmentDAL.Model;
using AssignmentDAL.Models;
using AssignmentDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssignmentBAL.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedList<TableView>> GetPatients(int pageNumber, int pageSize, string search)
        {
            var patients = _context.Patients.Include(_ => _.Doctor).AsQueryable();
            if (search != null)
            {
                patients = patients.Where(x =>
                x.Firstname.ToLower().Contains(search) ||
                x.Lastname.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search) ||
                x.Age.ToString().ToLower().Contains(search) ||
                x.Phonenumber.ToLower().Contains(search) ||
                x.Gender.ToLower().Contains(search) ||
                x.Disease.ToLower().Contains(search) ||
                x.Doctor.Specialist.ToLower().Contains(search)
                );
            }
            List<TableView> result = new List<TableView>();
            foreach (var pat in patients)
            {
                TableView tableView = new TableView
                {
                    Patientid = pat.Id,
                    Firstname = pat.Firstname,
                    Lastname = pat.Lastname,
                    Email = pat.Email,
                    Age = pat.Age,
                    Phonenumber = pat.Phonenumber,
                    Gender = pat.Gender,
                    Disease = pat.Disease,
                    Doctor = pat.Doctor.Specialist
                };
                result.Add(tableView);
            }
            List<TableView> list = result.ToList();
            list = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            PagedList<TableView> pageList = await PagedList<TableView>.CreateAsync(list, result.Count(), pageNumber, pageSize);
            return pageList;
        }

        public List<Doctor> GetDoctors()
        {
            return _context.Doctors.ToList();
        }

        public bool AddPatient(TableView model)
        {
            Doctor doctor = _context.Doctors.FirstOrDefault(x => x.Specialist == model.Doctor);
            if (doctor == null)
            {
                doctor = new Doctor();
                doctor.Specialist = model.Doctor;
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
            }

            Patient patient;

            if (model.Patientid != 0)
            {
                patient = _context.Patients.FirstOrDefault(x => x.Id == model.Patientid);
                patient.Firstname = model.Firstname;
                patient.Lastname = model.Lastname;
                patient.DoctorId = doctor.DoctorId;
                patient.Age = model.Age;
                patient.Email = model.Email;
                patient.Phonenumber = model.Phonenumber;
                patient.Gender = model.Gender;
                patient.Disease = model.Disease;
                patient.Specialist = model.Doctor;
                _context.Patients.Update(patient);
            }
            else
            {
                patient = new Patient
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    DoctorId = doctor.DoctorId,
                    Age = model.Age,
                    Email = model.Email,
                    Phonenumber = model.Phonenumber,
                    Gender = model.Gender,
                    Disease = model.Disease,
                    Specialist = model.Doctor
                };
                _context.Patients.Add(patient);
            }
            
            _context.SaveChanges();
            return true;
        }

        public bool DeletePatient(int patientid)
        {
            Patient pat = _context.Patients.FirstOrDefault(x => x.Id == patientid);
            if (pat != null)
            {
                _context.Patients.Remove(pat);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
