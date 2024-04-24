using AssignmentDAL.Model;
using AssignmentDAL.Models;
using AssignmentDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentBAL.Interfaces
{
    public interface IPatientService
    {
        public Task<PagedList<TableView>> GetPatients(int pageNumber = 1,int pageSize = 10,string search="");
        public List<Doctor> GetDoctors();
        public bool AddPatient(TableView model);
        public bool DeletePatient(int patientid);
    }
}
