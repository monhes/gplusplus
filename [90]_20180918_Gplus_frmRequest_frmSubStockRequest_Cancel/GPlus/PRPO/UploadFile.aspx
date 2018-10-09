<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadFile.aspx.cs" Inherits="GPlus.PRPO.UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-family: tahoma; font-size: 12px; background-color: #E0ECF8">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div style="border: 1px solid; width: 300px; background-color: #57a0ea; margin-top: auto; margin-bottom: auto; margin-left: auto; margin-right:auto">
	        <div id="header" style="padding: 2px">
		        <span style="color:White; width: 80px">&nbsp;เลือกไฟล์&nbsp;</span>
		        <span id="files" style="display: inline">
                    <input type="file" name="fileUploader" onchange="FileChange();" id="fileUploader" runat="server" />
                </span>
	        </div>
	        <div id="body" style="margin: 4px; overflow:auto; height: 150px; background-color: #E0ECF8; border: 1px solid #000;">
		        <div id="uploadedFiles"></div>
		        <div id="uploadingFiles"></div>
                <div id="hiddenId"></div>
	        </div>
	        <div id="footer" style="padding: 2px; text-align: right">
                <asp:Button ID="bDelete" runat="server" Text="ลบ" OnClick="bDelete_Click" ClientIDMode="Static" />
                <asp:Button ID="bUpload" runat="server" Text="อัพโหลด" OnClick="bUpload_Click" ClientIDMode="Static" />
	        </div>
        </div>
    </form>
    <script type='text/javascript'>
        // อิลิเมนต์ input=file ด้านบนสุดที่ผู้ใช้เห็น
//        var $topfile = document.getElementsByName('name_0')[0];
//        var $uploadingFiles = document.getElementById('uploadingFiles');
//        var $files = document.getElementById('files');
        //        var id = 1;
        document.getElementById('<%= bUpload.ClientID %>').style.display = 'none';

        function FileChange(filename) {

            document.getElementById('<%= bUpload.ClientID %>').click();

//            var $row = document.createElement('div');
//            var $delete = document.createElement('a');
//            var $filename = document.createElement('span');
//            var $input = document.createElement('input');

//            // สร้าง input=file ตัวใหม่
//            $input.type = 'file';
//            $input.name = 'name_' + id;
//            $input.onchange = function () { FileChange(this.value); };

//            $files.insertBefore($input, $topfile);

//            // ไฟล์
//            $filename.innerHTML = filename.substring(filename.lastIndexOf('\\') + 1) + '&nbsp;&nbsp;';
//            // ลบ
//            $delete.style.color = 'blue';
//            $delete.innerHTML = '<b>ลบ</b>';
//            $delete.name = 'name_' + (id - 1);
//            $delete.onclick = function () { DeleteUploadingFile(this); };
//            $delete.onmouseover = function () { this.style.cursor = 'pointer'; }

//            $row.style.padding = '5px';
//            $row.style.borderBottom = '1px solid #aaa';
//            $row.onmouseover = function () {
//                this.style.backgroundColor = 'pink';
//            };
//            $row.onmouseout = function () {
//                this.style.backgroundColor = '#E0ECF8';
//            };
//            $row.appendChild($filename);
//            $row.appendChild($delete);

//            $uploadingFiles.appendChild($row);

//            ++id;
//            $topfile.style.display = 'none';
//            $topfile = $input;
        }

        function DeleteUploadingFile(a) {
            var $remove = document.getElementsByName(a.name);

            for (i = 0; i < $remove.length; ++i)
                if ($remove[i].nodeName == 'INPUT')
                    $files.removeChild($remove[i]);

            $uploadingFiles.removeChild(a.parentNode);
        }

    </script>
</body>
</html>
