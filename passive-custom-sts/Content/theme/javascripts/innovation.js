$(document).ready(function(){

  // ---
  // EVENTS

  // anchors
  $('.step-0 .phase.one').click( function(){ smoothScroll($('#identify').offset().top); });
  $('.step-0 .phase.two').click( function(){ smoothScroll($('#define').offset().top); });
  $('.step-0 .phase.three').click( function(){ smoothScroll($('#refine').offset().top); });
  $('.step-0 .phase.four').click( function(){ smoothScroll($('#deliver').offset().top); });

  $('.step-1 .sign').click( function(){ smoothScroll($('#identify').offset().top); });
  $('.step-1 .go-to-next-step').click( function(){ smoothScroll($('#define').offset().top); });

  $('.step-2 .go-to-next-step').click( function(){ smoothScroll($('#refine').offset().top); });  
  $('.step-2 .go-to-step-1').click( function(){ 
    smoothScroll($('#identify').offset().top);
    showTrail(1);    
  });  

  $('.step-3 .go-to-next-step').click( function(){ smoothScroll($('#deliver').offset().top); });  
  $('.step-3 .go-to-step-1').click( function(){ 
    smoothScroll($('#identify').offset().top);
    showTrail(2);
  });

  $('.anchor').mouseover( function(){ $(this).css('color', '#003246') });
  $('.anchor').mouseout( function(){ $(this).css('color', 'white') });

  $('.more-info .button').click( function(){ showPopOver(this) });

  // ---
  // VARS

  var controller = $.superscrollorama();
  var offset;
  var delay;
  var time;

  // ---
  // INTRO

  // initial tween
  delay = 1;
  TweenMax.from($('.blue-flag'), 1, {delay:delay, ease:Expo.easeOut, css:{top:-80, opacity:0}});
  TweenMax.from($('.blue-flag h1'), .5, {delay:delay*.75, ease:Expo.easeIn, css:{opacity:0}});
  TweenMax.from($('.blue-flag h2'), .5, {delay:delay*.75, ease:Expo.easeIn, css:{opacity:0}});

  TweenMax.from($('.step-0 .intro'), 1, {delay:delay, ease:Expo.easeOut, css:{top:-290, opacity:0}});
  TweenMax.from($('.step-0 .title'), 1, {delay:delay, ease:Expo.easeOut, css:{top:-80, opacity:0}});

  // icons
  delay = 1.25;
  time = .5;
  TweenMax.from($('.phases .phase.one'), time, {delay:delay, ease:Cubic.easeOut, css:{marginLeft:-433, opacity:0}});
  TweenMax.from($('.phases .phase.two'), time, {delay:delay, ease:Expo.easeOut, css:{marginLeft:-206, opacity:0}});
  TweenMax.from($('.phases .phase.three'), time, {delay:delay, ease:Expo.easeOut, css:{marginLeft:62, opacity:0}});
  TweenMax.from($('.phases .phase.four'), time, {delay:delay, ease:Cubic.easeOut, css:{marginLeft:279, opacity:0}});  


  // ---
  // PHASE 1 - IDENTIFY
  offset = -300;

  var links = $('.step-1 .network .link');
  for (var i=0; i<links.length; i++) {
    // var link = links[i];
    var $link = $(links[i]);
    var delay = Math.random()*1;
    
    controller.addTween($link, 
      TweenMax.from($link, .5, {delay:delay, ease:Expo.easeOut, css:{scale:0, opacity:0}}), 0, offset
    );
  }

  var nodes = $('.step-1 .node');
  var nodeTitles = $('.step-1 .node-title');
  for (var i=0; i<nodes.length; i++) {
    var delay = Math.random()*1;

    var $node = $(nodes[i]);
    controller.addTween($node, 
      TweenMax.from($node, .5, {delay:delay, ease:Expo.easeOut, css:{left:182, top:160, opacity:0}}), 0, offset
    );

    var $nodeTitle = $(nodeTitles[i]);
    controller.addTween($nodeTitle, 
      TweenMax.from($nodeTitle, .5, {delay:delay, ease:Expo.easeIn, css:{opacity:0}}), 0, offset
    );
  }

  controller.addTween('.step-1 .title', 
    TweenMax.from($('.step-1 .title'), .5, {delay:1.15, ease:Cubic.easeIn, css:{opacity:0}}), 0, offset
  );

  controller.addTween('.step-1 .more-info', 
    TweenMax.from($('.step-1 .more-info'), .5, {delay:1.5, ease:Cubic.easeIn, css:{opacity:0}}), 0, offset
  );


  // ---
  // PHASE 2 - DEFINE

  var pieces = $('.step-2 .piece');
  var pieceTitles = $('.step-2 .piece-title');
  for (var i=0; i<pieces.length; i++) {    
    var delay = i*.15;
        
    var $piece = $(pieces[i]);
    controller.addTween($piece, 
      TweenMax.from($piece, .5, {delay:delay, ease:Expo.easeOut, css:{top:-170, opacity:0}}), 0, offset
    );
            
    var $pieceTitle = $(pieceTitles[i]);
    controller.addTween($pieceTitle, 
      TweenMax.from($pieceTitle, .5, {delay:delay, ease:Expo.easeIn, css:{opacity:0}}), 0, offset
    );
  }

  controller.addTween('.step-2 .title', 
    TweenMax.from($('.step-2 .title'), .5, {delay:1, ease:Cubic.easeIn, css:{opacity:0}}), 0, offset
  );

  controller.addTween('.step-2 .more-info', 
    TweenMax.from($('.step-2 .more-info'), .5, {delay:1.5, ease:Cubic.easeIn, css:{opacity:0}}), 0, offset
  );


  // ---
  // PHASE 3 - REFINE
  controller.addTween('.step-3 .cycle-background', 
    TweenMax.from($('.step-3 .cycle-background'), .5, {ease:Expo.easeOut, css:{rotation:-360, opacity:0}})
  );

  var cycleTitles = $('.step-3 .cycle-title');
  for (var i=0; i<cycleTitles.length; i++) {    
    var delay = .15;
        
    var $cycleTitle = $(cycleTitles[i]);
    controller.addTween($cycleTitle, 
      TweenMax.from($cycleTitle, .5, {delay:delay, ease:Expo.easeOut, css:{rotation:-180, opacity:0}})
    );
  }

  controller.addTween('.step-3 .title', 
    TweenMax.from($('.step-3 .title'), .5, {delay:1, ease:Cubic.easeIn, css:{opacity:0}})
  );

  controller.addTween('.step-3 .more-info', 
    TweenMax.from($('.step-3 .more-info'), .5, {delay:1.5, ease:Cubic.easeIn, css:{opacity:0}}), 0, offset
  );


  // ---
  // PHASE 4 - DELIVER

  controller.addTween('.step-4 .deliver-head', 
    TweenMax.from($('.step-4 .deliver-head'), .5, {ease:Expo.easeOut, css:{scale:0, opacity:0}}), 0, offset
  );

  var deliverPieces = $('.step-4 .deliver-piece');
  var deliverTitles = $('.step-4 .deliver-title');
  for (var i=0; i<deliverPieces.length; i++) {    
    var delay = .25+i*.075;
        
    var $deliverPiece = $(deliverPieces[i]);
    controller.addTween($deliverPiece, 
      TweenMax.from($deliverPiece, .75, {delay:delay, ease:Back.easeInOut, css:{top:-100, opacity:0}})
    ); 

    var $deliverTitle = $(deliverTitles[i]);
    controller.addTween($deliverTitle, 
      TweenMax.from($deliverTitle, .5, {delay:delay, ease:Expo.easeIn, css:{opacity:0}}), 0, offset-100
    );
  }

  controller.addTween('.step-4 .title', 
    TweenMax.from($('.step-4 .title'), .5, {delay:.75, ease:Cubic.easeIn, css:{opacity:0}}), 0, offset
  );

  controller.addTween('.step-4 .more-info', 
    TweenMax.from($('.step-4 .more-info'), .5, {delay:1.5, ease:Cubic.easeIn, css:{opacity:0}}), 0, offset
  );


  // ---
  // FUNCTIONS

  function showPopOver (element) {
    var $el = $(element);
    var $popOver = $($el.next());
    $popOver.toggleClass('hidden');

    var r = $popOver.hasClass('hidden')? 0:135;
    TweenMax.to($el, .5, {ease:Expo.easeOut, css:{rotation:r }});
  }

  function smoothScroll (value) { 
    TweenMax.to($('body'), 2, {ease:Expo.easeInOut, scrollTop:value-50});
  }

  function showTrail (trailID) {
    var $trail = $('.learning-trail-'+trailID);
    $trail.removeClass('hidden');
    TweenMax.from($trail, .5, {ease:Cubic.easeIn, opacity:0});
    TweenMax.to( $('.node-title-4'), .75, {delay:1.75, opacity:.2, repeat:5, yoyo:true});    

    if (trailID == 2) { $('.learning-trail-1').removeClass('hidden'); }
  }

});
