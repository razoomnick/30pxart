using System;
using System.Linq;
using Patterns.Objects.Aggregated;
using Patterns.Objects.Entities;

namespace Patterns.Data.Linq
{
    public static class LinqExtensions
    {
        public static IQueryable<ApiUser> SelectApiUser(this IQueryable<User> users)
        {
            return users.Select(u =>
                new ApiUser()
                {
                    Id = u.Id,
                    Name = u.Name,
                    AvatarImageId = u.Avatar.Id,
                    AvatarCdnUrl = u.Avatar.CdnUrl,
                    RegisteredTime = u.RegistrationTime
                });
        }

        public static IQueryable<ApiPattern> SelectApiPattern(this IQueryable<Pattern> patterns, PatternsContext context, Guid userId, int maxCommentsCount)
        {
            return patterns
                .Select(p =>
                new ApiPattern()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author != null ? new ApiUser()
                    {
                        Id = p.Author.Id,
                        Name = p.Author.Name,
                        AvatarImageId = p.Author.Avatar.Id,
                        AvatarCdnUrl = p.Author.Avatar.CdnUrl,
                        RegisteredTime = p.Author.RegistrationTime
                    } : null,
                    ImageId = p.PatternImage.Id,
                    IsDraft = p.IsDraft,
                    FilterName = p.FilterName,
                    ImageCdnUrl = p.PatternImage.CdnUrl,
                    OriginalImageId = p.OriginalImage.Id,
                    LikesCount = context.Likes.Count(l => l.Pattern.Id == p.Id),
                    CommentsCount = context.Comments.Count(c => c.Pattern.Id == p.Id),
                    Liked = context.Likes.Any(l => l.Pattern.Id == p.Id && l.User.Id == userId),
                    Comments =
                        context.Comments.Where(c => c.Pattern.Id == p.Id)
                            .OrderByDescending(c => c.CreationTime)
                            .Take(maxCommentsCount)
                            .OrderBy(c => c.CreationTime)
                            .Select(c =>
                                new ApiComment()
                                {
                                    Author = c.Author != null ? new ApiUser()
                                    {
                                        Id = c.Author.Id,
                                        Name = c.Author.Name,
                                        AvatarImageId = c.Author.Avatar.Id,
                                        AvatarCdnUrl = c.Author.Avatar.CdnUrl,
                                        RegisteredTime = c.Author.RegistrationTime
                                    } : null,
                                    PatternId = c.Pattern.Id,
                                    Text = c.Text,
                                    CreationTime = c.CreationTime
                                }),
                    CreationTime = p.CreationTime
                });
        }

        public static IQueryable<ApiLike> SelectApiLike(this IQueryable<Like> likes)
        {
            return likes.Select(l =>
                new ApiLike()
                {
                    User = new ApiUser()
                    {
                        Id = l.User.Id,
                        Name = l.User.Name,
                        AvatarImageId = l.User.Avatar.Id,
                        AvatarCdnUrl = l.User.Avatar.CdnUrl,
                        RegisteredTime = l.User.RegistrationTime
                    },
                    CreationTime = l.CreationTime
                });
        }

        public static IQueryable<ApiFollowing> SelectApiFollowing(this IQueryable<Following> followings)
        {
            return followings.Select(f =>
                new ApiFollowing()
                {
                    Publisher = new ApiUser()
                    {
                        Id = f.Publisher.Id,
                        Name = f.Publisher.Name,
                        AvatarImageId = f.Publisher.Avatar.Id,
                        AvatarCdnUrl = f.Publisher.Avatar.CdnUrl,
                        RegisteredTime = f.Publisher.RegistrationTime
                    },
                    Subscriber = new ApiUser()
                    {
                        Id = f.Subscriber.Id,
                        Name = f.Subscriber.Name,
                        AvatarImageId = f.Subscriber.Avatar.Id,
                        AvatarCdnUrl = f.Subscriber.Avatar.CdnUrl,
                        RegisteredTime = f.Subscriber.RegistrationTime
                    },
                    CreationTime = f.CreationTime
                });
        }

        public static IQueryable<ApiComment> SelectApiComment(this IQueryable<Comment> comments)
        {
            return comments.Select(c =>
                new ApiComment()
                {
                    Id = c.Id,
                    Author = new ApiUser()
                    {
                        Id = c.Author.Id,
                        Name = c.Author.Name,
                        AvatarImageId = c.Author.Avatar.Id,
                        AvatarCdnUrl = c.Author.Avatar.CdnUrl,
                        RegisteredTime = c.Author.RegistrationTime
                    },
                    PatternId = c.Pattern.Id,
                    Text = c.Text,
                    CreationTime = c.CreationTime
                });
        }
    }
}
