using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBook.Api.Data.Migrations{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 75, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: true),
                    mobile = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contacts", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contacts_name_email_mobile",
                table: "contacts",
                columns: new[] { "name", "email", "mobile" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contacts");
        }
    }
}
