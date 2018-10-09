<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiFileUpload.ascx.cs" Inherits="GPlus.UserControls.MultiFileUpload" %>

<div id="files" style="border: 1px solid; width: 302px; background-color: #57a0ea">
	<div id="header" style="padding: 2px">
		<span style="color:White">&nbsp;เลือกไฟล์&nbsp;</span>
		<div id="inputs" style='display:inline'><input type="file" name="file_0" /></div>
	</div>
	<div id="body" style="margin: 4px; overflow:auto; height: 120px; background-color: #E0ECF8; border: 1px solid #000;">
		<div id="uploadedFiles"></div>
		<div id="uploadingFiles"></div>
        <div id="hiddenId"></div>
	</div>
	<div id="footer" style="padding: 2px; text-align: right">
		<input type="button" id="addFile" width="50px" value="เพิ่มไฟล์" />
	</div>
</div>

<script type='text/javascript'>

    var $addFile = document.getElementById('addFile');
    var $uploadingFiles = document.getElementById('uploadingFiles');
    var $uploadedFiles = document.getElementById('uploadedFiles');
    var $header = document.getElementById('header');
    var $inputs = document.getElementById('inputs');
    var $hiddenId = document.getElementById('hiddenId');
    var $currentFile = $inputs.firstChild;

    var inputId = 1;

    $addFile.onclick = AddUploadingFile;

    function AddUploadingFile() {
        if ($currentFile.value.length == 0) {
            alert('กรุณาเลือกไฟล์');
            return;
        }

        var $input = document.createElement('input');
        $input.type = 'file';
        $input.name = 'file_' + inputId;

        $inputs.firstChild.style.display = 'none';
        $inputs.insertBefore($input, $inputs.firstChild);

        var $div = document.createElement('div');
        $div.style.padding = '4px';
        $div.style.borderBottom = '1px solid #B6B6B6';

        $div.onmouseover = function () {
            this.style.backgroundColor = 'pink';
            //this.style.cursor = 'pointer'
        };
        $div.onmouseout = function () {
            this.style.backgroundColor = '#E0ECF8';
        };

        var $filename = CreateFileName(GetFileName($currentFile.value));
        var $deletelink = CreateDeleteLink();
        var $span = document.createElement('span');
        $span.innerText = ' ';

        $div.appendChild($filename);
        $div.appendChild($span);
        $div.appendChild($deletelink);

        $uploadingFiles.appendChild($div);

        $currentFile = $input;

        inputId++;
    }

    function AddUploadedFile(fullpath, id) {

        var index = fullpath.lastIndexOf('_');
        filename = fullpath.substring(0, index);
        index = fullpath.lastIndexOf('.');
        filename += fullpath.substring(index);

        var $filename = CreateFileName(GetFileName(filename));

        var $div = document.createElement('div');
        $div.style.padding = '4px';
        $div.style.borderBottom = '1px solid #B6B6B6';

        var $aview = document.createElement('a');
        $aview.href = fullpath;
        $aview.innerHTML = '<b>ดู</b>';
        $aview.target = '_blank';
        $aview.style.color = 'blue';
        $aview.style.textDecoration = 'none';
        $aview.onmouseover = function () {
            this.style.cursor = 'pointer';
            this.style.textDecoration = 'underline';
        };
        $aview.onmouseout = function () {
            this.style.textDecoration = 'none';
        };

        var $adelete = document.createElement('a');
        $adelete.innerHTML = '<b>ลบ</b>';
        $adelete.style.color = 'red';
        $adelete.onclick = function () {

            if (!confirm('ต้องการลบไฟล์นี้หรือไม่?'))
                return;

            var $input = document.createElement('input');

            $input.type = 'hidden';
            $input.value = id;
            $input.name = 'attach_' + id;

            $hiddenId.appendChild($input);

            $uploadedFiles.removeChild(this.parentNode);
        };
        $adelete.onmouseover = function () {
            this.style.textDecoration = 'underline';
            this.style.cursor = 'pointer';
        };
        $adelete.onmouseout = function () {
            this.style.textDecoration = 'none';
        };

        var $span = document.createElement('span');
        $span.innerHTML = '&nbsp;&nbsp;'

        $div.appendChild($filename);
        $div.appendChild($aview);
        $div.appendChild($span);
        $div.appendChild($adelete);

        $uploadedFiles.appendChild($div);
    }

    function CreateDeleteLink() {
        var $a = document.createElement('a');
        $a.innerHTML = '<b>ลบ</b>';
        $a.name = 'file_' + (inputId - 1);
        $a.style.color = 'red';
        $a.onclick = function () {
            var $obj = document.getElementsByName(this.name);
            for (i = 0; i < $obj.length; ++i)
                if ($obj[i].nodeName == 'INPUT')
                    $inputs.removeChild($obj[i]);

            $uploadingFiles.removeChild(this.parentNode);
        };
        $a.onmouseover = function () {
            this.style.cursor = 'pointer';
        };

        return $a;
    }

    function CreateFileName(filename) {
        var $span = document.createElement('span')
        //$span.style.fontWeight = 'bold';
        $span.innerHTML = filename + '&nbsp;&nbsp;';

        return $span;
    }

    function GetFileName(fullpath) {
        var index = fullpath.lastIndexOf('\\') + 1;
        return fullpath.substring(index);
    }
</script>