using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;

namespace Repositories.Models
{
    /// <summary>
    /// A simple model of a Burger.  Good as anything to prove this point
    /// </summary>
    public class BurgerModel
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [Required]
        public string BurgerName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }
    }
}