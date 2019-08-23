using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.Display;
using ProTrack.Models.Products;

namespace ProTrack.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProduct _productService;

        public ProductsController(ApplicationDbContext context, IProduct productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            var deviceTypeList = _context.DeviceTypes
                .Select(dt => new SelectListItem()
                {
                    Value = dt.Id.ToString(),
                    Text = dt.Type
                }).ToList();

            var mfgList = _context.Manufacturers
                .Select(mfg => new SelectListItem()
                {
                    Value = mfg.Id.ToString(),
                    Text = mfg.ManufacturerName
                }).ToList();

            var model = new ProductsEntryModel
            {
                DeviceTypeList = deviceTypeList,
                ManufacturerList = mfgList
            };

            return View(model);
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(ProductsEntryModel model)
        {
            if (ModelState.IsValid)
            {
                var save = BuildProductEntry(model);
                _productService.Create(save).Wait();

                return RedirectToAction("Entry", "EndUser");
            }
            return View(model);
        }




        // GET: Product/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var userId = _userManager.GetUserId(User);
            var product = _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            var deviceTypeList = _context.DeviceTypes
                .Select(dt => new SelectListItem()
                    {
                    Value = dt.Id.ToString(),
                    Text = dt.Type,
                    Selected = product.DeviceType.Id == dt.Id ? true : false
                }).ToList();

            var mfgList = _context.Manufacturers
                .Select(mfg => new SelectListItem()
                {
                    Value = mfg.Id.ToString(),
                    Text = mfg.ManufacturerName,
                    Selected = product.Manufacturer.Id == mfg.Id ? true : false
                }).ToList();

            var model = new ProductsEntryModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                DeviceTypeList = deviceTypeList,
                ManufacturerList = mfgList
            };

            if (product == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductsEntryModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var save = BuildProductEntry(product);
                    

                    _context.Update(save);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetProducts(int deviceType)
        {
            List<SelectListItem> productList = new List<SelectListItem>();

            productList = _context.Products.Where(dt => dt.DeviceType.Id == deviceType)
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.Manufacturer.ManufacturerName
                }).OrderBy(p => p.Text).ToList();

            return Json(productList);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private Product BuildProductEntry(ProductsEntryModel model)
        {
            var deviceType = _context.DeviceTypes.Where(dt => dt.Id == model.DTId).FirstOrDefault();
            var mfg = _context.Manufacturers.Where(m => m.Id == model.MfgId).FirstOrDefault();

            return new Product
            {
                Id = model.Id,
                ProductName = model.ProductName,
                DeviceType = deviceType,
                Manufacturer = mfg
            };

        }
    }
}
