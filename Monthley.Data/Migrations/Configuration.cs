namespace Monthley.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Monthley.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Monthley.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Monthley.Data.ApplicationDbContext context)
        {
            // Seeding a user
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (userManager.FindByEmail("efa@monthley.com") == null)
            {
                var user = new ApplicationUser
                {
                    Email = "efa@monthley.com",
                    UserName = "efa@monthley.com"
                };
                userManager.Create(user, "monTHL3yTest!");

                var userIdString = userManager.FindByEmail("efa@monthley.com").Id;
                var userId = Guid.Parse(userIdString);



                // ---BEGIN USER REGISTRATION SEEDING-------------------------------------------------------------------------------------->
                //
                //    Below is code that appears in special methods of the MonthService, CategoryService, and SourceService.
                //    Normally, when a new user registers an account, the AccountController calls the methods from the 3 service 
                //    classes to write entities to the database.  Since the user 'efa@monTHLey.com' is seeded through this
                //    Configuration class, it doesn't pass through the AccountController, so it is necessary to execute the code 
                //    here to replicate the result of a typical user registration.

                // seeding Month entities for user 'efa@monthley.com'
                List<DateTime> dtList = new List<DateTime>();
                var endDate = new DateTime(2100, 12, 1);
                for (var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    (DateTime.Compare(date, endDate) <= 0);
                    date = date.AddMonths(1))
                    dtList.Add(date);

                foreach (var beginDate in dtList)
                {
                    var month = new Month
                    {
                        BeginDate = beginDate,
                        UserId = userId
                    };
                    context.Months.Add(month);
                }

                // seeding "Miscellaneous" Category entity for user 'efa@monthley.com'
                var category = new Category()
                {
                    Name = "Miscellaneous",
                    Type = CategoryType.Unbudgeted,
                    UserId = userId
                };
                context.Categories.Add(category);

                // seeding "Unplanned" Source entity for user 'efa@monthley.com'
                var source = new Source()
                {
                    Name = "Unplanned",
                    Type = SourceType.Unbudgeted,
                    UserId = userId
                };
                context.Sources.Add(source);

                context.SaveChanges();

                // <-----------------------------------------------------------------------------------------END USER REGISTRATION SEEDING


                // ---BEGIN SEEDING EXAMPLES OF USER DATA-------------------------------------------------------------------------------->
                //
                //    Below is code to seed example data that a user could make in the app. 

                // seeding example Source entities
                var source1 = new Source()
                {
                    Name = "My full-time job",
                    Type = SourceType.Budgeted,
                    UserId = userId
                };
                context.Sources.Add(source1);

                var source2 = new Source()
                {
                    Name = "Private teaching gig",
                    Type = SourceType.Budgeted,
                    UserId = userId
                };
                context.Sources.Add(source2);

                context.SaveChanges();

                // seeding example Income entities
                var income1 = new Income()
                {
                    Id = (context.Sources.Single(s => s.Name == "My full-time job" && s.UserId == userId)).Id,
                    Amount = 600.00m,
                    PayFreqType = PayFreqType.ByWeek,
                    FrequencyFactor = 1,
                    InitialPayDate = new DateTime(2021, 5, 6),
                    LastPayDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Incomes.Add(income1);

                var income2 = new Income()
                {
                    Id = (context.Sources.Single(s => s.Name == "Private teaching gig" && s.UserId == userId)).Id,
                    Amount = 36.00m,
                    PayFreqType = PayFreqType.ByWeek,
                    FrequencyFactor = 2,
                    InitialPayDate = new DateTime(2021, 5, 9),
                    LastPayDate = new DateTime(2022, 12, 31),
                    UserId = userId
                };
                context.Incomes.Add(income2);

                context.SaveChanges();

                // seeding example PayDay entities
                var payDates1 = new List<DateTime>();
                var initialPayDate = new DateTime(2021, 5, 6);
                var lastPayDate = new DateTime(2100, 12, 31);
                for (var date = initialPayDate;
                     (DateTime.Compare(date, lastPayDate)) <= 0;
                     date = date.AddDays(7 * 1))
                    payDates1.Add(date);
                foreach (var date in payDates1)
                {
                    var payDay = new PayDay()
                    {
                        IncomeId = (context.Sources.Single(s => s.Name == "My full-time job" && s.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 600,
                        UserId = userId
                    };
                    context.PayDays.Add(payDay);
                }

                var payDates2 = new List<DateTime>();
                var initialPayDate2 = new DateTime(2021, 5, 9);
                var lastPayDate2 = new DateTime(2022, 12, 31);
                for (var date = initialPayDate2;
                     (DateTime.Compare(date, lastPayDate2)) <= 0;
                     date = date.AddDays(7 * 2))
                    payDates2.Add(date);
                foreach (var date in payDates2)
                {
                    var payDay = new PayDay()
                    {
                        IncomeId = (context.Sources.Single(s => s.Name == "Private teaching gig" && s.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 36.00m,
                        UserId = userId
                    };
                    context.PayDays.Add(payDay);
                }

                context.SaveChanges();



























                // seeding example Category entities
                var category1 = new Category()
                {
                    Name = "Rent",
                    Type = CategoryType.Bill,
                    UserId = userId
                };
                context.Categories.Add(category1);

                var category2 = new Category()
                {
                    Name = "Car Insurance",
                    Type = CategoryType.Bill,
                    UserId = userId
                };
                context.Categories.Add(category2);

                var category3 = new Category()
                {
                    Name = "Car Lease",
                    Type = CategoryType.Bill,
                    UserId = userId
                };
                context.Categories.Add(category3);

                var category4 = new Category()
                {
                    Name = "Comcast ISP/TV",
                    Type = CategoryType.Bill,
                    UserId = userId
                };
                context.Categories.Add(category4);

                var category5 = new Category()
                {
                    Name = "Spotify",
                    Type = CategoryType.Bill,
                    UserId = userId
                };
                context.Categories.Add(category5);

                var category6 = new Category()
                {
                    Name = "Hulu",
                    Type = CategoryType.Bill,
                    UserId = userId
                };
                context.Categories.Add(category6);

                var category7 = new Category()
                {
                    Name = "Daycare",
                    Type = CategoryType.Bill,
                    UserId = userId
                };
                context.Categories.Add(category7);

                var category8 = new Category()
                {
                    Name = "General savings",
                    Type = CategoryType.Saving,
                    UserId = userId
                };
                context.Categories.Add(category8);

                var category9 = new Category()
                {
                    Name = "College fund",
                    Type = CategoryType.Saving,
                    UserId = userId
                };
                context.Categories.Add(category9);

                var category10 = new Category()
                {
                    Name = "Groceries",
                    Type = CategoryType.Expense,
                    UserId = userId
                };
                context.Categories.Add(category10);
               
                var category11 = new Category()
                {
                    Name = "Gasoline",
                    Type = CategoryType.Expense,
                    UserId = userId
                };
                context.Categories.Add(category11);

                var category12 = new Category()
                {
                    Name = "New clothes",
                    Type = CategoryType.Expense,
                    UserId = userId
                };
                context.Categories.Add(category12);

                var category13 = new Category()
                {
                    Name = "Entertainment",
                    Type = CategoryType.Expense,
                    UserId = userId
                };
                context.Categories.Add(category13);

                var category14 = new Category()
                {
                    Name = "Gas bill",
                    Type = CategoryType.Expense,
                    UserId = userId
                };
                context.Categories.Add(category14);

                var category15 = new Category()
                {
                    Name = "Electric bill",
                    Type = CategoryType.Expense,
                    UserId = userId
                };
                context.Categories.Add(category15);

                var category16 = new Category()
                {
                    Name = "Haircuts",
                    Type = CategoryType.Expense,
                    UserId = userId
                };
                context.Categories.Add(category16);

                context.SaveChanges();

                // seeding example Expense entities
                var expense1 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Rent" && c.UserId == userId)).Id,
                    Amount = 710.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 1),
                    EndDate = new DateTime(2022, 10, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense1);

                var expense2 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Car Insurance" && c.UserId == userId)).Id,
                    Amount = 92.33m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 3),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense2);

                var expense3 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Car Lease" && c.UserId == userId)).Id,
                    Amount = 151.39m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 16),
                    EndDate = new DateTime(2023, 8, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense3);

                var expense4 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Comcast ISP/TV" && c.UserId == userId)).Id,
                    Amount = 116.01m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 24),
                    EndDate = new DateTime(2022, 2, 24),
                    UserId = userId
                };
                context.Expenses.Add(expense4);

                var expense5 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Spotify" && c.UserId == userId)).Id,
                    Amount = 9.99m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 10),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense5);

                var expense6 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Hulu" && c.UserId == userId)).Id,
                    Amount = 11.99m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 16),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense6);

                var expense7 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Daycare" && c.UserId == userId)).Id,
                    Amount = 151.00m,
                    ExpenseFreqType = ExpenseFreqType.ByWeek,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 3),
                    EndDate = new DateTime(2022, 5, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense7);

                var expense8 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "General savings" && c.UserId == userId)).Id,
                    Amount = 100.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 15),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense8);

                var expense9 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "College fund" && c.UserId == userId)).Id,
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 15),
                    EndDate = new DateTime(2035, 06, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense9);

                var expense10 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Groceries" && c.UserId == userId)).Id,
                    Amount = 80.00m,
                    ExpenseFreqType = ExpenseFreqType.ByWeek,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 6),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense10);

                var expense11 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Gasoline" && c.UserId == userId)).Id,
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 31),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense11);

                var expense12 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "New clothes" && c.UserId == userId)).Id,
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 31),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense12);

                var expense13 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Entertainment" && c.UserId == userId)).Id,
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 31),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense13);

                var expense14 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Gas bill" && c.UserId == userId)).Id,
                    Amount = 30.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 22),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense14);

                var expense15 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Electric bill" && c.UserId == userId)).Id,
                    Amount = 70.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 11),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense15);

                var expense16 = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == "Haircuts" && c.UserId == userId)).Id,
                    Amount = 15.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 5, 31),
                    EndDate = new DateTime(2100, 12, 31),
                    UserId = userId
                };
                context.Expenses.Add(expense16);

                context.SaveChanges();

                // seeding example DueDate entities
                var dueDates1 = new List<DateTime>();
                var initialDueDate1 = new DateTime(2021, 5, 1);
                var endDate1 = new DateTime(2022, 10, 31);
                for (var date = initialDueDate1;
                    (DateTime.Compare(date, endDate1)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates1.Add(date);
                foreach (var date in dueDates1)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Rent" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 710.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates2 = new List<DateTime>();
                var initialDueDate2 = new DateTime(2021, 5, 3);
                var endDate2 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate2;
                    (DateTime.Compare(date, endDate2)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates2.Add(date);
                foreach (var date in dueDates2)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Car Insurance" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 92.33m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates3 = new List<DateTime>();
                var initialDueDate3 = new DateTime(2021, 5, 16);
                var endDate3 = new DateTime(2023, 8, 31);
                for (var date = initialDueDate3;
                    (DateTime.Compare(date, endDate3)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates3.Add(date);
                foreach (var date in dueDates3)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Car Lease" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 151.39m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates4 = new List<DateTime>();
                var initialDueDate4 = new DateTime(2021, 5, 24);
                var endDate4 = new DateTime(2022, 2, 24);
                for (var date = initialDueDate4;
                    (DateTime.Compare(date, endDate4)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates4.Add(date);
                foreach (var date in dueDates4)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Comcast ISP/TV" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 116.01m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates5 = new List<DateTime>();
                var initialDueDate5 = new DateTime(2021, 5, 10);
                var endDate5 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate5;
                    (DateTime.Compare(date, endDate5)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates5.Add(date);
                foreach (var date in dueDates5)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Spotify" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 9.99m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates6 = new List<DateTime>();
                var initialDueDate6 = new DateTime(2021, 5, 16);
                var endDate6 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate6;
                    (DateTime.Compare(date, endDate6)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates6.Add(date);
                foreach (var date in dueDates6)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Hulu" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 11.99m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates7 = new List<DateTime>();
                var initialDueDate7 = new DateTime(2021, 5, 3);
                var endDate7 = new DateTime(2022, 5, 31);
                for (var date = initialDueDate7;
                    (DateTime.Compare(date, endDate7)) <= 0;
                    date = date.AddDays(7 * 1))
                    dueDates7.Add(date);
                foreach (var date in dueDates7)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Daycare" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 151.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates8 = new List<DateTime>();
                var initialDueDate8 = new DateTime(2021, 5, 15);
                var endDate8 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate8;
                    (DateTime.Compare(date, endDate8)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates8.Add(date);
                foreach (var date in dueDates8)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "General savings" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 100.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates9 = new List<DateTime>();
                var initialDueDate9 = new DateTime(2021, 5, 15);
                var endDate9 = new DateTime(2035, 06, 31);
                for (var date = initialDueDate9;
                    (DateTime.Compare(date, endDate9)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates9.Add(date);
                foreach (var date in dueDates9)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "College fund" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 50.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates10 = new List<DateTime>();
                var initialDueDate10 = new DateTime(2021, 5, 6);
                var endDate10 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate10;
                    (DateTime.Compare(date, endDate10)) <= 0;
                    date = date.AddDays(7 * 1))
                    dueDates10.Add(date);
                foreach (var date in dueDates10)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Groceries" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 80.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates11 = new List<DateTime>();
                var initialDueDate11 = new DateTime(2021, 5, 31);
                var endDate11 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate11;
                    (DateTime.Compare(date, endDate11)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates11.Add(date);
                foreach (var date in dueDates11)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Gasoline" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 50.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates12 = new List<DateTime>();
                var initialDueDate12 = new DateTime(2021, 5, 31);
                var endDate12 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate12;
                    (DateTime.Compare(date, endDate12)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates12.Add(date);
                foreach (var date in dueDates12)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "New clothes" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 50.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates13 = new List<DateTime>();
                var initialDueDate13 = new DateTime(2021, 5, 31);
                var endDate13 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate13;
                    (DateTime.Compare(date, endDate13)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates13.Add(date);
                foreach (var date in dueDates13)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Entertainment" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 50.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates14 = new List<DateTime>();
                var initialDueDate14 = new DateTime(2021, 5, 22);
                var endDate14 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate14;
                    (DateTime.Compare(date, endDate14)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates14.Add(date);
                foreach (var date in dueDates14)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Gas bill" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 30.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates15 = new List<DateTime>();
                var initialDueDate15 = new DateTime(2021, 5, 11);
                var endDate15 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate15;
                    (DateTime.Compare(date, endDate15)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates15.Add(date);
                foreach (var date in dueDates15)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Electric bill" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 70.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                var dueDates16 = new List<DateTime>();
                var initialDueDate16 = new DateTime(2021, 5, 31);
                var endDate16 = new DateTime(2100, 12, 31);
                for (var date = initialDueDate16;
                    (DateTime.Compare(date, endDate16)) <= 0;
                    date = date.AddMonths(1 * 1))
                    dueDates16.Add(date);
                foreach (var date in dueDates16)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == "Haircuts" && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = 15.00m,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }

                context.SaveChanges();


                // seeding example PaymentReceived entities

                // seeding example PaymentMade entities








                // <-------------------------------------------------------------------------------------END SEEDING EXAMPLES OF USER DATA



            }
        }
    }
}
