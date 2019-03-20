using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Biluthyrning.Models;
using Biluthyrning.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biluthyrning.Controllers
{

    public class HomeController : Controller

    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {

            _context = context;

        }

        public IActionResult Index()
        {

            var list = _context.Bookings.OrderBy(x => x.CarId).ToList();

            return View(list);

        }

        public IActionResult Rent()
        {
            var viewmodel = new CarBookingVM();
            viewmodel.AllCars = _context.Cars.Select(RegNumber => new SelectListItem() { Text = RegNumber.RegNumber, Value = RegNumber.Id.ToString() });
            ViewData["Message"] = "";
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult SaveBooking([Bind("CustomerBirthday, CarId, StartDate, EndDate")] Booking booking)
        {

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                _context.SaveChanges();
                var viewmodel = new CarBookingVM();
                viewmodel.AllCars = _context.Cars.Select(RegNumber => new SelectListItem() { Text = RegNumber.RegNumber, Value = RegNumber.Id.ToString() });
                ViewData["message"] = "Booking accepted!";
                return View("Rent", viewmodel);
            }
            else
            {
                var viewmodel = new CarBookingVM();
                viewmodel.AllCars = _context.Cars.Select(RegNumber => new SelectListItem() { Text = RegNumber.RegNumber, Value = RegNumber.Id.ToString() });
                ViewData["message"] = "Error, please try again!";
                return View("Rent", viewmodel);
            }

        }


        public IActionResult Return()
        {
            List<Booking> list = new List<Booking>();
            list = _context.Bookings.Include(x => x.Car).ToList();
            return View(list);
        }


        public async Task<IActionResult> Seed()
        {
            var a1 = new Car
            {
                RegNumber = "YMC246",
                Km = 457,
            };

            var a2 = new Car
            {
                RegNumber = "CPD487",
                Km = 768,
            };

            var a3 = new Car
            {
                RegNumber = "OCS618",
                Km = 679,
            };

            _context.Cars.Add(a1);
            _context.Cars.Add(a2);
            _context.Cars.Add(a3);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = _context.Bookings.Where(x => x.Id == id).FirstOrDefault();
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Return));
        }

        public IActionResult ReturnBooking(int id)
        {
            var booking = _context.Bookings.Where(x => x.Id == id).Include(y => y.Car).FirstOrDefault();
            ViewData["message"] = "";
            ViewData["stringKm"] = "";
            return View(booking);
        }

        public IActionResult ReturnCarAfterKM(int id, int kmdroven)
        {
            var booking = _context.Bookings.Where(x => x.Id == id).Include(y => y.Car).FirstOrDefault();

            var price = CalculatePrice(kmdroven, booking.Id);

            ViewData["message"] = $"The calculated price for the period is {price} SEK.";
            ViewData["stringKm"] = $"{kmdroven}";

            return View("ReturnBooking", booking);


        }

        private object CalculatePrice(int kmdroven, int id)
        {
            var booking = _context.Bookings.Where(x => x.Id == id).Include(y => y.Car).Include(y => y.Car.CarType).FirstOrDefault();
            var kmNow = booking.Car.Km;
            var type = booking.Car.CarType;
            var daySpan =   DateTime.Now - booking.StartDate;
            var xx = daySpan.Days.ToString();
            var numberOfDays = int.Parse(xx);
            var numberOfKm = kmdroven - kmNow;

            int baseDayRental = 500;
            int kmPrice = 5;

            if (type.Id == 2)
            {
                return (baseDayRental * numberOfDays * 1.2m) + (kmPrice * numberOfKm);
            }
            else if (type.Id == 3)
            {
                return (baseDayRental * numberOfDays * 1.7) + (kmPrice * numberOfKm * 1.5);
            }
            else
            {
                return baseDayRental * numberOfDays;
            }

        }

        public async Task<IActionResult> UpdateAndRemove(int bookId, int carId, int kmNow)
        {
            await Update(carId, kmNow);

            await Delete(bookId);

            List<Booking> list = new List<Booking>();
            list = _context.Bookings.Include(x => x.Car).ToList();
            return View("Return", list);
        }

        private async Task<IActionResult> Update(int carId, int kmNow)
        {
            var car = _context.Cars.Where(x => x.Id == carId).Include(x => x.CarType).SingleOrDefault();
            car.Km = kmNow;

            _context.Update(car);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<IActionResult> Delete(int bookId)
        {
            var deleteBooking = _context.Bookings.Where(x => x.Id == bookId).SingleOrDefault();

            _context.Bookings.Remove(deleteBooking);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
