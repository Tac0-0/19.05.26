using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ProductIngredients
    {
        public int ProductIngredientId { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public int IngredientId { get; set; }
        public Ingredients Ingredient { get; set; }
        public decimal RequiredQuantity { get; set; }
    }
}
