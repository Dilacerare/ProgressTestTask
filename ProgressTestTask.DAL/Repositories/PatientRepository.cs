using ProgressTestTask.DAL.Interfaces;
using ProgressTestTask.Domain.Entity;

namespace ProgressTestTask.DAL.Repositories
{
    public class PatientRepository : IBaseRepository<Patient>
    {
        private readonly ApplicationDbContext _db;

        public PatientRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Patient> GetAll()
        {
            return _db.Patients;
        }

        public async Task Delete(Patient entity)
        {
            _db.Patients.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(Patient entity)
        {
            await _db.Patients.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Patient> Update(Patient entity)
        {
            _db.Patients.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
