using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingconcerns.Logging.Log4Net.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.Entityframework;
using Entities.DTOs;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{

    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(UserOperationClaimValidator))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserOperationClaimService.Get")]
        public IResult Add(UserOperationClaim entity)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserOperationClaimIdExist(entity.UserId, entity.OperationClaimId));
            if (rulesResult != null)
            {
                return rulesResult;
            }
            _userOperationClaimDal.Add(entity);
            return new SuccessResult(Messages.UserClaimAdded);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserOperationClaimService.Get")]
        public IResult Delete(int userId, int claimId)
        {
            var deletedClaim = _userOperationClaimDal.Get(x => x.UserId == userId && x.OperationClaimId == claimId);
            _userOperationClaimDal.Delete(deletedClaim);
            return new SuccessResult(Messages.UserClaimDelete);
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(UserOperationClaimValidator))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserOperationClaimService.Get")]
        public IResult Update(UserOperationClaim entity)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserOperationClaimIdExist(entity.UserId,entity.OperationClaimId));
            if (rulesResult != null)
            {
                return rulesResult;
            }
            _userOperationClaimDal.Update(entity);
            return new SuccessResult(Messages.UserClaimUpdated);
        }

        [CacheAspect(5)]
        public IDataResult<List<UserOperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetAll(),Messages.UsersClaimsListed);
        }

        //Business Rules

        private IResult CheckIfUserOperationClaimIdExist(int userId,int claimId)
        {
            var result = _userOperationClaimDal.GetAll(u => u.UserId==userId && u.OperationClaimId==claimId).Any();
            if (result)
            {
                return new ErrorResult(Messages.UserClaimExist);
            }
            return new SuccessResult();
        }
    }
}
