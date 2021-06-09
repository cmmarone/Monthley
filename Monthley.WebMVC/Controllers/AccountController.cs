using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Monthley.Data;
using Monthley.Data.Entities;
using Monthley.Models.ExpenseModels;
using Monthley.Models.IncomeModels;
using Monthley.Models.PaymentReceivedModels;
using Monthley.Services;
using Monthley.WebMVC.Models;

namespace Monthley.WebMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                    // ------BEGIN monTHLey "SEEDING"----------------------------------->

                    /* Upon new user registration success, seed entities in the database with 
                             a Guid property value belonging to the new user */

                    var userId = Guid.Parse(user.Id);

                    // seed all month entities
                    var monthService = new MonthService(userId);
                    monthService.SeedMonthsForNewUser();

                    // seed a "Miscellaneous" Category
                    var categoryService = new CategoryService(userId);
                    categoryService.SeedCategoryForNewUser();

                    // seed an "Unplanned" Source
                    var sourceService = new SourceService(userId);
                    sourceService.SeedSourceForNewUser();

                    // <-------------------------------------------END monTHLey "SEEDING"


                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Welcome");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // User can setup a pre-filled account with some seeded data to take the app for a test-drive.  They just provide
        // their name.

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterTestUser(string name)
        {
            var user = new ApplicationUser { UserName = $"{name}@monthley.com", Email = $"{name}@monthley.com" };
            var result = UserManager.Create(user, "monTHL3yTest!");

            if (result.Succeeded)
            {
                SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                var userId = Guid.Parse(user.Id);

                // seed test Month entities -- SeedMonthsForTestUser() generates months thru 2025
                var monthService = new MonthService(userId);
                monthService.SeedMonthsForTestUser();

                // seed an "Unplanned" Source
                var sourceService = new SourceService(userId);
                sourceService.SeedSourceForNewUser();

                // seed a "Miscellaneous" Category
                var categoryService = new CategoryService(userId);
                categoryService.SeedCategoryForNewUser();


                // seed test Income entities (seeds related Source and PayDay entities)
                var incomeService = new IncomeService(userId);
                incomeService.CreateIncome(new IncomeCreate
                {
                    SourceName = "My full-time job",
                    Amount = 700.00m,
                    PayFreqType = PayFreqType.ByWeek,
                    FrequencyFactor = 1,
                    InitialPayDate = new DateTime(2021, 3, 4),
                    LastPayDate = new DateTime(2025, 12, 31)
                });
                incomeService.CreateIncome(new IncomeCreate
                {
                    SourceName = "Private teaching gig",
                    Amount = 36.00m,
                    PayFreqType = PayFreqType.ByWeek,
                    FrequencyFactor = 2,
                    InitialPayDate = new DateTime(2021, 3, 14),
                    LastPayDate = new DateTime(2025, 12, 31)
                });


                // seed test Expense entities (seeds related Category and DueDate entities)
                var expenseService = new ExpenseService(userId);
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Bill,
                    CategoryName = "Rent",
                    Amount = 710.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 1),
                    EndDate = new DateTime(2022, 10, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Bill,
                    CategoryName = "Car Insurance",
                    Amount = 92.33m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 3),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Bill,
                    CategoryName = "Car Lease",
                    Amount = 151.39m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 16),
                    EndDate = new DateTime(2023, 8, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Bill,
                    CategoryName = "Comcast ISP/TV",
                    Amount = 116.01m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 24),
                    EndDate = new DateTime(2022, 2, 24)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Bill,
                    CategoryName = "Spotify",
                    Amount = 9.99m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 10),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Bill,
                    CategoryName = "Hulu",
                    Amount = 11.99m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 16),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Bill,
                    CategoryName = "Daycare",
                    Amount = 151.00m,
                    ExpenseFreqType = ExpenseFreqType.ByWeek,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 1),
                    EndDate = new DateTime(2022, 5, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Saving,
                    CategoryName = "General Savings",
                    Amount = 100.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 15),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Saving,
                    CategoryName = "College fund",
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 15),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Expense,
                    CategoryName = "Groceries",
                    Amount = 80.00m,
                    ExpenseFreqType = ExpenseFreqType.ByWeek,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 4),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Expense,
                    CategoryName = "Gasoline",
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 31),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Expense,
                    CategoryName = "New clothes",
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 31),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Expense,
                    CategoryName = "Entertainment",
                    Amount = 50.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 31),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Expense,
                    CategoryName = "Gas bill",
                    Amount = 30.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 22),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Expense,
                    CategoryName = "Electric bill",
                    Amount = 70.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 11),
                    EndDate = new DateTime(2025, 12, 31)
                });
                expenseService.CreateExpense(new ExpenseCreate
                {
                    CategoryType = CategoryType.Expense,
                    CategoryName = "Haircuts",
                    Amount = 15.00m,
                    ExpenseFreqType = ExpenseFreqType.ByMonth,
                    FrequencyFactor = 1,
                    InitialDueDate = new DateTime(2021, 3, 31),
                    EndDate = new DateTime(2025, 12, 31)
                });

                // seed test PaymentReceived entities
                var paymentReceivedService = new PaymentReceivedService(userId);
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 694.53m, new DateTime(2021, 3, 4));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 710.01m, new DateTime(2021, 3, 11));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 705.66m, new DateTime(2021, 3, 18));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 701.01m, new DateTime(2021, 3, 25));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 731.84m, new DateTime(2021, 4, 1));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 700.22m, new DateTime(2021, 4, 8));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 709.31m, new DateTime(2021, 4, 15));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 692.49m, new DateTime(2021, 4, 22));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 700.41m, new DateTime(2021, 4, 29));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 704.11m, new DateTime(2021, 5, 6));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 696.82m, new DateTime(2021, 5, 14));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 713.40m, new DateTime(2021, 5, 21));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("My full-time job", 700.41m, new DateTime(2021, 5, 27));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("Private teaching gig", 36.00m, new DateTime(2021, 3, 14));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("Private teaching gig", 36.00m, new DateTime(2021, 3, 28));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("Private teaching gig", 36.00m, new DateTime(2021, 4, 11));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("Private teaching gig", 36.00m, new DateTime(2021, 4, 25));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("Private teaching gig", 36.00m, new DateTime(2021, 5, 10));
                paymentReceivedService
                    .SeedPaymentReceivedForTestUser("Private teaching gig", 36.00m, new DateTime(2021, 5, 24));

                // seed test PaymentMade entities
                var paymentMadeService = new PaymentMadeService(userId);
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Rent", 710.00m, new DateTime(2021, 3, 1));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Rent", 710.00m, new DateTime(2021, 4, 1));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Rent", 710.00m, new DateTime(2021, 5, 1));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Car Insurance", 92.33m, new DateTime(2021, 3, 3));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Car Insurance", 92.33m, new DateTime(2021, 4, 3));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Car Insurance", 92.33m, new DateTime(2021, 5, 3));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Car Lease", 151.39m, new DateTime(2021, 3, 16));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Car Lease", 151.39m, new DateTime(2021, 4, 16));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Car Lease", 151.39m, new DateTime(2021, 5, 16));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Comcast ISP/TV", 116.01m, new DateTime(2021, 3, 24));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Comcast ISP/TV", 116.01m, new DateTime(2021, 4, 24));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Comcast ISP/TV", 116.01m, new DateTime(2021, 5, 24));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Spotify", 9.99m, new DateTime(2021, 3, 10));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Spotify", 9.99m, new DateTime(2021, 4, 10));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Spotify", 9.99m, new DateTime(2021, 5, 10));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Hulu", 11.99m, new DateTime(2021, 3, 16));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Hulu", 11.99m, new DateTime(2021, 4, 16));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Hulu", 11.99m, new DateTime(2021, 5, 16));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 3, 1));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 3, 8));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 3, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 3, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 3, 29));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 4, 5));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 4, 12));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 4, 19));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 4, 26));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 5, 3));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 5, 10));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 5, 17));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 5, 24));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Daycare", 151.00m, new DateTime(2021, 5, 31));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("General savings", 100.00m, new DateTime(2021, 3, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("General savings", 100.00m, new DateTime(2021, 4, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("General savings", 100.00m, new DateTime(2021, 5, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("College fund", 50.00m, new DateTime(2021, 3, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("College fund", 50.00m, new DateTime(2021, 4, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("College fund", 50.00m, new DateTime(2021, 5, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 81.43m, new DateTime(2021, 3, 4));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 83.89m, new DateTime(2021, 3, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 72.32m, new DateTime(2021, 3, 18));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 76.23m, new DateTime(2021, 3, 25));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 89.55m, new DateTime(2021, 4, 1));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 59.14m, new DateTime(2021, 4, 8));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 84.92m, new DateTime(2021, 4, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 81.36m, new DateTime(2021, 4, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 80.61m, new DateTime(2021, 4, 29));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 93.90m, new DateTime(2021, 5, 6));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 76.92m, new DateTime(2021, 5, 13));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 79.18m, new DateTime(2021, 5, 20));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Groceries", 73.24m, new DateTime(2021, 5, 27));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 20.04m, new DateTime(2021, 3, 5));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 20.03m, new DateTime(2021, 3, 17));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 20.41m, new DateTime(2021, 3, 26));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 10.09m, new DateTime(2021, 4, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 25.24m, new DateTime(2021, 4, 21));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 25.98m, new DateTime(2021, 4, 29));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 9.87m, new DateTime(2021, 5, 9));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 20.55m, new DateTime(2021, 5, 17));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gasoline", 18.18m, new DateTime(2021, 5, 26));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("New clothes", 31.18m, new DateTime(2021, 3, 1));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("New clothes", 20.77m, new DateTime(2021, 3, 18));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("New clothes", 18.14m, new DateTime(2021, 4, 4));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("New clothes", 22.89m, new DateTime(2021, 4, 20));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("New clothes", 25.98m, new DateTime(2021, 5, 4));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("New clothes", 20.48m, new DateTime(2021, 5, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Entertainment", 45.28m, new DateTime(2021, 3, 14));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Entertainment", 12.00m, new DateTime(2021, 4, 14));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Entertainment", 15.00m, new DateTime(2021, 4, 20));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Entertainment", 12.00m, new DateTime(2021, 5, 4));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Entertainment", 40.43m, new DateTime(2021, 5, 21));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gas bill", 85.13m, new DateTime(2021, 3, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gas bill", 52.43m, new DateTime(2021, 4, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Gas bill", 37.43m, new DateTime(2021, 5, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Electric bill", 38.43m, new DateTime(2021, 3, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Electric bill", 58.97m, new DateTime(2021, 4, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Electric bill", 82.51m, new DateTime(2021, 5, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Haircuts", 15.00m, new DateTime(2021, 3, 10));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Haircuts", 15.00m, new DateTime(2021, 4, 13));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Haircuts", 15.00m, new DateTime(2021, 5, 21));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 3, 3));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 8.98m, new DateTime(2021, 3, 7));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 3.86m, new DateTime(2021, 3, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 4.41m, new DateTime(2021, 3, 14));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 15.41m, new DateTime(2021, 3, 14));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 8.98m, new DateTime(2021, 3, 19));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 3, 19));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 10.08m, new DateTime(2021, 3, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 9.63m, new DateTime(2021, 3, 27));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 3, 28));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 13.87m, new DateTime(2021, 3, 31));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 6.55m, new DateTime(2021, 4, 2));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 1.89m, new DateTime(2021, 4, 6));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 4, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 11.32m, new DateTime(2021, 4, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 8.98m, new DateTime(2021, 4, 11));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 38.15m, new DateTime(2021, 4, 18));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 15.73m, new DateTime(2021, 4, 22));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 4, 23));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 46.13m, new DateTime(2021, 4, 23));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 11.30m, new DateTime(2021, 4, 27));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 4.15m, new DateTime(2021, 4, 28));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 4, 30));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 5, 6));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 15.11m, new DateTime(2021, 5, 7));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 14.68m, new DateTime(2021, 5, 7));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 8.98m, new DateTime(2021, 5, 12));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 15.92m, new DateTime(2021, 5, 15));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 5, 19));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 7.56m, new DateTime(2021, 5, 21));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 19.31m, new DateTime(2021, 5, 24));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 22.02m, new DateTime(2021, 5, 26));
                paymentMadeService
                    .SeedPaymentMadeForTestUser("Miscellaneous", 2.11m, new DateTime(2021, 5, 26));

                return RedirectToAction("CurrentBudget", "Month");
            }
            return View();
        }

        //---BEGIN monTHLey NEW USER SETUP WALKTHROUGH VIEWS------------------------>

        // GET: Account/Welcome
        public ActionResult Welcome()
        {
            return View();
        }

        // GET: Account/SetupIncome
        public ActionResult SetupIncome()
        {
            return View();
        }

        // POST: Account/SetupIncome
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupIncome(IncomeCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);

            if (service.CreateIncome(model))
            {
                TempData["SaveResult"] = "Your income was created.";
                return RedirectToAction("IncomeConfirm");
            }

            ModelState.AddModelError("", "Income could not be created.");
            return View(model);
        }

        // GET: Account/IncomeConfirm
        public ActionResult IncomeConfirm()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);
            var modelList = service.GetIncomes();

            return View(modelList);
        }

        // GET Account/ExpensesIntro
        public ActionResult ExpensesIntro()
        {
            return View();
        }

        // GET: Account/SetupBill
        public ActionResult SetupBill()
        {
            var expenseCreate = new ExpenseCreate()
            {
                CategoryType = CategoryType.Bill
            };
            return View(expenseCreate);
        }

        // POST: Account/SetupBill
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupBill(ExpenseCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);

            if (service.CreateExpense(model))
            {
                TempData["SaveResult"] = "Your expense was created.";
                return RedirectToAction("BillConfirm");
            }

            ModelState.AddModelError("", "Expense could not be created.");
            return View(model);
        }

        // GET: Account/BillConfirm
        public ActionResult BillConfirm()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            var modelList = service.GetExpenses();

            return View(modelList);
        }

        // GET: Account/SetupSaving
        public ActionResult SetupSaving()
        {
            var expenseCreate = new ExpenseCreate()
            {
                CategoryType = CategoryType.Saving
            };
            return View(expenseCreate);
        }

        // POST: Account/SetupSaving
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupSaving(ExpenseCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);

            if (service.CreateExpense(model))
            {
                TempData["SaveResult"] = "Your expense was created.";
                return RedirectToAction("SavingConfirm");
            }

            ModelState.AddModelError("", "Expense could not be created.");
            return View(model);
        }

        // GET: Account/SavingConfirm
        public ActionResult SavingConfirm()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            var modelList = service.GetExpenses();

            return View(modelList);
        }

        // GET: Account/SetupBudgetedExpense
        public ActionResult SetupBudgetedExpense()
        {
            var expenseCreate = new ExpenseCreate()
            {
                CategoryType = CategoryType.Expense
            };
            return View(expenseCreate);
        }

        // POST: Account/SetupBudgetedExpense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupBudgetedExpense(ExpenseCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);

            if (service.CreateExpense(model))
            {
                TempData["SaveResult"] = "Your expense was created.";
                return RedirectToAction("BudgetedExpenseConfirm");
            }

            ModelState.AddModelError("", "Expense could not be created.");
            return View(model);
        }

        // GET: Account/BudgetedExpenseConfirm
        public ActionResult BudgetedExpenseConfirm()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            var modelList = service.GetExpenses();

            return View(modelList);
        }

        // GET: Account/SetupSuccess
        public ActionResult SetupSuccess()
        {
            return View();
        }

        // <------------------------------END monTHLey NEW USER SETUP WALKTHROUGH VIEWS


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}