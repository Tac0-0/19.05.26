using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Ingredients
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public IngredientUnit Unit { get; set; }
        public int SupplierId { get; set; }
        public Suppliers Supplier { get; set;
    }
}
