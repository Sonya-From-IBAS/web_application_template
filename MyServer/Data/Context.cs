using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyServer.Models;

namespace MyServer.Data
{
    public class Context: IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
            
        }
    }
}
