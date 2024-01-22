using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Microsoft.AspNetCore.Http;
using IResult = Core.Utilities.Result.Abstract.IResult;

namespace Business.Abstract
{
    public interface IUserImageService
    {
        IDataResult<List<UserImage>> GetAll();
        IDataResult<List<UserImage>> GetUserImages(int userId);
        IDataResult<UserImage> GetById(int imageId);
        IResult Add(IFormFile file, int userId);
        IResult Update(UserImage userImage, IFormFile file);
        IResult Delete(UserImage userImage);
        IResult DeleteAllImagesOfUserByUserId(int userId);
    }
}
