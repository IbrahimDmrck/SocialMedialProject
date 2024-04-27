using Business.Constants;
using Core.Entities.Concrete;
using Core.Service;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserOperationClaimService 
    {
        IResult Delete(int id, int claimId);
        IResult Add(UserOperationClaim entity);
        IResult Update(UserOperationClaim entity);
        IDataResult<List<UserOperationClaim>> GetAll();
        IDataResult<List<UserOperationClaim>> GetAllByUserId(int id);
    }
}
