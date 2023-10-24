using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using WebTN.Models;

#nullable disable

namespace WebTN.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                });
                             //insert data

            Randomizer.Seed = new Random(8675309);

            var fakerArcticle = new Faker<Article>();

            fakerArcticle.RuleFor(a => a.Title,f => f.Lorem.Sentence(5,5));
            fakerArcticle.RuleFor(a => a.Created,f => f.Date.Between(new DateTime(2023,1,1),new DateTime(2023,12,31)));
            fakerArcticle.RuleFor(a => a.Content,f => f.Lorem.Paragraphs(1,4));

            for(int i =0 ;i <150;i++)
            {
                var article = fakerArcticle.Generate();
                migrationBuilder.InsertData(
                    table: "Articles",
                    columns: new[] {"Title","Created","Content"},
                    values: new object[] {
                        article.Title,
                        article.Created,
                        article.Content,
                    } 
                );
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
