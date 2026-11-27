using QuestPDF.Drawing;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SoonestShipmentTrackingWebsite
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Database + tables are created automatically the first time
            // they're touched, and Admin/Customer roles + a default admin
            // account are seeded (see Migrations/Configuration.cs).
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());

            //string fontPath = HttpContext.Current.Server.MapPath("~/Assets/fonts/");

            //using (var arial = File.OpenRead(Path.Combine(fontPath, "Arial.ttf")))
            //{
            //    FontManager.RegisterFontFromStream(arial);
            //}

            //using (var lato = File.OpenRead(Path.Combine(fontPath, "Lato-Regular.ttf")))
            //{
            //    FontManager.RegisterFontFromStream(lato);
            //}
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}