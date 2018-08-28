using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication10.Data;
using WebApplication10.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication10.Controllers
{
    //[Authorize(Roles = "admin")]
    public class InstructionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public object Users { get; private set; }

        public InstructionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Instructions
        public async Task<IActionResult> Index(string id)
        {
            var applicationDbContext = _context.Instructions.Include(i => i.ApplicationUserId).Include(f => f.instructionSteps);
            return View(await applicationDbContext.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {   

            if (id == null)
            {
                return NotFound();
            }

            var instruction = await _context.Instructions.Include(i => i.instructionSteps)
                .Include(i => i.ApplicationUser)
                .SingleOrDefaultAsync(m => m.InstructionId == id);
            if (instruction == null)
            {
                return NotFound();
            }

            return View(instruction);
        }

        // GET: Instructions/Create
        [Authorize]
        public IActionResult Create()
        {
            SelectList category = new SelectList(_context.Categories, "CategoryId", "NameCategory");
            ViewBag.Categories = category;
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructionId,ApplicationUserId,NameInstruction,Decription,Img")] Instruction instruction)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                instruction.ApplicationUser = currentUser;
                _context.Add(instruction);
                await _context.SaveChangesAsync();
                TempData["Instruction"] = instruction.InstructionId.ToString();
                return RedirectToAction("AddStep");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", instruction.ApplicationUserId);
            return View("AddStep");
        }

        [HttpPost]
        public IActionResult SaveChanges(InstructionStep instStep)
        {
            var insrtcId = Convert.ToInt32((string)TempData["Instruction"]);
            var instruction = _context.Instructions.Include(i => i.instructionSteps).Single(i => i.InstructionId == insrtcId);
            instruction.instructionSteps.Add(instStep);
            _context.SaveChanges();
            return RedirectToAction("Index","Home");

        }

        [HttpGet]
        public IActionResult AddStep()
        {
            return View("AddStep");
        }

        [HttpPost]
        public IActionResult AddStep(InstructionStep instStep)
        {
            var insrtcId = Convert.ToInt32((string)TempData["Instruction"]);
            var instruction = _context.Instructions.Include(i => i.instructionSteps).Single(i => i.InstructionId == insrtcId);
            instruction.instructionSteps.Add(instStep);
            _context.SaveChanges();
            return RedirectToAction("AddStep");
        }

        
        

        // GET: Instructions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instruction = await _context.Instructions.Include(d=>d.instructionSteps).SingleOrDefaultAsync(m => m.InstructionId == id);
            if (instruction == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", instruction.ApplicationUserId);
            return View(instruction);
        }
         

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructionId,ApplicationUserId,NameInstruction,Decription,Img")] Instruction instruction)
        {
            if (id != instruction.InstructionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instruction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructionExists(instruction.InstructionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", instruction.ApplicationUserId);
            return View(instruction);
        }

        // GET: Instructions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instruction = await _context.Instructions
                .Include(i => i.ApplicationUser)
                .SingleOrDefaultAsync(m => m.InstructionId == id);
            if (instruction == null)
            {
                return NotFound();
            }

            return View(instruction);
        }

        // POST: Instructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instruction = await _context.Instructions.SingleOrDefaultAsync(m => m.InstructionId == id);
           // var  instructionSteps = await _context.InstructionSteps.SingleOrDefault(k => k.InstructionStepId==id);
            _context.Instructions.Remove(instruction);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool InstructionExists(int id)
        {
            return _context.Instructions.Any(e => e.InstructionId == id);
        }
    }
}
