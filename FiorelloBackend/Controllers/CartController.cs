using FiorelloBackend.Data;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;

namespace FiorelloBackend.Controllers
{
    public class CartController : Controller
    {

        private readonly IBasketService _basketService;


        public CartController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _basketService.GetBasketDatasAsync());
        }

        public async Task<IActionResult> DeleteProductFromBasket(int? id)
        {
            if (id == null) return BadRequest();

            var data = await _basketService.DeleteProductFromBasket((int)id);
            return Ok(data);
        }

        public async Task<IActionResult> ProductCountPlus(int? id)
        {
            if (id == null) return BadRequest();
            var count = await _basketService.ProductCountPlus((int)id);
            return Ok(count);
        }
        public async Task<IActionResult> ProductCountMinus(int? id)
        {
            if (id == null) return BadRequest();
            var count = await _basketService.ProductCountMinus((int)id);
            return Ok(count);
        }


    }
}
