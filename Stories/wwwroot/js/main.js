(function($) {
    'use strict';

    // Page loading
    $(window).on('load', function() {
        $('.preloader').delay(450).fadeOut('slow');
    });

    $(".search-icon").click(function () {
        if ($("body").hasClass("open-search-form")) {
            $('input[name="keyword"]').blur(); 
        }
        else {
            $('input[name="keyword"]').focus();
        }
    });

    $("#agree-subscribe").change(function () {
        if ($(this).is(':not(:checked)')) {
            $(".form-subcriber .btn").addClass("disabled");
        }
        else {
            $(".form-subcriber .btn").removeClass("disabled");
        }
    });

    // Block Somee ads https://stackoverflow.com/questions/18999611/remove-auto-generated-advertisement-script-appended-to-the-results-returned-by-a
    function blockSomeeAds() {
        $("div[style='opacity: 0.9; z-index: 2147483647; position: fixed; left: 0px; bottom: 0px; height: 65px; right: 0px; display: block; width: 100%; background-color: #202020; margin: 0px; padding: 0px;']").remove();
        $("div[style='margin: 0px; padding: 0px; left: 0px; width: 100%; height: 65px; right: 0px; bottom: 0px; display: block; position: fixed; z-index: 2147483647; opacity: 0.9; background-color: rgb(32, 32, 32);']").remove();
        $("div[onmouseover='S_ssac();']").remove();
        $("center").remove();
        $("div[style='height: 65px;']").remove();
    };

    // Load Category
    function LoadLayout() {
        $.ajax({
            url: "/Blog/GetLayoutResponse",
            method: "GET",
            dataType: "json",
            success: function (data) {
                var categories = data.categories;
                var suggestedCategories = data.suggestedCategories;
                var hotTopics = data.hotTopics;
                var dontMiss = data.dontMiss;
                var ad = data.ad;
                var footerPosts = data.footerPosts;
                var tagCloud = data.tagCloud;

                var html = ' <li class="menu-item-has-children"><a href="/"><i class="elegant-icon icon_house_alt mr-5"></i>Trang chủ</a></li>';
                var html2 = "";
                var html3 = "";
                var html4 = "";
                var html5 = "";
                var footer = "";
                var html6 = "";
                var html7 = "";

                $(categories).each(function (index, cat) {
                    html += '<li><a href="/Blog/Category/' + cat.id + '">' + cat.name + '</a></li>';
                    html2 += '<li><a href="/Blog/Category/' + cat.id + '" role="menuitem" tabindex="0">' + cat.name + '</a></li>';
                    footer += '<div class="carausel-3-columns-item d-flex bg-grey has-border p-25 hover-up-2 transition-normal border-radius-5">'
                           + '<div class="post-thumb post-thumb-64 d-flex mr-15 border-radius-5 img-hover-scale overflow-hidden">'
                           +         '<a class="color-white" href="/Blog/Category/' + cat.id + '">'
                           +             '<img src="' + cat.thumbLink + '" alt="">'
                           +         '</a>'
                           +     '</div>'
                           +     '<div class="post-content media-body">'
                           +         '<h6><a href="/Blog/Category/' + cat.id + '">' + cat.name + '</a></h6>'
                           +         '<p class="text-muted font-small">' + cat.description + '</p>'
                           +     '</div>'
                           + '</div>';

                    if (index == 8) return;
                });

                $(suggestedCategories).each(function (index, cat) {
                    html7 += '<div class="col-lg-4 col-md-6">'
                          +      '<div class="d-flex bg-grey has-border p-25 hover-up-2 transition-normal border-radius-5 mb-30">'
                          +          '<div class="post-thumb post-thumb-64 d-flex mr-15 border-radius-5 img-hover-scale overflow-hidden">'
                          +              '<a class="color-white" href="/Blog/Category/' + cat.id + '">'
                          +                  '<img src="' + cat.thumbLink + '" alt="">'
                          +              '</a>'
                          +          '</div>'
                          +              '<div class="post-content media-body">'
                          +                  '<h6> <a href="/Blog/Category/' + cat.id + '">' + cat.name + '</a> </h6>'
                          +                  '<p class="text-muted font-small">' + cat.description + '</p>'
                          +              '</div>'
                          +          '</div>'
                          +  '</div>';
                });

                $(hotTopics).each(function (index, ht) {
                    html3 += '<li class="cat-item cat-item-6"><a href="/Blog/Category/' + ht.id + '">' + ht.name + '</a> <span class="post-count">' + ht.postCount + '</span></li>';
                });

                $(dontMiss).each(function (index, dm) {
                    html4 += '<li class="mb-30">'
                        + '<div class="d-flex hover-up-2 transition-normal">'
                        +    '<div class="post-thumb post-thumb-80 d-flex mr-15 border-radius-5 img-hover-scale overflow-hidden">'
                        +        '<a class="color-white" href="/Blog/Single/' + dm.link + '">'
                        +            '<img src="' + dm.imageLink + '" alt="">'
                        +            '</a>'
                        +        '</div>'
                        +        '<div class="post-content media-body">'
                        +            '<h6 class="post-title mb-15 text-limit-2-row font-medium"><a href="/Blog/Single/' + dm.link + '">' + dm.title +'</a></h6>'
                        +            '<div class="entry-meta meta-1 float-left font-x-small text-uppercase">'
                        +                '<span class="post-on">' + ConvertToPostDate(dm.createdDate) + '</span>'
                        +                '<span class="post-by has-dot">' + dm.views + ' lượt xem</span>'
                        +            '</div>'
                        +        '</div>'
                        +    '</div>'
                        +'</li>';
                });

                $(footerPosts).each(function (index, fp) {
                    var posts = fp.posts;

                    html5 += '<div class="col-lg-4 col-md-6">'
                          +      '<div class="sidebar-widget widget-latest-posts mb-30 wow fadeInUp animated">'
                          +              '<div class="widget-header-2 position-relative mb-30">'
                          +                  '<h5 class="mt-5 mb-30">' + fp.categoryName + '</h5>'
                          +              '</div>'
                          +              '<div class="post-block-list post-module-1">'
                          +                  '<ul class="list-post">';

                    $(posts).each(function (index, p) {
                        html5 += '<li class="mb-30">'
                              +  '<div class="d-flex hover-up-2 transition-normal">'
                              +  '<div class="post-thumb post-thumb-80 d-flex mr-15 border-radius-5 img-hover-scale overflow-hidden">'
                              +      '<a class="color-white" href="/Blog/Single/' + p.link + '">'
                              +          '<img src="' + p.imageLink + '" alt="">'
                              +                      '</a>'
                              +                  '</div>'
                              +      '<div class="post-content media-body">'
                              +          '<h6 class="post-title mb-15 text-limit-2-row font-medium"><a href="/Blog/Single/' + p.link + '">' + p.title + '</a></h6>'
                              +          '<div class="entry-meta meta-1 float-left font-x-small text-uppercase">'
                              +              '<span class="post-on">' + ConvertToPostDate(p.createdDate) + '</span>'
                              +              '<span class="post-by has-dot">' + p.views + ' lượt xem</span>'
                              +          '</div>'
                              +      '</div>'
                              +  '</div>'
                              +          '</li>';
                    });

                    html5 +=                '</ul>'
                          +             '</div>'
                          +         '</div>'
                          + '</div>';
                });

                $(tagCloud).each(function (index, tag) {
                    html6 += '<a class="tag-cloud-link" href="/Blog/Tag/' + tag + '">' + tag + ' </a>';
                });

                $("#desktop-menu").html(html);
                $(".mobile_menu .slicknav_menu .menu-item-has-children:nth-child(3) ul").html(html2);
                $("#hot-topics").html(html3);
                $("#dont-miss").html(html4);
                $("#footer-posts").html(html5);
                $("#categoriesFooter").html(footer);
                $(".tagcloud").html(html6);
                $("#sugg-categories").html(html7);

                $(".widget-ads a").attr("href", ad.link);
                $(".widget-ads a img").attr("src", ad.imageLink);
                $(".widget-ads span").html($(".widget-ads span").html() + ad.sponsoredBy);

                customSlickSlider();
            }
        });
    }

    function ConvertToPostDate(date) {
        var tmpDate = '';
        var postDate = '';

        var monthString = ["giêng", "hai", "ba", "tư", "năm", "sáu", "bảy", "tám", "chín", "mười", "mười một", "mười hai"];

        tmpDate = date.split("T")[0].split("-");
        postDate = tmpDate[2] + " Tháng " + monthString[parseInt(tmpDate[1]) - 1];


        return postDate;
    }

    // Scroll progress
    var scrollProgress = function() {
        var docHeight = $(document).height(),
            windowHeight = $(window).height(),
            scrollPercent;
        $(window).on('scroll', function() {
            scrollPercent = $(window).scrollTop() / (docHeight - windowHeight) * 100;
            $('.scroll-progress').width(scrollPercent + '%');
        });
    };

    // Off canvas sidebar
    var OffCanvas = function () {
        $('#off-canvas-toggle').on('click', function() {
            $('html, body').toggleClass("canvas-opened");
        });

        $('.dark-mark').on('click', function() {
            $('html, body').removeClass("canvas-opened");
        });
        $('.off-canvas-close').on('click', function() {
            $('html, body').removeClass("canvas-opened");
        });
    };

    // Search form
    var openSearchForm = function() {
        $('button.search-icon').on('click', function() {
            $('body').toggleClass("open-search-form");
            $('.mega-menu-item').removeClass("open");
            $("html, body").animate({ scrollTop: 0 }, "slow");
        });
        $('.search-close').on('click', function() {
            $('body').removeClass("open-search-form");
        });
    };

    // Mobile menu
    var mobileMenu = function() {
        var menu = $('ul#mobile-menu');
        if (menu.length) {
            menu.slicknav({
                prependTo: ".mobile_menu",
                closedSymbol: '+',
                openedSymbol: '-'
            });
        };
    };

    var SubMenu = function() {
        // $(".sub-menu").hide();
        $(".menu li.menu-item-has-children").on({
            mouseenter: function() {
                $('.sub-menu:first, .children:first', this).stop(true, true).slideDown('fast');
            },
            mouseleave: function() {
                $('.sub-menu:first, .children:first', this).stop(true, true).slideUp('fast');
            }
        });
    };

    var WidgetSubMenu = function() {
        //$(".sub-menu").hide();
        $('.menu li.menu-item-has-children').on('click', function() {
            var element = $(this);
            if (element.hasClass('open')) {
                element.removeClass('open');
                element.find('li').removeClass('open');
                element.find('ul').slideUp(200);
            } else {
                element.addClass('open');
                element.children('ul').slideDown(200);
                element.siblings('li').children('ul').slideUp(200);
                element.siblings('li').removeClass('open');
                element.siblings('li').find('li').removeClass('open');
                element.siblings('li').find('ul').slideUp(200);
            }
        });
    };

    // Slick slider
    var customSlickSlider = function() {

        // Slideshow Fade
        $('.slide-fade').slick({
            infinite: true,
            dots: false,
            arrows: true,
            autoplay: false,
            autoplaySpeed: 3000,
            fade: true,
            fadeSpeed: 1500,
            prevArrow: '<button type="button" class="slick-prev"><i class="elegant-icon arrow_left"></i></button>',
            nextArrow: '<button type="button" class="slick-next"><i class="elegant-icon arrow_right"></i></button>',
            appendArrows: '.arrow-cover',
        });

        // carausel 3 columns
        $('.carausel-3-columns').slick({
            dots: false,
            infinite: true,
            speed: 1000,
            arrows: false,
            autoplay: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            loop: true,
            adaptiveHeight: true,
            responsive: [{
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });

        // featured slider 2
        $('.featured-slider-2-items').slick({
            autoplay: true,
            autoplaySpeed: 3000,
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            dots: false,
            fade: true,
            asNavFor: '.featured-slider-2-nav',
        });

        $('.featured-slider-2-nav').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            vertical: true,
            asNavFor: '.featured-slider-2-items',
            dots: false,
            arrows: false,
            focusOnSelect: true,
            verticalSwiping: true
        });

        // featured slider 3
        $('.featured-slider-3-items').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: true,
            dots: false,
            fade: true,
            prevArrow: '<button type="button" class="slick-prev"><i class="elegant-icon arrow_left"></i></button>',
            nextArrow: '<button type="button" class="slick-next"><i class="elegant-icon arrow_right"></i></button>',
            appendArrows: '.slider-3-arrow-cover',
        });
    };

    var typeWriter = function() {
        var TxtType = function(el, toRotate, period) {
            this.toRotate = toRotate;
            this.el = el;
            this.loopNum = 0;
            this.period = parseInt(period, 10) || 2000;
            this.txt = '';
            this.tick();
            this.isDeleting = !1
        };
        TxtType.prototype.tick = function() {
            var i = this.loopNum % this.toRotate.length;
            var fullTxt = this.toRotate[i];
            if (this.isDeleting) {
                this.txt = fullTxt.substring(0, this.txt.length - 1)
            } else {
                this.txt = fullTxt.substring(0, this.txt.length + 1)
            }
            this.el.innerHTML = '<span class="wrap">' + this.txt + '</span>';
            var that = this;
            var delta = 200 - Math.random() * 100;
            if (this.isDeleting) {
                delta /= 2
            }
            if (!this.isDeleting && this.txt === fullTxt) {
                delta = this.period;
                this.isDeleting = !0
            } else if (this.isDeleting && this.txt === '') {
                this.isDeleting = !1;
                this.loopNum++;
                delta = 500
            }
            setTimeout(function() {
                that.tick()
            }, delta)
        };
        window.onload = function() {
            var elements = document.getElementsByClassName('typewrite');
            for (var i = 0; i < elements.length; i++) {
                var toRotate = elements[i].getAttribute('data-type');
                var period = elements[i].getAttribute('data-period');
                if (toRotate) {
                    new TxtType(elements[i], JSON.parse(toRotate), period)
                }
            }
            var css = document.createElement("style");
            css.type = "text/css";
            css.innerHTML = ".typewrite > .wrap { border-right: 0.05em solid #5869DA}";
            document.body.appendChild(css)
        }
    }

    // Nice Select
    var niceSelectBox = function() {
        var nice_Select = $('select');
        if (nice_Select.length) {
            nice_Select.niceSelect();
        }
    };

    //Header sticky
    var headerSticky = function() {
        $(window).on('scroll', function() {
            var scroll = $(window).scrollTop();
            if (scroll < 245) {
                $(".header-sticky").removeClass("sticky-bar");
            } else {
                $(".header-sticky").addClass("sticky-bar");
            }
        });
    };

    // Scroll up to top
    var scrollToTop = function() {
        $.scrollUp({
            scrollName: 'scrollUp', // Element ID
            topDistance: '300', // Distance from top before showing element (px)
            topSpeed: 300, // Speed back to top (ms)
            animation: 'fade', // Fade, slide, none
            animationInSpeed: 200, // Animation in speed (ms)
            animationOutSpeed: 200, // Animation out speed (ms)
            scrollText: '<i class="elegant-icon arrow_up"></i>', // Text for element
            activeOverlay: false, // Set CSS color to display scrollUp active point, e.g '#00FFFF'
        });
    };

    //VSticker
    var VSticker = function() {
        $('#news-flash').vTicker({
            speed: 800,
            pause: 3000,
            animation: 'fade',
            mousePause: false,
            showItems: 1
        });
        $('#date-time').vTicker({
            speed: 800,
            pause: 3000,
            animation: 'fade',
            mousePause: false,
            showItems: 1
        });
    };

    //sidebar sticky
    var stickySidebar = function() {
        $('.sticky-sidebar').theiaStickySidebar();
    };

    //Custom scrollbar
    var customScrollbar = function() {
        var $ = document.querySelector.bind(document);
        var ps = new PerfectScrollbar('.custom-scrollbar');
    };

    //Mega menu
    var megaMenu = function() {
        $('.sub-mega-menu .nav-pills > a').on('mouseover', function(event) {
            $(this).tab('show');
        });
    };

    //magnific Popup
    var magPopup = function() {
        if ($('.play-video').length) {
            $('.play-video').magnificPopup({
                disableOn: 700,
                type: 'iframe',
                mainClass: 'mfp-fade',
                removalDelay: 160,
                preloader: false,
                fixedContentPos: false
            });
        }
    };

    var masonryGrid = function() {
        if ($(".grid").length) {
            // init Masonry
            var $grid = $('.grid').masonry({
                itemSelector: '.grid-item',
                percentPosition: true,
                columnWidth: '.grid-sizer',
                gutter: 0
            });

            // layout Masonry after each image loads
            $grid.imagesLoaded().progress(function() {
                $grid.masonry();
            });
        }
    };

    /* More articles*/
    var moreArticles = function() {
        $.fn.vwScroller = function(options) {
            var default_options = {
                delay: 500,
                /* Milliseconds */
                position: 0.7,
                /* Multiplier for document height */
                visibleClass: '',
                invisibleClass: '',
            }

            var isVisible = false;
            var $document = $(document);
            var $window = $(window);

            options = $.extend(default_options, options);

            var observer = $.proxy(function() {
                var isInViewPort = $document.scrollTop() > (($document.height() - $window.height()) * options.position);

                if (!isVisible && isInViewPort) {
                    onVisible();
                } else if (isVisible && !isInViewPort) {
                    onInvisible();
                }
            }, this);

            var onVisible = $.proxy(function() {
                isVisible = true;

                /* Add visible class */
                if (options.visibleClass) {
                    this.addClass(options.visibleClass);
                }

                /* Remove invisible class */
                if (options.invisibleClass) {
                    this.removeClass(options.invisibleClass);
                }

            }, this);

            var onInvisible = $.proxy(function() {
                isVisible = false;

                /* Remove visible class */
                if (options.visibleClass) {
                    this.removeClass(options.visibleClass);
                }

                /* Add invisible class */
                if (options.invisibleClass) {
                    this.addClass(options.invisibleClass);
                }
            }, this);

            /* Start observe*/
            setInterval(observer, options.delay);

            return this;
        }

        if ($.fn.vwScroller) {
            var $more_articles = $('.single-more-articles');
            $more_articles.vwScroller({ visibleClass: 'single-more-articles--visible', position: 0.55 })
            $more_articles.find('.single-more-articles-close-button').on('click', function() {
                $more_articles.hide();
            });
        }

        $('button.single-more-articles-close').on('click', function() {
            $('.single-more-articles').removeClass('single-more-articles--visible');
        });
    }

    /* WOW active */
    new WOW().init();

    //Load functions
    $(document).ready(function () {
        openSearchForm();
        OffCanvas();
        customScrollbar();
        magPopup();
        scrollToTop();
        headerSticky();
        stickySidebar();
        megaMenu();
        mobileMenu();
        typeWriter();
        WidgetSubMenu();
        scrollProgress();
        masonryGrid();
        niceSelectBox();
        moreArticles();
        VSticker();
        LoadLayout();
        setTimeout(function () { blockSomeeAds(); }, 500);
    });

})(jQuery);