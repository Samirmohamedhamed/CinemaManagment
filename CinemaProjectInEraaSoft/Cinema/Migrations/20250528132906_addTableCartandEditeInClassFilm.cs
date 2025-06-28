using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class addTableCartandEditeInClassFilm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfTicket",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceOfTicket",
                table: "Films",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountOFTicet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.FilmId, x.ApplicationId });
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ApplicationId",
                table: "Carts",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropColumn(
                name: "CountOfTicket",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "PriceOfTicket",
                table: "Films");
        }
    }
}
