$(document).ready(function () {
    

    $(document).on("click", "#products .price a", function (e) {
        e.preventDefault();

       
        let id = parseInt($(this).closest(".product-item").attr("data-productId"))
        let basketCount = $("header .rounded-circle").text();
        $.ajax({

            url: `home/addbasket?id=${id}`,
            type: "Post",
            success: function (res) {

                basketCount++;
                $("header .rounded-circle").text(basketCount)
            }
        })

        
       
        

        
    })


    $(document).on("click", ".table-responsive .table-striped button", function (e) {
        e.preventDefault();

        let id = parseInt($(this).attr("data-id"))
        
        $.ajax({

            url: `cart/DeleteProductFromBasket?id=${id}`,
            type: "Post",
            success: function (res) {
                console.log(res.count)
                
                $("header .rounded-circle").text(res.count);
                $(e.target).closest("tr").remove();
                $(".grand-total h1 span").text(res.grandTotal);

                if (res.count === 0) {
                    $(".alert-basket-empty").removeClass("d-none");
                    $(".basket-table").addClass("d-none");
                }
                    
            }
        })
    })



    $(document).on("click", ".basket-table .fa-plus", function (e) {
        let id = parseInt($(this).attr("data-id"))
        let basketCount = $("header .rounded-circle").text();
        $.ajax({

            url: `cart/productcountplus?id=${id}`,
            type: "Post",
            success: function (res) {
                
                $(e.target).prev().text(res.count)
                $(".grand-total h1 span").text(res.basketGrandTotal);
                $(e.target).parent().next().children().text(res.productGrandTotal);
                basketCount++;
                $("header .rounded-circle").text(basketCount)
            }
        })

    })



    $(document).on("click", ".basket-table .fa-minus", function (e) {
        let id = parseInt($(this).attr("data-id"))
        let basketCount = $("header .rounded-circle").text();
        
        $.ajax({
            
            url: `cart/productcountminus?id=${id}`,
            type: "Post",
            success: function (res) {
                
                $(e.target).next().text(res.count)

                $(".grand-total h1 span").text(res.basketGrandTotal)

                $(e.target).parent().next().children().text(res.productGrandTotal);
                if (basketCount>1) {
                    basketCount--;
                }
                
                $("header .rounded-circle").text(basketCount);

            }
        })

    })

})