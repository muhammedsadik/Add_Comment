using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataAccess.Configurations
{
  internal class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.HasKey(m => m.ProductId);//product tablosunun birincil anahtarı
      builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
      builder.Property(m => m.DateAdded).HasDefaultValueSql("getdate()");//sql
      //builder.Property(m => m.DateAdded).HasDefaultValueSql("date('now')");//sqlite

    }
  }
}
