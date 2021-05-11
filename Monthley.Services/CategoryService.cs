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
    public class CategoryService
    {
        private readonly Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategory(ExpenseCreate model)
        {
            var categoryEntity = new Category()
            {
                Name = model.CategoryName,
                Type = model.CategoryType,
                UserId = _userId
            };

            using (var context = new ApplicationDbContext())
            {
                context.Categories.Add(categoryEntity);
                return context.SaveChanges() == 1;
            }
        }

        public bool EditCategory(ExpenseEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var categoryEntity = context.Categories.Single(c => c.Id == model.Id && c.UserId == _userId);

                categoryEntity.Name = model.CategoryName;
                categoryEntity.Type = model.CategoryType;

                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var categoryEntity = context.Categories.Single(c => c.Id == id && c.UserId == _userId);
                context.Categories.Remove(categoryEntity);
                return context.SaveChanges() == 1;
            }
        }
    }
}
