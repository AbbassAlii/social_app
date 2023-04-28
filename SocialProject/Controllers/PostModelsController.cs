using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialProject.Data;
using SocialProject.Models;
namespace SocialProject.Controllers
{
	public class PostModelsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<PostModelsController> _logger;
		private readonly IWebHostEnvironment _environment;
		public PostModelsController(ApplicationDbContext context, ILogger<PostModelsController> logger, IWebHostEnvironment environment)
		{
			_context = context;
			_logger = logger;
			_environment = environment;
		}


		//public async Task<IActionResult> Index()
		//{
		//    return _context.PostModel != null ?
		//                View(await _context.PostModel.ToListAsync()) :
		//                Problem("Entity set 'ApplicationDbContext.PostModel'  is null.");
		//}
		public IActionResult Index()
		{
			var username = HttpContext.Session.Get("AdminUserName");

			if (username == null)
				return RedirectToAction("Login", "Admin");
			// Get approved posts
			var activatePosts = _context.PostModel.Where(p => p.Status == "activate");

			return View(activatePosts);
		}
		// GET: PostModels/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.PostModel == null)
			{
				return NotFound();
			}

			var postModel = await _context.PostModel
				.FirstOrDefaultAsync(m => m.PostId == id);
			if (postModel == null)
			{
				return NotFound();
			}

			return View(postModel);
		}

		// GET: PostModels/Create
		public IActionResult Create(int postId)
		{
			var fullname = HttpContext.Session.Get("FullName");

			if (fullname == null)
				return RedirectToAction("Login", "User");
			var activatePosts = _context.PostModel.Where(p => p.Status == "activate").ToList();

			ViewBag.activatePosts = activatePosts;
			//var post = _context.PostModel.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);

			ViewBag.comments = _context.Comments.ToList();
			return View(new PostModel());
		}

		// POST: PostModels/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PostModel postModel, IList<IFormFile> files)
		{
			int? userId = HttpContext.Session.GetInt32("UserId");
			string? fullName = HttpContext.Session.GetString("FullName");
			string? createdby = HttpContext.Session.GetString("Attachment");

			if (userId.HasValue && !string.IsNullOrEmpty(fullName))
			{
				postModel.UserId = userId.Value;
				postModel.FullName = fullName; // add this line to assign the value to the postModel
				postModel.CreateBy = createdby;

				foreach (var file in files)
				{
					if (file != null && file.Length > 0)
					{
						// save file to the server
						var uploads = Path.Combine(_environment.WebRootPath, "uploads");
						if (!Directory.Exists(uploads))
						{
							Directory.CreateDirectory(uploads);
						}

						var fileName = Path.GetFileName(file.FileName);
						using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
						{
							await file.CopyToAsync(fileStream);

							if (postModel.Attachment == "" || postModel.Attachment == null)
							{
								postModel.Attachment = fileName;
							}
							else
							{

								postModel.Attachment = postModel.Attachment + "," + fileName;
							}
						}
					}

				}
			}




			_context.Add(postModel);
			await _context.SaveChangesAsync();
			{
				TempData["message"] = "Need admin approval: Your post has been recorded successfully";
			}
			return RedirectToAction(nameof(Create));



		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Activate(int id)
		{
			var post = await _context.PostModel.FindAsync(id);
			if (post != null)
			{
				post.Status = "Activate";
				_context.Update(post);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var post = await _context.PostModel.FindAsync(id);

			if (post == null)
			{
				return NotFound();
			}

			var postViewModel = new PostModel
			{
				PostId = post.PostId,
				Title = post.Title,
				Description = post.Description,
				Location = post.Location,
				Activity = post.Activity,
				Attachment = post.Attachment
			};

			return View(postViewModel);
		}

		// POST: Posts/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int postId, [Bind("PostId,Title,Description,Location,Activity,Attachment,CreateBy,CreateDate,UpdateBy,UpdateDate,Status,UserId")] PostModel postModel)
		{
			if (postId != postModel.PostId)
			{
				return NotFound();
			}
			var existingPost = await _context.PostModel.FindAsync(postId);

			existingPost.Title = postModel.Title;
			existingPost.Description = postModel.Description;
			existingPost.Location = postModel.Location;
			existingPost.Activity = postModel.Activity;
			existingPost.Attachment = postModel.Attachment;
			existingPost.UpdateBy = postModel.UpdateBy;
			existingPost.UpdateDate = postModel.UpdateDate;
			existingPost.Status = postModel.Status;

			if (ModelState.IsValid)
			{
				_context.Update(existingPost);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(postModel);
		}

		private bool PostExists(int id)
		{
			return _context.PostModel.Any(e => e.PostId == id);
		}


		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.PostModel == null)
			{
				return NotFound();
			}

			var postModel = await _context.PostModel
				.FirstOrDefaultAsync(m => m.PostId == id);
			if (postModel == null)
			{
				return NotFound();
			}

			return View(postModel);
		}

		// POST: PostModels/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int postId)
		{
			// Retrieve the post from the database using the postId
			var post = await _context.PostModel.FindAsync(postId);

			// If post not found, return not found status
			if (post == null)
			{
				return NotFound();
			}

			// Delete the post from the database
			_context.PostModel.Remove(post);
			await _context.SaveChangesAsync();

			// Redirect to index action after successful deletion
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> AdminDashboard()
		{
			var username = HttpContext.Session.Get("AdminUserName");

			if (username == null)
				return RedirectToAction("Login", "Admin");

			// Get all pending posts
			var pendingPosts = await _context.PostModel
				.Where(p => p.Status == "Inactive")
				.ToListAsync();

			// Pass the pending posts to the view
			return View(pendingPosts);
		}

		private bool PostModelExists(int id)
		{
			return (_context.PostModel?.Any(e => e.PostId == id)).GetValueOrDefault();
		}



		public IActionResult PIndex()
		{
			var posts = _context.PostModel.ToList();
			ViewBag.comments = _context.Comments.ToList();
			return View(new PostCommentModel());
		}

		//public IActionResult GetPostComments(int postId)
		//{
		//	var post = _context.PostModel.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);

		//	//var activatePosts = _context.PostModel.Where(p => p.Status == "activate").ToList();

		//	//ViewBag.activatePosts = activatePosts;
		//	var comments = post.Comments.Select(c => new
		//	{
		//		PostCommentId = c.PostCommentId,
		//		Body = c.Body,
		//		PostId = c.PostId,
		//		FullName = c.FullName,
		//              Attachment=c.Attachment

		//	});

		//	var Comments = _context.PostModel.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);
		//	//ViewBag.comments = _context.Comments.ToList();

		//	return View();
		//}
		[HttpGet]
		public IActionResult AddComment()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddComment(int postId, string body, string FullName, string Attachment)
		{
			Attachment = HttpContext.Session.GetString("Attachment");
			FullName = HttpContext.Session.GetString("FullName");
			if (string.IsNullOrWhiteSpace(body))
			{
				ModelState.AddModelError("body", "Comment body is required.");
				return View(postId);
			}


			var post = _context.PostModel.FirstOrDefault(p => p.PostId == postId);
			if (post == null)
			{
				return NotFound();
			}

			var comment = new PostCommentModel
			{
				Body = body,
				PostId = postId,
				FullName = FullName,
				Attachment = Attachment

			};

			_context.Comments.Add(comment);

			_context.SaveChanges();

			return RedirectToAction("Create", new { postId = postId });

		}

		public IActionResult AddCommentForm(int postId)
		{
			return View(postId);
		}

	}
}
