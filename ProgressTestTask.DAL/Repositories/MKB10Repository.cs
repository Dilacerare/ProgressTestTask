using ProgressTestTask.DAL.Interfaces;
using ProgressTestTask.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.DAL.Repositories
{
    public class MKB10Repository: IBaseRepository<MKB10>
    {
        private readonly ApplicationDbContext _db;

        public MKB10Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<MKB10> GetAll()
        {
            return _db.MKB10;
        }

        public async Task Delete(MKB10 entity)
        {
            _db.MKB10.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(MKB10 entity)
        {
            await _db.MKB10.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<MKB10> Update(MKB10 entity)
        {
            _db.MKB10.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
