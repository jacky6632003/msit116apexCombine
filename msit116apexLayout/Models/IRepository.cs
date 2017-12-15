using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public interface IRepository<T>
    {
        T GetByID(int id);
        T GetByID(string id);
        IEnumerable<T> GetAll();
        void Create(T _entity);
        void Update(T _entity);
        void UpdateWithoutNull(T _entity);
        void Delete(T _entity);
        void Delete(int id);
        void Delete(string id);
    }
}