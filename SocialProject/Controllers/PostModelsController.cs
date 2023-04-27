using System.Collections.Generic;
using System.IO;
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
		public IActionResult Create()
		{
			var activatePosts = _context.PostModel.Where(p => p.Status == "activate").ToList();

			ViewBag.activatePosts = activatePosts;
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
            //if (userId.HasValue)
            //{
            //    postModel.UserId = userId.Value; // or postModel.UserId = userId.Value; if UserId is an int property
            //}
            if (userId.HasValue)
            {

                postModel.UserId = userId.Value;
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
    }
}
