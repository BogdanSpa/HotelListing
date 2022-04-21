using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.Migrations
{
    public partial class addedShortNameToCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7566e911-93b4-4f32-b2e4-c54392fb20f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c88aeb0-aaa5-4a4c-9da8-40985b711c75");

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5b907c96-ebd2-4974-9b27-a3ddf549fee4", "522aaff1-10e3-4cfa-ad8a-646115a4dfa1", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81d3dec5-75d2-48a5-8dfe-752d86a7cb42", "5a4acb16-c15d-4f18-891d-6e4213460638", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b907c96-ebd2-4974-9b27-a3ddf549fee4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81d3dec5-75d2-48a5-8dfe-752d86a7cb42");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Countries");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7566e911-93b4-4f32-b2e4-c54392fb20f5", "155d441a-2016-4f38-a434-f53be05bf330", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9c88aeb0-aaa5-4a4c-9da8-40985b711c75", "298c0da7-d1bc-4657-a9cf-57c31c7f4341", "User", "USER" });
        }
    }
}
