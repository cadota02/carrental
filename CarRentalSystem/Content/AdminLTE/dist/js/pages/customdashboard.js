$(function () {

  'use strict'

  // Make the dashboard widgets sortable Using jquery UI
  $('.connectedSortable').sortable({
    placeholder         : 'sort-highlight',
    connectWith         : '.connectedSortable',
    handle              : '.card-header, .nav-tabs',
    forcePlaceholderSize: true,
    zIndex              : 999999
  })
  $('.connectedSortable .card-header, .connectedSortable .nav-tabs-custom').css('cursor', 'move')

  // jQuery UI sortable for the todo list
  $('.todo-list').sortable({
    placeholder         : 'sort-highlight',
    handle              : '.handle',
    forcePlaceholderSize: true,
    zIndex              : 999999
})
})


//monthly issued items
$(function () {
var areaChartCanvas = $('#areaChart').get(0).getContext('2d');
  var salesChartData = {
    labels  : ['Janu', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul' , 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [
      {
        label               : 'Previous',
        backgroundColor     : 'rgba(60,141,188,0.9)',
        borderColor         : 'rgba(60,141,188,0.8)',
        pointRadius          : false,
        pointColor          : '#3b8bba',
        pointStrokeColor    : 'rgba(60,141,188,1)',
        pointHighlightFill  : '#fff',
        pointHighlightStroke: 'rgba(60,141,188,1)',
        data                : currentyear_data
      },
      {
        label               : 'Current',
        backgroundColor     : 'rgba(210, 214, 222, 1)',
        borderColor         : 'rgba(210, 214, 222, 1)',
        pointRadius         : false,
        pointColor          : 'rgba(210, 214, 222, 1)',
        pointStrokeColor    : '#c1c7d1',
        pointHighlightFill  : '#fff',
        pointHighlightStroke: 'rgba(220,220,220,1)',
        data                : prevyear_data
      },
    ]
  }

  var salesChartOptions = {
    maintainAspectRatio : false,
    responsive : true,
    legend: {
      display: false
    },
    scales: {
      xAxes: [{
        gridLines : {
          display : false,
        }
      }],
      yAxes: [{
        gridLines : {
          display : false,
        }
      }]
    }
  }

  // This will get the first returned node in the jQuery collection.
  var salesChart = new Chart(areaChartCanvas, { 
      type: 'line', 
      data: salesChartData, 
      options: salesChartOptions
    }
  )

 
 })


 // monthly inventory andissuance, old dispensing
 $(function () { 
   var areaChartData = { 
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
   datasets: [ 
   { 
      label: 'New Stock', 
    backgroundColor: '#00a65a', 
    borderColor: 'rgba(220, 53, 69)', 
    pointRadius: false, 
    pointColor: 'rgba(220, 53, 69)', 
    pointStrokeColor: '#c1c7d1', 
    pointHighlightFill: '#fff', 
    pointHighlightStroke: 'rgba(220, 53, 69)', 
     data: newstock_data
  }, 
   { 
     label: 'Dispensing', 
     backgroundColor: 'rgba(232, 27, 44)', 
     borderColor: 'rgba(60,141,188,0.8)', 
     pointRadius: false, 
     pointColor: '#3b8bba', 
     pointStrokeColor: 'rgba(60,141,188,1)', 
     pointHighlightFill: '#fff', 
     pointHighlightStroke: 'rgba(60,141,188,1)', 
     data: dispensing_data
  
  }, 
  { 
      label: 'PTR', 
     backgroundColor: 'rgba(60,141,188,0.9)', 
    borderColor: 'rgba(60,141,188,0.8)', 
    pointRadius: false, 
   pointColor: '#3b8bba', 
   pointStrokeColor: 'rgba(60,141,188,1)', 
   pointHighlightFill: '#fff', 
   pointHighlightStroke: 'rgba(60,141,188,1)', 
    data: ptr_data
    //data: [0, 0, 0, 3, 8, 3, 0, 0, 0, 0, 0, 0] 
  }, 
  { 
     label: 'RIS', 
     backgroundColor: 'rgba(255, 193, 7)', 
     borderColor: 'rgba(255, 193, 7)', 
    pointRadius: false, 
    pointColor: 'rgba(255, 193, 7)', 
    pointStrokeColor: '#c1c7d1', 
    pointHighlightFill: '#fff', 
   pointHighlightStroke: 'rgba(255, 193, 7)', 
    data: ris_data
   }, 
   
  ] 
       } 
  var barChartCanvas = $('#barChart').get(0).getContext('2d') 
   var barChartData = jQuery.extend(true, {}, areaChartData) 
  var temp0 = areaChartData.datasets[0] 
  var temp1 = areaChartData.datasets[1]
  var temp2 = areaChartData.datasets[2]  
  barChartData.datasets[0] = temp0 
  barChartData.datasets[1] = temp1 
   barChartData.datasets[2] = temp2 
  var barChartOptions = { 
      responsive: true, 
     maintainAspectRatio: false, 
     datasetFill: false 
 } 
  var barChart = new Chart(barChartCanvas, { 
    type: 'bar', 
    data: barChartData, 
    options: barChartOptions 
  }) 
 }) 


 //donut char top 10 common used commodity
  $(function () 
 { 
    var donutChartCanvastop10 = $('#donutChart_top10').get(0).getContext('2d') 
  
 var donutDatatop10 =  { 
 labels:  top_descdata,  
          datasets: [ 
                     {  
                     data:  top_countdata,  backgroundColor : ['rgb(247, 43, 43)', 'rgba(247, 43, 43, 0.9)', 'rgba(247, 43, 43, 0.8)', 'rgba(247, 43, 43, 0.7)', 'rgba(247, 43, 43, 0.6)', 'rgba(247, 43, 43, 0.5)', 'rgba(247, 43, 43, 0.4)', 'rgba(247, 43, 43, 0.3)', 'rgba(247, 43, 43, 0.2)', 'rgba(247, 43, 43, 0.1)']
                     } 
                      ] 
                }  
 var donutOptionss = { 
 maintainAspectRatio : false, 
 responsive : true
 }  
 var donutCharttop10 = new Chart(donutChartCanvastop10, { 
 type: 'doughnut', 
 data: donutDatatop10, 
 options: donutOptionss 
 })
  }) 
   //end donut char top 10 common used commodity



   //transaction logs
$(function () { 

  'use strict'

  var ticksStyle = {
    fontColor: '#495057',
    fontStyle: 'bold'
  }

  var mode      = 'index'
  var intersect = true
  var $visitorsChart = $('#visitors-chart')
  var visitorsChart  = new Chart($visitorsChart, {
    data   : {
      labels  : ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
      datasets: [{
        type                : 'line',
        data                : newtranslog_data ,
        backgroundColor     : 'transparent',
        borderColor         : '#007bff',
        pointBorderColor    : '#007bff',
        pointBackgroundColor: '#007bff',
        fill                : false
        // pointHoverBackgroundColor: '#007bff',
        // pointHoverBorderColor    : '#007bff'
      },
        {
          type                : 'line',
          data                : oldtranslog_data ,
          backgroundColor     : 'tansparent',
          borderColor         : '#ced4da',
          pointBorderColor    : '#ced4da',
          pointBackgroundColor: '#ced4da',
          fill                : false
          // pointHoverBackgroundColor: '#ced4da',
          // pointHoverBorderColor    : '#ced4da'
        }]
    },
    options: {
      maintainAspectRatio: false,
      tooltips           : {
        mode     : mode,
        intersect: intersect
      },
      hover              : {
        mode     : mode,
        intersect: intersect
      },
      legend             : {
        display: false
      },
      scales             : {
        yAxes: [{
          // display: false,
          gridLines: {
            display      : true,
            lineWidth    : '4px',
            color        : 'rgba(0, 0, 0, .2)',
            zeroLineColor: 'transparent'
          },
          ticks    : $.extend({
            beginAtZero : true,
            suggestedMax: 200
          }, ticksStyle)
        }],
        xAxes: [{
          display  : true,
          gridLines: {
            display: false
          },
          ticks    : ticksStyle
        }]
      }
    }
  })
  })

   //end transaction logs

 
 