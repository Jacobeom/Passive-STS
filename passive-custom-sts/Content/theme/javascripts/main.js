(function($){
  $(document).ready(function(){

    var resizeVideos = function(){
      $('div.full-video').each(function(){
        var $this = $(this)
        var $iframe = $this.find('iframe');
        var $parent = $($this.parent());
        var width = $this.width();
        var height = width * 9 / 16;
        $iframe.attr({
          width: width,
          height: height
        });
      });

  
        $('#body-background').width($(window).width());
        $('#body-background').height($(window).height());
		$('#body-background').css('background-image', 'url(/sites/people/Style Library/theme/img/92545649.jpg)');

      $('#main-content').css({
        left: ( ( $(window).width() - $('#main-content').width() ) / 2 ) + 'px',
        opacity: 1
      });



      if ($(window).width() <= 979){
        $('#home .row.programmes').addClass('cycle-slideshow').cycle();
        $('#home .news-container-inner').addClass('cycle-slideshow').cycle();
        $('#home .projects-container').addClass('cycle-slideshow').cycle();
        $('#page-projects .logos').addClass('cycle-slideshow').cycle();

        var navigation = responsiveNav("#nav", { // Selector: The ID of the wrapper
          animate: true, // Boolean: Use CSS3 transitions, true or false
          transition: 400, // Integer: Speed of the transition, in milliseconds
          label: "Menu", // String: Label for the navigation toggle
          insert: "after", // String: Insert the toggle before or after the navigation
          customToggle: "#toogle-menu", // Selector: Specify the ID of a custom toggle
          openPos: "relative", // String: Position of the opened nav, relative or static
          jsClass: "js", // String: 'JS enabled' class which is added to <html> el
          init: function(){}, // Function: Init callback
          open: function(){}, // Function: Open callback
          close: function(){} // Function: Close callback
        });
      }
    };

    
    

    resizeVideos();

    $(window).on('resize', resizeVideos);

    // if (window.frontpageItems && window.frontpageItems.length > 0){
    //   var collection = new Backbone.Collection(frontpageItems);
    //   console.log('collection', collection);
    // }

  });

  $('.rolling-text .more, .rolling-text .less').on('click', function(e){
    
    var $rollingtext = $(this).closest('.rolling-text');
    $rollingtext.toggleClass('open');
    
    var $viewport = $(this).siblings('.viewport');

    var h = 180;
    if ($rollingtext.hasClass('open')){
      h = $viewport.find('.content').height() + 30; 
      setTimeout(function(){
        $rollingtext.find('.shadow').css({display: 'none'});
      }, 1000);
    }else{
      $rollingtext.find('.shadow').css({display: 'block'});
    }

    $viewport.css({ height: h+'px' });
  });

  var app = angular.module('tid', ['ngSanitize']);
  
  var safeApply = function safeApply(scope, fn) {
    (scope.$$phase || scope.$root.$$phase) ? fn() : scope.$apply(fn);
  };

  angular.module('tid')
    .directive('carrousel', function () {
      return {
        restrict: 'EA',
        link: function postLink($scope, $element, $attrs){
          var container = $element.find('.viewport ul');
          var items = $element.find('.viewport ul li');

          console.log('scope', $scope);
          $scope.currentIndex = 0;

          var resize = function(e){
            var width = $element.width();
            items = container.find('li');
            items.css({
              width: width+'px'
            });

            container.css({
              width: (items.length * width)+'px'
            });

            var pagWidth = (items.length * 14 );
            $element.find('.pagination ul').css({
              width: pagWidth+'px'
            });

            $element.find('.pagination').css({
              left: ((width - pagWidth) / 2) + 'px'
            });
            
          };

          $(window).on('resize', resize);

          setTimeout(resize, 100);
          
          $scope.goto = function(index){
            

            if (index < 0 ){
              index = items.length - 1;
            }else if (index > items.length - 1){
              index = 0;
            }
            
            safeApply($scope, function(){
              $scope.currentIndex = index;
            })
          }

          $scope.next = function(){
            $scope.goto($scope.currentIndex + 1);
          }

          $scope.$watch('currentIndex', function(data, oldData){
            
            if (data === oldData) return;

            if (!$scope.$eval($attrs.showFirst)){
              if (data === 0){
                $scope.currentIndex = data = 1;
              }
            }

            var item = $(items[data]);
            var pos = data  * $element.width();

            requestAnimationFrame(function(){
              
              if ($scope.currentIndex != 0){
                container.animo({animation: 'fadeOutLeft', duration: 0.6 }, function(){
                  container.css({
                    left: -pos+'px'
                  }).animo({animation: 'fadeInRight', duration: 0.5});
                });
               
              }
              lastTime = new Date().getTime();
            });

          })

          var startTime = new Date().getTime();
          var intervalTime = $scope.$eval($attrs.time) || 5;
          var lastTime = new Date().getTime()
          var frametime = new Date().getTime();
          console.log('interval', $scope.time, intervalTime);
          var canvas = $element.find('.next canvas')[0],
              ctx, canvasWidth, canvasHeight;
          if (canvas){
            ctx = canvas.getContext('2d');
            canvasWidth = canvas.width;
            canvasHeight = canvas.height;
          }
          var lastDrawTime = new Date().getTime();
          var enterframe = function(){
            frameTime = new Date().getTime();

            if (frameTime - lastTime <= intervalTime * 1000){
              if (canvas && frameTime - lastDrawTime > intervalTime * 1000 / 30){
                var pct = (frameTime - lastTime) / (intervalTime * 1000);
                var startAngle = -Math.PI / 2;
                var endAngle = startAngle + pct * 2 * Math.PI;
                var w = canvasWidth;
                var h = canvasHeight;
                ctx.clearRect(0, 0, w, h);

                ctx.beginPath()
                // ctx.lineWidth = 1;
                // ctx.strokeStyle = "rgba(255, 255,255, 0.6)";
                // ctx.fillStyle = "rgba(0, 0, 0, 0.6)";
                // ctx.arc( w / 2, h / 2, w / 2 - 4, startAngle, startAngle + 2 * Math.PI, false);
                // ctx.stroke();
                // ctx.fill();
                ctx.beginPath();
                ctx.lineWidth = 2;
                ctx.strokeStyle = "#35f799";
                ctx.arc( w / 2, h / 2, w / 2 - 4, startAngle, endAngle, false);
                ctx.stroke();
                lastDrawTime = frameTime;
              }
              
            }else{
              $scope.next();
            }

            requestAnimationFrame(enterframe);
          };

          requestAnimationFrame(enterframe);

          $scope.gotoSection = function(item){
            if (item.youtube_id){
              window.open("http://www.youtube.com/watch?v=" + item.youtube_id);  
            }else if (item.vimeo_id){
              window.open("http://vimeo.com/" + item.vimeo_id);  
            }else if (item.sectionUrl.indexOf('http://') > -1){
              window.open(item.sectionUrl);
            }else{
              location.href = item.sectionUrl  
            }
            
          };

        },
        // controller: function($scope, $element, $attrs){
          


        // }
      };
    });


  // app.directive('carrousel', ['', function(){
  //   // Runs during compile
  //   return {
  //     // name: '',
  //     // priority: 1,
  //     // terminal: true,
  //     // scope: {}, // {} = isolate, true = child, false/undefined = no change
  //     // cont­rol­ler: function($scope, $element, $attrs, $transclue) {},
  //     // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
  //     // restrict: 'A', // E = Element, A = Attribute, C = Class, M = Comment
  //     // template: '',
  //     // templateUrl: '',
  //     // replace: true,
  //     // transclude: true,
  //     // compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
  //     link: function($scope, iElm, iAttrs, controller) {
  //       iElm.html('directive')
  //     }
  //   };
  // }]);

})(jQuery)
;
