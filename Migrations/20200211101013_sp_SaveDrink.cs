using Microsoft.EntityFrameworkCore.Migrations;

namespace DrinkAndGo.Migrations
{
    public partial class sp_SaveDrink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var insert_sp = @"CREATE PROCEDURE sp_SaveDrink

	        @Name nvarchar(max),
            @ShortDescription nvarchar(max),
            @LongDescription nvarchar(max),
            @Price decimal(18,2),
            @ImageUrl nvarchar(max),
            @ImageThumbnailUrl nvarchar(max),
            @IsPreferredDrink bit,
            @InStock bit,
            @CategoryId int
AS
BEGIN

	SET NOCOUNT ON;

  
	INSERT INTO [dbo].[Drinks]
           ([Name]
           ,[ShortDescription]
           ,[LongDescription]
           ,[Price]
           ,[ImageUrl]
           ,[ImageThumbnailUrl]
           ,[IsPreferredDrink]
           ,[InStock]
           ,[CategoryId])
     VALUES
           (@Name,
            @ShortDescription,
            @LongDescription,
            @Price,
            @ImageUrl,
            @ImageThumbnailUrl,
            @IsPreferredDrink,
            @InStock,
            @CategoryId)
END";

            migrationBuilder.Sql(insert_sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
