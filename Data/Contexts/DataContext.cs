﻿using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUserEntity>(options)
{
    public virtual DbSet<AppUserAddressEntity> AppUserAddresses { get; set; }
    public virtual DbSet<AppUserProfileEntity> AppUserProfiles { get; set; }
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<ClientInformationEntity> ClientInformation { get; set; }
    public virtual DbSet<ClientAddressEntity> ClientAddresses { get; set; }
    public virtual DbSet<ProjectEntity> Projects { get; set; }
    public virtual DbSet<ProjectStatusEntity> ProjectStatuses { get; set; }
}
