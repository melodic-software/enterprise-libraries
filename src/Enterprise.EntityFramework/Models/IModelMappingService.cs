using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.Models;

public interface IModelMappingService
{
    public void ConfigureMapping(DbContext dbContext, ModelBuilder modelBuilder);
}