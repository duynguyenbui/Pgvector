using System.ComponentModel.DataAnnotations;

namespace Database.PgVector.Model;

public class CatalogType
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; }
}
