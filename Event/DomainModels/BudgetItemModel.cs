using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EventManagerPro.Model.DomainModels
{
    class BudgetItemModel
    {
        public static BudgetItem create(String name, int cost, int budgetId)
        {
            using (var context = new EventContainer())
            {
                BudgetItem item = new BudgetItem
                {
                    Name = name,
                    Cost = cost,
                    BudgetId = budgetId
                };
                context.BudgetItems.Add(item);
                context.SaveChanges();
                return item;
            }
        }

        public static BudgetItem update(String name, int cost, int budgetId)
        {
            using (var context = new EventContainer())
            {
                BudgetItem item = new BudgetItem
                {
                    Name = name,
                    Cost = cost,
                    BudgetId = budgetId
                };
                context.BudgetItems.Attach(item);
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
                return item;
            }
        }

        public static BudgetItem createObj(BudgetItem e)
        {
            using (var context = new EventContainer())
            {
                context.BudgetItems.Add(e);
                context.SaveChanges();
                return e;
            }
        }

        public static BudgetItem updateObj(BudgetItem e)
        {
            using (var context = new EventContainer())
            {
                context.Entry(e).State = EntityState.Modified;
                context.SaveChanges();

                return e;
            }
        }

        public static void deleteById(int id)
        {
            using (var context = new EventContainer())
            {
                BudgetItem e = getByID(id);
                context.BudgetItems.Attach(e);
                context.BudgetItems.Remove(e);
                context.SaveChanges();
            }
        }

        public static BudgetItem getByID(int id)
        {
            using (var context = new EventContainer())
            {
                var items = from s in context.BudgetItems
                              where s.Id == id
                              select s;
                return items.FirstOrDefault();
            }
        }

        public static List<BudgetItem> getAll()
        {
            using (var context = new EventContainer())
            {
                IEnumerable<BudgetItem> budgets = from s in context.BudgetItems
                                              select s;
                return budgets.ToList();
            }
        }
    }
}
