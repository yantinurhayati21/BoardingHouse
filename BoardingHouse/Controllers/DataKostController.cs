﻿using BoardingHouse.Data;
using BoardingHouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardingHouse.Controllers
{
	public class DataKostController:Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;

		public DataKostController(AppDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public IActionResult Detail()
		{
			List<DataKost> kost = _context.KostData.ToList();
			return View(kost);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] KostForm data, IFormFile Photo)
		{
			if (!ModelState.IsValid)
			{
				return View(data);
			}

			var kostdata = new DataKost()
			{
				Name = data.Name,
				Address = data.Address,
				Price = data.Price,
				Room = data.Room,
			};

			if (Photo != null)
			{
				if (Photo.Length > 0)
				{
					var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
					var fileExt = Path.GetExtension(Photo.FileName).ToLower();
					if (!allowedExtensions.Contains(fileExt))
					{
						ModelState.AddModelError("Foto", "File type is not allowed. Please upload a JPG or PNG file.");
						return View(data);
					}

					var fileFolder = Path.Combine(_env.WebRootPath, "Upload");

					if (!Directory.Exists(fileFolder))
					{
						Directory.CreateDirectory(fileFolder);
					}

					var fileName = "photo_" + data.Name + Path.GetExtension(Photo.FileName);
					var fullFilePath = Path.Combine(fileFolder, fileName);
					using (var stream = new FileStream(fullFilePath, FileMode.Create))
					{
						await Photo.CopyToAsync(stream);
					}

					kostdata.Photo = fileName;
				}
			}
			_context.KostData.Add(kostdata);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			var kost = _context.KostData.FirstOrDefault(y => y.Id == id);
			if (kost == null)
			{
				return NotFound();
			}

			return View(kost);
		}

		[HttpPost]
		public IActionResult Edit([FromForm] DataKost kosts)
		{
			var eKost = _context.KostData.FirstOrDefault(y => y.Id == kosts.Id);
			if (eKost != null)
			{
				eKost.Name = kosts.Name;
				eKost.Address = kosts.Address;
				eKost.Price = kosts.Price;
				eKost.Room = kosts.Room;

				_context.KostData.Update(eKost);
				_context.SaveChanges();
			}
			return RedirectToAction("Index", "DataKost");
		}


		public IActionResult Delete(int id)
		{
			var kost = _context.KostData.FirstOrDefault(x => x.Id == id);

			_context.KostData.Remove(kost);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult Index()
		{
			var kosts = _context.KostData.ToList();
			return View(kosts);
		}

		[HttpGet("DataKost/Index")]
		public async Task<IActionResult> Detail(string search)
		{
			IQueryable<DataKost> kosts = _context.KostData;

			if (!string.IsNullOrEmpty(search))
			{
				string[] prices = search.Split('-');
				if (prices.Length == 2 && decimal.TryParse(prices[0], out decimal lowPrice) && decimal.TryParse(prices[1], out decimal highPrice))
				{
					kosts = kosts.Where(k => k.Name.Contains(search) || (k.Price >= lowPrice && k.Price <= highPrice));
				}
				else
				{
					kosts = kosts.Where(k => k.Name.Contains(search));
				}
			}

			var kostList = await kosts.ToListAsync();
			return View(kostList);
		}
	}
}
