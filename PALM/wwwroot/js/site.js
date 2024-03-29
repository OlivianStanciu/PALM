﻿!function (e) { if ("function" == typeof define && define.amd) define(e); else if ("object" == typeof exports) module.exports = e(); else { var n = window.Cookies, o = window.Cookies = e(window.jQuery); o.noConflict = function () { return window.Cookies = n, o } } }(function () { function e() { for (var e = 0, n = {}; e < arguments.length; e++) { var o = arguments[e]; for (var t in o) n[t] = o[t] } return n } function n(o) { function t(n, r, i) { var c; if (arguments.length > 1) { if (i = e({ path: "/" }, t.defaults, i), "number" == typeof i.expires) { var s = new Date; s.setMilliseconds(s.getMilliseconds() + 864e5 * i.expires), i.expires = s } try { c = JSON.stringify(r), /^[\{\[]/.test(c) && (r = c) } catch (a) { } return r = encodeURIComponent(String(r)), r = r.replace(/%(23|24|26|2B|3A|3C|3E|3D|2F|3F|40|5B|5D|5E|60|7B|7D|7C)/g, decodeURIComponent), n = encodeURIComponent(String(n)), n = n.replace(/%(23|24|26|2B|5E|60|7C)/g, decodeURIComponent), n = n.replace(/[\(\)]/g, escape), document.cookie = [n, "=", r, i.expires && "; expires=" + i.expires.toUTCString(), i.path && "; path=" + i.path, i.domain && "; domain=" + i.domain, i.secure ? "; secure" : ""].join("") } n || (c = {}); for (var p = document.cookie ? document.cookie.split("; ") : [], u = /(%[0-9A-Z]{2})+/g, d = 0; d < p.length; d++) { var f = p[d].split("="), l = f[0].replace(u, decodeURIComponent), m = f.slice(1).join("="); '"' === m.charAt(0) && (m = m.slice(1, -1)); try { if (m = o && o(m, l) || m.replace(u, decodeURIComponent), this.json) try { m = JSON.parse(m) } catch (a) { } if (n === l) { c = m; break } n || (c[l] = m) } catch (a) { } } return c } return t.get = t.set = t, t.getJSON = function () { return t.apply({ json: !0 }, [].slice.call(arguments)) }, t.defaults = {}, t.remove = function (n, o) { t(n, "", e(o, { expires: -1 })) }, t.withConverter = n, t } return n() });


		// Run the cookie notification functions once the page has loaded
		$( document ).ready( function() {
			cookieNotification();
			hideCookieNotification();
		});

		var hideCookieNotification = function() {

			// Hide the	cookie notification after 5 sec 
			//$('.js-cookie-notification').delay(5000).fadeOut("slow");
	
		// Set a cookie
			Cookies.set('CookieNotificationCookie', 'true', {expires: 365 });

	};

		var cookieNotification = function() {

			var setCookieNotification = function() {

			// Hide the	cookie notification	  
			$('.js-cookie-notification').fadeOut("slow");
	   
			// Set a cookie
			  Cookies.set('CookieNotificationCookie', 'true', {expires: 365 });

		// Stop the page reloading
		  return false;

	  };

	  // Check to see if a cookie notification has been set
			if ( Cookies.get('CookieNotificationCookie') === 'true' ) {

			// Tell me a cookie has been set
			console.log('cookie notification set');

			} else {

			// Tell me a cookie has not been set
			console.log('cookie notification not set');

		// Show cookie notification
				$('.js-cookie-notification').css({'display' : 'block'});

		// Hide cookie notification if link clicked
		$('.js-cookie-notification').find('.js-cookie-notification-hide').click( setCookieNotification );

	};

}



