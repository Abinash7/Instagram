using Dapper;
using Instagram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Instagram.Controllers
{
    public class AccountController : Controller
    {
        public IDbConnection _db;
        public AccountController()
        {
            _db = new SqlConnection("Data Source=.; Integrated Security=true; Initial Catalog=InstagramDB");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserDB u, string ReturnURL="")
        {
            var user = _db.Query<UserDB>("Select Username,Email,MobileNo,Password from tblUser").FirstOrDefault();
            if (user!=null)
            {
               
                if (Url.IsLocalUrl(ReturnURL))
                {
                    return Redirect(ReturnURL);
                }
                else
                {
                    return RedirectToAction("Index", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("","Invalid User!!!");
            }
            return View();            
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Signup(UserDB d)
        {
            SqlParameter[] sql =
            {
                new SqlParameter("@Username",d.Username),
                new SqlParameter("@Fullname",d.Fullname),
                new SqlParameter("@Email",d.Email),
                new SqlParameter("@MobileNo",d.MobileNo),
                new SqlParameter("@Password",d.Password)
            };
            string query = "Insert into tblUser values(@Fullname,@Username,@Password,@Email,@MobileNo)";
            var args = new DynamicParameters { };
            sql.ToList().ForEach(m => args.Add(m.ParameterName, m.Value));
            this._db.Execute(query, args);
            return View();
        }
    }
}