using DemoApplication.Areas.Client.ViewModels.Account;
using DemoApplication.Areas.Client.ViewModels.Account.Address;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;


namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;

        public AccountController(DataContext dataContext, IUserService userService)
        {
            _dataContext = dataContext;
            _userService = userService;
        }

        [HttpGet("dashboard", Name = "client-account-dashboard")]
        public IActionResult Dashboard()
        {
            var user = _userService.CurrentUser;
            var user2 = _userService.CurrentUser;

            return View();
        }

        [HttpGet("download", Name = "client-account-download")]
        public IActionResult Download()
        {


            return View();
        }

        [HttpGet("orders", Name = "client-account-orders")]
        public IActionResult Orders()
        {


            return View();
        }

        [HttpGet("address", Name = "client-account-address")]
        public IActionResult Address()
        {
            var user = _userService.CurrentUser;

            var address = _dataContext.Addresses.FirstOrDefault(a => a.UserId == user.Id);

            if (address is null)
            {
                return RedirectToRoute("client-account-add-address");
            }

            var model = new ItemViewModel
            {
                AcseptorFirstName = address.AcseptorFirstName,
                AcseptorSurName = address.AcseptorSurName,
                PhoneNumber = address.PhoneNumber,
                Name = address.Name,

            };

            return View(model);
        }


        [HttpGet("add-address", Name = "client-account-add-address")]
        public IActionResult AddAddress()
        {

            return View();
        }

        [HttpPost("add-address", Name = "client-account-add-address")]
        public async Task<IActionResult> AddAddress(AddAddressViewModel model)
        {
            var user = _userService.CurrentUser;

            var address = new Address
            {
                UserId = user.Id,
                AcseptorFirstName = model.AcseptorFirstName,
                AcseptorSurName = model.AcseptorSurName,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,


            };

            await _dataContext.Addresses.AddAsync(address);
            await _dataContext.SaveChangesAsync();
            return RedirectToRoute("client-account-address");
        }

        [HttpGet("upate-address", Name = "client-account-update-address")]
        public IActionResult UpdateAddress()
        {
            var user = _userService.CurrentUser;

            var address = _dataContext.Addresses.FirstOrDefault(a => a.UserId == user.Id);

            if (address is null)
            {
                return RedirectToRoute("client-account-add-address");
            }

            var model = new UpdateAddressViewModel
            {
                AcseptorFirstName = address.AcseptorFirstName,
                AcseptorSurName = address.AcseptorSurName,
                PhoneNumber = address.PhoneNumber,
                Name = address.Name,

            };

            return View(model);
        }

        [HttpPost("upate-address", Name = "client-account-update-address")]
        public async Task<IActionResult> UpdateAddress(UpdateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userService.CurrentUser;

            var address = _dataContext.Addresses.FirstOrDefault(a => a.UserId == user.Id);

            if (address is null)
            {
                return RedirectToRoute("client-account-add-address");
            }

            address.AcseptorFirstName = model.AcseptorFirstName;
            address.AcseptorSurName = model.AcseptorSurName;
            address.PhoneNumber = model.PhoneNumber;
            address.Name = model.Name;

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-account-address");

        }

        [HttpGet("payment", Name = "client-account-payment")]
        public IActionResult Payment()
        {


            return View();
        }
        [HttpGet("details", Name = "client-account-details")]
        public IActionResult Details()
        {

            var user = _userService.CurrentUser;

            var model = new UpdateDetailsViewModel { FirstName = user.FirstName, LastName = user.LastName };


            return View(model);
        }

        [HttpPost("details", Name = "client-account-details")]
        public async Task<IActionResult> Details(UpdateDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.CurrentUser;


            if (user is null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-account-details");
        }



        [HttpGet("password", Name = "client-account-password")]
        public IActionResult ChangePassword()
        {


            return View();
        }


        [HttpPost("password", Name = "client-account-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userService.CurrentUser;


            if (!BC.Verify(model.CurrentPassword, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Password is not match");
                return View(model);
            }

            user.Password = BC.HashPassword(model.Password);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-account-password");
        }
    }
}
