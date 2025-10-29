using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMvc.Models
{
    public class Product
    {
        public int Id { get; set; }
        // Name initialized to an empty string to avoid null reference issues
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(100)]
        public string Brand { get; set; } = "";
        [MaxLength(100)]
        public string Category { get; set; } = "";
        [Precision(16,2)]
        public decimal Price { get; set; }

        public string Description { get; set; } = "";
        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
        public DateTime CreatedAt{ get; set; }
    }
}