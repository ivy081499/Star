﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Star.Models;
using Star.Settings;
using Star.ViewModels;

namespace Star.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CustomerSettings _customerSettings;

    public HomeController(
        ILogger<HomeController> logger,
        CustomerSettings customerSettings)
    {
        _logger = logger;
        _customerSettings = customerSettings;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult SpltSourceString()
    {
        return View();
    }

    public IActionResult Bet()
    {
        BetViewModel model = new BetViewModel()
        {
            CustomerSettings = _customerSettings,
        };

        return View(model);
    }


    public IActionResult TodayBet()
    {
        TodayBetViewModel model = new TodayBetViewModel();
       

        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

