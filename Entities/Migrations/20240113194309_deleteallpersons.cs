using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class deleteallpersons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_deleteAllpersons = @"
                CREATE PROCEDURE [dbo].[DELETEALLPERSONS] 
                AS BEGIN
                    DELETE FROM PERSONS;
                END
                        ";
            migrationBuilder.Sql(sp_deleteAllpersons);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_deleteAllpersons = @"
            DROP PROCEDURE [dbo].[DELETEALLPERSONS]
            ";

            migrationBuilder.Sql(sp_deleteAllpersons);
        }
    }
}
