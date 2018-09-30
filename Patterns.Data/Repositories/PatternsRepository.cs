using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Patterns.Data.Linq;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Data.Repositories
{
    public class PatternsRepository: BaseRepository<Pattern>, IPatternsRepository
    {
        public IList<ApiPattern> GetTopApiPatterns(Guid currentUserId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Patterns
                .Where(p => !p.IsDraft && p.Author != null)
                .SelectApiPattern(Context, currentUserId,3)
                .OrderByDescending(ap => ap.LikesCount)
                .ThenByDescending(ap => ap.CreationTime)
                .Skip(skip)
                .Take(count)
                .ToList();
            return apiPatterns;
        }

        public IList<ApiPattern> GetTopUnregisteredApiPatterns(Guid currentUserId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Patterns
                .Where(p => !p.IsDraft && p.Author == null)
                .SelectApiPattern(Context, currentUserId,3)
                .OrderByDescending(ap => ap.LikesCount)
                .ThenByDescending(ap => ap.CreationTime)
                .Skip(skip)
                .Take(count)
                .ToList();
            return apiPatterns;
        }

        public IList<ApiPattern> GetMostCommentedApiPatterns(Guid currentUserId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Patterns
                .Where(p => !p.IsDraft && p.Author != null)
                .SelectApiPattern(Context, currentUserId, 3)
                .OrderByDescending(ap => ap.CommentsCount)
                .ThenByDescending(ap => ap.CreationTime)
                .Skip(skip)
                .Take(count)
                .ToList();
            return apiPatterns;
        }

        public IList<ApiPattern> GetRecentUnregisteredApiPatterns(Guid currentUserId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Patterns
                .Where(p => !p.IsDraft && p.Author == null)
                .SelectApiPattern(Context, currentUserId, 3)
                .OrderByDescending(ap => ap.CreationTime)
                .Skip(skip)
                .Take(count)
                .ToList();
            return apiPatterns;
        }

        public IList<ApiPattern> GetUsersFeedPatterns(Guid userId, Guid currentUserId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Followings
                .Where(f => f.Subscriber.Id == userId)
                .SelectMany(f => Context.Patterns.Where(p => p.Author.Id == f.Publisher.Id && !p.IsDraft))
                .OrderByDescending(p => p.CreationTime)
                .Skip(skip)
                .Take(count)
                .SelectApiPattern(Context, currentUserId,3)
                .ToList();
            return apiPatterns;
        }

        public ApiPattern GetApiPattern(Guid id, Guid currentUserId)
        {
            var apiPattern = Context
                .Patterns
                .Where(p => p.Id == id)
                .SelectApiPattern(Context, currentUserId, MaxCount)
                .FirstOrDefault();
            return apiPattern;
        }

        public IList<ApiPattern> GetRecentApiPatterns(Guid currentUserId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Patterns
                .Where(p => !p.IsDraft && p.Author != null)
                .SelectApiPattern(Context, currentUserId,3)
                .OrderByDescending(ap => ap.CreationTime)
                .Skip(skip)
                .Take(count)
                .ToList();
            return apiPatterns;
        }

        public IList<ApiPattern> GetUsersDraftPatterns(Guid userId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Patterns
                .Where(p => p.IsDraft)
                .Where(p => p.Author.Id == userId)
                .OrderByDescending(p => p.CreationTime)
                .Skip(skip)
                .Take(count)
                .SelectApiPattern(Context, userId, 3)
                .ToList();
            return apiPatterns;
        }

        public IList<ApiPattern> GetUsersApiPatterns(Guid userId, Guid currentUserId, int skip, int count)
        {
            count = count <= MaxCount ? count : MaxCount;
            var apiPatterns = Context
                .Patterns
                .Where(p => !p.IsDraft)
                .Where(p => p.Author.Id == userId)
                .OrderByDescending(p => p.CreationTime)
                .Skip(skip)
                .Take(count)
                .SelectApiPattern(Context, currentUserId, 3)
                .ToList();
            return apiPatterns;
        }

        protected override DbSet<Pattern> Collection
        {
            get { return Context.Patterns; }
        }
    }
}
