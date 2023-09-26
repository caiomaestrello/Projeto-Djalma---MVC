using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Djalma___MVC.Models.Interface;

namespace Projeto_Djalma___MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        
    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Cadastros> Cadastros { get; set; }



        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            PreencheIStatusModificacao();
            return base.SaveChanges();
        }

        private void ModelStatusModificacao<TEntity>(EntityTypeBuilder<TEntity> entity) where TEntity : class, IStatusModificacao
        {
            entity.HasQueryFilter(x => !x.Excluido);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            PreencheIStatusModificacao();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void PreencheIStatusModificacao()
        {

            foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity != null
                        && typeof(IStatusModificacao).IsAssignableFrom(e.Entity.GetType())))
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("Excluido").CurrentValue = true;
                }
            }
        }
    }

    public class Produtos
    {
    }

    public class Cadastros
    {
    }
}

