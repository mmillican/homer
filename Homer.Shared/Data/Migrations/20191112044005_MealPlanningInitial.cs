using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Homer.Shared.Data.Migrations
{
    public partial class MealPlanningInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    PrepEffort = table.Column<int>(nullable: false),
                    IsFavorite = table.Column<bool>(nullable: false),
                    IsKidFriendly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledMeals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealDate = table.Column<DateTime>(nullable: false),
                    MealTime = table.Column<int>(nullable: false),
                    MealId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledMeals_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledMeals_MealId",
                table: "ScheduledMeals",
                column: "MealId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledMeals");

            migrationBuilder.DropTable(
                name: "Meals");
        }
    }
}
