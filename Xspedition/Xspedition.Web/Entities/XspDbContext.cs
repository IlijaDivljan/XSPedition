using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Xspedition.Web.Entities.Registries;
using Xspedition.Web.Entities.Registries.Maps;
using Xspedition.Web.Entities.System;

namespace Xspedition.Web.Entities
{
    public class XspDbContext: DbContext
    {

        #region ENTITIES

        public DbSet<CaTimeline> CaTimeline { get; set; }
        public DbSet<ScrubbingInfo> ScrubbingInfo { get; set; }
        public DbSet<NotificationInfo> NotificationsInfo { get; set; }
        public DbSet<ResponseInfo> ResponsesInfo { get; set; }
        public DbSet<InstructionInfo> InstructionsInfo { get; set; }
        public DbSet<PaymentInfo> PaymentsInfo { get; set; }

        #endregion

        #region REGISTRIES

        public DbSet<CaTypeRegistry> CaTypeRegistry { get; set; }
        public DbSet<FieldRegistry> FieldRegistry { get; set; }
        public DbSet<OptionTypeRegistry> OptionTypeRegistry { get; set; }
        public DbSet<PayoutTypeRegistry> PayoutTypeRegistry { get; set; }

        public DbSet<CaTypeFieldMap> CaTypeFieldMap { get; set; }
        public DbSet<OptionTypeFieldMap> OptionTypeFieldMap { get; set; }
        public DbSet<PayoutTypeFieldMap> PayoutTypeFieldMap { get; set; }

        #endregion REGISTRIES

        #region SYSTEM

        public DbSet<CaTypeDateConfiguration> DatesConfigurations { get; set; }

        #endregion SYSTEM

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}