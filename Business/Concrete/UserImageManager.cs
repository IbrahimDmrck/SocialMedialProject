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
using DataAccess.Concrete.Entityframework;
using Microsoft.AspNetCore.Http;

using IResult = Core.Utilities.Result.Abstract.IResult;

namespace Business.Concrete
{
    public class UserImageManager : IUserImageService
    {
        private readonly IUserImageDal _userImageDal;

        public UserImageManager(IUserImageDal userImageDal)
        {
            _userImageDal = userImageDal;
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserImageService.Get")]
        public IResult Add(IFormFile file, int userId)
        {
            IResult rulesResult = BusinessRules.Run(CheckIfUserImageLimitExceeded(userId));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            var imageResult = FileHelper.Upload(file);
            if (!imageResult.Success)
            {
                return new ErrorResult(imageResult.Message);
            }

            UserImage userImage = new UserImage
            {
                ImagePath = imageResult.Message,
                UserId = userId,
                Date = DateTime.Now
            };
            _userImageDal.Add(userImage);
            return new SuccessResult(Messages.UserImageAdded);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserImageService.Get")]
        public IResult Delete(UserImage userImage)
        {
            IResult rulesResult = BusinessRules.Run(CheckIfUserImageIdExist(userImage.Id));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            var deletedImage = _userImageDal.Get(c => c.Id == userImage.Id);
            var result = FileHelper.Delete(deletedImage.ImagePath);
            if (!result.Success)
            {
                return new ErrorResult(Messages.ErrorDeletingImage);
            }
            _userImageDal.Delete(deletedImage);
            return new SuccessResult(Messages.UserImageDeleted);
        }
        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserImageService.Get")]
        public IResult DeleteAllImagesOfUserByUserId(int userId)
        {
            var deletedImages = _userImageDal.GetAll(c => c.UserId == userId);
            if (deletedImages == null)
            {
                return new ErrorResult(Messages.NoPictureOfTheUser);
            }
            foreach (var deletedImage in deletedImages)
            {
                _userImageDal.Delete(deletedImage);
                FileHelper.Delete(deletedImage.ImagePath);
            }
            return new SuccessResult(Messages.UserImageDeleted);
        }
        [CacheAspect(2)]
        public IDataResult<List<UserImage>> GetAll()
        {
           return new SuccessDataResult<List<UserImage>>(_userImageDal.GetAll(), Messages.UserImagesListed);
        }

        public IDataResult<UserImage> GetById(int imageId)
        {
            return new SuccessDataResult<UserImage>(_userImageDal.Get(c => c.Id == imageId), Messages.UserImageListed);
        }

        public IDataResult<List<UserImage>> GetUserImages(int userId)
        {
            var checkIfUserImage = CheckIfUserHasImage(userId);
            var images = checkIfUserImage.Success
                ? checkIfUserImage.Data
                : _userImageDal.GetAll(c => c.UserId == userId);
            return new SuccessDataResult<List<UserImage>>(images, checkIfUserImage.Message);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IUserImageService.Get")]
        public IResult Update(UserImage userImage, IFormFile file)
        {
            IResult rulesResult = BusinessRules.Run(CheckIfUserImageIdExist(userImage.Id),
                CheckIfUserImageLimitExceeded(userImage.UserId));
            if (rulesResult != null)
            {
                return rulesResult;
            }

            var updatedImage = _userImageDal.Get(c => c.Id == userImage.Id);
            var result = FileHelper.Update(file, updatedImage.ImagePath);
            if (!result.Success)
            {
                return new ErrorResult(Messages.ErrorUpdatingImage);
            }
            userImage.UserId = userImage.UserId;
            userImage.ImagePath = result.Message;
            userImage.Date = DateTime.Now;
            _userImageDal.Update(userImage);
            return new SuccessResult(Messages.UserImageUpdated);
        }

        //Business Rules

        private IResult CheckIfUserImageLimitExceeded(int userId)
        {
            int result = _userImageDal.GetAll(c => c.UserId == userId).Count;
            if (result >= 5)
            {
                return new ErrorResult(Messages.UserImageLimitExceeded);
            }
            return new SuccessResult();
        }

        private IDataResult<List<UserImage>> CheckIfUserHasImage(int userId)
        {
            string logoPath = "/images/default.jpg";
            bool result = _userImageDal.GetAll(c => c.UserId == userId).Any();
            if (!result)
            {
                List<UserImage> imageList = new List<UserImage>
                {
                    new UserImage
                    {
                        ImagePath = logoPath,
                        UserId = userId,
                        Date = DateTime.Now
                    }
                };
                return new SuccessDataResult<List<UserImage>>(imageList, Messages.GetDefaultImage);
            }
            return new ErrorDataResult<List<UserImage>>(new List<UserImage>(), Messages.UserImagesListed);
        }

        private IResult CheckIfUserImageIdExist(int imageId)
        {
            var result = _userImageDal.GetAll(c => c.Id == imageId).Any();
            if (!result)
            {
                return new ErrorResult(Messages.UserImageIdNotExist);
            }
            return new SuccessResult();
        }
    }
}
