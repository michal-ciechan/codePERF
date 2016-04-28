var formatter = new Intl.NumberFormat('en-GB', {
    style: 'currency',
    currency: 'GBP',
    minimumFractionDigits: 0,
    maximumFractionDigits: 0
});

export const formats = {
    Integer: (value = 0) => {
        var res = Number(value).toFixed(0);
        return res;
    },
    Currency: (value) => {
        return formatter.format(value);
    },
    Percent: (value) => {
        return value + "%";
    },
}
