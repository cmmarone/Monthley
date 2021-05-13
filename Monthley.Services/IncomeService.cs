﻿using Monthley.Data;
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
            if (model.LastPayDate == null)
                model.LastPayDate = new DateTime(2100, 13, 31);
            if (model.PayFreqType == PayFreqType.Once)
                model.LastPayDate = model.InitialPayDate;

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

        public IEnumerable<IncomeListItem> GetIncomes()
        {
            using (var context = new ApplicationDbContext())
            {
                var incomes = context.Incomes.Where(e => e.UserId == _userId);

                var incomeList = new List<IncomeListItem>();
                foreach (var entity in incomes)
                {
                    // getting Frequency
                    string frequency;
                    switch (entity.PayFreqType)
                    {
                        case PayFreqType.ByWeek:
                            if (entity.FrequencyFactor == 1)
                                frequency = "Weekly";
                            else
                                frequency = $"Every {entity.FrequencyFactor} weeks";
                            break;
                        case PayFreqType.ByMonth:
                            if (entity.FrequencyFactor == 1)
                                frequency = "Monthly";
                            else
                                frequency = $"Every {entity.FrequencyFactor} months";
                            break;
                        default:
                            frequency = "Once";
                            break;
                    }

                    // getting NextPayDate
                    PayDay[] datesArray = entity.PayDays.OrderBy(d => d.Date).ToArray();
                    DateTime nextPayDate = (datesArray.FirstOrDefault(d => d.Date >= DateTime.Now)).Date;

                    var incomeListItem = new IncomeListItem
                    {
                        Id = entity.Id,
                        SourceType = entity.Source.Type.ToString(),
                        SourceName = entity.Source.Name,
                        Amount = entity.Amount,
                        Frequency = frequency,
                        NextPayDate = nextPayDate
                    };
                    incomeList.Add(incomeListItem);
                }
                return incomeList.OrderBy(e => e.SourceType);
            }
        }
        public bool UpdateIncome(IncomeEdit model)
        {
            if (model.LastPayDate == null)
                model.LastPayDate = new DateTime(2100, 13, 31);
            if (model.PayFreqType == PayFreqType.Once)
                model.LastPayDate = model.InitialPayDate;

            var sourceService = CreateSourceService();
            if (!sourceService.UpdateSource(model))
                return false;

            var payDayService = CreatePayDayService();
            if (!payDayService.UpdatePayDays(model))
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
