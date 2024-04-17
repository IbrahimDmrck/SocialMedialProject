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
        private readonly IArticleDal _articleDal;
        private readonly ICommentDal _commentDal;
        private readonly IUserImageDal _userImageDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        public UserManager(IUserDal userDal, IArticleDal articleDal, ICommentDal commentDal, IUserImageDal userImageDal, IUserOperationClaimDal userOperationClaimDal)
        {
            _userDal = userDal;
            _articleDal = articleDal;
            _commentDal = commentDal;
            _userImageDal = userImageDal;
            _userOperationClaimDal = userOperationClaimDal;
        }

        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll(), Messages.UsersListed);
        }

        public IDataResult<List<UserDto>> GetAllDto()
        {
            return new SuccessDataResult<List<UserDto>>(_userDal.GetUsersDtos(), Messages.UsersListed);
        }

        public IDataResult<User> GetUserById(int userId)
        {
            var user = _userDal.Get(u => u.Id == userId);
            if (user != null)
            {
                return new SuccessDataResult<User>(user, Messages.UserListed);
            }

            return new ErrorDataResult<User>(Messages.UserNotExist);
        }

        public IDataResult<UserDto> GetUserDtoById(int userId)
        {
            return new SuccessDataResult<UserDto>(_userDal.GetUsersDtos(u => u.Id == userId).SingleOrDefault(), Messages.UserListed);
        }

        // [ValidationAspect(typeof(UserValidator))]
        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Add(User user)
        {
            var rulesResult = BusinessRules.Run(CheckIfEmailExist(user.Email));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            _userDal.Add(user);
            return new SuccessResult(Messages.UserAdded);
        }

        [ValidationAspect(typeof(UserValidator))]
        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(User user)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(user.Id), CheckIfEmailAvailable(user.Email));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdated);
        }

        //  [ValidationAspect(typeof(UserDtoValidator))]
        [LogAspect(typeof(FileLogger))]
        public IResult UpdateByDto(UserDto userDto)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(userDto.Id), CheckIfEmailAvailable(userDto.Email));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            var updatedUser = _userDal.Get(u => u.Id == userDto.Id && u.Email == userDto.Email);
            if (updatedUser == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            updatedUser.FirstName = userDto.FirstName;
            updatedUser.LastName = userDto.LastName;
            updatedUser.Gender = userDto.Gender;
            updatedUser.Email = userDto.Email;
            updatedUser.PhoneNumber = userDto.PhoneNumber;

            _userDal.Update(updatedUser);
            return new SuccessResult(Messages.UserUpdated);
        }

        //  [ValidationAspect(typeof(UserValidator))]
        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(user.Id));
            if (rulesResult != null)
            {
                return new ErrorDataResult<List<OperationClaim>>(rulesResult.Message);
            }

            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }
        [LogAspect(typeof(FileLogger))]
        public async Task<IDataResult<User>> GetUserByMail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Email == email));
        }
        [LogAspect(typeof(FileLogger))]
        public IDataResult<UserDto> GetUserDtoByMail(string email)
        {
            return new SuccessDataResult<UserDto>(_userDal.GetUsersDtos(u => u.Email == email).SingleOrDefault(), message: Messages.UserListed);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IArticleService.Get")]
        [CacheRemoveAspect("ICommentService.Get")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(int userId)
        {
            var rulesResult = BusinessRules.Run(CheckIfUserIdExist(userId));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            var deletedUser = _userDal.Get(u => u.Id == userId);
            var deletedArticle = _articleDal.GetAll(x => x.UserId == userId);
            if (deletedArticle != null)
            {
                foreach (var item in deletedArticle)
                {
                    var deletedComment = _commentDal.GetAll(x => x.ArticleId == item.Id);
                    if (deletedComment != null)
                    {
                        foreach (var comment in deletedComment)
                        {
                            _commentDal.Delete(comment);
                        }
                    }
                    _articleDal.Delete(item);
                }
            }

            var deletedImage = _userImageDal.Get(c => c.UserId == userId);
            var result = FileHelper.Delete(deletedImage.ImagePath);
            if (!result.Success)
            {
                return new ErrorResult(Messages.ErrorDeletingImage);
            }
            _userImageDal.Delete(deletedImage);

            var deletedClaims = _userOperationClaimDal.GetAll(x=>x.UserId==userId);
            if (deletedClaims !=null)
            {
                foreach (var userClaim in deletedClaims)
                {
                    var deletedClaim = _userOperationClaimDal.Get(x => x.UserId == userClaim.UserId && x.OperationClaimId == userClaim.OperationClaimId);
                    _userOperationClaimDal.Delete(deletedClaim);
                }
            }

            _userDal.Delete(deletedUser);
            return new SuccessResult(Messages.UserDeleted);

        }

        //Business Rules

        private IResult CheckIfUserIdExist(int userId)
        {
            var result = _userDal.GetAll(u => u.Id == userId).Any();
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
                return new ErrorResult(Messages.UserEmailNotAvailable);
            }
            return new SuccessResult();
        }

        private bool BaseCheckIfEmailExist(string userEmail)
        {
            return _userDal.GetAll(u => u.Email == userEmail).Any();
        }
    }
}
