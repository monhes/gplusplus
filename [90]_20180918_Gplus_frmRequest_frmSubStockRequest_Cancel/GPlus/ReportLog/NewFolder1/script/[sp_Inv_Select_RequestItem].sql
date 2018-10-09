USE [GPlus_new1]
GO

/****** Object:  StoredProcedure [dbo].[sp_Inv_Select_RequestItem]    Script Date: 28/9/2561 4:58:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

alter PROCEDURE [dbo].[sp_Inv_Select_RequestItem]
	
	@Request_Id		INT = NULL
	,@Summary_ReqId		INT = NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	select INV_REQUEST.Request_No, Inv_ReqItem.Req_ItemID, Inv_ReqItem.Request_Id, Inv_ReqItem.Inv_ItemId, Inv_ReqItem.Pack_Id
	, Inv_Item.Inv_ItemCode, Inv_Item_Search.Item_Search_Desc, Inv_Item_Search. Pack_Description
		,Inv_ReqItem.Req_ItemStatus
		,Inv_ItemPack.Avg_Cost
		,Inv_ReqItem.Remark AS RemarkOrg
		--, case when
		--	( Inv_ReqItem.Req_ItemStatus not in ('2','3') ) 
		--	 THEN Inv_ItemPack.Avg_Cost
		--	 ELSE
		--	 (	
		--		  Select sum(Inv_StockPayItem.Amount) / sum(Inv_StockPayItem.Pay_Quantity) 
		--		  From  Inv_StockPay, Inv_StockPayItem
		--		  Where    Inv_StockPay.Pay_Id  =  Inv_StockPayItem.INV_Pay_Id
		--					and Inv_StockPay.Request_Id  = STR(@Request_Id) 
		--					and Pay_Status <> '0'

				  
		--	 )
		--	 END AS Avg_Cost
		
		,INV_REQITEM.Order_Quantity
		,INV_REQITEM.Pay_Qty
		, case when 
			( Inv_ReqItem.Req_ItemStatus in ('0','1') )
		  Then (INV_REQITEM.Order_Quantity - INV_REQITEM.Pay_Qty)
		  Else
			0
		  END AS Remain
		  
		,(select OnHand_Qty from Inv_Stock_onhand 
			Where Inv_Stock_onhand.Stock_ID = INV_REQUEST.Stock_Id_From
					and inv_itemID = INV_REQITEM.Inv_ItemId
				    and Pack_ID = INV_REQITEM.Pack_Id
		  ) as OnHand_Qty
		  
		  
		 ,INV_REQUEST.OrgStruc_Id_Req
		 ,INV_REQUEST.Stock_Id_Req
		 
		 ,(select INV_SUMMARYREQ_ITEM.Summary_ReqItem_Id
			from INV_SUMMARYREQ_DETAIL left join
					INV_SUMMARYREQ_ITEM on INV_SUMMARYREQ_DETAIL.Summary_ReqId = INV_SUMMARYREQ_ITEM.Summary_ReqId
			where INV_SUMMARYREQ_DETAIL.Request_Id = str(@Request_Id)
					and INV_SUMMARYREQ_DETAIL.Summary_ReqId = str(@Summary_ReqId)
					and INV_SUMMARYREQ_ITEM.Request_Id = str(@Request_Id)
					and INV_SUMMARYREQ_ITEM.Summary_ReqId = str(@Summary_ReqId)
					and INV_SUMMARYREQ_ITEM.Inv_ItemId = INV_REQITEM.Inv_ItemId
					and INV_SUMMARYREQ_ITEM.Pack_Id = INV_REQITEM.Pack_Id
					and ((INV_SUMMARYREQ_ITEM.OrgStruc_Id = INV_REQUEST.OrgStruc_Id_Req and INV_SUMMARYREQ_ITEM.Stock_Id is null) 
						 or (INV_SUMMARYREQ_ITEM.Stock_Id = INV_REQUEST.Stock_Id_Req and INV_SUMMARYREQ_ITEM.OrgStruc_Id is null))
		   ) as Summary_ReqItem_Id
		 
		 ,(select INV_SUMMARYREQ_ITEM.Allocate_Qty
			from INV_SUMMARYREQ_DETAIL left join
					INV_SUMMARYREQ_ITEM on INV_SUMMARYREQ_DETAIL.Summary_ReqId = INV_SUMMARYREQ_ITEM.Summary_ReqId
			where INV_SUMMARYREQ_DETAIL.Request_Id = str(@Request_Id)
					and INV_SUMMARYREQ_DETAIL.Summary_ReqId = str(@Summary_ReqId)
					and INV_SUMMARYREQ_ITEM.Request_Id = str(@Request_Id)
					and INV_SUMMARYREQ_ITEM.Summary_ReqId = str(@Summary_ReqId)
					and INV_SUMMARYREQ_ITEM.Inv_ItemId = INV_REQITEM.Inv_ItemId
					and INV_SUMMARYREQ_ITEM.Pack_Id = INV_REQITEM.Pack_Id
					and ((INV_SUMMARYREQ_ITEM.OrgStruc_Id = INV_REQUEST.OrgStruc_Id_Req and INV_SUMMARYREQ_ITEM.Stock_Id is null) 
						 or (INV_SUMMARYREQ_ITEM.Stock_Id = INV_REQUEST.Stock_Id_Req and INV_SUMMARYREQ_ITEM.OrgStruc_Id is null))
		   ) as Allocate,INV_REQUEST.delivery_location
		  
		  
		 
		  
		--, case when
		--	( Inv_ReqItem.Req_ItemStatus not in ('2','3') ) 
		--	 THEN Inv_ItemPack.Avg_Cost
		--	 ELSE
		--	 (	
		--		  Select sum(Inv_StockPayItem.Amount) / Inv_StockPayItem.Pay_Quantity 
		--		  From  Inv_StockPay, Inv_StockPayItem
		--		  Where    Inv_StockPay.Pay_Id  =  Inv_StockPayItem.Pack_Id
		--					and Inv_StockPay.Request_Id  = '2'   
		--					and Pay_Status <> '0'

				  
		--	 )
		--	 END AS Avg_Cost
		
	from INV_REQUEST left join INV_REQITEM
			on INV_REQUEST.Request_Id = INV_REQITEM.Request_Id left join Inv_Item
			on INV_REQITEM.Inv_ItemId = Inv_Item.Inv_ItemID left join Inv_Item_Search
			on (INV_REQITEM.Inv_ItemId = Inv_Item_Search.Inv_ItemID and INV_REQITEM.Pack_Id = Inv_Item_Search.Pack_ID) left join Inv_ItemPack
			on (INV_REQITEM.Inv_ItemId = Inv_ItemPack.Inv_ItemID and INV_REQITEM.Pack_Id = Inv_ItemPack.Pack_ID)
			
	where INV_REQUEST.Request_Id = str(@Request_Id)
			AND (INV_REQITEM.Is_Delete <> 'Y' or INV_REQITEM.Is_Delete is null)


    
END
GO


