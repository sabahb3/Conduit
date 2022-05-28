using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class PopularTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION PopularTags() 
                                    RETURNS TABLE  
                                    AS  
                                    RETURN  
                                        SELECT  TOP 4 Tag
                                        FROM ArticlesTags
                                        GROUP BY Tag
                                        ORDER BY COUNT(Tag) DESC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS dbo.PopularTags;");
        }
    }
}
