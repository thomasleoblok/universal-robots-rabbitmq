using MessageModel;
using Microsoft.EntityFrameworkCore;

namespace Api_universal_robots_rmq.DAL
{
    public class robotcontext : DbContext
    {
        public robotcontext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Message> messages { get; set; }
       
    }
}
