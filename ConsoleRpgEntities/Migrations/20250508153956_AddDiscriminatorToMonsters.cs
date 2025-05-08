using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class AddDiscriminatorToMonsters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAbilities_Abilities_AbilitiesId",
                table: "PlayerAbilities");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAbilities_Players_PlayersId",
                table: "PlayerAbilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerAbilities",
                table: "PlayerAbilities");

            migrationBuilder.DropColumn(
                name: "Damage",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Abilities");

            migrationBuilder.RenameTable(
                name: "PlayerAbilities",
                newName: "AbilityPlayer");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerAbilities_PlayersId",
                table: "AbilityPlayer",
                newName: "IX_AbilityPlayer_PlayersId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Monsters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Items",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Abilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbilityPlayer",
                table: "AbilityPlayer",
                columns: new[] { "AbilitiesId", "PlayersId" });

            migrationBuilder.CreateIndex(
                name: "IX_Items_PlayerId",
                table: "Items",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbilityPlayer_Abilities_AbilitiesId",
                table: "AbilityPlayer",
                column: "AbilitiesId",
                principalTable: "Abilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbilityPlayer_Players_PlayersId",
                table: "AbilityPlayer",
                column: "PlayersId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Players_PlayerId",
                table: "Items",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbilityPlayer_Abilities_AbilitiesId",
                table: "AbilityPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_AbilityPlayer_Players_PlayersId",
                table: "AbilityPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Players_PlayerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_PlayerId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbilityPlayer",
                table: "AbilityPlayer");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Monsters");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Abilities");

            migrationBuilder.RenameTable(
                name: "AbilityPlayer",
                newName: "PlayerAbilities");

            migrationBuilder.RenameIndex(
                name: "IX_AbilityPlayer_PlayersId",
                table: "PlayerAbilities",
                newName: "IX_PlayerAbilities_PlayersId");

            migrationBuilder.AddColumn<int>(
                name: "Damage",
                table: "Abilities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "Abilities",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerAbilities",
                table: "PlayerAbilities",
                columns: new[] { "AbilitiesId", "PlayersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAbilities_Abilities_AbilitiesId",
                table: "PlayerAbilities",
                column: "AbilitiesId",
                principalTable: "Abilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAbilities_Players_PlayersId",
                table: "PlayerAbilities",
                column: "PlayersId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
