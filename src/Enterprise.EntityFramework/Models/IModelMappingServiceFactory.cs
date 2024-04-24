using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Enterprise.EntityFramework.Models;

public interface IModelMappingServiceFactory
{
    IModelMappingService? Create(DatabaseFacade database);
}