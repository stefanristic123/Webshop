using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Data.Repository
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int SourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        // Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);

        Task<PagedList <LikeDto>> GetUserLikes(LikeParams likeParams);

    }
}