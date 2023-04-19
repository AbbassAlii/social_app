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

        // GET: PostModels
        public async Task<IActionResult> Index()
        {
              return _context.PostModel != null ? 
                          View(await _context.PostModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PostModel'  is null.");
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
            return View();
        }

        // POST: PostModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostModel postModel, IFormFile file)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //if (userId.HasValue)
            //{
            //    postModel.UserId = userId.Value; // or postModel.UserId = userId.Value; if UserId is an int property
            //}
            if (ModelState.IsValid && userId.HasValue)
            {
                postModel.UserId = userId.Value;
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
                        postModel.Attachment = Path.Combine(uploads, fileName);
                    }
                }

                _context.Add(postModel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            return View(postModel);
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


        private bool PostModelExists(int id)
        {
          return (_context.PostModel?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
