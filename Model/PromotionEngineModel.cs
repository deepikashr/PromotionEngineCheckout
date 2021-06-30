using System.Collections.Generic;
using System.Linq;

namespace PromotionEngineCheckout.Model {
    class PromotionEngineModel {
    }
    public class Item {
        public Item() { }
        public Item(Item item) {
            sku_id = this.sku_id;
            quantity = this.quantity;
        }
        public char sku_id { get; set; }
        public int quantity { get; set; }
    }
    public class Price {
        public char sku_id { get; set; }
        public int unit_price { get; set; }
    }
    public class Order {
        public List<Item> order_items { get; set; }
        public double total_amount { get; set; }
    }
    public class Promotion : Order {
        public IEnumerable<Item> Validate(Order order, IEnumerable<Item> validatedItems) {
            var foundItems = new List<Item>();
            if (this.order_items == null || this.order_items.Count < 1)
                return foundItems; //not in order

            foreach (var promotionItem in order_items) {
                var foundItem = validatedItems.FirstOrDefault(x => x.sku_id == promotionItem.sku_id) ??
                  order.order_items.FirstOrDefault(x => x.sku_id == promotionItem.sku_id);
                if (foundItem == null || foundItem.quantity < promotionItem.quantity)
                    return null;

                if (!foundItems.Any(x => x.sku_id == foundItem.sku_id))
                    foundItems.Add(foundItem);
            }

            ApplyPromotionPriceAndCalculateRestQuantity(order, foundItems);

            return foundItems;
        }

        private void ApplyPromotionPriceAndCalculateRestQuantity(Order order, List<Item> foundItems) {
            var found = foundItems.Count() > 0;
            if (found) {
                do {
                    order.total_amount += total_amount;
                    foreach (var promotionItem in order_items) {
                        var item = foundItems.FirstOrDefault(x => x.sku_id == promotionItem.sku_id);
                        if (item != null) {
                            item.quantity -= promotionItem.quantity;
                            if (found)
                                found = item.quantity >= promotionItem.quantity;
                        }
                    }
                } while (found);
            }
        }
    }
}
