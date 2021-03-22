using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    private readonly ToDoListContext _db; 

    public CategoriesController(ToDoListContext db)
    { // sets new _db property b/c of dependency injection in Startup.cs
      _db = db;
    }

    // public ActionResult Index()
    // {
    //   List<Category> model = _db.Categories.ToList();
    //   return View(model);
    // }

     public async Task<IActionResult> Index(string searchString)
    {
      var categories = from model in _db.Categories
                  select model;

      // This if
      if (!(String.IsNullOrEmpty(searchString)))
      {
        categories = categories.Where(model => model.Name.Contains(searchString));
      }
      return View(await categories.ToListAsync());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Category category)
    {
      _db.Categories.Add(category);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult CreateItem()
    {
      return View();
    }

    [HttpPost]
    public ActionResult CreateItem(Item item)
    {
      _db.Items.Add(item);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
{
    var thisCategory = _db.Categories
        .Include(category => category.JoinEntities)
        .ThenInclude(join => join.Item)
        .FirstOrDefault(category => category.CategoryId == id);
    return View(thisCategory);
}

    public ActionResult Edit(int id)
    {
      var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
    }

    [HttpPost]
    public ActionResult Edit(Category category)
    {
      _db.Entry(category).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      _db.Categories.Remove(thisCategory);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}