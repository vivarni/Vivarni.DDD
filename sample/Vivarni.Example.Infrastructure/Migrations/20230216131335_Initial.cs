using Microsoft.EntityFrameworkCore.Migrations;

namespace Vivarni.Example.Infrastructure.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "GuestMessages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GuestMessages", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GuestMessages");
    }
}
