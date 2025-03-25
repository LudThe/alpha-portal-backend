using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<ClientInformationEntity> ClientInformation { get; set; }
    public virtual DbSet<ClientAddressEntity> ClientAddresses { get; set; }
}
