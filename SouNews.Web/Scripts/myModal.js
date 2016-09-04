
$(function () {


    //不带iframe 
  /*  if (!$("#myModal")[0]) {
        $("body").append('<!-- Modal --><div id="myModal" class="modal fade" tabindex="-1" role="dialog" style="display: none;">    <div class="modal-dialog" style="width:850px;">        <div class="modal-content">            <div class="modal-header">                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>               <h4 class="modal-title" id="myModalLabel"></h4>            </div>            <div class="modal-body">     </div> <!-- / .modal-body -->            <div class="modal-footer">                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>            </div>        </div> <!-- / .modal-content -->    </div> <!-- / .modal-dialog --></div> <!-- /.modal --><!-- / Modal -->')
        $("a[href='#myModal']").click(function () {
            $("#myModal .modal-body").load($(this).attr("data-url"));
            $("#myModalLabel").text($(this).attr("title"));
        }); 
    }
    //modal-backdrop fade in

    //带iframe
   // if (!$("#myiframeModal")[0]) {
    $("body").append('<!-- Modal --><div id="myiframeModal" class="modal fade" tabindex="-1" role="dialog"  >    <div class="modal-dialog"><div class="modal-content" style="width:800px;"><div class="modal-header">                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>               <h4 class="modal-title" id="myiframeModalLabel"></h4></div><div class="modal-body" style=""><iframe class="frame" src="" style="height:500px;width:100%;overflow-y:auto;" frameborder="0"></iframe></div> <!-- / .modal-body --> </div> <!-- / .modal-content -->    </div> <!-- / .modal-dialog --></div> <!-- /.modal --><!-- / Modal -->')
    $("a[href='#myiframeModal']").click(function () {
        $("#myiframeModal .frame").prop("src", $(this).attr("data-url"));
        $("#myiframeModalLabel").text($(this).attr("title"));
    })
   // }*/

    $("a[href='#myiframeModal']").click(function () {
        var width = 830;
        var height = 630;
        var calllback = "";
        if ($(this).attr("data-width") != undefined) {
            width = $(this).attr("data-width");
        }
        if ($(this).attr("data-height") != undefined) {
            height = $(this).attr("data-height");
        } 
        appendModel($(this).attr("title"), $(this).attr("data-url"), width, height)
       /* $("#myiframeModal .frame").prop("src", $(this).attr("data-url"));
        $("#myiframeModalLabel").text($(this).attr("title"));*/
    })
})

function appendModel(title, src, width, height) {
    $("#myiframeModal").remove();
    if (!$("#myiframeModal")[0]) {
        $("body").append('<!-- Modal --><div id="myiframeModal" class="modal fade" tabindex="-1" role="dialog"  ><div class="modal-dialog" style="width:' + width + 'px;"><div class="modal-content" ><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true" style="font-size:25px">×</span></button>               <h4 class="modal-title" id="myiframeModalLabel">' + title + '</h4></div><div class="modal-body"  ><iframe class="frame" src="' + src + '" style="height:' + height + 'px;width:100%;overflow-y:auto;" frameborder="0"></iframe></div> <!-- / .modal-body --></div> <!-- / .modal-content --></div> <!-- / .modal-dialog --></div> <!-- /.modal --><!-- / Modal -->')
        
    } 
}
 