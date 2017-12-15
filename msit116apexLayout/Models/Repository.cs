using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        MSIT116APEXEntities db = null;
        DbSet<T> dbSet = null;
        public Repository()
        {
            db = new MSIT116APEXEntities();
            dbSet = db.Set<T>();
        }

        public void Create(T _entity)
        {
            dbSet.Add(_entity);
            db.SaveChanges();
        }

        public void Delete(T _entity)
        {
            dbSet.Remove(_entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            dbSet.Remove(dbSet.Find(id));
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            dbSet.Remove(dbSet.Find(id));
            db.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetByID(string id)
        {
            return dbSet.Find(id);
        }

        public T GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public void Update(T _entity)
        {
            db.Entry(_entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateWithoutNull(T _entity)
        {
            db.Entry(_entity).State = EntityState.Modified;
            DbEntityEntry<T> entry = db.Entry(_entity);
            foreach (var property in entry.CurrentValues.PropertyNames)
            {
                var current = entry.CurrentValues.GetValue<object>(property);
                if (current == null)
                    entry.Property(property).IsModified = false;
            }
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                //throw;
            }
            
        }
    }
}