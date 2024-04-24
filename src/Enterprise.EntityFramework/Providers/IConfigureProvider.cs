using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.Providers;

public interface IConfigureProvider
{
    public void Configure(DbContextOptionsBuilder optionsBuilder);
}