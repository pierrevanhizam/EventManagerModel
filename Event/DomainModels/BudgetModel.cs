using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EventManagerPro.Model.DomainModels
{
    class BudgetModel
    {
        public static Budget create(int allocatedBudget)
        {
            using (var context = new EventContainer())
            {
                Budget budget = new Budget
                {
                    AllocatedBudget = allocatedBudget
                };
                context.Budgets.Add(budget);
                context.SaveChanges();
                return budget;
            }
        }

        public static Budget update(int allocatedBudget)
        {
            using (var context = new EventContainer())
            {
                Budget budget = new Budget
                {
                    AllocatedBudget = allocatedBudget
                };
                context.Budgets.Attach(budget);
                context.Entry(budget).State = EntityState.Modified;
                context.SaveChanges();
                return budget;
            }
        }

        public static Budget createObj(Budget e)
        {
            using (var context = new EventContainer())
            {
                context.Budgets.Add(e);
                context.SaveChanges();
                return e;
            }
        }

        public static Budget updateObj(Budget e)
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
                Budget e = getByID(id);
                context.Budgets.Attach(e);
                context.Budgets.Remove(e);
                context.SaveChanges();
            }
        }

        public static Budget getByID(int id)
        {
            using (var context = new EventContainer())
            {
                var budgets = from s in context.Budgets.Include("BudgetItem")
                                where s.Id == id
                                select s;
                return budgets.FirstOrDefault();
            }
        }

        public static List<Budget> getAll()
        {
            using (var context = new EventContainer())
            {
                IEnumerable<Budget> budgets = from s in context.Budgets.Include("BudgetItem")
                                                  select s;
                return budgets.ToList();
            }
        }

        
    }
}
