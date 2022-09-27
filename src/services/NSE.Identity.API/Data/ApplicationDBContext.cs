using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtSigningCredentials;
using NetDevPack.Security.JwtSigningCredentials.Store.EntityFrameworkCore;
using NSE.Identity.API.Models;

namespace NSE.Identity.API.Data
{
    public class ApplicationDBContext : IdentityDbContext, ISecurityKeyContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        //Cria uma tabela no Banco chamada SecurityKeys
        public DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}