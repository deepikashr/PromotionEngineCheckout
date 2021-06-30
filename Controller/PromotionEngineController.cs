using PromotionEngineCheckout.Model;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngineCheckout.Controller {
    public class EngineController {
        private IEnumerable<Price> PriceList;
        private IEnumerable<Promotion> Promotions;

        public EngineController(IEnumerable<Price> priceList, IEnumerable<Promotion> promotions) {
            this.PriceList = priceList;
            this.Promotions = promotions;
        }

        public void CalculatePrice(Order order) {
            var foundItems = new List<Item>();
            if (Promotions != null && Promotions.Count() > 0)
                foreach (var promotion in Promotions) {
                    var validatedItems = promotion.Validate(order, foundItems);
                    UpdateValidatedItems(foundItems, validatedItems);
                }

            ApplyUnitPrice(order, foundItems);
        }

        private void ApplyUnitPrice(Order order, List<Item> foundItems) {
            foreach (var item in order.order_items) {
                var validateItem = foundItems.FirstOrDefault(x => x.sku_id == item.sku_id) ?? item;
                var quantity = validateItem.quantity;
                if (quantity > 0)
                    order.total_amount += quantity * PriceList.First(x => x.sku_id == item.sku_id).unit_price;
            }
        }

        private static void UpdateValidatedItems(List<Item> foundItems, IEnumerable<Item> validatedItems) {
            if (validatedItems == null || validatedItems.Count() < 1)
                return;

            foreach (var item in validatedItems)
                if (!foundItems.Any(x => x.sku_id == item.sku_id))
                    foundItems.Add(item);
        }
    }
}
