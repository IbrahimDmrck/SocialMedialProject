using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Entityframework
{
    public class EfArticleDal : EfEntityRepositoryBase<Article, SocialMediaContext>, IArticleDal
    {
        public List<ArticleDetailDto> GetArticleDetails(Expression<Func<ArticleDetailDto, bool>> filter=null)
        {
            using (var context = new SocialMediaContext())
            {
                var result = from A in context.Articles
                             join T in context.Topics on A.TopicId equals T.Id
                             join U in context.Users on A.UserId equals U.Id
                             select new ArticleDetailDto
                             {
                                 Id = A.Id,
                                 TopicId = A.TopicId,
                                 TopicTitle = T.TopicTitle,
                                 UserId = A.UserId,
                                 Content=A.Content,
                                 UserName = U.FirstName + " " + U.LastName,
                                 SharingDate = A.SharingDate.ToShortDateString(),
                                 UserImage = ((from I in context.UserImages join user in context.Users on I.UserId equals user.Id
                                               where(A.UserId == I.UserId)
                                               select I.ImagePath)).First(),
                                 CommentDetails = ((from C in context.Comments
                                                    join User in context.Users on C.UserId equals User.Id
                                                    join uimg in context.UserImages on C.UserId equals uimg.UserId
                                                    where (A.Id == C.ArticleId)
                                                    select new CommentDetail
                                                    {
                                                        Id = C.Id,
                                                        ArticleId = C.ArticleId,
                                                        CommentText = C.CommentText,
                                                        UserId = C.UserId,
                                                        UserName = User.FirstName + " " + User.LastName,
                                                        Image= uimg.ImagePath,
                                                        CommentDate = C.CommentDate,
                                                        Status = C.Status
                                                    }).ToList()).Count == 0 ? new List<CommentDetail> { new CommentDetail { Id = -1, ArticleId = -1, CommentText = "Henüz yorum yapılmadı", CommentDate = DateTime.Now, UserId = -1, UserName = "", Image= "images/default.jpg" } }
                                            : (from C in context.Comments
                                               join User in context.Users on C.UserId equals User.Id
                                               join uimg in context.UserImages on C.UserId equals uimg.UserId
                                               where (A.Id == C.ArticleId)
                                               select new CommentDetail
                                               {
                                                   Id = C.Id,
                                                   ArticleId = C.ArticleId,
                                                   UserName = User.FirstName + " " + User.LastName,
                                                   Image = uimg.ImagePath,
                                                   CommentText = C.CommentText,
                                                   UserId = C.UserId,
                                                   CommentDate = C.CommentDate,
                                                   Status = C.Status
                                               }).ToList()
                             };

                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }

        public ArticleDetailDto GetArticleDetailsById(Expression<Func<ArticleDetailDto, bool>> filter)
        {
            using (var context = new SocialMediaContext())
            {
                var result = from A in context.Articles
                             join T in context.Topics on A.TopicId equals T.Id
                             join U in context.Users on A.UserId equals U.Id
                             select new ArticleDetailDto
                             {
                                 Id = A.Id,
                                 TopicId = A.TopicId,
                                 TopicTitle = T.TopicTitle,
                                 UserId = A.UserId,
                                 Content = A.Content,
                                 UserName = U.FirstName + " " + U.LastName,
                                 SharingDate = A.SharingDate.ToShortDateString(),
                                 UserImage = ((from I in context.UserImages
                                               join user in context.Users on I.UserId equals user.Id
                                               where (A.UserId == I.UserId)
                                               select I.ImagePath)).First(),
                                 CommentDetails = ((from C in context.Comments
                                                    join User in context.Users on C.UserId equals User.Id
                                                    join uimg in context.UserImages on C.UserId equals uimg.UserId
                                                    where (A.Id == C.ArticleId)
                                                    select new CommentDetail
                                                    {
                                                        Id = C.Id,
                                                        ArticleId = C.ArticleId,
                                                        CommentText = C.CommentText,
                                                        UserId = C.UserId,
                                                        UserName = User.FirstName + " " + User.LastName,
                                                        Image = uimg.ImagePath,
                                                        CommentDate = C.CommentDate,
                                                        Status = C.Status
                                                    }).ToList()).Count == 0 ? new List<CommentDetail> { new CommentDetail { Id = -1, ArticleId = -1, CommentText = "Henüz yorum yapılmadı", CommentDate = DateTime.Now, UserId = -1, UserName = "", Image = "images/default.jpg" } }
                                            : (from C in context.Comments
                                               join User in context.Users on C.UserId equals User.Id
                                               join uimg in context.UserImages on C.UserId equals uimg.UserId
                                               where (A.Id == C.ArticleId)
                                               select new CommentDetail
                                               {
                                                   Id = C.Id,
                                                   ArticleId = C.ArticleId,
                                                   UserName = User.FirstName + " " + User.LastName,
                                                   Image = uimg.ImagePath,
                                                   CommentText = C.CommentText,
                                                   UserId = C.UserId,
                                                   CommentDate = C.CommentDate,
                                                   Status = C.Status
                                               }).ToList()
                             };

                return result.Where(filter).FirstOrDefault();
            }
        }
    }
}
