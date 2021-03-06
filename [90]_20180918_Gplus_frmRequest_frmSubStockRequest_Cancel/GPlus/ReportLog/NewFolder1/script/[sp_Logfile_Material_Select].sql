USE [GPlus_new]
GO
/****** Object:  StoredProcedure [dbo].[sp_Logfile_Material_Select]    Script Date: 28/9/2561 12:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Logfile_Material_Select]
	@Process_date_Start nvarchar(20)=null,
    @Process_date_End nvarchar(20)=null,
	@Transfer_date_Start nvarchar(20)=null,
    @Transfer_date_End nvarchar(20)=null,
    @PO_CODE nvarchar(20)=null,
    @EP_CODE nvarchar(20)=null,
	@MALT nvarchar(20)=null,
    @Err Char(1)=null
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @SQL varchar(max)
	DECLARE @cond_where varchar(max)
	DECLARE @where varchar(max)

	SET @where = ' WHERE 1 = 1'
		
	--IF ISNULL(@SUPPLIER_CODE ,'') <> ''
	--	BEGIN
	--		Set @cond_where = ' AND SUPPLIER_CODE = ' +  @SUPPLIER_CODE
	--		SET @where = @where + @cond_where;
	--	END		
	IF ISNULL(@MALT,'') <> ''
		BEGIN
			SET @cond_where = ' AND MATL_CODE =  ''' + @MALT + ''''
			SET @where = @where + @cond_where;
		END
	
	IF ISNULL(@Err ,'') = '0'
		BEGIN
			Set @cond_where = ' AND (ISNULL(Cate_Code_Err , '''') <> '''' OR ISNULL(Type_code_Err , '''') <> '''' OR ISNULL(SubCate_Code_Err , '''') <> '''' OR ISNULL(Status , '''') <> '''') '
			SET @where = @where + @cond_where;
		END
	ELSE IF ISNULL(@Err ,'') = '1'
		BEGIN
			Set @cond_where = ' AND (ISNULL(Cate_Code_Err , '''') = '''' AND ISNULL(Type_code_Err , '''') = '''' AND ISNULL(SubCate_Code_Err , '''') = '''' AND ISNULL(Status , '''') = '''' ) '
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
		
		
	 SET @SQL = '	SELECT ROW_NUMBER() OVER(ORDER BY Process_date) AS RowNum, Process_date , Transfer_date ,
					MATL_CODE , MATL_NAME , a.Cate_Code  , 
					Cate_name  = (Select  Cat_Name from Inv_Category where Cate_Code =  a.Cate_Code ) , 
					Type_code  , Type_Name = ( Select TYPE_NAME from Inv_Type where Cate_ID = a.Cate_Code 
					and Type_Code = a.Type_code), SubCate_Code , 
					SubCate_Name = (Select SubCate_Name from Inv_SubCate where Cate_ID = (Select Cate_ID from Inv_Category where Cate_Code =  a.Cate_Code)  
					and SubCate_Code = a.SubCate_Code), Attribute , BaseUnit_Pack_Name,
					Status =  Case When Status = ''C'' Then ''ยกเลิก'' Else '''' END,
					Cate_Code_Err,
					Type_code_Err,
					SubCate_Code_Err,
					progress =	Case
								When ISNULL(Cate_Code_Err , '''') = '''' and  ISNULL(Type_code_Err , '''') = '''' and  ISNULL(SubCate_Code_Err , '''') = '''' AND ISNULL(Status , '''') = '''' 
									Then ''เพิ่มสำเร็จ''
								Else (ISNULL(Cate_Code_Err , '''')+'' ''+ISNULL(Type_code_Err , '''')+'' ''+ISNULL(SubCate_Code_Err , '''')+'' ''+ISNULL(Status , ''''))
								END 
					FROM Logfile_material as a ' + @where 
	 
	 exec (@SQL);
	 --SELECT @SQL
    
END
