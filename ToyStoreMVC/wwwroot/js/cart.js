// Khi người dùng nhấn vào nút thêm vào giỏ hàng
$(".add-to-cart").click(function (e) {
    // Ngăn chặn hành động mặc định của nút
    e.preventDefault();
    // Lấy cartId từ Session hoặc tạo mới nếu chưa có
    var cartId = sessionStorage.getItem("cartId") || Math.floor(Math.random() * 1000000);
    // Lưu lại cartId vào Session
    sessionStorage.setItem("cartId", cartId);
    // Lấy productId và sl từ thuộc tính data-id và data-sl của nút
    var productId = $(this).data("id");
    var sl = $(this).data("sl");
    // Gọi action AddCart bằng ajax với cartId, productId và sl
    $.ajax({
        url: "/Customer/Cart/AddCart",
        type: "POST",
        data: JSON.stringify({ cartId: cartId, productId: productId, sl: sl }),
        contentType: "application/json",
        success: function (result) {
            // Nếu gọi thành công, cập nhật lại nội dung của view _CartPartial
            $(".cart").html(result);
        },
        error: function () {
            // Nếu gọi thất bại, hiển thị thông báo lỗi
            alert("Có lỗi xảy ra. Vui lòng thử lại."); 
        }
    });
});
