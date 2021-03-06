USE [GPlus_new]
GO
/****** Object:  StoredProcedure [dbo].[sp_Logfile_OP_Select]    Script Date: 28/9/2561 12:15:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Logfile_OP_Select]
    
	@Process_date_Start nvarchar(20)=null,
    @Process_date_End nvarchar(20)=null,
	@Transfer_date_Start nvarchar(20)=null,
	@Transfer_date_End nvarchar(20)=null,
	@Search_PO_Status VARCHAR(20) = null, 
	@Search_PO_NUM VARCHAR(60) = null,
	@Err Char(1)=null

AS
BEGIN
	

	SET NOCOUNT ON;

	DECLARE @SQL varchar(max)
	DECLARE @cond_where varchar(max)
	DECLARE @where varchar(max)
	SET @where = ' WHERE 1 = 1 '
	
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


	IF ISNULL(@Err ,'') = '0'
		BEGIN
			set @cond_where = ' AND (ISNULL(PO_DATE_Err , '''') <> '''' or  
			ISNULL(PO_Type_Err , '''') <> ''''  or 
			ISNULL(Div_Code_Err , '''') <> '''' or  
			ISNULL(Dep_Code_ERR , '''') <> ''''  or
			ISNULL(SUPPLIER_CODE_ERR , '''') <> '''' or  
			ISNULL(Payment_type_Err , '''') <> ''''  or
			ISNULL(CreditTerm_Err , '''') <> '''' or  
			ISNULL(MATL_CODE_Err , '''') <> ''''  or
			ISNULL(PACKAGE_NAME_Err , '''') <> '''' or  
			ISNULL(Unit_Price_Err , '''') <> '''' or
			ISNULL(Order_Qty_Err , '''') <> '''' or  
			ISNULL(TradeDiscount_Percent_Err , '''') <> '''' or
			ISNULL(TradeDiscount_Amount_Err , '''') <> '''' or  
			ISNULL(Total_before_Vat_Err , '''') <> ''''  or
			ISNULL(Vat_Err , '''') <> ''''  or
			ISNULL(Vat_Amount_Err , '''') <> ''''  or
			ISNULL(Net_Amount_Err , '''') <> ''''  or
			ISNULL(status , '''')<> '''' ) '
			set @where = @where + @cond_where;
		END
	ELSE IF ISNULL(@Err ,'') = '1'
		BEGIN
			set @cond_where = ' AND (ISNULL(PO_DATE_Err , '''') = '''' and  
			ISNULL(PO_Type_Err , '''') = ''''  and 
			ISNULL(Div_Code_Err , '''') = '''' and  
			ISNULL(Dep_Code_ERR , '''') = ''''  and
			ISNULL(SUPPLIER_CODE_ERR , '''') = '''' and  
			ISNULL(Payment_type_Err , '''') = ''''  and
			ISNULL(CreditTerm_Err , '''') = '''' and  
			ISNULL(MATL_CODE_Err , '''') = ''''  and
			ISNULL(PACKAGE_NAME_Err , '''') = '''' and  
			ISNULL(Unit_Price_Err , '''') = '''' and
			ISNULL(Order_Qty_Err , '''') = '''' and  
			ISNULL(TradeDiscount_Percent_Err , '''') = '''' and
			ISNULL(TradeDiscount_Amount_Err , '''') = '''' and  
			ISNULL(Total_before_Vat_Err , '''') = ''''  and
			ISNULL(Vat_Err , '''') = ''''  and
			ISNULL(Vat_Amount_Err , '''') = ''''  and
			ISNULL(Net_Amount_Err , '''') = ''''  and
			ISNULL(status , '''') = '''' ) '
			set @where = @where + @cond_where;
		END


		
	IF ISNULL(@Search_PO_NUM,'') <> ''
		BEGIN
			Set @cond_where = ' AND PO_NUM = ''' + @Search_PO_NUM + ''''
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

SET @SQL = 'SELECT ROW_NUMBER() OVER(ORDER BY Transfer_date) AS RowNum,
	concat(convert(varchar,dateadd(year,543,Transfer_date),103),'' '',convert(varchar,Transfer_date, 8)) AS Transfer_date ,
	concat(convert(varchar,dateadd(year,543,Transfer_date),103),'' '',convert(varchar,Transfer_date, 8)) AS Process_date,
	PO_NUM,
	case when PO_Type =''1'' then ''สั่งซื้อ'' when PO_Type = ''2'' then ''สั่งจ้าง'' else '''' end AS PO_Type,
	Div_Code,
	Dep_Code,
	SUPPLIER_CODE,
	MATL_CODE,
	convert(numeric,Order_Qty) AS Order_Qty,
	PACKAGE_NAME,
	Unit_Price,
	TradeDiscount_Percent,
	cast(TradeDiscount_Amount as decimal(10,2)) AS TradeDiscount_Amount,
	cast(Total_before_Vat as decimal(10,2)) AS Total_before_Vat,
	Vat,
	cast(Vat_Amount as decimal(10,2)) AS Vat_Amount,
	cast(Net_Amount as decimal(10,2)) AS Net_Amount,
	 PO_DATE_Err, 
	PO_Type_Err, 
	Div_Code_Err ,
	Dep_Code_ERR ,
	SUPPLIER_CODE_ERR ,
	Payment_type_Err ,
	CreditTerm_Err ,
	MATL_CODE_Err ,
	PACKAGE_NAME_Err ,
	Unit_Price_Err ,
	Order_Qty_Err ,
	TradeDiscount_Percent_Err ,
	TradeDiscount_Amount_Err ,
	Total_before_Vat_Err ,
	Vat_Err ,
	Vat_Amount_Err ,
	Net_Amount_Err,
	status = CASE when
			ISNULL(PO_DATE_Err , '''') = '''' and  
			ISNULL(PO_Type_Err , '''') = ''''  and 
			ISNULL(Div_Code_Err , '''') = '''' and  
			ISNULL(Dep_Code_ERR , '''') = ''''  and
			ISNULL(SUPPLIER_CODE_ERR , '''') = '''' and  
			ISNULL(Payment_type_Err , '''') = ''''  and
			ISNULL(CreditTerm_Err , '''') = '''' and  
			ISNULL(MATL_CODE_Err , '''') = ''''  and
			ISNULL(PACKAGE_NAME_Err , '''') = '''' and  
			ISNULL(Unit_Price_Err , '''') = '''' and
			ISNULL(Order_Qty_Err , '''') = '''' and  
			ISNULL(TradeDiscount_Percent_Err , '''') = '''' and
			ISNULL(TradeDiscount_Amount_Err , '''') = '''' and  
			ISNULL(Total_before_Vat_Err , '''') = ''''  and
			ISNULL(Vat_Err , '''') = ''''  and
			ISNULL(Vat_Amount_Err , '''') = ''''  and
			ISNULL(Net_Amount_Err , '''') = '''' and 
			ISNULL(status , '''') = ''''
	THEN ''เพิ่มสำเร็จ''
	ELSE (ISNULL(PO_DATE_Err , '''')+'' ''+ISNULL(PO_Type_Err , '''')+'' ''+ISNULL(Div_Code_Err , '''')+'' ''+ISNULL(Dep_Code_ERR , '''')+'' ''
		 +ISNULL(SUPPLIER_CODE_ERR , '''')+'' ''+ISNULL(Payment_type_Err , '''')+'' ''+ISNULL(CreditTerm_Err , '''')+'' ''
		 +ISNULL(MATL_CODE_Err , '''')+'' ''+ISNULL(PACKAGE_NAME_Err , '''')+'' ''+ISNULL(Unit_Price_Err , '''')+'' ''
		 + ISNULL(Order_Qty_Err , '''')+'' ''+ISNULL(TradeDiscount_Percent_Err , '''')+'' ''+ISNULL(TradeDiscount_Amount_Err , '''')+'' ''
		 + ISNULL(Total_before_Vat_Err , '''')+'' ''+ ISNULL(Vat_Err , '''')+'' ''+ISNULL(Vat_Amount_Err , '''')+'' ''+ ISNULL(Net_Amount_Err , '''')+'' ''
		 +ISNULL(status , ''''))
	END
	FROM Logfile_PO ' + @where
exec (@SQL);
		
END
