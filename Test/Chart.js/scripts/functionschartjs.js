
let myChart

function ChartBasico() {
    const ctx = InitChart();
    myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
            datasets: [{
                label: '# of Votes',
                data: [12, 19, 3, 5, 2, 3],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

function ChartTemporal() {
    const data = {
        datasets: [{
            label: 'Dataset with string point data',
            backgroundColor: Utils.transparentize(Utils.CHART_COLORS.red, 0.5),
            borderColor: Utils.CHART_COLORS.red,
            fill: false,
            data: [{
                x: Utils.newDateString(0),
                y: Utils.rand(0, 100)
            }, {
                x: Utils.newDateString(2),
                y: Utils.rand(0, 100)
            }, {
                x: Utils.newDateString(4),
                y: Utils.rand(0, 100)
            }, {
                x: Utils.newDateString(6),
                y: Utils.rand(0, 100)
            }],
        }, {
            label: 'Dataset with date object point data',
            backgroundColor: Utils.transparentize(Utils.CHART_COLORS.blue, 0.5),
            borderColor: Utils.CHART_COLORS.blue,
            fill: false,
            data: [{
                x: Utils.newDate(0),
                y: Utils.rand(0, 100)
            }, {
                x: Utils.newDate(2),
                y: Utils.rand(0, 100)
            }, {
                x: Utils.newDate(5),
                y: Utils.rand(0, 100)
            }, {
                x: Utils.newDate(6),
                y: Utils.rand(0, 100)
            }]
        }]
    };
    const config = {
        type: 'line',
        data: data,
        options: {
            spanGaps: 1000 * 60 * 60 * 24 * 2, // 2 days
            responsive: true,
            interaction: {
                mode: 'nearest',
            },
            plugins: {
                title: {
                    display: true,
                    text: 'Chart.js Time - spanGaps: 172800000 (2 days in ms)'
                },
            },
            scales: {
                x: {
                    type: 'time',
                    display: true,
                    title: {
                        display: true,
                        text: 'Date'
                    },
                    ticks: {
                        autoSkip: false,
                        maxRotation: 0,
                        major: {
                            enabled: true
                        },
                        // color: function(context) {
                        //   return context.tick && context.tick.major ? '#FF0000' : 'rgba(0,0,0,0.1)';
                        // },
                        font: function (context) {
                            if (context.tick && context.tick.major) {
                                return {
                                    weight: 'bold',
                                };
                            }
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'value'
                    }
                }
            }
        },
    };
    const ctx = InitChart();
    myChart = new Chart(ctx, config);
}


function InitChart() {
    let ctx = document.getElementById('myChart');
    if (myChart) {
        myChart.destroy();
    }
    return ctx;
}