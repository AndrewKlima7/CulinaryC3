﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaryC3.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameofGroup = table.Column<string>(maxLength: 50, nullable: true),
                    InviteeId = table.Column<int>(nullable: true),
                    InviterEmail = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<string>(maxLength: 450, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Score = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Picture = table.Column<string>(maxLength: 100, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Favorite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Favorite__UserId__6477ECF3",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    FriendId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.id);
                    table.ForeignKey(
                        name: "FK__Friends__UserId__6A30C649",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Admin = table.Column<bool>(nullable: true),
                    Score = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK__Group__UserId__5EBF139D",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    Score = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RecipeName = table.Column<string>(maxLength: 50, nullable: true),
                    Servings = table.Column<int>(nullable: true),
                    Picture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Recipes__UserId__619B8048",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(nullable: true),
                    Item = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<double>(nullable: true),
                    Unit = table.Column<string>(maxLength: 10, nullable: true),
                    Calories = table.Column<double>(nullable: true),
                    Carbs = table.Column<double>(nullable: true),
                    Protein = table.Column<double>(nullable: true),
                    Fats = table.Column<double>(nullable: true),
                    Aisle = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Ingredien__Recip__6754599E",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_UserId",
                table: "Favorite",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_UserId",
                table: "Group",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
