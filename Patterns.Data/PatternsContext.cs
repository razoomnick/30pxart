using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using Patterns.Objects.Entities;

namespace Patterns.Data
{
    public class PatternsContext : DbContext
    {

        public PatternsContext() : base("Patterns")
        {
        }

        public DbSet<PatternImage> PatternImages { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pattern>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Pattern>().Property(p => p.CreationTime).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            modelBuilder.Entity<Pattern>().HasRequired(p => p.PatternImage).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Pattern>().HasRequired(p => p.OriginalImage).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Like>().HasRequired(l => l.Pattern).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Like>().HasRequired(l => l.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasRequired(c => c.Pattern).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasRequired(c => c.Author).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Following>().HasRequired(c => c.Publisher).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Following>().HasRequired(c => c.Subscriber).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Token>().HasRequired(c => c.User);
        }
    }
}