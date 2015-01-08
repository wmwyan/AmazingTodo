using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AmazingTodo.Models;

namespace AmazingTodo.Controllers
{
    public class TodoController : ApiController
    {
        private AmazingTodoContext db = new AmazingTodoContext();

        //public IEnumerable<TodoModel> GetTodoItems(string q = null, string sort = null, bool desc = false,
        //                                                int? limit = null, int offset = 0)
        //{
        //    var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<TodoModel>();

        //    IQueryable<TodoModel> items = string.IsNullOrEmpty(sort) ? list.OrderBy(o => o.Priority)
        //        : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

        //    if (!string.IsNullOrEmpty(q) && q != "undefined") items = items.Where(t => t.Todo.Contains(q));

        //    if (offset > 0) items = items.Skip(offset);
        //    if (limit.HasValue) items = items.Take(limit.Value);
        //    return items;
        //}

        // GET api/Todo
        public IEnumerable<TodoModel> GetTodoModels(string q = null, string sort = null, bool desc = false,
                                                        int? limit = null, int offset = 0)
        {
            var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<TodoModel>();

            IQueryable<TodoModel> items = string.IsNullOrEmpty(sort) ? list.OrderBy(o => o.Priority)
                : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

            if (!string.IsNullOrEmpty(q) && q != "undefined") items = items.Where(t => t.Todo.Contains(q));

            if (offset > 0) items = items.Skip(offset);
            if (limit.HasValue) items = items.Take(limit.Value);
            return items;
            //return db.TodoModels.AsEnumerable();
        }

        // GET api/Todo/5
        public TodoModel GetTodoModel(int id)
        {
            TodoModel todomodel = db.TodoModels.Find(id);
            if (todomodel == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return todomodel;
        }

        // PUT api/Todo/5
        public HttpResponseMessage PutTodoModel(int id, TodoModel todomodel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != todomodel.TodoItemId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(todomodel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Todo
        public HttpResponseMessage PostTodoModel(TodoModel todomodel)
        {
            if (ModelState.IsValid)
            {
                db.TodoModels.Add(todomodel);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, todomodel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = todomodel.TodoItemId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Todo/5
        public HttpResponseMessage DeleteTodoModel(int id)
        {
            TodoModel todomodel = db.TodoModels.Find(id);
            if (todomodel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.TodoModels.Remove(todomodel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, todomodel);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}