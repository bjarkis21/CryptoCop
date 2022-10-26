using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptocop.Software.API.Migrations
{
    public partial class fixPaymentCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCard_Users_UserId",
                table: "PaymentCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentCard",
                table: "PaymentCard");

            migrationBuilder.RenameTable(
                name: "PaymentCard",
                newName: "PaymentCards");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentCard_UserId",
                table: "PaymentCards",
                newName: "IX_PaymentCards_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCards_Users_UserId",
                table: "PaymentCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCards_Users_UserId",
                table: "PaymentCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards");

            migrationBuilder.RenameTable(
                name: "PaymentCards",
                newName: "PaymentCard");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentCards_UserId",
                table: "PaymentCard",
                newName: "IX_PaymentCard_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentCard",
                table: "PaymentCard",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCard_Users_UserId",
                table: "PaymentCard",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
