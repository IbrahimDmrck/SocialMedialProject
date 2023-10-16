using Core.Entities;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public interface IBusinessRepository<T> where T : class, IEntity, new()
    {
        IResult Add(T entity);
        IResult Delete(int entityId);
        IResult Update(T entity);
        IDataResult<List<T>> GetAll();
        IDataResult<T> GetEntityById(int entityId);
    }
}
