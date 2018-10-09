USE [GPlus_new1]
GO

/****** Object:  StoredProcedure [dbo].[sp_Cancel_Good_Receiving]    Script Date: 28/9/2561 4:58:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


alter PROCEDURE [dbo].[sp_Cancel_Good_Receiving]

	@RefCancel_EP		VARCHAR(50)		= null,
	@PO_NUM				VARCHAR(50)		= null,
	@Reference_EP		VARCHAR(50)		= null,
	@MATL_CODE			VARCHAR(50)		= null,
	@Inv_ItemID			int				= null,
	@Pack_ID			int				= null,
	@UpdateBY			int				= null,
	@Recive_Quantity	int				= null

AS
BEGIN

	SET NOCOUNT ON;

	Declare @stock_id int = 1
	Declare @Receive_Stk_ID int = 0
	Declare @Receive_StkItem_ID int = 0
	Declare @PO_ID int = 0
	
	
	Declare @Total_Price		decimal(18, 4) = 0
	Declare @Total_Discount		decimal(18, 4) = 0
	Declare @Total_Before_Vat	decimal(18, 4) = 0
	Declare @VAT_Amount			decimal(18, 4) = 0
	Declare @Net_Amount			decimal(18, 4) = 0
	Declare @status				varchar(50) = NULL 
	Declare @Return_GiveAway_QTY varchar(50) = NULL
	Declare @Return_QTY			Int = NULL
	
	--Select	@Receive_Stk_ID = Receive_Stk_ID,  @PO_ID = PO_ID
	--from	Inv_Receive_Stk 
	--where	Receive_Stk_No = @Reference_EP
		
	Select	@Total_Price			= (Total_before_Vat - Total_Discount) , 
			@Total_Discount			= Total_Discount, 
			@Total_Before_Vat		= Total_before_Vat, 
			@VAT_Amount				= Vat_Amount, 
			@Net_Amount				= Net_Amonut,
			@Receive_Stk_ID			= Receive_Stk_ID --Monnapat
	from	Inv_Receive_Stk
	where	Receive_Stk_No = @Reference_EP

	PRINT @MATL_CODE
	
	IF @MATL_CODE <> 'ALL_ITEMS'
		Begin
			PRINT '<>'
			--Select	@Total_Price			= (SUM(Total_before_Vat) - SUM(Total_Discount)) , 
			--		@Total_Discount			= SUM(Total_Discount), 
			--		@Total_Before_Vat		= SUM(Total_before_Vat), 
			--		@VAT_Amount				= SUM(Vat_Amount), 
			--		@Net_Amount				= SUM(Net_Amount) , 
			--		@Receive_StkItem_ID		= Receive_StkItem_ID
			Select	@Total_Price			= @Total_Price		-	(Total_before_Vat - Total_Discount) , 
					@Total_Discount			= @Total_Discount	-	Total_Discount, 
					@Total_Before_Vat		= @Total_Before_Vat	-	Total_before_Vat, 
					@VAT_Amount				= @VAT_Amount		-	Vat_Amount, 
					@Net_Amount				= @Net_Amount		-	Net_Amount , 
					@Receive_StkItem_ID		= Receive_StkItem_ID,
					@inv_itemid				= inv_itemid, 		-- Author: Monnapat 
					@Recive_Quantity		= Recive_Quantity  	-- Author: Monnapat 
						
			from	Inv_Receive_StkItem
			where	Receive_Stk_ID = @Receive_Stk_ID	and Cancel_Flag = 0
					and Inv_ItemID = @Inv_ItemID		and Pack_ID = @Pack_ID
					and Recive_Quantity > 0	-- Monnapat
			

			--A. ทำการลดยอดจำนวนสินค้าในคลัง(inv_stock_onhand)  และ Insert Movement , Update  c] 
			--และ dbo.INV_STOCK_LOT, INV_STOCK_LOT_LOCATION, INV_STOCK_LOT_LOG และ
			--UPDATE dbo.Inv_Receive_StkItem SET Cancel_flag = '1' ให้เรียกใช้ Store Procedure ด้านล่าง
			
			exec sp_Get_Inv_Receive_StkItem_Cancel	@Receive_StkItem_ID = @Receive_StkItem_ID,
													@Stock_ID			= @stock_id,
													@Update_By			= @UpdateBY
													
			--B. จากบรรทัดที่ 334  recvModel.CalculateUnCancelPrice(); // re- calculate price ++
			--มันเข้าไปทำอะไรบ้าง  ??   หาให้หน่อยค่ะ
			--C. Update  Inv_Receive_Stk   เพื่อ  Update ค่าจำนวนเงินที่รับตามเลขที่การรับนี้ใหม่
			exec sp_Get_Update_Stk_Price		@Receive_Stk_Id		= @Receive_Stk_ID, 
												@Total_Price		= @Total_Price, 
												@Total_Discount		= @Total_Discount, 
												@Total_Before_Vat	= @Total_Before_Vat, 
												@VAT_Amount			= @VAT_Amount, 
												@Net_Amount			= @Net_Amount 

			--D.	Update Inv_PO_Form1  เพื่อ Set สถานะว่ารับครบแล้วหรือไม่ครบ
			exec sp_Get_UpdatePO_IsReceiveComplete @PO_ID = @PO_ID

			--E.	Update  Inv_Receive_Stk  ว่าเลขที่รับนี้ยกเลิกทุกรายการใน  inv_receive_stkItem หรือไม่ 
			--เขียนเพื่ออธิบายความเข้าใขว่า ถ้าทุกรายการใน inv_receive_stkItem ถูกยกเลิก  
			--update  Inv_Receive_Stk.status  = 0 Else 1   ให้เรียกใช้ Store Procedure ด้านล่าง
			exec sp_Get_Update_ReceiveStk_Status @Receive_Stk_ID = @Receive_Stk_ID
	
		End
	
	ELSE 

	Begin

	-- Author: Monnapat --
	IF @Recive_Quantity  <  @Return_QTY
		Begin
			SET @status = 'จำนวนที่รับเข้าคลังน้อยกว่าจำนวนที่ยกเลิก'
		End

	IF @Return_GiveAway_QTY  > 0
		Begin
			Declare @GiveAway_unit varchar = null		  
				Select  @GiveAway_unit = GiveAway_unit
				from inv_receive_stkItem 
				where Receive_Stk_ID  = @Receive_Stk_ID  
				and inv_ItemID = @Inv_ItemID  and pack_id = Pack_ID and  cancel_flag  = '0' and GiveAway_unit > 0 			
		END		
			                               
	Else IF ISNULL(@Return_GiveAway_QTY,'') = ''
		Begin
			SET @status = @status + 'ไม่มีการรับของแถม'
		END

	Else IF @GiveAway_unit < @GiveAway_unit 
		Begin
			SET @status = 'จำนวนที่รับเข้าคลังน้อยกว่าจำนวนของแถมที่ยกเลิก'
		End
	
	-- Author : Monnapat
Declare @package_id varchar(50) = Null
Declare @PackageName_Err varchar(100) = Null
Declare @Package_Name varchar(50) = Null

IF  @package_id <> 'ALL_ITEMS'
Begin	
      Select @Pack_ID = Pack_ID from Inv_Package 
       Where Package_Name = @Package_Name
      IF ISNULL(@Pack_ID,'') = ''
		SET @PackageName_Err = 'รหัสนี้ไม่มีในฐานข้อมูล'
End
End
	-- END IF --

		Begin
			PRINT '='
			-- Declare cursor from select table 'CUSTOEMR'
			DECLARE cursor_Inv_Item CURSOR FOR 
			Select	Receive_StkItem_ID
			from	Inv_Receive_StkItem 
			where	Receive_Stk_ID  = @Receive_Stk_ID  
					--and	Cancel_Flag  = '0'


			-- Open Cursor
			OPEN cursor_Inv_Item 
			FETCH NEXT FROM cursor_Inv_Item 
			INTO @Receive_StkItem_ID;
			--PRINT '=' + CAST( @@FETCH_STATUS as varchar(100)) + ' : ' + CAST(@Receive_Stk_ID as varchar(100))
			-- Loop From Cursor
			WHILE (@@FETCH_STATUS = 0) 
			BEGIN 
				PRINT @Receive_StkItem_ID
				--A
				exec sp_Get_Inv_Receive_StkItem_Cancel	@Receive_StkItem_ID = @Receive_StkItem_ID,
														@Stock_ID			= @stock_id,
														@Update_By			= @UpdateBY
														
				--B
				--C
				exec sp_Get_Update_Stk_Price		@Receive_Stk_Id		= @Receive_Stk_ID, 
													@Total_Price		= 0, 
													@Total_Discount		= 0, 
													@Total_Before_Vat	= 0, 
													@VAT_Amount			= 0, 
													@Net_Amount			= 0 
													
				--D			
				exec sp_Get_UpdatePO_IsReceiveComplete @PO_ID = @PO_ID
				
				--E
				exec sp_Get_Update_ReceiveStk_Status @Receive_Stk_ID = @Receive_Stk_ID
				
				FETCH NEXT FROM cursor_Inv_Item -- Fetch next cursor
				INTO @Receive_StkItem_ID  -- Next into variable
			END
	
			-- Close cursor
			CLOSE cursor_Inv_Item; 
			DEALLOCATE cursor_Inv_Item; 
	
		End

END
GO


