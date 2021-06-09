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
            /* 
             
            
            OLD SEEDING APPROACH-->






             
            // Seeding a demo account
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
                var endDate = new DateTime(2050, 12, 1);
                for (var date = new DateTime(2021, 3, 1);
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
                context.SaveChanges();

                // seeding "Miscellaneous" Category entity for user 'efa@monthley.com'
                var category = new Category()
                {
                    Name = "Miscellaneous",
                    Type = CategoryType.Unbudgeted,
                    UserId = userId
                };
                context.Categories.Add(category);
                context.SaveChanges();

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
                SeedIncome("My full-time job", 700.00m, PayFreqType.ByWeek, 1, new DateTime(2021, 3, 4), new DateTime(2025, 12, 31), userId);
                SeedIncome("Private teaching gig", 36.00m, PayFreqType.ByWeek, 2, new DateTime(2021, 3, 14), new DateTime(2022, 12, 31), userId);

                // seeding example PayDay entities
                SeedPayDays("My full-time job", new DateTime(2021, 3, 4), new DateTime(2025, 12, 31), PayFreqType.ByWeek, 1, 700.00m, userId);
                SeedPayDays("Private teaching gig", new DateTime(2021, 3, 14), new DateTime(2022, 12, 31), PayFreqType.ByWeek, 2, 36.00m, userId);

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
                SeedExpense("Rent", 710.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 1), new DateTime(2022, 10, 31), userId);
                SeedExpense("Car Insurance", 92.33m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 3), new DateTime(2050, 12, 31), userId);
                SeedExpense("Car Lease", 151.39m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 16), new DateTime(2023, 8, 31), userId);
                SeedExpense("Comcast ISP/TV", 116.01m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 24), new DateTime(2022, 2, 24), userId);
                SeedExpense("Spotify", 9.99m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 10), new DateTime(2050, 12, 31), userId);
                SeedExpense("Hulu", 11.99m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 16), new DateTime(2050, 12, 31), userId);
                SeedExpense("Daycare", 151.00m, ExpenseFreqType.ByWeek, 1, new DateTime(2021, 3, 1), new DateTime(2022, 5, 31), userId);
                SeedExpense("General savings", 100.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 15), new DateTime(2050, 12, 31), userId);
                SeedExpense("College fund", 50.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 15), new DateTime(2035, 06, 30), userId);
                SeedExpense("Groceries", 80.00m, ExpenseFreqType.ByWeek, 1, new DateTime(2021, 3, 4), new DateTime(2050, 12, 31), userId);
                SeedExpense("Gasoline", 50.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 31), new DateTime(2050, 12, 31), userId);
                SeedExpense("New clothes", 50.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 31), new DateTime(2050, 12, 31), userId);
                SeedExpense("Entertainment", 50.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 31), new DateTime(2050, 12, 31), userId);
                SeedExpense("Gas bill", 30.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 22), new DateTime(2050, 12, 31), userId);
                SeedExpense("Electric bill", 70.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 11), new DateTime(2050, 12, 31), userId);
                SeedExpense("Haircuts", 15.00m, ExpenseFreqType.ByMonth, 1, new DateTime(2021, 3, 31), new DateTime(2050, 12, 31), userId);

                // seeding example DueDate entities
                SeedDueDates("Rent", new DateTime(2021, 3, 1), new DateTime(2022, 10, 31), ExpenseFreqType.ByMonth, 1, 710.00m, userId);
                SeedDueDates("Car Insurance", new DateTime(2021, 3, 3), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 92.33m, userId);
                SeedDueDates("Car Lease", new DateTime(2021, 3, 16), new DateTime(2023, 8, 31), ExpenseFreqType.ByMonth, 1, 151.39m, userId);
                SeedDueDates("Comcast ISP/TV", new DateTime(2021, 3, 24), new DateTime(2022, 2, 24), ExpenseFreqType.ByMonth, 1, 116.01m, userId);
                SeedDueDates("Spotify", new DateTime(2021, 3, 10), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 9.99m, userId);
                SeedDueDates("Hulu", new DateTime(2021, 3, 16), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 11.99m, userId);
                SeedDueDates("Daycare", new DateTime(2021, 3, 1), new DateTime(2022, 5, 31), ExpenseFreqType.ByWeek, 1, 151.00m, userId);
                SeedDueDates("General savings", new DateTime(2021, 3, 15), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 100.00m, userId);
                SeedDueDates("College fund", new DateTime(2021, 3, 15), new DateTime(2035, 06, 30), ExpenseFreqType.ByMonth, 1, 50.00m, userId);
                SeedDueDates("Groceries", new DateTime(2021, 3, 4), new DateTime(2050, 12, 31), ExpenseFreqType.ByWeek, 1, 80.00m, userId);
                SeedDueDates("New clothes", new DateTime(2021, 3, 31), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 50.00m, userId);
                SeedDueDates("Entertainment", new DateTime(2021, 3, 31), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 50.00m, userId);
                SeedDueDates("Gas bill", new DateTime(2021, 3, 22), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 30.00m, userId);
                SeedDueDates("Electric bill", new DateTime(2021, 3, 11), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 70.00m, userId);
                SeedDueDates("Haircuts", new DateTime(2021, 3, 31), new DateTime(2050, 12, 31), ExpenseFreqType.ByMonth, 1, 15.00m, userId);

                // seeding example PaymentReceived entities
                SeedPaymentReceived("My full-time job", 694.53m, new DateTime(2021, 3, 4), userId);
                SeedPaymentReceived("My full-time job", 710.01m, new DateTime(2021, 3, 11), userId);
                SeedPaymentReceived("My full-time job", 705.66m, new DateTime(2021, 3, 18), userId);
                SeedPaymentReceived("My full-time job", 701.01m, new DateTime(2021, 3, 25), userId);
                SeedPaymentReceived("My full-time job", 731.84m, new DateTime(2021, 4, 1), userId);
                SeedPaymentReceived("My full-time job", 700.22m, new DateTime(2021, 4, 8), userId);
                SeedPaymentReceived("My full-time job", 709.31m, new DateTime(2021, 4, 15), userId);
                SeedPaymentReceived("My full-time job", 692.49m, new DateTime(2021, 4, 22), userId);
                SeedPaymentReceived("My full-time job", 700.41m, new DateTime(2021, 4, 29), userId);
                SeedPaymentReceived("My full-time job", 704.11m, new DateTime(2021, 5, 6), userId);
                SeedPaymentReceived("My full-time job", 696.82m, new DateTime(2021, 5, 14), userId);
                SeedPaymentReceived("My full-time job", 713.40m, new DateTime(2021, 5, 21), userId);
                SeedPaymentReceived("My full-time job", 700.41m, new DateTime(2021, 5, 27), userId);
                SeedPaymentReceived("Private teaching gig", 36.00m, new DateTime(2021, 3, 14), userId);
                SeedPaymentReceived("Private teaching gig", 36.00m, new DateTime(2021, 3, 28), userId);
                SeedPaymentReceived("Private teaching gig", 36.00m, new DateTime(2021, 4, 11), userId);
                SeedPaymentReceived("Private teaching gig", 36.00m, new DateTime(2021, 4, 25), userId);
                SeedPaymentReceived("Private teaching gig", 36.00m, new DateTime(2021, 5, 10), userId);
                SeedPaymentReceived("Private teaching gig", 36.00m, new DateTime(2021, 5, 24), userId);

                // seeding example PaymentMade entities
                SeedPaymentMade("Rent", 710.00m, new DateTime(2021, 3, 1), userId);
                SeedPaymentMade("Rent", 710.00m, new DateTime(2021, 4, 1), userId);
                SeedPaymentMade("Rent", 710.00m, new DateTime(2021, 5, 1), userId);
                SeedPaymentMade("Car Insurance", 92.33m, new DateTime(2021, 3, 3), userId);
                SeedPaymentMade("Car Insurance", 92.33m, new DateTime(2021, 4, 3), userId);
                SeedPaymentMade("Car Insurance", 92.33m, new DateTime(2021, 5, 3), userId);
                SeedPaymentMade("Car Lease", 151.39m, new DateTime(2021, 3, 16), userId);
                SeedPaymentMade("Car Lease", 151.39m, new DateTime(2021, 4, 16), userId);
                SeedPaymentMade("Car Lease", 151.39m, new DateTime(2021, 5, 16), userId);
                SeedPaymentMade("Comcast ISP/TV", 116.01m, new DateTime(2021, 3, 24), userId);
                SeedPaymentMade("Comcast ISP/TV", 116.01m, new DateTime(2021, 4, 24), userId);
                SeedPaymentMade("Comcast ISP/TV", 116.01m, new DateTime(2021, 5, 24), userId);
                SeedPaymentMade("Spotify", 9.99m, new DateTime(2021, 3, 10), userId);
                SeedPaymentMade("Spotify", 9.99m, new DateTime(2021, 4, 10), userId);
                SeedPaymentMade("Spotify", 9.99m, new DateTime(2021, 5, 10), userId);
                SeedPaymentMade("Hulu", 11.99m, new DateTime(2021, 3, 16), userId);
                SeedPaymentMade("Hulu", 11.99m, new DateTime(2021, 4, 16), userId);
                SeedPaymentMade("Hulu", 11.99m, new DateTime(2021, 5, 16), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 3, 1), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 3, 8), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 3, 15), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 3, 22), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 3, 29), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 4, 5), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 4, 12), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 4, 19), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 4, 26), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 5, 3), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 5, 10), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 5, 17), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 5, 24), userId);
                SeedPaymentMade("Daycare", 151.00m, new DateTime(2021, 5, 31), userId);
                SeedPaymentMade("General savings", 100.00m, new DateTime(2021, 3, 15), userId);
                SeedPaymentMade("General savings", 100.00m, new DateTime(2021, 4, 15), userId);
                SeedPaymentMade("General savings", 100.00m, new DateTime(2021, 5, 15), userId);
                SeedPaymentMade("College fund", 50.00m, new DateTime(2021, 3, 15), userId);
                SeedPaymentMade("College fund", 50.00m, new DateTime(2021, 4, 15), userId);
                SeedPaymentMade("College fund", 50.00m, new DateTime(2021, 5, 15), userId);
                SeedPaymentMade("Groceries", 81.43m, new DateTime(2021, 3, 4), userId);
                SeedPaymentMade("Groceries", 83.89m, new DateTime(2021, 3, 11), userId);
                SeedPaymentMade("Groceries", 72.32m, new DateTime(2021, 3, 18), userId);
                SeedPaymentMade("Groceries", 76.23m, new DateTime(2021, 3, 25), userId);
                SeedPaymentMade("Groceries", 89.55m, new DateTime(2021, 4, 1), userId);
                SeedPaymentMade("Groceries", 59.14m, new DateTime(2021, 4, 8), userId);
                SeedPaymentMade("Groceries", 84.92m, new DateTime(2021, 4, 15), userId);
                SeedPaymentMade("Groceries", 81.36m, new DateTime(2021, 4, 22), userId);
                SeedPaymentMade("Groceries", 80.61m, new DateTime(2021, 4, 29), userId);
                SeedPaymentMade("Groceries", 93.90m, new DateTime(2021, 5, 6), userId);
                SeedPaymentMade("Groceries", 76.92m, new DateTime(2021, 5, 13), userId);
                SeedPaymentMade("Groceries", 79.18m, new DateTime(2021, 5, 20), userId);
                SeedPaymentMade("Groceries", 73.24m, new DateTime(2021, 5, 27), userId);
                SeedPaymentMade("Gasoline", 20.04m, new DateTime(2021, 3, 5), userId);
                SeedPaymentMade("Gasoline", 20.03m, new DateTime(2021, 3, 17), userId);
                SeedPaymentMade("Gasoline", 20.41m, new DateTime(2021, 3, 26), userId);
                SeedPaymentMade("Gasoline", 10.09m, new DateTime(2021, 4, 11), userId);
                SeedPaymentMade("Gasoline", 25.24m, new DateTime(2021, 4, 21), userId);
                SeedPaymentMade("Gasoline", 25.98m, new DateTime(2021, 4, 29), userId);
                SeedPaymentMade("Gasoline", 9.87m, new DateTime(2021, 5, 9), userId);
                SeedPaymentMade("Gasoline", 20.55m, new DateTime(2021, 5, 17), userId);
                SeedPaymentMade("Gasoline", 18.18m, new DateTime(2021, 5, 26), userId);
                SeedPaymentMade("New clothes", 31.18m, new DateTime(2021, 3, 1), userId);
                SeedPaymentMade("New clothes", 20.77m, new DateTime(2021, 3, 18), userId);
                SeedPaymentMade("New clothes", 18.14m, new DateTime(2021, 4, 4), userId);
                SeedPaymentMade("New clothes", 22.89m, new DateTime(2021, 4, 20), userId);
                SeedPaymentMade("New clothes", 25.98m, new DateTime(2021, 5, 4), userId);
                SeedPaymentMade("New clothes", 20.48m, new DateTime(2021, 5, 22), userId);
                SeedPaymentMade("Entertainment", 45.28m, new DateTime(2021, 3, 14), userId);
                SeedPaymentMade("Entertainment", 12.00m, new DateTime(2021, 4, 14), userId);
                SeedPaymentMade("Entertainment", 15.00m, new DateTime(2021, 4, 20), userId);
                SeedPaymentMade("Entertainment", 12.00m, new DateTime(2021, 5, 4), userId);
                SeedPaymentMade("Entertainment", 40.43m, new DateTime(2021, 5, 21), userId);
                SeedPaymentMade("Gas bill", 85.13m, new DateTime(2021, 3, 22), userId);
                SeedPaymentMade("Gas bill", 52.43m, new DateTime(2021, 4, 22), userId);
                SeedPaymentMade("Gas bill", 37.43m, new DateTime(2021, 5, 22), userId);
                SeedPaymentMade("Electric bill", 38.43m, new DateTime(2021, 3, 11), userId);
                SeedPaymentMade("Electric bill", 58.97m, new DateTime(2021, 4, 11), userId);
                SeedPaymentMade("Electric bill", 82.51m, new DateTime(2021, 5, 11), userId);
                SeedPaymentMade("Haircuts", 15.00m, new DateTime(2021, 3, 10), userId);
                SeedPaymentMade("Haircuts", 15.00m, new DateTime(2021, 4, 13), userId);
                SeedPaymentMade("Haircuts", 15.00m, new DateTime(2021, 5, 21), userId);

                // seeding example PaymentMade entities (unbudgeted spending)
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 3, 3), userId);
                SeedPaymentMade("Miscellaneous", 8.98m, new DateTime(2021, 3, 7), userId);
                SeedPaymentMade("Miscellaneous", 3.86m, new DateTime(2021, 3, 11), userId);
                SeedPaymentMade("Miscellaneous", 4.41m, new DateTime(2021, 3, 14), userId);
                SeedPaymentMade("Miscellaneous", 15.41m, new DateTime(2021, 3, 14), userId);
                SeedPaymentMade("Miscellaneous", 8.98m, new DateTime(2021, 3, 19), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 3, 19), userId);
                SeedPaymentMade("Miscellaneous", 10.08m, new DateTime(2021, 3, 22), userId);
                SeedPaymentMade("Miscellaneous", 9.63m, new DateTime(2021, 3, 27), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 3, 28), userId);
                SeedPaymentMade("Miscellaneous", 13.87m, new DateTime(2021, 3, 31), userId);
                SeedPaymentMade("Miscellaneous", 6.55m, new DateTime(2021, 4, 2), userId);
                SeedPaymentMade("Miscellaneous", 1.89m, new DateTime(2021, 4, 6), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 4, 11), userId);
                SeedPaymentMade("Miscellaneous", 11.32m, new DateTime(2021, 4, 11), userId);
                SeedPaymentMade("Miscellaneous", 8.98m, new DateTime(2021, 4, 11), userId);
                SeedPaymentMade("Miscellaneous", 38.15m, new DateTime(2021, 4, 18), userId);
                SeedPaymentMade("Miscellaneous", 15.73m, new DateTime(2021, 4, 22), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 4, 23), userId);
                SeedPaymentMade("Miscellaneous", 46.13m, new DateTime(2021, 4, 23), userId);
                SeedPaymentMade("Miscellaneous", 11.30m, new DateTime(2021, 4, 27), userId);
                SeedPaymentMade("Miscellaneous", 4.15m, new DateTime(2021, 4, 28), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 4, 30), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 5, 6), userId);
                SeedPaymentMade("Miscellaneous", 15.11m, new DateTime(2021, 5, 7), userId);
                SeedPaymentMade("Miscellaneous", 14.68m, new DateTime(2021, 5, 7), userId);
                SeedPaymentMade("Miscellaneous", 8.98m, new DateTime(2021, 5, 12), userId);
                SeedPaymentMade("Miscellaneous", 15.92m, new DateTime(2021, 5, 15), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 5, 19), userId);
                SeedPaymentMade("Miscellaneous", 7.56m, new DateTime(2021, 5, 21), userId);
                SeedPaymentMade("Miscellaneous", 19.31m, new DateTime(2021, 5, 24), userId);
                SeedPaymentMade("Miscellaneous", 22.02m, new DateTime(2021, 5, 26), userId);
                SeedPaymentMade("Miscellaneous", 2.11m, new DateTime(2021, 5, 26), userId);
                // <-------------------------------------------------------------------------------------END SEEDING EXAMPLES OF USER DATA
            }
            */
        }


        /*
        
        OLD SEEDING APPROACH-->





        // helpers
        private void SeedIncome(string sourceName, decimal amount, PayFreqType payFreqType, int frequencyFactor, DateTime initialPayDate, DateTime lastPayDate, Guid userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var income = new Income()
                {
                    Id = (context.Sources.Single(c => c.Name == sourceName && c.UserId == userId)).Id,
                    Amount = amount,
                    PayFreqType = payFreqType,
                    FrequencyFactor = frequencyFactor,
                    InitialPayDate = initialPayDate,
                    LastPayDate = lastPayDate,
                    UserId = userId
                };
                context.Incomes.Add(income);
                context.SaveChanges();
            }
        }
        private void SeedPayDays(string sourceName, DateTime initialPayDate, DateTime lastPayDate, PayFreqType freqType, int frequencyFactor, decimal amount, Guid userId)
        {
            var payDates = new List<DateTime>();
            if (freqType == PayFreqType.ByMonth)
            {
                for (var date = initialPayDate;
                 (DateTime.Compare(date, lastPayDate)) <= 0;
                 date = date.AddMonths(1 * frequencyFactor))
                    payDates.Add(date);
            }
            else
            {
                for (var date = initialPayDate;
                 (DateTime.Compare(date, lastPayDate)) <= 0;
                 date = date.AddDays(7 * frequencyFactor))
                    payDates.Add(date);
            }
            using (var context = new ApplicationDbContext())
            {
                foreach (var date in payDates)
                {
                    var payDay = new PayDay()
                    {
                        IncomeId = (context.Sources.Single(s => s.Name == sourceName && s.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = amount,
                        UserId = userId
                    };
                    context.PayDays.Add(payDay);
                }
                context.SaveChanges();
            }
        }

        private void SeedExpense(string categoryName, decimal amount, ExpenseFreqType expenseFreqType, int frequencyFactor, DateTime initialDueDate, DateTime endDate, Guid userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var expense = new Expense()
                {
                    Id = (context.Categories.Single(c => c.Name == categoryName && c.UserId == userId)).Id,
                    Amount = amount,
                    ExpenseFreqType = expenseFreqType,
                    FrequencyFactor = frequencyFactor,
                    InitialDueDate = initialDueDate,
                    EndDate = endDate,
                    UserId = userId
                };
                context.Expenses.Add(expense);
                context.SaveChanges();
            }
        }

        private void SeedDueDates(string categoryName, DateTime initialDueDate, DateTime endDate, ExpenseFreqType freqType, int frequencyFactor, decimal amount, Guid userId)
        {
            var dueDates = new List<DateTime>();
            if (freqType == ExpenseFreqType.ByMonth)
            {
                for (var date = initialDueDate;
                 (DateTime.Compare(date, endDate)) <= 0;
                 date = date.AddMonths(1 * frequencyFactor))
                    dueDates.Add(date);
            }
            else
            {
                for (var date = initialDueDate;
                 (DateTime.Compare(date, endDate)) <= 0;
                 date = date.AddDays(7 * frequencyFactor))
                    dueDates.Add(date);
            }
            using (var context = new ApplicationDbContext())
            {
                foreach (var date in dueDates)
                {
                    var dueDate = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == categoryName && c.UserId == userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == userId)).Id,
                        Date = date,
                        Amount = amount,
                        UserId = userId
                    };
                    context.DueDates.Add(dueDate);
                }
                context.SaveChanges();
            }
        }

        private void SeedPaymentReceived(string sourceName, decimal amount, DateTime paymentDate, Guid userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentReceived = new PaymentReceived()
                {
                    SourceId = context.Sources.SingleOrDefault(c => c.Name == sourceName && c.UserId == userId).Id,
                    MonthId = context.Months.SingleOrDefault(m => m.BeginDate.Month == paymentDate.Month && m.BeginDate.Year == paymentDate.Year && m.UserId == userId).Id,
                    Amount = amount,
                    PaymentDate = paymentDate,
                    UserId = userId
                };
                context.PaymentsReceived.Add(paymentReceived);
                context.SaveChanges();
            }
        }

        private void SeedPaymentMade(string categoryName, decimal amount, DateTime paymentDate, Guid userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentMade = new PaymentMade()
                {
                    CategoryId = context.Categories.SingleOrDefault(c => c.Name == categoryName && c.UserId == userId).Id,
                    MonthId = context.Months.SingleOrDefault(m => m.BeginDate.Month == paymentDate.Month && m.BeginDate.Year == paymentDate.Year && m.UserId == userId).Id,
                    Amount = amount,
                    PaymentDate = paymentDate,
                    UserId = userId
                };
                context.PaymentsMade.Add(paymentMade);
                context.SaveChanges();
            }
        }

        */
    }
}