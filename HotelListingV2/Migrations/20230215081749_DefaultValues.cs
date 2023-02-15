using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListingV2.Migrations
{
    /// <inheritdoc />
    public partial class DefaultValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Srbija", "Srb" },
                    { 2, "Mauricijus", "Mau" },
                    { 3, "Germany", "Ger" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CountryId", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, 1, "Markov Hotel", 10.0 },
                    { 2, 2, "Marijin Hotel", 9.9000000000000004 },
                    { 3, 3, "Milanov Hotel", 6.0 },
                    { 4, 2, "Milicin Hotle", 6.7999999999999998 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
