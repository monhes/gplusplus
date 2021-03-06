USE [GPlus_new]
GO
/****** Object:  StoredProcedure [dbo].[sp_Logfile_SUPPLIER_Select]    Script Date: 28/9/2561 12:15:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Logfile_SUPPLIER_Select]
	@Process_date_Start nvarchar(20)=null,
    @Process_date_End nvarchar(20)=null,
	@Transfer_date_Start nvarchar(20)=null,
    @Transfer_date_End nvarchar(20)=null,
    @SUPPLIER_CODE nvarchar(20)=null,
    @Err Char(1)=null
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @SQL varchar(max)
	DECLARE @cond_where varchar(max)
	DECLARE @where varchar(max)

	SET @where = ' WHERE 1 = 1'
	
	IF ISNULL(@SUPPLIER_CODE ,'') <> ''
		BEGIN
			Set @cond_where = ' AND SUPPLIER_CODE = ''' +  @SUPPLIER_CODE + ''''
			SET @where = @where + @cond_where;
		END
		
		
	IF ISNULL(@Err ,'') = '0'
		BEGIN
			Set @cond_where = ' AND (ISNULL(SUPPLIER_CODE_Err , '''') <> '''' OR ISNULL(SUPPLIER_NAME_Err , '''') <> '''' OR ISNULL(Status , '''') <> '''' ) '
			SET @where = @where + @cond_where;
		END
	ELSE IF ISNULL(@Err ,'') = '1'
		BEGIN
			Set @cond_where = ' AND (ISNULL(SUPPLIER_CODE_Err , '''') = '''' AND ISNULL(SUPPLIER_NAME_Err , '''') = '''' AND ISNULL(Status , '''') = '''' ) '
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
		
		
	 SET @SQL = 'SELECT ROW_NUMBER() OVER(ORDER BY Process_date) AS RowNum, Process_date , Transfer_date ,
SUPPLIER_CODE, SUPPLIER_NAME, IncludeVat_Flag,SUPPLIER_CODE_Err,SUPPLIER_NAME_Err,
Status =  Case When Status = ''C'' Then ''ยกเลิก'' Else '''' END, 
Supplier_Err = Case
	When ISNULL(SUPPLIER_CODE_Err , '''') = '''' and  ISNULL(SUPPLIER_NAME_Err , '''') = '''' AND ISNULL(Status , '''') = '''' Then ''เพิ่มสำเร็จ''
	Else  (ISNULL(SUPPLIER_CODE_Err,'''')+''''+ ISNULL(SUPPLIER_NAME_Err,'''') + '''' + ISNULL(Status,''''))  
	END 
	
FROM Logfile_Supplier  ' + @where 
	 
	 exec (@SQL);
	 --SELECT @SQL
    
END
