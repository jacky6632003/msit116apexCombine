using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.CRM.Models
{
    public class InterviewRepository<T> : IRepository<T> where T : class
    {
        private MSIT116APEXEntities db = null;
        private DbSet<T> dbset = null;

        public InterviewRepository()
        {
            db = new MSIT116APEXEntities();
            dbset = db.Set<T>();
        }
        public void Create(T _entity)
        {
            dbset.Add(_entity);
            db.SaveChanges();
        }

        public void Delete(T _entity)
        {
            dbset.Remove(_entity);
            db.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return dbset.ToList();
        }

        public T GetById(int? id)
        {
            
            return dbset.Find(id);
        }

        public void Update(T _entity)
        {
            db.Entry(_entity).State = EntityState.Modified;
            db.SaveChanges(); 
        }
    }
}