using Business.Abstract;
using Business.Constants;
using Core.Entities;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.Entityframework;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OperationClaimManager: IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        public IResult Add(OperationClaim entity)
        {
            _operationClaimDal.Add(entity);
            return new SuccessResult(Messages.ClaimAdded);
        }

        public IResult Delete(int id)
        {
            var deletedClaim = _operationClaimDal.Get(x => x.Id == id);
            _operationClaimDal.Delete(deletedClaim);
            return new SuccessResult(Messages.ClaimDeleted);
        }

        public IDataResult<List<OperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll(),Messages.ClaimsListed);
        }

        public IDataResult<List<ClaimDto>> GetClaimByUsers(int claimId)
        {
            return new SuccessDataResult<List<ClaimDto>>(_operationClaimDal.GetClaims(x=>x.OperationClaimId== claimId), Messages.ClaimsListed);
        }

        public IDataResult<OperationClaim> GetEntityById(int id)
        {
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(x=>x.Id==id), Messages.ClaimListed);
        }

        public IResult Update(OperationClaim entity)
        {
            _operationClaimDal.Update(entity);
            return new SuccessResult(Messages.ClaimUpdated);
        }
    }
}
