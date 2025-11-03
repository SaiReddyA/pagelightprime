using PageLightPrime.API.Models.Entities;

namespace PageLightPrime.API.Data;

public partial class PageLightPrimeContext : DbContext
{
    public PageLightPrimeContext()
    {
    }

    public PageLightPrimeContext(DbContextOptions<PageLightPrimeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<LocationMapping> LocationMappings { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<CountryDto> CountryDtos { get; set; } = null!;
    public virtual DbSet<StateDto> StateDtos { get; set; } = null!;
    public virtual DbSet<DistrictDto> DistrictDtos { get; set; } = null!;
    public virtual DbSet<LocationMappingDto> LocationMappingDtos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Country__10D1609F47EEC16E");

            entity.ToTable("Country");

            entity.HasIndex(e => e.CountryName, "UQ__Country__E056F2017C083225").IsUnique();

            entity.Property(e => e.CountryName).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PK__District__85FDA4C6F89DC478");

            entity.ToTable("District");

            entity.HasIndex(e => new { e.StateId, e.DistrictName }, "UQ_District_StateId_DistrictName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DistrictName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.State).WithMany(p => p.Districts)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_District_State");
        });

        modelBuilder.Entity<LocationMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId).HasName("PK__Location__8B57819DA2172FAA");

            entity.ToTable("LocationMapping");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.HasOne(d => d.Country).WithMany(p => p.LocationMappings)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LocationMapping_Country");

            entity.HasOne(d => d.District).WithMany(p => p.LocationMappings)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LocationMapping_District");

            entity.HasOne(d => d.State).WithMany(p => p.LocationMappings)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LocationMapping_State");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__State__C3BA3B3A17735A98");

            entity.ToTable("State");

            entity.HasIndex(e => new { e.CountryId, e.StateName }, "UQ_State_CountryId_StateName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.StateName).HasMaxLength(200);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_State_Country");
        });

        modelBuilder.Entity<CountryDto>().HasNoKey();
        modelBuilder.Entity<StateDto>().HasNoKey();
        modelBuilder.Entity<DistrictDto>().HasNoKey();
        modelBuilder.Entity<LocationMappingDto>().HasNoKey();


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
