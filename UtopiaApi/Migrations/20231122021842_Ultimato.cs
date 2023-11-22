using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_UTOPIA.Migrations
{
    /// <inheritdoc />
    public partial class Ultimato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "carteira",
                table: "Cliente",
                newName: "Carteira");

            migrationBuilder.AddColumn<int>(
                name: "CarrinhoId",
                table: "Cliente",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Carrinho",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CarrinhoId",
                table: "Cliente",
                column: "CarrinhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Carrinho_CarrinhoId",
                table: "Cliente",
                column: "CarrinhoId",
                principalTable: "Carrinho",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Carrinho_CarrinhoId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_CarrinhoId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "CarrinhoId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Carrinho");

            migrationBuilder.RenameColumn(
                name: "Carteira",
                table: "Cliente",
                newName: "carteira");
        }
    }
}
