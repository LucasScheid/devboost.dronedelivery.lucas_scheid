using Microsoft.EntityFrameworkCore.Migrations;

namespace grupo4.devboost.dronedelivery.Migrations
{
    public partial class capacidades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Perfomance",
                table: "Drone",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<int>(
                name: "CapacidadeRestante",
                table: "Drone",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PerfomanceRestante",
                table: "Drone",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapacidadeRestante",
                table: "Drone");

            migrationBuilder.DropColumn(
                name: "PerfomanceRestante",
                table: "Drone");

            migrationBuilder.AlterColumn<float>(
                name: "Perfomance",
                table: "Drone",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
