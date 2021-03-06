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

            int frequencyFactor = model.FrequencyFactor ?? 1;

            if (model.CategoryType == CategoryType.Once)
                model.ExpenseFreqType = ExpenseFreqType.Once;

            // the only time model.InitialDueDate won't be filled out by user is if model.CategoryType == CategoryType.Expense,
            // in which case, start the due date on the last day of the current month
            DateTime lastDayCurrentMonth = 
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            DateTime initialDueDate = model.InitialDueDate ?? lastDayCurrentMonth;
            if (model.ExpenseFreqType == ExpenseFreqType.ByWeek && model.CategoryType == CategoryType.Expense)
                initialDueDate = DateTime.Now;

            DateTime endDate = model.EndDate ?? new DateTime(2035, 12, 31);
            if (model.CategoryType == CategoryType.Once)
                endDate = initialDueDate;

            using (var context = new ApplicationDbContext())
            {
                var expenseEntity = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == model.CategoryName && c.UserId == _userId)).Id,
                    Amount = model.Amount,
                    ExpenseFreqType = model.ExpenseFreqType,
                    FrequencyFactor = frequencyFactor,
                    InitialDueDate = initialDueDate,
                    EndDate = endDate,
                    UserId = _userId
                };
                context.Expenses.Add(expenseEntity);
                if (!(context.SaveChanges() == 1))
                    return false;
            }

            var dueDateService = CreateDueDateService();
            if (!dueDateService.CreateDueDates(model))
                return false;

            return true;
        }

        public IEnumerable<ExpenseListItem> GetExpenses()
        {
            using (var context = new ApplicationDbContext())
            {
                var expenses = context.Expenses.Where(e => e.UserId == _userId).ToList();

                var expenseList = new List<ExpenseListItem>();
                foreach (var entity in expenses)
                {
                    // getting Frequency
                    string frequency;
                    switch (entity.ExpenseFreqType)
                    {
                        case ExpenseFreqType.ByWeek:
                            if (entity.FrequencyFactor == 1)
                                frequency = "Weekly";
                            else
                                frequency = $"Every {entity.FrequencyFactor} weeks";
                            break;
                        case ExpenseFreqType.ByMonth:
                            if (entity.FrequencyFactor == 1)
                                frequency = "Monthly";
                            else
                                frequency = $"Every {entity.FrequencyFactor} months";
                            break;
                        default:
                            frequency = "Once";
                            break;
                    }

                    // getting NextDueDate
                    DueDate[] datesArray = entity.DueDates.OrderBy(d => d.Date).ToArray();
                    DateTime nextDueDate = (datesArray.FirstOrDefault(d => d.Date >= DateTime.Now)).Date;

                    var expenseListItem = new ExpenseListItem
                    {
                        Id = entity.Id,
                        CategoryType = entity.Category.Type.ToString(),
                        CategoryName = entity.Category.Name,
                        Amount = entity.Amount,
                        Frequency = frequency,
                        NextDueDate = nextDueDate.ToString("D")
                    };
                    expenseList.Add(expenseListItem);
                }
                return expenseList.OrderBy(e => e.CategoryType);
            }
        }

        public ExpenseDetail GetExpenseById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var expenseEntity = ctx.Expenses.Single(e => e.Id == id && e.UserId == _userId);
                return new ExpenseDetail
                {
                    Id = expenseEntity.Id,
                    CategoryName = expenseEntity.Category.Name,
                    CategoryType = expenseEntity.Category.Type,
                    Amount = expenseEntity.Amount,
                    ExpenseFreqType = expenseEntity.ExpenseFreqType,
                    FrequencyFactor = expenseEntity.FrequencyFactor,
                    InitialDueDate = expenseEntity.InitialDueDate,
                    EndDate = expenseEntity.EndDate
                };
            }
        }

        public ExpenseListItem GetExpenseListItemById(int id)
        {
            var expenseListItems = GetExpenses();
            return expenseListItems.FirstOrDefault(e => e.Id == id);
        }

        public bool UpdateExpense(ExpenseEdit model)
        {
            if (model.EndDate == null)
                model.EndDate = new DateTime(2035, 12, 31);
            if (model.ExpenseFreqType == ExpenseFreqType.Once)
                model.EndDate = model.InitialDueDate;

            var categoryService = CreateCategoryService();
            if (!categoryService.UpdateCategory(model))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var expenseEntity = context.Expenses.Single(e => e.Id == model.Id && e.UserId == _userId);

                if (expenseEntity.Amount == model.Amount
                    && expenseEntity.ExpenseFreqType == model.ExpenseFreqType
                    && expenseEntity.FrequencyFactor == model.FrequencyFactor
                    && expenseEntity.InitialDueDate == model.InitialDueDate
                    && expenseEntity.EndDate == model.EndDate)
                    return true;

                expenseEntity.Amount = model.Amount;
                expenseEntity.ExpenseFreqType = model.ExpenseFreqType;
                expenseEntity.FrequencyFactor = model.FrequencyFactor;
                expenseEntity.InitialDueDate = model.InitialDueDate;
                expenseEntity.EndDate = model.EndDate;

                if (!(context.SaveChanges() == 1))
                    return false;
            }

            var dueDateService = CreateDueDateService();
            if (!dueDateService.UpdateDueDates(model))
                return false;

            return true;
        }

        public bool DeleteExpense(int id)
        {
            var dueDateService = CreateDueDateService();
            if (!dueDateService.DeleteDueDates(id))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var expenseEntity = context.Expenses.Single(e => e.Id == id && e.UserId == _userId);
                context.Expenses.Remove(expenseEntity);
                if (!(context.SaveChanges() == 1))
                    return false;
            }

            var categoryService = CreateCategoryService();
            if (!categoryService.DeleteCategory(id))
                return false;

            return true;
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
