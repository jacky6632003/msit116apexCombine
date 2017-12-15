using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace msit116apexLayout.Controllers
{
    public class ValuesController : ApiController
    {
        Repository<TestWebApi> db = new Repository<TestWebApi>();
        // GET: api/Values
        public IEnumerable<TestWebApi> Get()
        {
            return db.GetAll();
        }

        // GET: api/Values/5
        public TestWebApi Get(int id)
        {
            return db.GetByID(id);
        }

        // POST: api/Values
        public void Post(TestWebApi _entity)
        {
            db.Create(_entity);
        }

        // PUT: api/Values/5
        public void Put(int id, TestWebApi _entity)
        {
            db.Update(_entity);
        }

        // DELETE: api/Values/5
        public void Delete(int id)
        {
            db.Delete(id);
        }
    }
}
