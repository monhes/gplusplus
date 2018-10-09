using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public static class PRPOSession
    {
        public static readonly string PurchaseActualTable   = "PRPOPurchaseActualTable";
        public static readonly string PurchaseVirtualTable  = "PRPOPurchaseVirtualTable";

        public static readonly string HireActualTable       = "PRPOHireActualTable";
        public static readonly string HireVirtualTable      = "PRPOHireVirtualTable";

        public static readonly string PrintFormTable        = "PRPOPrintFormTable";
        public static readonly string Form2Table            = "PRPOForm2Table";

        public static readonly string DeleteItemTable       = "PRPODeleteItemTable";
        public static readonly string PrintFormDeleteTable  = "PRPOPrintFormDeleteTable";
        public static readonly string Form2DeleteTable      = "PRPOForm2DeleteTable";
        public static readonly string AttachDeleteTable     = "PRPOAttachDeleteTable";

        public static readonly string UploadFileTable       = "PRPOUploadFileTable";

        public static void InitializePOPurchase() 
        { 
            new PRPOPurchaseActualTable(); 
            new PRPOPurchaseVirtualTable();
            new PRPOPrintFormTable();
            new PRPOUploadFileTable();
        }

        public static void InitializePOHire()
        {
            new PRPOHireActualTable();
            new PRPOForm2ActualTable();
        }

        public static void InitializePRPurchase()
        {
            new PRPOPurchaseActualTable();
            new PRPOPrintFormTable();
            new PRPOUploadFileTable();
        }

        public static PRPOAction Action
        {
            get { return (PRPOAction)HttpContext.Current.Session["PRPOAction"]; }
            set { HttpContext.Current.Session["PRPOAction"] = value; }
        }

        public static string UserID
        {
            get { return (string)HttpContext.Current.Session["PRPOUserID"]; }
            set { HttpContext.Current.Session["PRPOUserID"] = value; }
        }

        public static bool InitializeAction
        {
            get 
            { 
                if (HttpContext.Current.Session["PRPOAction"] != null) 
                    return true; 
                else 
                    return false; 
            }
        }

        public static string PoID
        {
            get { return (string) HttpContext.Current.Session["PoID"]; }
            set { HttpContext.Current.Session["PoID"] = value; }
        }

        public static string PrID
        {
            get { return (string)HttpContext.Current.Session["PrID"]; }
            set { HttpContext.Current.Session["PrID"] = value; }
        }

        public static bool PrPoForm2Binded
        {
            get { return (bool)HttpContext.Current.Session["PrPoForm2Binded"]; }
            set { HttpContext.Current.Session["PrPoForm2Binded"] = value; }
        }
    }

    public enum PRPOAction { ADD_PO, VIEW_PO, ADD_PR, VIEW_PR };
    public enum PRPOType { PO, PR };

    /// <summary>
    ///     ใช้สำหรับเก็บค่าว่า "รายการ" ที่เลือกมาจาก ชนิดใด
    ///         - 0 มาจาก เลือกจาก Reorder point
    ///         - 1 มาจาก เลือกจาก รายการสินค้า
    ///         - 2 มาจาก เลือกจาก PR
    /// </summary>
    public static class PRPOPopup
    {
        public static readonly string ReorderPoint = "0";
        public static readonly string Product = "1";
        public static readonly string PR = "2";
    }

    public static class PRPOPath
    {
        public static readonly string POUpload = "~/Uploads/PO";
        public static readonly string PRUpload = "~/Uploads/PR";

        public static readonly string POTmpUpload = "~/Uploads/PO/tmp";
        public static readonly string PRTmpUpload = "~/Uploads/PR/tmp";
    }
}