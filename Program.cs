using System;
using System.Collections.Generic;
using System.Linq;
using PromotionEngineCheckout.Controller;
using PromotionEngineCheckout.Model;

namespace PromotionEngineCheckout {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("Promotion Test started:");

            IEnumerable<Price> priceList = new List<Price> {
                    new Price { sku_id = 'A', unit_price = 50 },
                    new Price { sku_id = 'B', unit_price = 30 },
                    new Price { sku_id = 'C', unit_price = 20 },
                    new Price { sku_id = 'D', unit_price = 15 }
                };

            IEnumerable<Promotion> promotions = new List<Promotion> {
                new Promotion { order_items = new List<Item> { new Item { sku_id = 'A', quantity = 3 }}, total_amount = 130 }, // 3 of A for 130
                new Promotion { order_items = new List<Item> { new Item { sku_id = 'B', quantity = 2 }}, total_amount = 45 }, // 2 of B for 45
                new Promotion { order_items = new List<Item> { new Item { sku_id = 'C', quantity = 1 }, new Item { sku_id = 'D', quantity = 1 }}, total_amount = 30 } }; // C + D for 30

            Console.WriteLine("per unit price:" + string.Join(", ", priceList.Select(o => o.sku_id + " => " + o.unit_price).ToArray()));
            Console.WriteLine("promotion running:" + string.Join(", ", promotions.Select(o => string.Join(", ", o.order_items.Select( xx=> xx.quantity + " " + xx.sku_id).ToArray()) + " for => " + o.total_amount).ToArray()));

            EngineController engine = new EngineController(priceList, promotions);

            scenario_1(engine, promotions, priceList);
            scenario_2(engine, promotions, priceList);
            scenario_3(engine, promotions, priceList);

            Console.WriteLine("Promotion Test ended:");
            Console.ReadLine();
        }
        public static void scenario_1(EngineController engine, IEnumerable<Promotion> promotions, IEnumerable<Price> priceList) {
            Console.WriteLine("scenario_1");
            var order = new Order {
                order_items = new List<Item> {
                    new Item { sku_id = 'A', quantity = 1 },
                    new Item { sku_id = 'B', quantity = 1 },
                    new Item { sku_id = 'C', quantity = 1 }}
            };

            Console.WriteLine("ordered items:" + string.Join(", ", order.order_items.Select(o => o.sku_id + " => " + o.quantity).ToArray()));

            engine.CalculatePrice(order);
            Console.WriteLine("calculated: " + order.total_amount);
            if(order.total_amount == 100)
                Console.WriteLine("promotion success");
            else
                Console.WriteLine("promotion failed");           
        }
        public static void scenario_2(EngineController engine, IEnumerable<Promotion> promotions, IEnumerable<Price> priceList) {
            Console.WriteLine("scenario_2");
            var order =
              new Order {
                  order_items = new List<Item>
                {
            new Item { sku_id = 'A', quantity = 5 },
            new Item { sku_id = 'B', quantity = 5 },
            new Item { sku_id = 'C', quantity = 1 }}
              };

            Console.WriteLine("ordered items:" + string.Join(", ", order.order_items.Select(o => o.sku_id + " => " + o.quantity).ToArray()));

            engine.CalculatePrice(order);
            Console.WriteLine("calculated: " + order.total_amount);
            if (order.total_amount == 370)
                Console.WriteLine("promotion success");
            else
                Console.WriteLine("promotion failed");
        }
        public static void scenario_3(EngineController engine, IEnumerable<Promotion> promotions, IEnumerable<Price> priceList) {
            Console.WriteLine("scenario_3");
            var order =
              new Order {
                  order_items = new List<Item>
                {
            new Item { sku_id = 'A', quantity = 3 },
            new Item { sku_id = 'B', quantity = 5 },
            new Item { sku_id = 'C', quantity = 1 },
            new Item { sku_id = 'D', quantity = 1 }}
              };

            Console.WriteLine("ordered items:" + string.Join(", ", order.order_items.Select(o => o.sku_id + " => " + o.quantity).ToArray()));

            engine.CalculatePrice(order);
            Console.WriteLine("calculated: " + order.total_amount);
            if (order.total_amount == 280)
                Console.WriteLine("promotion success");
            else
                Console.WriteLine("promotion failed");
        }
    }
}
