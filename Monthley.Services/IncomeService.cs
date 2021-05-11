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
    public class IncomeService
    {
        private readonly Guid _userId;

        public IncomeService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateIncome(IncomeCreate model)
        {
            var sourceService = CreateSourceService();
            if (!sourceService.CreateSource(model))
                return false;

            var payDayService = CreatePayDayService();
            if (!payDayService.CreatePayDays(model))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var incomeEntity = new Income()
                {
                    Id = (context.Sources.Single(s => s.Name == model.SourceName && s.UserId == _userId)).Id,
                    Amount = model.Amount,
                    PayFreqType = model.PayFreqType,
                    FrequencyFactor = model.FrequencyFactor,
                    InitialPayDate = model.InitialPayDate,
                    LastPayDate = model.LastPayDate,
                    UserId = _userId
                };
                context.Incomes.Add(incomeEntity);
                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteIncome(int id)
        {
            var sourceService = CreateSourceService();
            if (!sourceService.DeleteSource(id))
                return false;

            var payDayService = CreatePayDayService();
            if (!payDayService.DeletePayDays(id))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var incomeEntity = context.Incomes.Single(i => i.Id == id && i.UserId == _userId);
                context.Incomes.Remove(incomeEntity);
                return context.SaveChanges() == 1;
            }
        }

        public bool UpdateIncome(IncomeEdit model)
        {
            var sourceService = CreateSourceService();
            if (!sourceService.EditSource(model))
                return false;

            var payDayService = CreatePayDayService();
            if (!payDayService.EditPayDays(model))
                return false;

            using (var context = new ApplicationDbContext())
            {
                var incomeEntity = context.Incomes.Single(i => i.Id == model.Id && i.UserId == _userId);

                incomeEntity.Amount = model.Amount;
                incomeEntity.PayFreqType = model.PayFreqType;
                incomeEntity.FrequencyFactor = model.FrequencyFactor;
                incomeEntity.InitialPayDate = model.InitialPayDate;
                incomeEntity.LastPayDate = model.LastPayDate;

                return context.SaveChanges() == 1;
            }
        }

        private SourceService CreateSourceService()
        {
            var userId = _userId;
            var service = new SourceService(userId);
            return service;
        }

        private PayDayService CreatePayDayService()
        {
            var userId = _userId;
            var service = new PayDayService(userId);
            return service;
        }
    }
}

