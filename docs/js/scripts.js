// Detect IE version
var ie = (function () {

    var undef,
        v = 3,
        div = document.createElement('div'),
        all = div.getElementsByTagName('i');

    while (
        div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->',
        all[0]
    );

    return v > 4 ? v : undef;

}());

(function ($, undefined) {


    var initCommon = function(){

        // Init common sticky elements
        if (!ie || ie > 9)
        {
            $(".sticky").each(function (idx, itm) {
                $(itm).stick_in_parent({
                    inner_scrolling: false
                });
            });
        }

    }

    var initDesktop = function(){

        //============================================
        // Undo mobile
        //============================================

        // Undo sticky mobile
        $(".sticky--mobile").trigger("sticky_kit:detach");

        // Undo any mobile nav styles
        $(".main-nav ul").css("display", "");

        // Cancel mobile events handlers
        $("*").off(".fl-mobile");

        //============================================
        // Setup desktop
        //============================================

        // Sticky desktop elements
        if (!ie || ie > 9)
        {
            $(".sticky--desktop").each(function (idx, itm) {
                $(itm).stick_in_parent({
                    offset_top: 40
                });
            });
        }
    }

    var initMobile = function(){
        
        //============================================
        // Undo desktop
        //============================================

        // Undo sticky desktop
        $(".sticky--desktop").trigger("sticky_kit:detach");
        
        // Cancel desktop events handlers
        $("*").off(".fl-desktop");

        //============================================
        // Setup mobile
        //============================================

        // Sticky mobile elements
        if (!ie || ie > 9)
        {
            $(".sticky--mobile").each(function (idx, itm) {
                $(itm).stick_in_parent({
                    offset_top: 20,
                });
            });
        }

        // Mobile nav
        $("body").on("click.fl-mobile", ".nav-toggle", function(e){
            e.preventDefault();
            $(".main-nav ul").slideToggle("fast");
        });
    }
    
    // Register media query handlers
    $(function () {

        initCommon();

        enquire.register('screen and (min-width: 960px)', {
            match: function() {
                initDesktop();
            }
        });

        enquire.register('screen and (max-width: 960px)', {
            match: function() {
                initMobile();
            }
        });

    });

})(jQuery);