$(function () {


    $(document).on("click", ".image-delete button", function (e) {

        let id = parseInt($(this).attr("data-id"))
        $.ajax({

            url: `/admin/product/deleteimage?id=${id}`,
            type: "Post",
            success: function (res) {
                $(e.target).parent().remove();
               
            }
        })


    })




    $(document).on("click", ".image-delete img", function (e) {
       
        let id = parseInt($(this).attr("data-id"))
        $.ajax({

            url: `/admin/product/changemainimage?id=${id}`,
            type: "Post",
            success: function (res) {
                

            }
        })



    })

})