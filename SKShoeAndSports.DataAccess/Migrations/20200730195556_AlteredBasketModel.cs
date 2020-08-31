using Microsoft.EntityFrameworkCore.Migrations;

namespace SKShoeAndSports.DataAccess.Migrations
{
    public partial class AlteredBasketModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Products_ProductId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_ProductVariants_ProductVariantId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_ProductId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Baskets");

            migrationBuilder.AlterColumn<int>(
                name: "ProductVariantId",
                table: "Baskets",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_ProductVariants_ProductVariantId",
                table: "Baskets",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_ProductVariants_ProductVariantId",
                table: "Baskets");

            migrationBuilder.AlterColumn<int>(
                name: "ProductVariantId",
                table: "Baskets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Baskets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ProductId",
                table: "Baskets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Products_ProductId",
                table: "Baskets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_ProductVariants_ProductVariantId",
                table: "Baskets",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
