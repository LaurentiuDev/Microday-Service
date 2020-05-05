using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.Features.BaseRepository.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        IQueryable<T> GetAll();

        T Get(Guid id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        bool Exists(Expression<Func<T, bool>> whereCondition);
    }
}
