using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DotNetAngularVoterEnrollmentForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAngularVoterEnrollmentForm.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly DatabaseContext _context;
        public SampleDataController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet("[action]")]
        public List<States> GetStates()
        {
            List<States> stateList = new List<States>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            stateList = (from state in _context.States
                         select state).ToList(); // For getting list of stated from dbcontext

            // ------- Inserting Select Item in List -------
            stateList.Insert(0, new States { StateId = 0, StateName = "Select" });

            // ------- Assigning categorylist to ViewBag.ListofCategory -------
            return stateList;
        }


        [HttpGet("[action]")]
        public List<City> GetCity(int StateId)
        {
            List<City> cityList = new List<City>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            cityList = (from city in _context.City
                        where city.StateId == StateId
                        select city).ToList();

            // ------- Inserting Select Item in List -------
            cityList.Insert(0, new City { CityId = 0, CityName = "Select" });


            return cityList;
        }
        [HttpGet("[action]")]
        public List<Constituency> GetConstituency(int cityId)
        {
            List<Constituency> constituencyList = new List<Constituency>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            constituencyList = (from constituency in _context.Constituency
                                where constituency.CityId == cityId
                                select constituency).ToList();

            // ------- Inserting Select Item in List -------
            constituencyList.Insert(0, new Constituency { ConstituencyId = 0, ConstituencyName = "Select" });


            return constituencyList;
        }

        [HttpGet("[action]")]
        public List<WardNo> GetWard(int constituencyId)
        {
            List<WardNo> wardList = new List<WardNo>();

            // ------- Getting Data from Database Using EntityFrameworkCore -------
            wardList = (from WardNo in _context.WardNo
                        where WardNo.ConstituencyId == constituencyId
                        select WardNo).ToList();

            // ------- Inserting Select Item in List -------
            wardList.Insert(0, new WardNo { WardNumberId = 0, WardNumber = "Select" });


            return wardList;
        }

        [HttpGet("[action]")]
        public JsonResult GetEnrollmentNumber(VoterEnrollment enrollment)
        {
            string EnrollNumber = generateEnrollmentNumber();
            enrollment.EnrollmentNumber = EnrollNumber;
            enrollment.DateCreated = DateTime.Now;
            _context.Update(enrollment);
            _context.SaveChanges();
            SendEmail(enrollment.Email, enrollment.EnrollmentNumber, enrollment.EnrollerName);
            ViewBag.EnrollNumber = EnrollNumber;
            return Json(enrollment.EnrollmentNumber);

        }

        private string generateEnrollmentNumber()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        private void SendEmail(string emailId, string EnrollmentNumber, string name)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("testemail@gmail.com");
                    mail.To.Add(emailId);
                    mail.Subject = "Your Enrollment ID";
                    mail.Body = "Hi " + name + ", Your Enrollment ID is " + EnrollmentNumber;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("suyash@gmail.com", "123456789");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
    }


    //private static string[] Summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    //[HttpGet("[action]")]
    //public IEnumerable<WeatherForecast> WeatherForecasts()
    //{
    //    var rng = new Random();
    //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //    {
    //        DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
    //        TemperatureC = rng.Next(-20, 55),
    //        Summary = Summaries[rng.Next(Summaries.Length)]
    //    });
    //}

    //public class WeatherForecast
    //{
    //    public string DateFormatted { get; set; }
    //    public int TemperatureC { get; set; }
    //    public string Summary { get; set; }

    //    public int TemperatureF
    //    {
    //        get
    //        {
    //            return 32 + (int)(TemperatureC / 0.5556);
    //        }
    //    }
    //}
}

