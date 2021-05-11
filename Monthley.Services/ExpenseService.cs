using Monthley.Data;
using Monthley.Data.Entities;
using Monthley.Models.ExpenseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Services
{
    public class ExpenseService
    {
        private readonly Guid _userId;

        public ExpenseService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateExpense(ExpenseCreate model)
        {
            var categoryService = CreateCategoryService();
            if (!categoryService.CreateCategory(model))
                return false;

            var dueDateService = CreateDueDateService();
            if (!dueDateService.CreateDueDates(model))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var expenseEntity = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == model.CategoryName && c.UserId == _userId)).Id,
                    Amount = model.Amount,
                    ExpenseFreqType = model.ExpenseFreqType,
                    FrequencyFactor = model.FrequencyFactor,
                    InitialDueDate = model.InitialDueDate,
                    EndDate = model.EndDate,
                    UserId = _userId
                };
                context.Expenses.Add(expenseEntity);
                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteExpense(int id)
        {
            var categoryService = CreateCategoryService();
            if (!categoryService.DeleteCategory(id))
                return false;

            var dueDateService = CreateDueDateService();
            if (!dueDateService.DeleteDueDates(id))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var expenseEntity = context.Expenses.Single(e => e.Id == id && e.UserId == _userId);
                context.Expenses.Remove(expenseEntity);
                return context.SaveChanges() == 1;
            }
        }

        public bool UpdateExpense(ExpenseEdit model)
        {
            var categoryService = CreateCategoryService();
            if (!categoryService.EditCategory(model))
                return false;

            var dueDateService = CreateDueDateService();
            if (!dueDateService.EditDueDates(model))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var expenseEntity = context.Expenses.Single(e => e.Id == model.Id && e.UserId == _userId);

                expenseEntity.Amount = model.Amount;
                expenseEntity.ExpenseFreqType = model.ExpenseFreqType;
                expenseEntity.FrequencyFactor = model.FrequencyFactor;
                expenseEntity.InitialDueDate = model.InitialDueDate;
                expenseEntity.EndDate = model.EndDate;

                return context.SaveChanges() == 1;
            }
        }

        private CategoryService CreateCategoryService()
        {
            var userId = _userId;
            var service = new CategoryService(userId);
            return service;
        }

        private DueDateService CreateDueDateService()
        {
            var userId = _userId;
            var service = new DueDateService(userId);
            return service;
        }
    }
}
