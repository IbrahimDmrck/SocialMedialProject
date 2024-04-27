using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingconcerns.Logging.Log4Net.Loggers;
using Core.Entities;
using Core.Entities.Concrete;
using Core.Utilities.Business;
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

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(OperationCValidator))]
        [SecuredOperation("admin")]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        public IResult Add(OperationClaim entity)
        {
            var rulesResult = BusinessRules.Run(CheckIfOperationClaimIdExist(entity.Name));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            _operationClaimDal.Add(entity);
            return new SuccessResult(Messages.ClaimAdd);
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(OperationCValidator))]
        [SecuredOperation("admin")]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        public IResult Delete(int id)
        {
            var deletedClaim = _operationClaimDal.Get(x => x.Id == id);
            if (deletedClaim != null)
            {
                _operationClaimDal.Delete(deletedClaim);
                return new SuccessResult(Messages.ClaimDeleted);
            }
            return new ErrorResult(Messages.ClaimNotFound);
        }

        [CacheAspect(5)]
        public IDataResult<List<OperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll(),Messages.ClaimsListed);
        }

        [CacheAspect(5)]
        public IDataResult<List<ClaimDto>> GetClaimByUsers(int claimId)
        {
            return new SuccessDataResult<List<ClaimDto>>(_operationClaimDal.GetClaims(x=>x.OperationClaimId== claimId), Messages.ClaimsListed);
        }

        public IDataResult<OperationClaim> GetEntityById(int id)
        {
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(x=>x.Id==id), Messages.ClaimListed);
        }

        public IDataResult<List<ClaimDto>> GetClaimsById(int claimId)
        {
            return new SuccessDataResult<List<ClaimDto>>(_operationClaimDal.GetClaims(x => x.OperationClaimId == claimId), Messages.ClaimsListed);
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(OperationCValidator))]
        [SecuredOperation("admin")]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        public IResult Update(OperationClaim entity)
        {
            var rulesResult = BusinessRules.Run(CheckIfOperationClaimIdExist(entity.Name));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            _operationClaimDal.Update(entity);
            return new SuccessResult(Messages.ClaimAdd);
        }

        //Business Rules

        private IResult CheckIfOperationClaimIdExist(string claim)
        {
            var result = _operationClaimDal.GetAll(u => u.Name.ToLower() == claim.ToLower()).Any();
            if (result)
            {
                return new ErrorResult(Messages.ClaimExist);
            }
            return new SuccessResult();
        }
    }
}
