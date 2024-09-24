using ProgressTestTask.DAL.Interfaces;
using ProgressTestTask.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.DAL.Repositories
{
    public class VisitRepository: IBaseRepository<Visit>
    {
        private readonly ApplicationDbContext _db;

        public VisitRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Visit> GetAll()
        {
            return _db.Visits;
        }

        public async Task Delete(Visit entity)
        {
            _db.Visits.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(Visit entity)
        {
            await _db.Visits.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Visit> Update(Visit entity)
        {
            _db.Visits.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
