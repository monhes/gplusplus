using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus;

public partial class Other_ReceiveOther : Pagebase
{
    clsConnDb obj_db = new clsConnDb();
    clsOther obj_other = new clsOther();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            this.PageID = "405";
        }

        Pagebase Page_p = new Pagebase();
        int counter = 0;
        List<ListItem> list_Items = new List<ListItem>();
        list_Items = obj_other._fn_GetStock();
        drp_searchStock.Items.AddRange(list_Items.ToArray<ListItem>());
        list_Items = obj_other._fn_GetType();
        drp_typeReceive.Items.AddRange(list_Items.ToArray<ListItem>());
        list_Items = obj_other._fn_GetSupplier();
        drp_supplier.Items.AddRange(list_Items.ToArray<ListItem>());
        //list_Items = obj_other._fn_GetLocation();
        //drp_idStock1_1.Items.AddRange(list_Items.ToArray<ListItem>());
        hidUser.Value = Page_p.UserID;
        hidUserFName.Value = Page_p.FirstName;
        hidUserLName.Value = Page_p.LastName;
        
        obj_other.clearList();    
    }


  
}