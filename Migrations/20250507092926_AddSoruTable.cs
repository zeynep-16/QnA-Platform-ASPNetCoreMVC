using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdevProjesi.Migrations
{
    /// <inheritdoc />
    public partial class AddSoruTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KullaniciAdi",
                table: "Sorular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KullaniciAdi",
                table: "Sorular");
        }
    }
}
