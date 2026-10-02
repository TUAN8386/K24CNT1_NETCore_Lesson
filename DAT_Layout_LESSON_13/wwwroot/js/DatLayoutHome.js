// DatLayoutHome.js – Link nào được click (trang hiện tại) thì link đó đổi màu
$(function () {
    var datUrl = location.pathname.toLowerCase();

    // Trang gốc "/" mặc định là Danh sách sản phẩm
    if (datUrl === "/" || datUrl === "/datproducts" || datUrl === "/datproducts/") {
        datUrl = "/datproducts/datindex";
    }

    $("#dat-navbar ul li a").each(function () {
        if (datUrl === $(this).attr("href").toLowerCase()) {
            $(this).addClass("dat-active");
        } else {
            $(this).removeClass("dat-active");
        }
    });
});
