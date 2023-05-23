using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataAccess.Configurations
{
  internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {
      builder.HasKey(m => m.CategoryId);//product tablosunun birincil anahtarı
      builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
      
    }
  }
}
