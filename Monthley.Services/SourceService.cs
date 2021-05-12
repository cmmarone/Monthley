using Monthley.Data;
using Monthley.Data.Entities;
using Monthley.Models.IncomeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Services
{
    public class SourceService
    {
        private readonly Guid _userId;

        public SourceService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateSource(IncomeCreate model)
        {
            var sourceEntity = new Source()
            {
                Name = model.SourceName,
                Type = model.SourceType,
                UserId = _userId
            };

            using (var context = new ApplicationDbContext())
            {
                context.Sources.Add(sourceEntity);
                return context.SaveChanges() == 1;
            }
        }

        public bool UpdateSource(IncomeEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var sourceEntity = context.Sources.Single(s => s.Id == model.Id && s.UserId == _userId);

                sourceEntity.Name = model.SourceName;
                sourceEntity.Type = model.SourceType;

                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteSource(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var sourceEntity = context.Sources.Single(s => s.Id == id && s.UserId == _userId);
                context.Sources.Remove(sourceEntity);
                return context.SaveChanges() == 1;
            }
        }
    }
}
