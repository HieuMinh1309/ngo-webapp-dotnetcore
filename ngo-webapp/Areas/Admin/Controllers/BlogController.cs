﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ngo_webapp.Areas.Admin.Models;
using ngo_webapp.Models.Entities;

namespace ngo_webapp.Areas.Admin.Controllers;
[Area("Admin")]
public class BlogController : Controller
{
    private readonly NgoManagementContext _dbContext;

    public BlogController(NgoManagementContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IActionResult Index()
    {
        return View();
    }
	public async Task<IActionResult> Show()
	{
        var blogs = await _dbContext.Blogs.ToListAsync();
        var app = await _dbContext.Appeals.ToListAsync();
        ViewBag.Ids = new List<int>();
        foreach (var item in app)
        {
            ViewBag.Ids.Add(item.AppealsId);
        }
        return View(blogs);
	}
	[HttpGet]
    public IActionResult Add()
    {

        return View();
    }
	[HttpPost]
    public async Task<IActionResult> Add(BlogViewModel model)
	{
        
        Blog bl = new();
		try
		{ 
			bl.Title = model.Title;
			bl.Content = model.Content;
			bl.CreationDate = model.CreationDate;
			bl.UserId = model.UserID;
			bl.AppealId = model.AppealID;
			await _dbContext.Blogs.AddAsync(bl);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction("Show", "Blog");
		}
		catch (Exception)
		{

			throw;
		}
	}

	public ActionResult Delete(int id)
	{

		var blog = _dbContext.Blogs.Find(id);

		_dbContext.Blogs.Remove(blog);
		_dbContext.SaveChanges();

		return RedirectToAction("Show");
	}

	public ActionResult Edit(int id)
	{

		var blog = _dbContext.Blogs.Find(id);

		return View("Edit");
	}

	[HttpPost]
	public ActionResult Edit(BlogViewModel model)
	{
		var blog = _dbContext.Blogs.Find(model.BlogId);

		blog.Title = model.Title;
		blog.Content = model.Content;
		blog.CreationDate = model.CreationDate;


		_dbContext.SaveChanges();

		return RedirectToAction("Show");
	}
}
