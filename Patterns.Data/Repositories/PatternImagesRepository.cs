using System.Data.Entity;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Data.Repositories
{
    public class PatternImagesRepository: BaseRepository<PatternImage>, IPatternImagesRepository
    {
        protected override DbSet<PatternImage> Collection
        {
            get { return Context.PatternImages; }
        }
    }
}
