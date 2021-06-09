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

        public bool UpdateCategory(ExpenseEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var categoryEntity = context.Categories.Single(c => c.Id == model.Id && c.UserId == _userId);

                if (categoryEntity.Name == model.CategoryName && categoryEntity.Type == model.CategoryType)
                    return true;

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

        public ICollection<string> GetCategoryNames()
        {
            using (var context = new ApplicationDbContext())
            {
                var categories = context.Categories.Where(c => c.UserId == _userId);
                var categoryNames = new List<string>();
                foreach (var category in categories)
                    categoryNames.Add(category.Name);
                return categoryNames;
            }
        }

        public bool SeedCategoryForNewUser()
        {
            using (var context = new ApplicationDbContext())
            {
                var category = new Category()
                {
                    Name = "Miscellaneous",
                    Type = CategoryType.Unbudgeted,
                    UserId = _userId
                };
                context.Categories.Add(category);
                return context.SaveChanges() == 1;
            }
        }
    }
}
