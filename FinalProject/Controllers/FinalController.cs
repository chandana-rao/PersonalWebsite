using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.Controllers
{
    public class FinalController : Controller
    {
        private readonly ApplicationDbContext context_;
        private const string sessionId1 = "SessionId";
        private const string sessionId2 = "SessionId";
        public FinalController(ApplicationDbContext context, IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            context_ = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //displaying the list of education history, professional experience and projects
        public IActionResult education()
        {
            return View(context_.academics.ToList<Education>());
        }
        public IActionResult files()
        {
            return View(context_.documents.ToList<Files>());
        }
        
        public IActionResult pexperience()
        {
            return View(context_.experiences.ToList<PExperience>());
        }
        public IActionResult project()
        {
            return View(context_.projects.ToList<Project>());
        }
        
        [HttpGet]
        public IActionResult TypeProjects(int id)
        {
            List<Project> projects = context_.projects.ToList<Project>();
            List<Education> acads = context_.academics.ToList<Education>();
            Education ed = context_.academics.Find(id);
            var edtype = ed.degreeType;
            var getType1 = from p in projects
                           where p.projectType == TypeProject.Academic
                           where p.degreeType == edtype
                           select p;

            return View(getType1.ToList());
        }

        //files
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddFile()
        {
            return View();
        }
        
        //academics
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateAcad()
        {
            var model = new Education();
            return View(model);
        }
        
        [HttpPost]
        public IActionResult CreateAcad(int id, Education ed)
        {
            if(ModelState.IsValid)
            {
                context_.academics.Add(ed);
                context_.SaveChanges();
                return RedirectToAction("education");
            }
            else
                return View(ed);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditAcad(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            Education ed = context_.academics.Find(id);
            if (ed == null)
                return StatusCode(StatusCodes.Status404NotFound);
            return View(ed);
        }

        [HttpPost]
        public IActionResult EditAcad(int? id, Education ed)
        {
            if(ModelState.IsValid)
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status400BadRequest);
                var edu = context_.academics.Find(id);
                if (edu != null)
                {
                    edu.university = ed.university;
                    edu.major = ed.major;
                    edu.year = ed.year;
                    edu.coursework = ed.coursework;
                    edu.degreeType = ed.degreeType;
                    context_.SaveChanges();
                }
                return RedirectToAction("education");
            }
            else
                return View(ed);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAcad(int? id)
        {
            if(id==null)
                return StatusCode(StatusCodes.Status400BadRequest);
            try
            {
                var edu = context_.academics.Find(id);
                if(edu!=null)
                {
                    context_.Remove(edu);
                    context_.SaveChanges();
                }
            }
            catch(Exception)
            {

            }
            return RedirectToAction("education");
        }

        //projects
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateProject()
        {
            var model = new Project();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateProject(int id, Project pr)
        {
            if (ModelState.IsValid)
            {
                if (pr.projectType == TypeProject.Personal && (pr.degreeType == TypeDegree.Graduate || pr.degreeType == TypeDegree.UnderGraduate))
                    return View(pr);
                if (pr.projectType == TypeProject.Academic && (pr.degreeType == null))
                    return View(pr);
                if (pr.projectType == TypeProject.Personal)
                    pr.degreeType = null;
                context_.projects.Add(pr);
                context_.SaveChanges();
                return RedirectToAction("project");
            }
            else
                return View(pr);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditProject(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            Project pr = context_.projects.Find(id);
            if (pr == null)
                return StatusCode(StatusCodes.Status404NotFound);
            return View(pr);
        }

        [HttpPost]
        public IActionResult EditProject(int? id, Project pr)
        {
            if(ModelState.IsValid)
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status400BadRequest);
                var pro = context_.projects.Find(id);
                if (pro != null)
                {
                    pro.name = pr.name;
                    pro.description = pr.description;
                    pro.year = pr.year;
                    pro.technologies = pr.technologies;
                    pro.projectType = pr.projectType;
                    pro.degreeType = pr.degreeType;
                    context_.SaveChanges();
                }
                return RedirectToAction("project");
            }
            else
                return View(pr);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProject(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            try
            {
                var pr = context_.projects.Find(id);
                if (pr != null)
                {
                    context_.Remove(pr);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("project");
        }

        //professional experience
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateExp()
        {
            var model = new PExperience();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateExp(int id, PExperience pr)
        {
            if(ModelState.IsValid)
            {
                context_.experiences.Add(pr);
                context_.SaveChanges();
                return RedirectToAction("pexperience");
            }
            return View(pr);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditExp(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            PExperience pr = context_.experiences.Find(id);
            if (pr == null)
                return StatusCode(StatusCodes.Status404NotFound);
            return View(pr);
        }

        [HttpPost]
        public IActionResult EditExp(int? id, PExperience pr)
        {
            if(ModelState.IsValid)
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status400BadRequest);
                var pro = context_.experiences.Find(id);
                if (pro != null)
                {
                    pro.company = pr.company;
                    pro.description = pr.description;
                    pro.year = pr.year;
                    pro.technologies = pr.technologies;
                    pro.title = pr.title;
                    context_.SaveChanges();
                }
                return RedirectToAction("pexperience");
            }
            return View(pr);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteExp(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            try
            {
                var pr = context_.experiences.Find(id);
                if (pr != null)
                {
                    context_.Remove(pr);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("project");
        }

        [Roles("Admin", "User")]
        [HttpGet]
        public IActionResult Comments(string email)
        {
            email = User.Identity.Name;
            List<Recruiter> recruiters = context_.recruiters.ToList<Recruiter>();
            var getEmail = from r in recruiters
                           where r.email == email
                           select r;
            List<Recruiter> data;
            if (getEmail != null && User.IsInRole("User"))
                data = getEmail.ToList();
            else
                data = recruiters;
            return View(data);
        }

        [Authorize(Roles ="User")]
        [HttpPost]
        public IActionResult AddComment( int id, Recruiter r)
        {
            if(ModelState.IsValid)
            {
                var rec = context_.recruiters.Find(id);
                if (rec != null)
                {
                    rec.comments = r.comments;
                    context_.SaveChanges();
                }
                return RedirectToAction("Comments");
            }
            return View(r);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult AddComment(int? id)
        {
            Recruiter r = context_.recruiters.Find(id);
            return View(r);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult EditComment(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            Recruiter r = context_.recruiters.Find(id);
            if (r == null)
                return StatusCode(StatusCodes.Status404NotFound);
            return View(r);
        }

        [HttpPost]
        public IActionResult EditComment(int? id, Recruiter r)
        {
            if(ModelState.IsValid)
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status400BadRequest);
                var rec = context_.recruiters.Find(id);
                if (rec != null)
                {
                    rec.comments = r.comments;
                    context_.SaveChanges();
                }
                return RedirectToAction("Comments");
            }
            return View(r);
        }

        [Authorize(Roles = "User")]
        public IActionResult DeleteComments(int? id)
        {
            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            try
            {
                var r = context_.recruiters.Find(id);
                if (r != null)
                {
                    r.comments = null;
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Comments");
        }
    }

    public class RolesAttribute : AuthorizeAttribute
    {
        public RolesAttribute(params string[] roles)
        {
            Roles = String.Join(",", roles);
        }
    }
}