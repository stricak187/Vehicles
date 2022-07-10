using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vehicles.Data;
using Vehicles.Models;

namespace Vehicles.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VehicleModel
        public async Task<IActionResult> Index(string sortOrder,
            string searchString,
            string currentFilter,
            int? pageNumber)
        {
            var applicationDbContext = _context.VehicleModel.Include(v => v.Make);
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameOrder"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["MakeOrder"] = sortOrder == "Make" ? "make_desc" : "Make";
            ViewData["CurrentFilter"] = searchString;
            var model = from s in _context.VehicleModel
                           select s;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.Name);
                    break;
                case "Make":
                    model = model.OrderBy(s => s.Make);
                    break;
                case "make_desc":
                    model = model.OrderByDescending(s => s.Make);
                    break;
                default:
                    model = model.OrderBy(s => s.Name);
                    break;
            }
            
            int pageSize = 10;
            return View(await PaginatedList<VehicleModel>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: VehicleModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModel
                .Include(v => v.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // GET: VehicleModel/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["MakeId"] = new SelectList(_context.VehicleMake, "Id", "Id");
            return View();
        }

        // POST: VehicleModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MakeId")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.VehicleMake, "Id", "Id", vehicleModel.MakeId);
            return View(vehicleModel);
        }

        // GET: VehicleModel/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModel.FindAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            ViewData["MakeId"] = new SelectList(_context.VehicleMake, "Id", "Id", vehicleModel.MakeId);
            return View(vehicleModel);
        }

        // POST: VehicleModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MakeId")] VehicleModel vehicleModel)
        {
            if (id != vehicleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleModelExists(vehicleModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.VehicleMake, "Id", "Id", vehicleModel.MakeId);
            return View(vehicleModel);
        }

        // GET: VehicleModel/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModel
                .Include(v => v.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // POST: VehicleModel/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleModel = await _context.VehicleModel.FindAsync(id);
            _context.VehicleModel.Remove(vehicleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleModelExists(int id)
        {
            return _context.VehicleModel.Any(e => e.Id == id);
        }
    }
}
