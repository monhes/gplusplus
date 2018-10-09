USE [GPlus_new1]
GO

/****** Object:  StoredProcedure [dbo].[sp_Inv_Select_Request]    Script Date: 28/9/2561 4:58:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

alter PROCEDURE [dbo].[sp_Inv_Select_Request]
	
	@Stock_Id_From		INT = NULL
	,@Request_stts		varchar(100) = NULL
	,@Request_No		varchar(50) = NULL
	,@Request_Date_from varchar(20) = NULL
	,@Request_Date_to	varchar(20) = NULL
	,@Pay_Date_from varchar(20) = NULL
	,@Pay_Date_to	varchar(20) = NULL
	,@Stock_stts		CHAR(1) = NULL
	,@OrgStruc_stts		CHAR(1) = NULL
	
	,@Request_Id		INT = NULL
	,@Summary_ReqId		INT = NULL
	,@delivery_location varchar(100) = NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @cond nvarchar(max)
	DECLARE @where nvarchar(max)

	SET @where = ' 1 = 1 '
	
	IF(@Stock_Id_From IS NOT NULL)
	BEGIN
		SET @cond = ' AND Req.Stock_Id_From = ''' + str(@Stock_Id_From) + '''' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Request_stts IS NOT NULL)
	BEGIN
		SET @cond = ' AND ' + @Request_stts + '' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Request_No IS NOT NULL)
	BEGIN
		SET @cond = ' AND Req.Request_No LIKE ''%' + @Request_No + '%''' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Request_Date_from IS NOT NULL)
	BEGIN
		SET @cond = ' AND (CONVERT(DATE,Req.Request_Date) >= ''' + @Request_Date_from + ''')' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Request_Date_to IS NOT NULL)
	BEGIN
		SET @cond = ' AND (CONVERT(DATE,Req.Request_Date) <= ''' + @Request_Date_to + ''')' ;
		SET @where = @where + @cond;	
	END
	
	
	IF((@Stock_stts IS NOT NULL) AND (@OrgStruc_stts IS NOT NULL))
	BEGIN
		SET @cond = ' AND (Req.Stock_Id_Req IS NOT NULL OR Req.OrgStruc_Id_Req IS NOT NULL)' ;
		SET @where = @where + @cond;	
	END
	ELSE IF(@Stock_stts IS NOT NULL)
	BEGIN
		SET @cond = ' AND Req.Stock_Id_Req IS NOT NULL' ;
		SET @where = @where + @cond;	
	END
	ELSE IF(@OrgStruc_stts IS NOT NULL)
	BEGIN
		SET @cond = ' AND Req.OrgStruc_Id_Req IS NOT NULL' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Request_Id IS NOT NULL)
	BEGIN
		SET @cond = ' AND Req.Request_Id = ''' + str(@Request_Id) + '''' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Summary_ReqId IS NOT NULL)
	BEGIN
		SET @cond = ' AND (Req.Request_Id in (select Request_Id from INV_SUMMARYREQ_DETAIL where Summary_ReqId = ''' + str(@Summary_ReqId) + '''))' ;
		SET @where = @where + @cond;	
	END
	
	
	IF(@Request_stts = 'Request_Status = ''6''' AND (ISNULL(@Pay_Date_from,'') <> '') AND (ISNULL(@Pay_Date_to,'') <> ''))
	BEGIN
		SET @cond = ' AND (Req.Request_Id in (select pay.[Request_Id] from [dbo].[INV_STOCKPAY] pay where (CONVERT(DATE,pay.[Pay_Date]) >= ''' + @Pay_Date_from + ''') AND (CONVERT(DATE,pay.[Pay_Date]) <= ''' + @Pay_Date_to + ''')))' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Request_stts = 'Request_Status = ''6''' AND (ISNULL(@Pay_Date_from,'') <> '') AND (ISNULL(@Pay_Date_to,'') = '')) 
	BEGIN
		SET @cond = ' AND (Req.Request_Id in (select pay.[Request_Id] from [dbo].[INV_STOCKPAY] pay where (CONVERT(DATE,pay.[Pay_Date]) = ''' + @Pay_Date_from + ''')))' ;
		SET @where = @where + @cond;	
	END
	
	IF(@Request_stts = 'Request_Status = ''6''' AND (ISNULL(@Pay_Date_to,'') <> '') AND (ISNULL(@Pay_Date_from,'') = '')) 
	BEGIN
		SET @cond = ' AND (Req.Request_Id in (select pay.[Request_Id] from [dbo].[INV_STOCKPAY] pay where (CONVERT(DATE,pay.[Pay_Date]) = ''' + @Pay_Date_to + ''')))' ;
		SET @where = @where + @cond;	
	END
	
	EXEC('

	select 
			Req.Request_Id,
			Req.Request_No,
			Req.Stock_Id_From,
			Req.Request_Date,
			
			--(select top 1 Description from Inv_OrgStructure org where org.Div_Code = Dep.Div_Code and org.Dep_Code ='''' ) Div,
			--(select top 1 Description from Inv_OrgStructure org where org.Div_Code = Dep.Div_Code and org.Dep_Code = Dep.Dep_Code  ) Dep,
			
			(select 
				case when 
				Inv_OrgStructure.Dep_Code IS NOT NULL
				Then (select [Description] from Inv_OrgStructure org where org.Div_Code = Inv_OrgStructure.Div_Code and (org.Dep_Code = '''' or org.Dep_Code IS NULL))
				ELSE  
					[Description]
				END as Div
			 from Inv_OrgStructure
			 where OrgStruc_Id = Req.OrgStruc_Id_Req) Div,
			 
			 
			 (select 
				case when 
				Inv_OrgStructure.Dep_Code IS NULL or Inv_OrgStructure.Dep_Code = ''''
				Then NULL
				ELSE  
					[Description]
				END as Dep  
			  from Inv_OrgStructure 
			  where OrgStruc_Id = Req.OrgStruc_Id_Req) Dep,
			
			Req.OrgStruc_Id_Req,
			Req.Stock_Id_Req,
			(select top 1 Stock_Name from Inv_Stock where Req.Stock_Id_Req = Inv_Stock.Stock_Id) as Stock_Name,
			--Req.Stock_Id_Req,
			--Inv_Stock.Stock_Name,
			GPLUZ_ACCOUNT.Account_Fname+'' ''+ GPLUZ_ACCOUNT.Account_Lname as Request_By,
			sum(Inv_ReqItem.Pay_Amount) as Pay_Amount,
			Req.Request_Type, -- 0 = รอบ, 1 = เบิกด่วน,2 =  เบิกผิดวัน
			Req.Request_Status, --  0 = ยกเลิกการเบิก,  1 = รอนุมัติเบิก, 2 = รอจ่าย, 3 = พิมพ์สรุปจ่าย, 4 =  Allocated, 5 = ค้างจ่าย, 6 = จ่ายเรียบร้อยแล้ว
			Req.delivery_location
			
			,(Select top 1 Pay_Date from Inv_StockPay where Request_Id = Req.Request_Id  AND Pay_Status <> ''0'' order by Pay_Date DESC) as Pay_Date
			,(Select top 1 GPLUZ_ACCOUNT.Account_Fname+'' ''+ GPLUZ_ACCOUNT.Account_Lname from Inv_StockPay left join GPLUZ_ACCOUNT
							on Inv_StockPay.Pay_By = GPLUZ_ACCOUNT.Account_ID 
				where Request_Id = Req.Request_Id  AND Pay_Status <> ''0'' order by Pay_Date DESC) as Pay_By,Req.Request_Name

		From Inv_Request Req
			LEFT JOIN Inv_OrgStructure Dep ON Req.OrgStruc_Id_Req = Dep.OrgStruc_Id --เอาออก
			LEFT JOIN Inv_ReqItem ON Req.Request_Id = Inv_ReqItem.Request_Id
			LEFT JOIN GPLUZ_ACCOUNT ON Req.Request_By = GPLUZ_ACCOUNT.Account_ID
			--LEFT JOIN INV_STOCKPAY on INV_STOCKPAY.Request_Id = Req.Request_Id
			--LEFT JOIN Inv_Stock ON Inv_Stock.Stock_Id = INV_STOCKPAY.Stock_Id_Pay
			
			
		
	where ' + @where + ' 

	Group by
			Req.Request_Id, 
			Req.Request_No,
			Req.Stock_Id_From,
			Req.Request_Date,
			Dep.Div_Code,
			Dep.Dep_Code,
			Req.OrgStruc_Id_Req,
			Req.Stock_Id_Req,
			--Inv_Stock.Stock_Name,
			GPLUZ_ACCOUNT.Account_Fname,
			GPLUZ_ACCOUNT.Account_Lname,
			Req.Request_Type,
			Req.Request_Status,
			Req.delivery_location,
			Req.Request_Name
	');
	
	--select
	--	Req.Request_Id, 
	--	Req.Request_No,
	--	Req.Request_Date,
	--	(select top 1 Description from Inv_OrgStructure org where org.Div_Code = Dep.Div_Code and org.Dep_Code ='' ) Div,
	--	(select top 1 Description from Inv_OrgStructure org where org.Div_Code = Dep.Div_Code and org.Dep_Code = Dep.Dep_Code  ) Dep,
	--	Req.Stock_Id_Req,
	--	(select top 1 Stock_Name from Inv_Stock where Req.Stock_Id_Req = Inv_Stock.Stock_Id) as Stock_Name,
	--	--Req.Stock_Id_Req,
	--	--Inv_Stock.Stock_Name,
	--	GPLUZ_ACCOUNT.Account_Fname+' '+ GPLUZ_ACCOUNT.Account_Lname as Request_By,
	--	sum(Inv_ReqItem.Pay_Amount) as Pay_Amount,
	--	Req.Request_Type, -- 0 = รอบ, 1 = เบิกด่วน,2 =  เบิกผิดวัน
	--	Req.Request_Status --  0 = ยกเลิกการเบิก,  1 = รอนุมัติเบิก, 2 = รอจ่าย, 3 = พิมพ์สรุปจ่าย, 4 =  Allocated, 5 = ค้างจ่าย, 6 = จ่ายเรียบร้อยแล้ว

	--From Inv_Request Req
	--	LEFT JOIN Inv_OrgStructure Dep ON Req.OrgStruc_Id_Req = Dep.OrgStruc_Id
	--	LEFT JOIN Inv_ReqItem ON Req.Request_Id = Inv_ReqItem.Request_Id
	--	LEFT JOIN GPLUZ_ACCOUNT ON Req.Request_By = GPLUZ_ACCOUNT.Account_ID
	--	--LEFT JOIN INV_STOCKPAY on INV_STOCKPAY.Request_Id = Req.Request_Id
	--	--LEFT JOIN Inv_Stock ON Inv_Stock.Stock_Id = INV_STOCKPAY.Stock_Id_Pay
	--	--where Dep.OrgStruc_Status = 1
	--		--and Inv_Stock.Stock_Status = 1
	--		--and INV_STOCKPAY.Pay_Status = 1
	--Group by 
	--	Req.Request_Id,
	--	Req.Request_No,
	--	Req.Request_Date,
	--	Div_Code,
	--	Dep.Dep_Code,
	--	Req.Stock_Id_Req,
	--	--Inv_Stock.Stock_Name,
	--	GPLUZ_ACCOUNT.Account_Fname,
	--	GPLUZ_ACCOUNT.Account_Lname,
	--	Req.Request_Type,
	--	Req.Request_Status

    
END
GO


