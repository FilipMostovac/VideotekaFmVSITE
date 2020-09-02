using Microsoft.EntityFrameworkCore.Migrations;

namespace VideotekaFm.Migrations
{
    public partial class AddYoutubeLinkFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YoutubeLink",
                table: "Filmovi",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YoutubeLink",
                table: "Filmovi");
        }
    }
}
