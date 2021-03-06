USE [GPlus_new]
GO
/****** Object:  StoredProcedure [dbo].[sp_Logfile_Conversion_Select]    Script Date: 28/9/2561 12:15:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Logfile_Conversion_Select]
	@Process_date_Start nvarchar(20)=null,
    @Process_date_End nvarchar(20)=null,
	@Transfer_date_Start nvarchar(20)=null,
    @Transfer_date_End nvarchar(20)=null,
    @Materia_CODE nvarchar(20)=null,
    @Err Char(1)=null
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @SQL varchar(max)
	DECLARE @cond_where varchar(max)
	DECLARE @where varchar(max)

	SET @where = ' WHERE 1 = 1'
			
	IF ISNULL(@Materia_CODE ,'') <> ''
		BEGIN
			Set @cond_where = ' AND MATL_CODE = ''' +  @Materia_CODE + ''''
			SET @where = @where + @cond_where;
		END
	
	
	IF ISNULL(@Err ,'') = '0'
		BEGIN
			Set @cond_where = ' AND (ISNULL(MATL_CODE_err , '''') <> '''' OR ISNULL(PackageName_err , '''') <> '''' OR ISNULL(Pack_Content_err , '''') <> '''' OR ISNULL(PackName_Base_err , '''') <> '''' OR ISNULL(Status , '''') <> '''' ) '
			SET @where = @where + @cond_where;
		END
	ELSE IF ISNULL(@Err ,'') = '1'
		BEGIN
			Set @cond_where = ' AND (ISNULL(MATL_CODE_err , '''') = '''' AND ISNULL(PackageName_err , '''') = '''' AND ISNULL(Pack_Content_err , '''') = '''' AND ISNULL(PackName_Base_err , '''') = '''' AND ISNULL(Status , '''') = '''' ) '
			SET @where = @where + @cond_where;
		END
		
	IF ISNULL(@Process_date_Start ,'') <> ''
		BEGIN
			Set @cond_where = ' AND Process_date >= ''' + @Process_date_Start + ''''
			SET @where = @where + @cond_where;
		END
		
	IF ISNULL(@Process_date_End ,'') <> ''
		BEGIN
			Set @cond_where = ' AND Process_date <= ''' +  @Process_date_End + ' 23:59:59' + ''''
			SET @where = @where + @cond_where;
		END
		
	IF ISNULL(@Transfer_date_Start ,'') <> ''
		BEGIN
			Set @cond_where = ' AND Transfer_date >= ''' + @Transfer_date_Start  + ''''
			SET @where = @where + @cond_where;
		END
		
	IF ISNULL(@Transfer_date_End ,'') <> ''
		BEGIN
			Set @cond_where = ' AND Transfer_date <= ''' +  @Transfer_date_End + ' 23:59:59' + ''''
			SET @where = @where + @cond_where;
		END
		
		
	 SET @SQL = ' SELECT ROW_NUMBER() OVER(ORDER BY Process_date) AS RowNum, Process_date , Transfer_date ,
					MATL_CODE, ItemName  = (Select Inv_ItemName from Inv_Item where Inv_ItemCode = MATL_CODE),  
					PackageName , Pack_Content  , PackName_Base, MATL_CODE_err ,  PackageName_err, Pack_Content_err, PackName_Base_err ,
					Status =  Case When Status = ''C'' Then ''ยกเลิก'' Else '''' END,
					Supplier_Err = Case
						When ISNULL(MATL_CODE_err , '''') = '''' and  ISNULL(PackageName_err , '''') = '''' 
							and  ISNULL(Pack_Content_err , '''') = '''' and  ISNULL(PackName_Base_err , '''') = '''' and ISNULL(Status , '''') = ''''
						Then ''เพิ่มสำเร็จ''
						Else (ISNULL(MATL_CODE_err , '''')+'' ''+ISNULL(PackageName_err , '''')+'' ''+ISNULL(Pack_Content_err , '''')+'' ''+ISNULL(PackName_Base_err , '''')+'' ''+ISNULL(Status , ''''))
						END 
					from Logfile_Conversion ' 
	 + @where 
	 
	 exec (@SQL);
	 --SELECT @SQL
    
END
