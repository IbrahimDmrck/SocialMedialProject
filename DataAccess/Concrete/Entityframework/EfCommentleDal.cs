﻿using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.Entityframework
{
    public class EfCommentleDal : EfEntityRepositoryBase<Comment, SocialMediaContext>, ICommentDal
    {
    }
}
