using System.IO;
using Xspedition.Web.Entities;

namespace Xspedition.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Xspedition.Web.Entities.XspDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Xspedition.Web.Entities.XspDbContext context)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
            context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\seed.sql"));

            //context.CaTimeline.AddOrUpdate(ctv => ctv.CaTimelineId,
            //    new CaTimeline
            //    {
            //        CaId = 1,
            //        ScrubbingTarget = DateTime.Now.AddDays(5),
            //        ScrubbingCritical = DateTime.Now.AddDays(10),
            //        NotificationTarget = DateTime.Now.AddDays(15),
            //        NotificationCritical = DateTime.Now.AddDays(20),
            //        ResponseTarget = DateTime.Now.AddDays(25),
            //        ResponseCritical = DateTime.Now.AddDays(30),
            //        InstructionTarget = DateTime.Now.AddDays(35),
            //        InstructionCritical = DateTime.Now.AddDays(40),
            //        PaymentTarget = DateTime.Now.AddDays(45),
            //        PaymentCritical = DateTime.Now.AddDays(50),
            //    }
            //);
        }
    }
}
