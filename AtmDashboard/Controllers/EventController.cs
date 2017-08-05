using AtmDashboard.Models;
using AtmDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AtmDashboard.Controllers
{
    public class EventController : ApiController
    {
        public ATMEvent[] GetEvents()
        {
            return ATMEventAggregator.GetLoggedEvents();
        }
    }
}
