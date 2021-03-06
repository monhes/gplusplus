USE [GPlus_new]
GO
/****** Object:  StoredProcedure [dbo].[sp_Logfile_GoodReceiving_Select]    Script Date: 28/9/2561 12:15:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Logfile_GoodReceiving_Select]
	@Process_date_Start nvarchar(20)=null,
    @Process_date_End nvarchar(20)=null,
	@Transfer_date_Start nvarchar(20)=null,
    @Transfer_date_End nvarchar(20)=null,
    @PO_CODE nvarchar(20)=null,
    @EP_CODE nvarchar(20)=null,
    @Err Char(1)=null
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @SQL varchar(max)
	DECLARE @cond_where varchar(max)
	DECLARE @where varchar(max)

	SET @where = ' WHERE 1 = 1'
	
	IF ISNULL(@PO_CODE ,'') <> ''
		BEGIN
			Set @cond_where = ' AND PO_NUM = ''' +  @PO_CODE + ''''
			SET @where = @where + @cond_where;
		END
		
	IF ISNULL(@EP_CODE ,'') <> ''
		BEGIN
			Set @cond_where = ' AND Reference_EP = ''' +  @EP_CODE + ''''
			SET @where = @where + @cond_where;
		END
		
		
	IF ISNULL(@Err ,'') = '0'
		BEGIN
			Set @cond_where = ' AND (
			ISNULL(PO_NUM_Err , '''') <> '''' or  ISNULL(MATL_CODE_Err , '''') <> ''''  or 
			ISNULL(PACKAGE_NAME_Err , '''') <> '''' or  ISNULL(Unit_Price_Err , '''') <> ''''  or
			ISNULL(Receive_Qty_err , '''') <> '''' or  ISNULL(TradeDiscount_Price_err , '''') <> ''''  or
			ISNULL(TradeDiscount_Percent_err , '''') <> '''' or  ISNULL(CashDiscount_Percent_err , '''') <> ''''  or
			ISNULL(CashDiscount_Price_err , '''') <> '''' or  ISNULL(Total_before_Vat_err , '''') <> '''' or
			ISNULL(Vat_err , '''') <> '''' or  ISNULL(Vat_Amount_err , '''') <> '''' or
			ISNULL(Net_Amount_err , '''') <> '''' or  ISNULL(GiveAway_QTY_err , '''') <> '''' or 
			ISNULL(status , '''') <> ''''
			) '
			SET @where = @where + @cond_where;
		END
	ELSE IF ISNULL(@Err ,'') = '1'
		BEGIN
			Set @cond_where = ' AND (
			ISNULL(PO_NUM_Err , '''') = '''' and  ISNULL(MATL_CODE_Err , '''') = ''''  and 
			ISNULL(PACKAGE_NAME_Err , '''') = '''' and  ISNULL(Unit_Price_Err , '''') = ''''  and
			ISNULL(Receive_Qty_err , '''') = '''' and  ISNULL(TradeDiscount_Price_err , '''') = ''''  and
			ISNULL(TradeDiscount_Percent_err , '''') = '''' and  ISNULL(CashDiscount_Percent_err , '''') = ''''  and
			ISNULL(CashDiscount_Price_err , '''') = '''' and  ISNULL(Total_before_Vat_err , '''') = '''' and
			ISNULL(Vat_err , '''') = '''' and  ISNULL(Vat_Amount_err , '''') = '''' and
			ISNULL(Net_Amount_err , '''') = '''' and  ISNULL(GiveAway_QTY_err , '''') = '''' and 
			ISNULL(status , '''') = ''''
			) '
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
PO_NUM , Reference_EP , MATL_CODE , Receive_Qty , GiveAway_QTY , PACKAGE_NAME,
Unit_Price , TradeDiscount_Percent , TradeDiscount_Price , CashDiscount_Percent , CashDiscount_Price,
Total_before_Vat , Vat , Vat_Amount , Net_Amount ,
			PO_NUM_Err
			,MATL_CODE_Err
			,PACKAGE_NAME_Err
			,Unit_Price_Err
			,Receive_Qty_err
			,TradeDiscount_Percent_err
			,TradeDiscount_Price_err
			,CashDiscount_Percent_err
			,CashDiscount_Price_err
			,Total_before_Vat_err
			,Vat_err
			,Vat_Amount_err
			,Net_Amount_err
			,GiveAway_QTY_err
			,status = Case
				When ISNULL(PO_NUM_Err , '''') = '''' and  ISNULL(MATL_CODE_Err , '''') = ''''  and 
					ISNULL(PACKAGE_NAME_Err , '''') = '''' and  ISNULL(Unit_Price_Err , '''') = ''''  and
					ISNULL(Receive_Qty_err , '''') = '''' and  ISNULL(TradeDiscount_Price_err , '''') = ''''  and
					ISNULL(TradeDiscount_Percent_err , '''') = '''' and  ISNULL(CashDiscount_Percent_err , '''') = ''''  and
					ISNULL(CashDiscount_Price_err , '''') = '''' and  ISNULL(Total_before_Vat_err , '''') = '''' and
					ISNULL(Vat_err , '''') = '''' and  ISNULL(Vat_Amount_err , '''') = '''' and
					ISNULL(Net_Amount_err , '''') = '''' and  ISNULL(GiveAway_QTY_err , '''') = '''' and 
					ISNULL(status , '''') = ''''
				Then ''เพิ่มสำเร็จ''
				Else (ISNULL(PO_NUM_Err , '''')+'' ''+ISNULL(MATL_CODE_Err , '''')+'' ''+ISNULL(PACKAGE_NAME_Err , '''')+'' ''+ISNULL(Unit_Price_Err , '''')+'' ''
				+ISNULL(Receive_Qty_err , '''')+'' ''+ISNULL(TradeDiscount_Percent_err , '''')+'' ''+ISNULL(TradeDiscount_Price_err , '''')+'' ''
				+ISNULL(CashDiscount_Percent_err , '''')+'' ''+ISNULL(CashDiscount_Price_err , '''')+'' ''+ISNULL(Total_before_Vat_err , '''')+'' ''
				+ISNULL(Vat_err , '''')+'' ''+ISNULL(Vat_Amount_err , '''')+'' ''+ISNULL(Net_Amount_err , '''')+'' ''+ISNULL(GiveAway_QTY_err , '''')+'' ''+ISNULL(status , ''''))
				END 
			FROM Logfile_Good_Receiving ' + @where 
	 
	 exec (@SQL);
	 --SELECT @SQL
    
END
