using System.ComponentModel.DataAnnotations;

namespace Database.PgVector.Model;

public class CatalogBrand
{
    public int Id { get; set; }

    [Required]
    public string Brand { get; set; }
}
