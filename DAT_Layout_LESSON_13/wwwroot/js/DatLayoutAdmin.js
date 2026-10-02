// DatLayoutAdmin.js – đánh dấu menu Admin đang được chọn
$(function () {
    var datUrl = location.pathname.toLowerCase().replace(/\/$/, "");
    if (datUrl === "/admin" || datUrl === "/admin/datdashboard") {
        datUrl = "/admin/datdashboard/datindex";
    }
    if (datUrl === "/admin/datproductmanage") {
        datUrl = "/admin/datproductmanage/datindex";
    }

    $("#dat-sidebar ul li a").each(function () {
        $(this).toggleClass("dat-active", $(this).attr("href").toLowerCase() === datUrl);
    });
});
