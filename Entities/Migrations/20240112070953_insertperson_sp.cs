using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class insertperson_sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_insertPersons = @"
            CREATE PROCEDURE [dbo].[InsertPerson]
            (@PersonID uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(10), @DateOfBirth datetime2(7), @Gender nvarchar(10), @CountryId uniqueidentifier, @Address nvarchar(30), @ReceiveNewsLetters bit )
            AS BEGIN
                Insert into [dbo].Persons(PersonID,PersonName, Email,DateOfBirth,Gender,CountryId, Address, ReceiveNewsLetters ) values ( @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters )

            END
            ";

            migrationBuilder.Sql(sp_insertPersons);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_insertPersons = @"
            DROP PROCEDURE [dbo].[InsertPerson]
            ";

            migrationBuilder.Sql(sp_insertPersons);
        }

    }
}
