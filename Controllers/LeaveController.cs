using LeaveManagement.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LeaveManagement.Controllers
{
    public class LeaveController : Controller
    {
        // GET: Leave
        ApplicationDbContext context=new ApplicationDbContext();
        [HttpGet]
        public ActionResult Index(string leaveType = "All",int pageno=1)
        {
           UserEntity loggedInUser = Session["loggedInUser"] as UserEntity;
           List<LeaveEntity> leaves = new List<LeaveEntity>();
            var leaveQuery = context.Leaves.Include("User")
                .Where(l => l.UserId == loggedInUser.UserId).AsQueryable();
            if (leaveType != null && leaveType.Equals("Approved"))
            {
                leaveQuery = leaveQuery.Where(l => l.Status.Equals("Approved"));
            }
            if (leaveType != null && leaveType.Equals("Pending"))
            {
                leaveQuery = leaveQuery.Where(l => l.Status.Equals("Pending"));
            }
            if (leaveType != null && leaveType.Equals("Denied"))
            {
                leaveQuery = leaveQuery.Where(l => l.Status.Equals("Denied"));
            }
            leaves = leaveQuery.ToList();
           ViewBag.TotalLeaveCount = leaves.Count;



            int noofrecordperpage = 5;
            int noofpage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(leaves.Count) / Convert.ToDouble(noofrecordperpage)));
            int noofrecordstoskip = (pageno - 1) * noofrecordperpage;
            ViewBag.pageno = pageno;
            ViewBag.noofpage = noofpage;
            leaves = leaves.Skip(noofrecordstoskip).Take(noofrecordperpage).ToList();
            return View(leaves);
           
        }
        

        [HttpGet]
        public ActionResult GetStudentOrStaffLeaves(int userTypeId,int pageno=1)
        {
            List<LeaveEntity> leaves = new List<LeaveEntity>();
            if(userTypeId == 2)
            {
                leaves = context.Leaves.Include("User").Where(l => l.User.UserTypeId == 2).ToList();
            }
            if (userTypeId == 3)
            {
                 leaves = context.Leaves.Include("User").Where(l => l.User.UserTypeId == 3).ToList();
            }
            
            int noofrecordperpage = 5;
            int noofpage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(leaves.Count) / Convert.ToDouble(noofrecordperpage)));
            int noofrecordstoskip = (pageno - 1) * noofrecordperpage;
            ViewBag.pageno = pageno;
            ViewBag.noofpage = noofpage;
            leaves = leaves.Skip(noofrecordstoskip).Take(noofrecordperpage).ToList();
            ViewBag.RequestLeaveCount = leaves.Count;
            return View(leaves);
        }




        [HttpGet]
        public ActionResult ApplyLeave()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ApplyLeave(LeaveEntity leave)
        {
            UserEntity loggedInUser = Session["loggedInUser"] as UserEntity;
            if (ModelState.IsValid)
            {
                if (leave != null)
                {
                    if (leave.StartDate > leave.EndDate)
                    {
                        ModelState.AddModelError("LessthanEnd", "Start Date must be less than or equal to End Date.");
                        return View();
                    }
                    leave.Status = "Pending";
                    leave.UserId = loggedInUser.UserId;
                    leave.NumberOfDays = Convert.ToInt16((leave.EndDate - leave.StartDate).Days) + 1;
                    context.Leaves.Add(leave);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public ActionResult ApproveLeave(int leaveId)
        {
            UserEntity loggedInUser = Session["loggedInUser"] as UserEntity;
            LeaveEntity leave = context.Leaves.Include("User").FirstOrDefault(l => l.LeaveId == leaveId);
            if(leave != null)
            {
                leave.Status = "Approved";
                leave.ApprovedBy = loggedInUser.Name;
                context.Entry(leave).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            return RedirectToAction("GetStudentOrStaffLeaves", new { userTypeId = leave.User.UserTypeId});
        }

        public ActionResult DenyLeave(int leaveId)
        {
            UserEntity loggedInUser = Session["loggedInUser"] as UserEntity;
            LeaveEntity leave = context.Leaves.Include("User").FirstOrDefault(l => l.LeaveId == leaveId);
            if (leave != null)
            {
                leave.Status = "Denied";
                leave.ApprovedBy = loggedInUser.Name;
                context.Entry(leave).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            return RedirectToAction("GetStudentOrStaffLeaves", new { userTypeId = leave.User.UserTypeId });
        }

        [HttpGet]
       public ActionResult Update(int leaveId)
        {
            LeaveEntity leave=context.Leaves.FirstOrDefault(u => u.LeaveId == leaveId);
            return View(leave);
        }
        [HttpPost]

        public ActionResult Update(LeaveEntity leave)
        {
            if(leave != null)
            {
                context.Entry(leave).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            return View("index");
        }











        protected UserEntity GetuserByEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                return context.Users.FirstOrDefault(u => u.Email.Equals(email));
            }
            return null;
        }
    }
}