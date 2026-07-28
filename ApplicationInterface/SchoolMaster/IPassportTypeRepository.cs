using DomainModel.SchoolMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInterface.SchoolMaster
{
    public interface IPassportTypeRepository
    {

        public Task<IEnumerable<PassportTypeModel>> GetAllAsync();

        // public Task<int> AddUpdatePassportType(PassportTypeModel objPassportType);
        public Task<string> AddUpdatePassportType(PassportTypeModel objPassportType);

        public Task<int> DeletePassportTypeData(int passportTypeId);
    }   
}
