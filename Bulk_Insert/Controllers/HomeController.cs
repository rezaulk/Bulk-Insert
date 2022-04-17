using Bulk_Insert.Models;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bulk_Insert.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentContext _appDbContext;
        private DateTime Start;
        private TimeSpan TimeSpan;
        //The "duration" variable contains Execution time when we doing the operations (Insert)  
        public HomeController(StudentContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            List<Names> names = new(); // new syntax from C#9
            //start time now
            Start = DateTime.Now;
            //create fake data
            for (int i = 0; i < 100000; i++)
            {
                names.Add(new Names()
                {
                    Name = "Name_" + i,
                    UniqueID = Guid.NewGuid()
                });
            }

            //open database connection
            using (var transaction = _appDbContext.Database.BeginTransaction())
            {
                //insert list data using BulkInsert
                _appDbContext.BulkInsert(names);
                //commit, save changes
                transaction.Commit();
            }
            TimeSpan = DateTime.Now - Start; // check total time taken

            return View();
        }
    }
}
