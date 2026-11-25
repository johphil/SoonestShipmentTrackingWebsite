using System.Data.Entity.Migrations;

namespace SoonestShipmentTrackingWebsite.Migrations
{
    public partial class AddRiderManagementModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Riders",
                c => new
                    {
                        RiderId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 256),
                        MobileNumber = c.String(nullable: false, maxLength: 20),
                        LicenseNumber = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RiderId)
                .Index(t => t.Email, unique: true, name: "IX_Rider_Email")
                .Index(t => t.LicenseNumber, unique: true, name: "IX_Rider_LicenseNumber");

            AddColumn("dbo.Shipments", "RiderId", c => c.Int());
            CreateIndex("dbo.Shipments", "RiderId");
            AddForeignKey("dbo.Shipments", "RiderId", "dbo.Riders", "RiderId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "RiderId", "dbo.Riders");
            DropIndex("dbo.Shipments", new[] { "RiderId" });
            DropIndex("dbo.Riders", "IX_Rider_LicenseNumber");
            DropIndex("dbo.Riders", "IX_Rider_Email");
            DropColumn("dbo.Shipments", "RiderId");
            DropTable("dbo.Riders");
        }
    }
}
