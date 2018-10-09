using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;


/// <summary>
/// Summary description for jsonService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class jsonService : System.Web.Services.WebService {

    
    public jsonService () {

       
    }

    [WebMethod]
    public string GetEmployeeDetails(string employeeId)
    {
      
        string str_json ="";
        return str_json;
    }
    //        
    [WebMethod]
    public string SaveDetail(string str_lotProduct,string str_lotProductName, string str_lotProductQty, string str_lotProductType, string str_lotProductTypeText, string str_lotDetail,string str_lotDetailRun, string str_qtyDetail, string str_expireDetail, string str_priceDetail
        , string str_totalDetail, string str_hidLotStock, string str_hidLotStockRun, string str_qtyStock, string str_idStock, string str_nameStock)
    {

        clsOther obj_other = new clsOther();
        string str_result = "";
        String[] strArr_lotDetail = str_lotDetail.Substring(0, str_lotDetail.LastIndexOf(",")).Split(',');
        String[] strArr_lotDetailRun = str_lotDetailRun.Substring(0, str_lotDetailRun.LastIndexOf(",")).Split(',');
        String[] strArr_qtyDetail = str_qtyDetail.Substring(0, str_qtyDetail.LastIndexOf(",")).Split(',');
        String[] strArr_expireDetail = str_expireDetail.Substring(0, str_expireDetail.LastIndexOf(",")).Split(',');
        String[] strArr_priceDetail = str_priceDetail.Substring(0, str_priceDetail.LastIndexOf(",")).Split(',');
        String[] strArr_totalDetail = str_totalDetail.Substring(0, str_totalDetail.LastIndexOf(",")).Split(',');
        String[] strArr_hidLotStock = str_hidLotStock.Substring(0, str_hidLotStock.LastIndexOf(",")).Split(',');
        String[] strArr_hidLotStockRun = str_hidLotStockRun.Substring(0, str_hidLotStockRun.LastIndexOf(",")).Split(',');
        String[] strArr_qtyStock = str_qtyStock.Substring(0, str_qtyStock.LastIndexOf(",")).Split(',');
        String[] strArr_idStock = str_idStock.Substring(0, str_idStock.LastIndexOf(",")).Split(',');
        String[] strArr_nameStock = str_nameStock.Substring(0, str_nameStock.LastIndexOf(",")).Split(',');


        str_result = obj_other.fn_addLot(str_lotProduct, str_lotProductName, str_lotProductQty, str_lotProductType, str_lotProductTypeText, strArr_lotDetail, strArr_lotDetailRun, strArr_qtyDetail, strArr_expireDetail, strArr_priceDetail, strArr_totalDetail);

       str_result = obj_other.fn_addStockLot(str_lotProduct, strArr_hidLotStock, strArr_hidLotStockRun, strArr_qtyStock, strArr_idStock, strArr_nameStock);

        if(str_result == "")
        {
                
        }

        return "";
       
    }

    [WebMethod]
    public List<clsOther> GetHeadLotDetail(string str_TransactionID)
    {   
        clsOther obj_other = new clsOther();
        if (str_TransactionID == "")
        {
            return obj_other.fn_getLotHeadDetail();
        }
        else
        {
            return obj_other.fn_getLotHeadAddObject(str_TransactionID);
        }
    }

    [WebMethod]
    public List<clsOther> GetHeadLotDetailPay(string str_TransactionID)
    {
        clsOther obj_other = new clsOther();
        if (str_TransactionID == "")
        {
            return obj_other.fn_getLotHeadDetail();
        }
        else
        {
            return obj_other.fn_getLotHeadAddObjectForPay(str_TransactionID);
        }
    }

    [WebMethod]
    public List<clsOtherHead> GetSearch(String str_sort,String str_rowPer,String str_rowStart , String str_dateStart , String str_dateEnd ,String str_receive ,String str_stock)//บันทึกรับอื่นๆ
    {
        clsOtherHead obj_head = new clsOtherHead();
        return obj_head.fn_getHead(str_sort,str_rowPer,str_rowStart , str_dateStart , str_dateEnd ,str_receive ,str_stock);
    }

    [WebMethod]
    public void clearList()
    {
        clsOther obj_other = new clsOther();
        obj_other.clearList();
    }

    [WebMethod]
    public List<clsOtherHead> GetSearchPay(String str_sort, String str_rowPer, String str_rowStart, String str_dateStart, String str_dateEnd, String str_pay, String str_stock)
    {
        clsOtherHead obj_head = new clsOtherHead();
        return obj_head.fn_getHeadPay(str_sort, str_rowPer, str_rowStart, str_dateStart, str_dateEnd, str_pay, str_stock);
    }

    [WebMethod]
    public List<clsOther> GetLotDetail(string str_product)
     {
        clsOther obj_other = new clsOther();
        return obj_other.fn_getLotDetail(str_product);
    }

    [WebMethod]
    public List<clsOther> GetLotStockDetail(string str_product)
    {
        clsOther obj_other = new clsOther();
        return obj_other.fn_getLotStockDetail(str_product);
    }

    [WebMethod]
    public void deleteLotProduct(string str_lotProduct)
    {
        clsOther obj_other = new clsOther();
        string[] strArr_lot = str_lotProduct.Substring(0, str_lotProduct.LastIndexOf(",")).Split(',');

        foreach (String str in strArr_lot)
        {
            obj_other.fn_deleteLot(str);
        }
    }

    [WebMethod]
    public void deleteLotStockLast(string str_lotProduct) //ล่าสุด
    {
        clsOther obj_other = new clsOther();

        obj_other.fn_deleteLotStockLast(str_lotProduct);
        
    }


    [WebMethod]
    public void delete(string str_ID, string str_type,string str_user)
    {
        clsOther obj_other = new clsOther();
        obj_other.fn_delete(str_ID,str_type,str_user);

    }


    [WebMethod] //หน่วยงานคืน ID
    public string GetRequestID(string str_ID)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getRequestID(str_ID);

    }

    [WebMethod] //หน่วยงานคืน รายละเอียด
    public string GetRequestDesc(string str_ID)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getRequestDesc(str_ID);
       
    }


    [WebMethod] //หน่วยงานคืน วันที่
    public List<ListItem> GetRequestDate(string str_ID)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getRequestDate(str_ID);

    }

    [WebMethod] //หน่วยงานคืน Request No
    public string GetRequestForPayID(string str_ID)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getRequestForPayID(str_ID);

    }
   
    [WebMethod] //หน่วยงานคืน Request No
    public string GetOrgForPayID(string str_ID)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getOrgForPayID(str_ID);

    }

    [WebMethod] //หน่วยงานคืน Request No
    public string GetDateForPayID(string str_ID)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getDateForPayID(str_ID);

    }

    [WebMethod] //หน่วยงานคืน ราคา
    public string GetUnitPrice(string str_ID, string str_itemID, string str_packID)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getUnitPrice(str_ID,str_itemID,str_packID);

    }

    [WebMethod] //Check หน่วยงานคืนได้ไหม
    public string GetCheckRequest(string str_itemID, string str_packID, string str_payID, string str_total)
    {
        clsOther obj_other = new clsOther();
        return obj_other.getCheckRequest(str_itemID, str_packID, str_payID, str_total);
         
    }



    //"{'str_receiveID':'" + $("#txt_receiveID").val() + "','str_receiveDate':'" + $("#ContentPlaceHolder1_uc_receiveDate_txtDate").val() + "'
    //,'str_number':'" + $("#txt_number").val() + "','str_typeReceive':'" + $("#ContentPlaceHolder1_drp_typeReceive").val() + "'
    //,'str_remark':'" + $("#txt_remark").val() + "','str_supplier':'" + $("#ContentPlaceHolder1_drp_supplier").val() + "','str_preSend':'" + $("#txt_preSend").val() + "'
    //,'str_preReceive':'" + $("#hid_preReceive").val() + "','str_stock':'" + $("#ContentPlaceHolder1_drp_searchStock").val() + "'}",


    [WebMethod]
    public string saveReceive(string str_receiveID, string str_receiveDate, string str_number, string str_typeReceive, string str_remark, string str_supplier, string str_preSend, string str_preReceive,string str_stock,string str_user,string str_payID)
    {
        clsOther obj_other = new clsOther();

        obj_other.fn_saveReceive(str_receiveID,str_receiveDate,str_number, str_typeReceive,str_remark , str_supplier, str_preSend, str_preReceive,str_stock,str_user,str_payID );
        return "บันทึกเรียบร้อย";
        
    
    }

    [WebMethod]
    public string savePay(string str_payID, string str_payDate, string str_typePay, string str_give, string str_preSend, string str_prePay, string str_stock, string str_user)
    {
        clsOther obj_other = new clsOther();
        return obj_other.fn_savePay(str_payID, str_payDate, str_typePay, str_give, str_preSend, str_prePay, str_stock, str_user);

    }

    [WebMethod]
    public List<ListItem> GetPack(string str_lotProduct)
    {
        clsOther obj_other = new clsOther();
        return obj_other._fn_GetPack(str_lotProduct);
    }


    [WebMethod]
    public List<ListItem> GetProduct(string str_lotProduct, string str_lotProductCode)
    {
        clsOther obj_other = new clsOther();
        return obj_other._fn_GetProduct(str_lotProduct, str_lotProductCode);
    }

    [WebMethod]
    public List<ListItem> GetUser(string str_lotUser)
    {
        clsOther obj_other = new clsOther();
        return obj_other._fn_GetUser(str_lotUser);
    }

    [WebMethod]
    public List<ListItem> GetLocation(string str_stock)
    {
        clsOther obj_other = new clsOther();
        return obj_other._fn_GetLocation(str_stock);
    }


}
