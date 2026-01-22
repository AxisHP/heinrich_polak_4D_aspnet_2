using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserApp.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class RenameFavoritesToFavourites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserPublicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ItemPublicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PublicId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favourites_Items_ItemPublicId",
                        column: x => x.ItemPublicId,
                        principalTable: "Items",
                        principalColumn: "PublicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favourites_Users_UserPublicId",
                        column: x => x.UserPublicId,
                        principalTable: "Users",
                        principalColumn: "PublicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_ItemPublicId",
                table: "Favourites",
                column: "ItemPublicId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_UserPublicId",
                table: "Favourites",
                column: "UserPublicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemPublicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserPublicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PublicId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Items_ItemPublicId",
                        column: x => x.ItemPublicId,
                        principalTable: "Items",
                        principalColumn: "PublicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserPublicId",
                        column: x => x.UserPublicId,
                        principalTable: "Users",
                        principalColumn: "PublicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ItemPublicId",
                table: "Favorites",
                column: "ItemPublicId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserPublicId",
                table: "Favorites",
                column: "UserPublicId");
        }
    }
}
