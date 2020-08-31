using Microsoft.EntityFrameworkCore.Migrations;

namespace SKShoeAndSports.DataAccess.Migrations
{
    public partial class AlteredProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "isDiscount",
                table: "Products",
                newName: "IsDiscount");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sizes",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDiscount",
                table: "Products",
                newName: "isDiscount");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sizes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
