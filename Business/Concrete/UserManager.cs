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
using Core.Utilities.Helpers.FileHelper;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IUserImageService _userImageService;
        private readonly IArticleService _articleService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly ICommentService _commentService;

        public UserManager(IUserDal userDal, IUserImageService userImageService, IArticleService articleService, IUserOperationClaimService userOperationClaimService, ICommentService commentService)
        {
            _userDal = userDal;
            _userImageService = userImageService;
            _articleService = articleService;
            _userOperationClaimService = userOperationClaimService;
            _commentService = commentService;
        }

        [LogAspect(typeof(FileLogger))]
        //[SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Add(User entity)
        {
            var rulesResult = BusinessRules.Run(CheckIfEmailExist(entity.Email));
            if (rulesResult != null)
            {
                return rulesResult;
            }
            _userDal.Add(entity);
            return new SuccessResult(Messages.UserAdded);
        }


        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(int id)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(id));
            if (rulesResult != null)
            {
                return rulesResult;
            }
            var deletedUser = _userDal.Get(x => x.Id == id);
            _userDal.Delete(deletedUser);
            return new SuccessResult(Messages.userDeleted);

        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult DeleteById(int userId)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(userId));
            if (rulesResult != null)
            {
                return rulesResult;
            }
            var deletedUser = _userDal.Get(x => x.Id == userId);
            _userImageService.DeleteAllImagesOfUserByUserId(userId);

            var userArticles = _articleService.GetArticleDetailsByUserId(userId);
            foreach (var item in userArticles.Data)
            {
                _articleService.Delete(item.Id);
            }

            _commentService.AllCommentDeleteByUserId(userId);

            var userClaims = _userOperationClaimService.GetAllByUserId(userId);
            foreach (var item in userClaims.Data)
            {
                _userOperationClaimService.Delete(item.UserId, item.OperationClaimId);
            }

            _userDal.Delete(deletedUser);
            return new SuccessResult(Messages.userDeleted);
        }

        [CacheAspect(2)]
        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll(), Messages.UsersListed);
        }
        [CacheAspect(2)]
        public IDataResult<List<UserDto>> GetAllDto()
        {
            return new SuccessDataResult<List<UserDto>>(_userDal.GetUsersDtos(), Messages.UsersListed);
        }

        public IDataResult<User> GetUserById(int id)
        {
            var user = _userDal.Get(x => x.Id == id);
            if (user != null)
            {
                return new SuccessDataResult<User>(user, Messages.UserListed);
            }
            return new ErrorDataResult<User>(Messages.UserNotExist);
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(user.Id));
            if (rulesResult != null)
            {
                return new ErrorDataResult<List<OperationClaim>>(rulesResult.Message);
            }
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IDataResult<User> GetUserByMail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(x => x.Email == email));
        }


        public IDataResult<UserDto> GetUserDtoById(int userId)
        {
            return new SuccessDataResult<UserDto>(_userDal.GetUsersDtos(x => x.Id == userId).SingleOrDefault(), Messages.UserListed);
        }

        [LogAspect(typeof(FileLogger))]
        // [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(User entity)
        {
            var result = BusinessRules.Run(CheckIfUserIdExist(entity.Id), CheckIfEmailAvailable(entity.Email));
            if (result != null)
            {
                return result;
            }

            _userDal.Update(entity);
            return new SuccessResult(Messages.UserUpdated);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [ValidationAspect(typeof(UserValidator))]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult UpdateByDto(UserDto userDto)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(userDto.Id), CheckIfEmailAvailable(userDto.Email));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            var updatedUser = _userDal.Get(x => x.Id == userDto.Id && x.Email == userDto.Email);
            if (updatedUser == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            updatedUser.FirstName = userDto.FirstName;
            updatedUser.LastName = userDto.LastName;
            updatedUser.Email = userDto.Email;
            updatedUser.Gender = userDto.Gender;
            updatedUser.PhoneNumber = userDto.PhoneNumber;

            _userDal.Update(updatedUser);
            return new SuccessResult(Messages.UserUpdated);
        }

        //Business Rules

        private IResult CheckIfUserIdExist(int userId)
        {
            var result = _userDal.GetAll(x => x.Id == userId).Any();
            if (!result)
            {
                return new ErrorResult(Messages.UserNotExist);
            }
            return new SuccessResult();
        }

        private IResult CheckIfEmailExist(string userEmail)
        {
            var result = BaseCheckIfEmailExist(userEmail);
            if (result)
            {
                return new ErrorResult(Messages.UserEmailExist);
            }
            return new SuccessResult();
        }

        private IResult CheckIfEmailAvailable(string userEmail)
        {
            var result = BaseCheckIfEmailExist(userEmail);
            if (!result)
            {
                return new ErrorResult(Messages.userEmailNotAvailable);
            }
            return new SuccessResult();
        }

        private bool BaseCheckIfEmailExist(string userEmail)
        {
            return _userDal.GetAll(x => x.Email == userEmail).Any();
        }

      
    }
}
