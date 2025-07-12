using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id='0c1eddfe-c656-43b8-beb7-90a901d2b602')
BEGIN
	INSERT AspNetRoles (Id,[Name],[NormalizedName])
	VALUES ('0c1eddfe-c656-43b8-beb7-90a901d2b602','admin','ADMIN');
END
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE AspNetRoles WHERE Id = '0c1eddfe-c656-43b8-beb7-90a901d2b602'");
        }
    }
}
