using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication4.Models;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    //[Authorize]
    public class CoursesController : ApiController
    {
        private CourseDbContext db = new CourseDbContext();

        // GET: api/Courses
        public IQueryable<Course> GetCourses()
        {
            return db.Courses;
        }

        [Route("api/Courses/Students/Include")]
        public IQueryable<Course> GetCourseWithStudents()
        {
            return db.Courses.Include(x => x.Students).AsQueryable();
        }


        // GET: api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [Route("api/Courses/{id}/Include")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourseWithStudents(int id)
        {
            Course course = db.Courses.Include(x => x.Students).FirstOrDefault(x => x.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }


        // PUT: api/Courses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseId)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Courses
        [ResponseType(typeof(Course))]
        public IHttpActionResult PostCourse(Course course)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.Courses.Add(course);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.CourseId }, course);
        }

        //[HttpPost, Route("api/Students/Upload")]
        //public IHttpActionResult Upload()
        //{
        //    if (HttpContext.Current.Request.Files.Count > 0)
        //    {
        //        var file = HttpContext.Current.Request.Files[0];
        //        string ext = Path.GetExtension(file.FileName);
        //        string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
        //        string savePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Pictures"), f);
        //        file.SaveAs(savePath);
        //        return Ok(new FileUploadResult { NewFileName = f });
        //    }
        //    return BadRequest();
        //}

        [HttpPost,Route("api/students/upload")]
        public IHttpActionResult Upload()
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                var file = HttpContext.Current.Request.Files[0];
                string ext = Path.GetExtension(file.FileName);
                string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+ext;
                string savepath = Path.Combine(HttpContext.Current.Server.MapPath("~/Pictures"), f);
                file.SaveAs(savepath);
                return Ok(new FileUploadResult { NewFileName = f });
            }

            return BadRequest();
        }


        // DELETE: api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}