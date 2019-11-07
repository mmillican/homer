using Homer.Shared.Entities.Shopping;
using Homer.Shared.Entities.TaskLists;
using Homer.Shared.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Homer.Shared.Data
{
    public class HomerDbContext : DbContext, IDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        public HomerDbContext(DbContextOptions<HomerDbContext> options)
            : base(options)
        {
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, new() //BaseEntity
        {
            return base.Set<TEntity>();
        }

        // Perhaps we'll eventually bring these mappings back...

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes()
        //         .Where(x => (x.BaseType?.IsGenericType ?? false)
        //             && (x.BaseType.GetGenericTypeDefinition() == typeof(FamEntityTypeConfiguration<>)
        //                 || x.BaseType.GetGenericTypeDefinition() == typeof(FamQueryTypeConfiguration<>)));

        //     foreach(var typeConfig in typeConfigurations)
        //     {
        //         var config = (IMappingConfiguration)Activator.CreateInstance(typeConfig);
        //         config.ApplyConfiguration(builder);
        //     }

        //     base.OnModelCreating(builder);
        // }
    }
}
