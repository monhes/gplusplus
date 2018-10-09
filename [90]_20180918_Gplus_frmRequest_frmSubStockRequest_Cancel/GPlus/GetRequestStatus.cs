using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus
{
    public class GetRequestStatus : Pagebase
    {
        public string GetReqStatus(string req_status, string consider_type, string req_id)
        {
            string result_status = "";
            if (req_status == "0") // Case '0' - ยกเลิกการเบิก
            {
                if (consider_type == "0")
                {
                    result_status = "ยกเลิกการเบิก (ไม่อนุมัติ)";
                }
                else
                {
                    result_status = "ยกเลิกการเบิก";
                }
            }
            else if (req_status == "1") // Case '1' - รออนุมัติเบิก
            {
                if (consider_type == "1")
                {
                    result_status = "ส่งกลับแก้ไข";
                }
                else
                {
                    result_status = "รออนุมัติเบิก";
                }  
            }
            else if (req_status == "2") // Case '2' - รอจ่าย
            {
                result_status = "รอจ่าย";
            }
            else if (req_status == "3") // Case '3' - พิมพ์สรุปจ่าย
            {
                result_status = "พิมพ์สรุปจ่าย";
            }
            else if (req_status == "4") // Case '4' - Allocated
            {
                result_status = "Allocated";
            }
            else if (req_status == "5") // Case '5' - ค้างจ่าย
            {
                result_status = "ค้างจ่าย";
            }
            else if (req_status == "6") // Case '6' - จ่ายเรียบร้อยแล้ว
            {
                DataTable dt = new DataAccess.RequestDAO().GetReqRecStatus(req_id);
                if (Convert.ToInt32(dt.Rows[0]["Cnt_Req"].ToString()) > 0)
                {
                    result_status = "จ่ายเรียบร้อยแล้ว";
                }
                else
                {
                    result_status = "ทำรับแล้ว";
                }
                
            }
            return result_status;
        }
    }


    #region PT

    public class GetRequestStatusSubStock : Pagebase
    {
        public string GetReqStatus(string req_status, string consider_type, string req_id)
        {
            string result_status = "";
            if (req_status == "0") // Case '0' - ยกเลิกการเบิก
            {
                if (consider_type == "0")
                {
                    result_status = "ยกเลิกการเบิก (ไม่อนุมัติ)";
                }
                else
                {
                    result_status = "ยกเลิกการเบิก";
                }
            }
            else if (req_status == "1") // Case '1' - รออนุมัติเบิก
            {
                if (consider_type == "1")
                {
                    result_status = "ส่งกลับแก้ไข";
                }
                else
                {
                    result_status = "รออนุมัติเบิก";
                }
            }
            else if (req_status == "2") // Case '2' - รอจ่าย
            {
                result_status = "รอจ่าย";
            }
            else if (req_status == "3") // Case '3' - พิมพ์สรุปจ่าย
            {
                result_status = "พิมพ์สรุปจ่าย";
            }
            else if (req_status == "4") // Case '4' - Allocated
            {
                result_status = "Allocated";
            }
            else if (req_status == "5") // Case '5' - ค้างจ่าย
            {
                result_status = "ค้างจ่าย";
            }
            else if (req_status == "6") // Case '6' - จ่ายเรียบร้อยแล้ว
            {
                bool isComplete = new DataAccess.RequestDAO().ReqInvRequestSelectReqStatus(int.Parse(req_id));
                if (!isComplete)
                {
                    result_status = "จ่ายเรียบร้อยแล้ว";
                }
                else
                {
                    result_status = "ทำรับแล้ว";
                }

            }
            return result_status;
        }
    }



    #endregion
}