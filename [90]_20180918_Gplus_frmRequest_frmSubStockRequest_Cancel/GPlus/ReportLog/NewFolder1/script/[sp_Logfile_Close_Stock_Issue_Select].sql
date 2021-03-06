USE [GPlus_new]
GO
/****** Object:  StoredProcedure [dbo].[sp_Logfile_Close_Stock_Issue_Select]    Script Date: 28/9/2561 12:19:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Logfile_Close_Stock_Issue_Select]
		@Process_date_Start nvarchar(30)=null,
		@Process_date_End nvarchar(30)=null,
		@Transfer_date_Start nvarchar(30)=null,
		@Transfer_date_End nvarchar(30)=null,
		@Request_No VARCHAR(100) = null,
		@Request_No_Err varchar(100) = null,
		@MATL_CODE nvarchar(20)=null,
		@MATL_CODE_Err varchar(100) = null,
		@status varchar(100) = null,
		@Err char(1) = nul


AS
BEGIN
SET NOCOUNT ON;
	
	DECLARE @SQL varchar(max)
	DECLARE @cond_where varchar(max)
	DECLARE @where varchar(max)

	SET @where = ' WHERE 1 = 1'

	--IF ISNULL(@MATL_CODE, '') = ''
	--		BEGIN
	--			SET @MATL_CODE_Err = 'ไม่ระบุข้อมูล'
	--			SET @status = 'dd'
	--		END
	--IF ISNULL(@Request_No, '') = ''
	--		BEGIN
	--			SET @Request_No_Err = 'ไม่ระบุข้อมูล' ;
	--		END 
	--IF ISNULL(@Request_No,)
	IF ISNULL(@Request_No ,'') <> ''
		BEGIN
			Set @cond_where = ' AND Request_No = ''' + @Request_No + ''''
			SET @where = @where + @cond_where;
		END
	IF ISNULL(@MATL_CODE ,'') <> ''
		BEGIN
			Set @cond_where = ' AND MATL_CODE = ''' + @MATL_CODE + ''''
			SET @where = @where + @cond_where;
		END

	IF ISNULL(@Err ,'') = '0'
		BEGIN
			set @cond_where = ' AND (ISNULL(Request_No_Err , '''') <> '''' or 
			ISNULL(MATL_CODE_Err , '''') <> ''''  or 
			ISNULL(PACKAGE_NAME_Err , '''') <> '''' or  
			ISNULL(Return_Qty_Err , '''') <> ''''  or 
			ISNULL(status , '''') <> '''' ) '
			SET @where = @where + @cond_where;
		END
	IF ISNULL(@Err ,'') = '1'
		BEGIN
			set @cond_where = ' AND (ISNULL(Request_No_Err , '''') = '''' and 
			ISNULL(MATL_CODE_Err , '''') = ''''  and 
			ISNULL(PACKAGE_NAME_Err , '''') = '''' and  
			ISNULL(Return_Qty_Err , '''') = ''''  and 
			ISNULL(status , '''') = '''' ) '
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



	SET @SQL = 'SELECT 
				concat(convert(varchar,dateadd(year,543,Process_date),103),'' '',convert(varchar,Process_date, 8)) AS Process_date,
				concat(convert(varchar,dateadd(year,543,Transfer_date),103),'' '',convert(varchar,Transfer_date, 8)) AS Transfer_date,
				Filename_transfer,
				RowNum,
				Request_No,
				Request_No_Err,
				MATL_CODE,
				MATL_CODE_Err,
				PACKAGE_NAME,
				PACKAGE_NAME_Err,
				Return_Qty,
				Return_Qty_Err,
				status = case 
							when
							ISNULL(Request_No_Err , '''') = '''' and 
							ISNULL(MATL_CODE_Err , '''') = ''''  and 
							ISNULL(PACKAGE_NAME_Err , '''') = '''' and  
							ISNULL(Return_Qty_Err , '''') = ''''  and 
							ISNULL(status , '''') = ''''

						then ''สำเร็จ''
						else (ISNULL(Request_No_Err , '''')+'' ''+ISNULL(MATL_CODE_Err , '''')+'' ''+ISNULL(PACKAGE_NAME_Err , '''')+'' ''+ISNULL(Return_Qty_Err , '''')+'' ''+ISNULL(status , ''''))
						END FROM Logfile_Close_IS  ' + @where 
	 
	 exec (@SQL);
END

