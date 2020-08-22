/// <reference path="../../../jquery-1.10.2.min.js" />

$(document).ready(function () {
    let rand = randomColor.get(4);
    let chtUserData = {
        label: 'تعداد کاربران',
        backgroundColor: rand[0].color,
        borderColor: rand[0].bColor,
        borderWidth: 1,
        data: Object.values(userInDays)
    };

    initBarChart($('#user_in_days'), Object.keys(userInDays), chtUserData);

    let chtOrderData = {
        label: 'تعداد سفارشات',
        backgroundColor: rand[1].color,
        borderColor: rand[1].bColor,
        borderWidth: 1,
        data: Object.values(orderInDays)
    };

    initBarChart($('#order_in_days'), Object.keys(orderInDays), chtOrderData);

    let chtPayCountData = {
        label: 'تعداد پرداخت ها',
        backgroundColor: rand[2].color,
        borderColor: rand[2].bColor,
        borderWidth: 1,
        data: Object.values(payCountInDays)
    };

    initBarChart($('#payment_in_days'), Object.keys(payCountInDays), chtPayCountData);

    let chtPayAmountData = {
        label: 'میزان پرداخت ها',
        backgroundColor: rand[3].color,
        borderColor: rand[3].bColor,
        borderWidth: 1,
        data: Object.values(payAmountInDays),
        fill: false
    };

    initLineChart($('#amount_in_days'), Object.keys(payAmountInDays), chtPayAmountData);
});


