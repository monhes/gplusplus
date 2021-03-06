USE [GPlus_new]
GO
/****** Object:  StoredProcedure [dbo].[sp_Logfile_Stock_Issue_Select]    Script Date: 28/9/2561 12:15:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Logfile_Stock_Issue_Select]
	
	@Request_No nvarchar(50) =null,
	@MATL_CODE nvarchar(50) =null,
	@status nvarchar(20) =null,
	@Err Char(1)=null,

	@Process_date_Start nvarchar(20)=null,
    @Process_date_End nvarchar(20)=null,
	@Transfer_date_Start nvarchar(20)=null,
    @Transfer_date_End nvarchar(20)=null,

	@Req_date_Start nvarchar(20)=null,
	@Req_date_End nvarchar(20)=null

 AS
 BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @SQL varchar(max)
	DECLARE @cond_where varchar(max)
	DECLARE @where varchar(max)

	SET @where = ' WHERE 1 = 1 '
	
	IF ISNULL(@Req_date_Start , '') <> ''
		BEGIN
			SET @cond_where = ' AND Request_Date >= ''' + @Req_date_Start +''''
			SET @where = @where + @cond_where;
		END
		IF ISNULL(@Req_date_End , '') <> ''
		BEGIN
			SET @cond_where = ' AND Request_Date <= ''' + @Req_date_End +''''
			SET @where = @where + @cond_where;
		END

	IF ISNULL(@Request_No,'') <> ''
		BEGIN
			Set @cond_where = ' AND Request_No = ''' + @Request_No + ''''
			SET @where = @where + @cond_where;
		END

	IF ISNULL(@MATL_CODE,'') <> ''
		BEGIN 
			Set @cond_where = ' AND MATL_CODE = ''' + @MATL_CODE + ''''
			SET @where = @where + @cond_where;
		END

	IF ISNULL(@Err,'') = '1'
		BEGIN
			set @cond_where = ' AND (ISNULL(Request_No_Err , '''') = '''' and
					ISNULL(Request_Date_Err, '''') = '''' and
					ISNULL(Request_type_Err, '''') = '''' and
					ISNULL(Div_code_Err, '''') = '''' and
					ISNULL(Dep_code_Err, '''') = '''' and
					ISNULL(Stock_code_Err, '''') = '''' and
					ISNULL(Request_By_Err, '''') = '''' and
					ISNULL(Request_Name_Err, '''') = '''' and
					ISNULL(MATL_CODE_Err, '''') = '''' and
					ISNULL(PACKAGE_NAME_Err, '''') = '''' and 
					ISNULL(Order_Quantity_Err, '''') = ''''	 and
					ISNULL(Status , '''' ) = '''' ) ' 
			set @where = @where + @cond_where;
		END 
	ELSE IF ISNULL(@Err,'') = '0'
		BEGIN
			set @cond_where = ' AND (ISNULL(Request_No_Err , '''') <> '''' or
					ISNULL(Request_Date_Err, '''') <> '''' or
					ISNULL(Request_type_Err, '''') <> '''' or
					ISNULL(Div_code_Err, '''') <> '''' or
					ISNULL(Dep_code_Err, '''') <> '''' or
					ISNULL(Stock_code_Err, '''') <> '''' or
					ISNULL(Request_By_Err, '''') <> '''' or
					ISNULL(Request_Name_Err, '''') <> '''' or
					ISNULL(MATL_CODE_Err, '''') <> '''' or
					ISNULL(PACKAGE_NAME_Err, '''') <> '''' or 
					ISNULL(Order_Quantity_Err, '''') <> '''' or
					ISNULL(Status , '''') <> '''' ) ' 
			set @where = @where + @cond_where;
		END
	--IF ISNULL(@status,'') <> ''
	--	BEGIN 
	--		Set @cond_where = ' AND status = ''' + @status + ''''
	--		SET @where = @where + @cond_where;
	--	END
	


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
		
		
	 SET @SQL = ' SELECT ROW_NUMBER() OVER(ORDER BY Process_date) AS RowNum
			   , concat(convert(varchar,dateadd(year,543,Transfer_date),103),'' '',convert(varchar,Transfer_date, 8)) AS Transfer_date
			   , concat(convert(varchar,dateadd(year,543,Process_date),103),'' '',convert(varchar,Process_date, 8)) AS Process_date
			   , Request_No
			   , Request_Date
			   , Request_type = Case When Request_type = ''0'' Then ''เบิกตามรอบ'' When Request_type = ''1'' Then ''เบิกด่วน'' When Request_type = ''2'' Then ''เบิกผิดวัน'' END
			   , Div_code
			   , Dep_code
			   , Stock_Code = Case When Stock_Code = ''1'' Then ''คลังเมืองไทย'' When Stock_Code = ''3'' Then ''คลังเชียงใหม่'' Else ''ไม่ระบุ'' END
			   , Request_By
			   , Request_Name
			   , MATL_CODE
			   , PACKAGE_NAME
			   , Order_Quantity
			   , Request_No_Err , Request_Date_Err , Request_type_Err 
			   , Div_code_Err , Dep_code_Err , Stock_code_Err , Request_By_Err 
			   , Request_Name_Err , MATL_CODE_Err , PACKAGE_NAME_Err
			   , Order_Quantity_Err
			   ,Status = case when 
					ISNULL(Request_No_Err , '''') = '''' and
					ISNULL(Request_Date_Err, '''') = '''' and
					ISNULL(Request_type_Err, '''') = '''' and
					ISNULL(Div_code_Err, '''') = '''' and
					ISNULL(Dep_code_Err, '''') = '''' and
					ISNULL(Stock_code_Err, '''') = '''' and
					ISNULL(Request_By_Err, '''') = '''' and
					ISNULL(Request_Name_Err, '''') = '''' and
					ISNULL(MATL_CODE_Err, '''') = '''' and
					ISNULL(PACKAGE_NAME_Err, '''') = '''' and 
					ISNULL(Order_Quantity_Err, '''') = '''' and
					ISNULL(Status , '''' ) = ''''
				then ''สำเร็จ''
				ELSE (ISNULL(Request_No_Err,'''')+'' ''+ISNULL(Request_Date_Err,'''')+'' ''+ ISNULL(Request_type_Err,'''')+'' ''+ ISNULL(Div_code_Err,'''')+'' ''+ ISNULL(Dep_code_Err,'''')+'' ''+ISNULL(Stock_code_Err,'''')+'' ''+ISNULL(Request_By_Err,'''')+'' ''+ISNULL(Request_Name_Err,'''')+'' ''+ISNULL(MATL_CODE_Err,'''')+'' ''+ISNULL(PACKAGE_NAME_Err,'''')+'' ''+ISNULL(Order_Quantity_Err,'''')+'' ''+ISNULL(Status,''''))
				END FROM Logfile_Stock_Issue ' + @where 
	 
	 exec (@SQL);

	
			   
		
END
