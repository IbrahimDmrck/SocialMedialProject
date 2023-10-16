using Core.Business;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService: IBusinessRepository<User>
    {
        IDataResult<List<UserDto>> GetAllDto();
        IDataResult<UserDto> GetUserDtoById(int userId);
        IResult UpdateByDto(UserDto userDto);
        IDataResult<List<OperationClaim>> GetClaims(User user);
        IDataResult<User> GetUserByMail(string email);
        IDataResult<UserDto> GetUserDtoByMail(string email);
    }
}
