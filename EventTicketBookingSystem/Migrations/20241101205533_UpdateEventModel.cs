using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicketBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventOrganizers_OrganizerId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "EventOrganizerOrganizerId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventOrganizerOrganizerId",
                table: "Events",
                column: "EventOrganizerOrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventOrganizers_EventOrganizerOrganizerId",
                table: "Events",
                column: "EventOrganizerOrganizerId",
                principalTable: "EventOrganizers",
                principalColumn: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventOrganizers_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "EventOrganizers",
                principalColumn: "OrganizerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventOrganizers_EventOrganizerOrganizerId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventOrganizers_OrganizerId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventOrganizerOrganizerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventOrganizerOrganizerId",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventOrganizers_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "EventOrganizers",
                principalColumn: "OrganizerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
