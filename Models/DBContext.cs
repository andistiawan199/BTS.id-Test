using Microsoft.EntityFrameworkCore;
namespace Web_API.Models
{
    public class DBContext: DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        public DbSet<UserModel> User { get; set; }
        public DbSet<ChecklistModel> Checklist { get; set; }
        public DbSet<ChecklistItemModel> ChecklistItem { get; set; }
    }
}
