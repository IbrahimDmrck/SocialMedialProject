using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingconcerns.Logging.Log4Net.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private readonly IUserDal _userDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        private readonly IUserImageDal _userImageDal;
        public AuthManager(ITokenHelper tokenHelper, IUserService userService, IUserDal userDal, IUserOperationClaimDal userOperationClaimDal, IUserImageDal userImageDal)
        {
            _tokenHelper = tokenHelper;
            _userService = userService;
            _userDal = userDal;
            _userOperationClaimDal = userOperationClaimDal;
            _userImageDal = userImageDal;
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(AdminChangePasswordValidator))]
        public IResult AdminChangePassword(ChangePasswordModel changePassword)
        {
            var user = _userService.GetUserByMail(changePassword.Email);

            if (user != null)
            {
                var data = user.Data;
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(changePassword.NewPassword, out passwordHash, out passwordSalt);
                data.PasswordHash = passwordHash;
                data.PasswordSalt = passwordSalt;
                _userService.Update(data);
                return new SuccessResult(Messages.PasswordChanged);
            }
            return new ErrorResult(user.Message);
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(ChangePasswordValidator))]
        public IResult ChangePassword(ChangePasswordModel changePassword)
        {
            var checkedUser = new UserForLoginDto
            {
                Email = changePassword.Email,
                Password = changePassword.OldPassword
            };

            var loginResult = Login(checkedUser);

            if (loginResult.Success)
            {
                var user = loginResult.Data;
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(changePassword.NewPassword, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                _userService.Update(user);
                return new SuccessResult(Messages.PasswordChanged);
            }
            return new ErrorResult(loginResult.Message);
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, user.FirstName);
        }

        [ValidationAspect(typeof(UserForLoginDtoValidator))]
        [LogAspect(typeof(FileLogger))]
        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetUserByMail(userForLoginDto.Email);
            if (userToCheck.Data == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
        }

        [ValidationAspect(typeof(UserForRegisterDtoValidator))]
        [LogAspect(typeof(FileLogger))]
        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string passowrd)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(passowrd, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Gender = userForRegisterDto.Gender,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                Status = true
            };
            _userService.Add(user);
            var registeredUser = _userDal.Get(x => x.Email == user.Email);
            var rulesResult = BusinessRules.Run(CheckIfEmailExist(registeredUser.Email), CheckIfUserClaimExist(registeredUser), CheckIfUserImageExist(registeredUser.Id));
            if (rulesResult != null)
            {
                return new SuccessDataResult<User>(rulesResult.Message);
            }

            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IResult UserExist(string email)
        {
            var check = _userService.GetUserByMail(email);
            if (check.Data != null)
            {
                return new ErrorResult(Messages.UserExist);
            }
            return new SuccessResult();
        }

        //Business Rules


        private IResult CheckIfEmailExist(string userEmail)
        {
            var result = BaseCheckIfEmailExist(userEmail);
            if (result)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.UserNotFound);
        }

        private IResult CheckIfUserClaimExist(User user)
        {
            var result = _userDal.GetClaims(user);
            if (result.Count() == 0)
            {
                var userOperationClaim = new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = 2
                };
                _userOperationClaimDal.Add(userOperationClaim);
                return new SuccessResult();
            }
            return new ErrorResult(Messages.UserExist);
        }

        private IResult CheckIfUserImageExist(int userId)
        {
            var getUserImage = _userImageDal.Get(x => x.UserId == userId);
            if (getUserImage == null)
            {
                UserImage userImage = new UserImage
                {
                    ImagePath = "images/default.jpg",
                    UserId = userId,
                    Date = DateTime.Now
                };
                _userImageDal.Add(userImage);
                return new SuccessResult();
            }
            return new ErrorResult(Messages.UserImageIdExist);
        }

        private bool BaseCheckIfEmailExist(string userEmail)
        {
            return _userDal.GetAll(x => x.Email == userEmail).Any();
        }
    }
}
