$(document).ready(function () {
    // Bước 1: Gọi API để lấy dữ liệu doanh thu
    $.ajax({
        type: "GET",
        url: "/Admin/Dashboard/GetMonthlyRevenueData",
        success: function (data) {
            // Bước 2: Sử dụng dữ liệu để vẽ biểu đồ
            var ctx = document.getElementById("lineChart").getContext('2d');

            var chartData = {
                labels: data.map(item => item.Month),
                datasets: [{
                    label: 'Doanh thu hàng tháng',
                    data: data.map(item => item.Revenue),
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 1,
                    fill: false
                }]
            };

            var chartOptions = {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            };

            var lineChart = new Chart(ctx, {
                type: 'line',
                data: chartData,
                options: chartOptions
            });
        }
    });

});