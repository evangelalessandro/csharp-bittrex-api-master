using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRexSql.Repo
{
    public interface IRepository<T> : IDisposable where T : class
    {
        //Method to get all rows in a table
        IEnumerable<T> DataSet { get; }

        //Method to add row to the table
        void Add(T entity);


        //Method to fetch row from the table
        T GetById(int? id);

        //Method to update a row in the table
        void Update(T entity);

        //Method to delete a row from the table
        void Delete(T entity);
    }
}
