using System;
using System.Data.Entity;
using System.Linq;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Data.Repositories
{
    public class TokensRepository: BaseRepository<Token>, ITokensRepository
    {
        public Token Get(Guid id)
        {
            var token = Context.Tokens.FirstOrDefault(t => t.Id == id);
            return token;
        }

        public void SaveToken(Token token)
        {
            Context.Tokens.Add(token);
            Context.SaveChanges();
        }

        protected override DbSet<Token> Collection
        {
            get { return Context.Tokens; }
        }
    }
}
