'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; /* eslint-disable */


exports.default = cryptoProPlugin;

var _utils = require('./utils');

function cryptoProPlugin() {

    if (window.cadesplugin)
        return;

    var pluginObject;
    var plugin_resolved = 0;
    var plugin_reject;
    var plugin_resolve;
    var isOpera = 0;
    var isFireFox = 0;
    var isSafari = 0;
    var isYandex = 0;
    var canPromise = !!window.Promise;
    var cadesplugin_loaded_event_recieved = false;
    var cadesplugin;

    if (canPromise) {
        cadesplugin = new Promise(function (resolve, reject) {
            plugin_resolve = resolve;
            plugin_reject = reject;
        });
    } else {
        cadesplugin = {};
    }

    function check_browser() {
        var ua = navigator.userAgent, tem, M = ua.match(/(opera|yabrowser|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
        if (/trident/i.test(M[1])) {
            tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
            return { name: 'IE', version: (tem[1] || '') };
        }
        if (M[1] === 'Chrome') {
            tem = ua.match(/\b(OPR|Edg|YaBrowser)\/(\d+)/);
            if (tem != null)
                return { name: tem[1].replace('OPR', 'Opera'), version: tem[2] };
        }
        M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
        if ((tem = ua.match(/version\/(\d+)/i)) != null)
            M.splice(1, 1, tem[1]);
        return { name: M[0], version: M[1] };
    }
    var browserSpecs = check_browser();

    function cpcsp_console_log(level, msg) {
        //IE9 РЅРµ РјРѕР¶РµС‚ РїРёСЃР°С‚СЊ РІ РєРѕРЅСЃРѕР»СЊ РµСЃР»Рё РЅРµ РѕС‚РєСЂС‹С‚Р° РІРєР»Р°РґРєР° developer tools
        if (typeof (console) === 'undefined')
            return;
        if (level <= cadesplugin.current_log_level) {
            if (level === cadesplugin.LOG_LEVEL_DEBUG)
                console.log("DEBUG: %s", msg);
            if (level === cadesplugin.LOG_LEVEL_INFO)
                console.info("INFO: %s", msg);
            if (level === cadesplugin.LOG_LEVEL_ERROR)
                console.error("ERROR: %s", msg);
            return;
        }
    }

    function set_log_level(level) {
        if (!((level === cadesplugin.LOG_LEVEL_DEBUG) ||
            (level === cadesplugin.LOG_LEVEL_INFO) ||
            (level === cadesplugin.LOG_LEVEL_ERROR))) {
            cpcsp_console_log(cadesplugin.LOG_LEVEL_ERROR, "cadesplugin_api.js: Incorrect log_level: " + level);
            return;
        }
        cadesplugin.current_log_level = level;
        if (cadesplugin.current_log_level === cadesplugin.LOG_LEVEL_DEBUG)
            cpcsp_console_log(cadesplugin.LOG_LEVEL_INFO, "cadesplugin_api.js: log_level = DEBUG");
        if (cadesplugin.current_log_level === cadesplugin.LOG_LEVEL_INFO)
            cpcsp_console_log(cadesplugin.LOG_LEVEL_INFO, "cadesplugin_api.js: log_level = INFO");
        if (cadesplugin.current_log_level === cadesplugin.LOG_LEVEL_ERROR)
            cpcsp_console_log(cadesplugin.LOG_LEVEL_INFO, "cadesplugin_api.js: log_level = ERROR");
        if (isNativeMessageSupported()) {
            if (cadesplugin.current_log_level === cadesplugin.LOG_LEVEL_DEBUG)
                window.postMessage("set_log_level=debug", "*");
            if (cadesplugin.current_log_level === cadesplugin.LOG_LEVEL_INFO)
                window.postMessage("set_log_level=info", "*");
            if (cadesplugin.current_log_level === cadesplugin.LOG_LEVEL_ERROR)
                window.postMessage("set_log_level=error", "*");
        }
    }

    function set_constantValues() {
        cadesplugin.CAPICOM_MEMORY_STORE = 0;
        cadesplugin.CAPICOM_LOCAL_MACHINE_STORE = 1;
        cadesplugin.CAPICOM_CURRENT_USER_STORE = 2;
        cadesplugin.CADESCOM_MEMORY_STORE = 0;
        cadesplugin.CADESCOM_LOCAL_MACHINE_STORE = 1;
        cadesplugin.CADESCOM_CURRENT_USER_STORE = 2;
        cadesplugin.CADESCOM_CONTAINER_STORE = 100;

        cadesplugin.CAPICOM_MY_STORE = "My";

        cadesplugin.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED = 2;

        cadesplugin.CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME = 1;

        cadesplugin.CADESCOM_XML_SIGNATURE_TYPE_ENVELOPED = 0;
        cadesplugin.CADESCOM_XML_SIGNATURE_TYPE_ENVELOPING = 1;
        cadesplugin.CADESCOM_XML_SIGNATURE_TYPE_TEMPLATE = 2;

        cadesplugin.CADESCOM_XADES_DEFAULT = 0x00000010;
        cadesplugin.CADESCOM_XADES_BES = 0x00000020;
        cadesplugin.CADESCOM_XADES_T = 0x00000050;
        cadesplugin.CADESCOM_XADES_X_LONG_TYPE_1 = 0x000005d0;
        cadesplugin.CADESCOM_XMLDSIG_TYPE = 0x00000000;

        cadesplugin.XmlDsigGost3410UrlObsolete = "http://www.w3.org/2001/04/xmldsig-more#gostr34102001-gostr3411";
        cadesplugin.XmlDsigGost3411UrlObsolete = "http://www.w3.org/2001/04/xmldsig-more#gostr3411";
        cadesplugin.XmlDsigGost3410Url = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102001-gostr3411";
        cadesplugin.XmlDsigGost3411Url = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr3411";

        cadesplugin.XmlDsigGost3411Url2012256 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256";
        cadesplugin.XmlDsigGost3410Url2012256 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256";
        cadesplugin.XmlDsigGost3411Url2012512 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-512";
        cadesplugin.XmlDsigGost3410Url2012512 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-512";

        cadesplugin.CADESCOM_CADES_DEFAULT = 0;
        cadesplugin.CADESCOM_CADES_BES = 1;
        cadesplugin.CADESCOM_CADES_T = 0x5;
        cadesplugin.CADESCOM_CADES_X_LONG_TYPE_1 = 0x5d;
        cadesplugin.CADESCOM_PKCS7_TYPE = 0xffff;

        cadesplugin.CADESCOM_ENCODE_BASE64 = 0;
        cadesplugin.CADESCOM_ENCODE_BINARY = 1;
        cadesplugin.CADESCOM_ENCODE_ANY = -1;

        cadesplugin.CAPICOM_CERTIFICATE_INCLUDE_CHAIN_EXCEPT_ROOT = 0;
        cadesplugin.CAPICOM_CERTIFICATE_INCLUDE_WHOLE_CHAIN = 1;
        cadesplugin.CAPICOM_CERTIFICATE_INCLUDE_END_ENTITY_ONLY = 2;

        cadesplugin.CAPICOM_CERT_INFO_SUBJECT_SIMPLE_NAME = 0;
        cadesplugin.CAPICOM_CERT_INFO_ISSUER_SIMPLE_NAME = 1;

        cadesplugin.CAPICOM_CERTIFICATE_FIND_SHA1_HASH = 0;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME = 1;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_ISSUER_NAME = 2;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_ROOT_NAME = 3;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_TEMPLATE_NAME = 4;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_EXTENSION = 5;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_EXTENDED_PROPERTY = 6;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_APPLICATION_POLICY = 7;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_CERTIFICATE_POLICY = 8;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_TIME_VALID = 9;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_TIME_NOT_YET_VALID = 10;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_TIME_EXPIRED = 11;
        cadesplugin.CAPICOM_CERTIFICATE_FIND_KEY_USAGE = 12;

        cadesplugin.CAPICOM_DIGITAL_SIGNATURE_KEY_USAGE = 128;

        cadesplugin.CAPICOM_PROPID_ENHKEY_USAGE = 9;

        cadesplugin.CAPICOM_OID_OTHER = 0;
        cadesplugin.CAPICOM_OID_KEY_USAGE_EXTENSION = 10;

        cadesplugin.CAPICOM_EKU_CLIENT_AUTH = 2;
        cadesplugin.CAPICOM_EKU_SMARTCARD_LOGON = 5;
        cadesplugin.CAPICOM_EKU_OTHER = 0;

        cadesplugin.CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME = 0;
        cadesplugin.CAPICOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_NAME = 1;
        cadesplugin.CAPICOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_DESCRIPTION = 2;
        cadesplugin.CADESCOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME = 0;
        cadesplugin.CADESCOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_NAME = 1;
        cadesplugin.CADESCOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_DESCRIPTION = 2;
        cadesplugin.CADESCOM_ATTRIBUTE_OTHER = -1;

        cadesplugin.CADESCOM_STRING_TO_UCS2LE = 0;
        cadesplugin.CADESCOM_BASE64_TO_BINARY = 1;

        cadesplugin.CADESCOM_DISPLAY_DATA_NONE = 0;
        cadesplugin.CADESCOM_DISPLAY_DATA_CONTENT = 1;
        cadesplugin.CADESCOM_DISPLAY_DATA_ATTRIBUTE = 2;

        cadesplugin.CADESCOM_ENCRYPTION_ALGORITHM_RC2 = 0;
        cadesplugin.CADESCOM_ENCRYPTION_ALGORITHM_RC4 = 1;
        cadesplugin.CADESCOM_ENCRYPTION_ALGORITHM_DES = 2;
        cadesplugin.CADESCOM_ENCRYPTION_ALGORITHM_3DES = 3;
        cadesplugin.CADESCOM_ENCRYPTION_ALGORITHM_AES = 4;
        cadesplugin.CADESCOM_ENCRYPTION_ALGORITHM_GOST_28147_89 = 25;

        cadesplugin.CADESCOM_HASH_ALGORITHM_SHA1 = 0;
        cadesplugin.CADESCOM_HASH_ALGORITHM_MD2 = 1;
        cadesplugin.CADESCOM_HASH_ALGORITHM_MD4 = 2;
        cadesplugin.CADESCOM_HASH_ALGORITHM_MD5 = 3;
        cadesplugin.CADESCOM_HASH_ALGORITHM_SHA_256 = 4;
        cadesplugin.CADESCOM_HASH_ALGORITHM_SHA_384 = 5;
        cadesplugin.CADESCOM_HASH_ALGORITHM_SHA_512 = 6;
        cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411 = 100;
        cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_256 = 101;
        cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_512 = 102;
        cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_HMAC = 110;
        cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_256_HMAC = 111;
        cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_512_HMAC = 112;

        cadesplugin.LOG_LEVEL_DEBUG = 4;
        cadesplugin.LOG_LEVEL_INFO = 2;
        cadesplugin.LOG_LEVEL_ERROR = 1;

        cadesplugin.CADESCOM_AllowNone = 0;
        cadesplugin.CADESCOM_AllowNoOutstandingRequest = 0x1;
        cadesplugin.CADESCOM_AllowUntrustedCertificate = 0x2;
        cadesplugin.CADESCOM_AllowUntrustedRoot = 0x4;
        cadesplugin.CADESCOM_SkipInstallToStore = 0x10000000;

        cadesplugin.ENABLE_CARRIER_TYPE_CSP = 0x01;
        cadesplugin.ENABLE_CARRIER_TYPE_FKC_NO_SM = 0x02;
        cadesplugin.ENABLE_CARRIER_TYPE_FKC_SM = 0x04;
        cadesplugin.ENABLE_ANY_CARRIER_TYPE = 0x07;

        cadesplugin.DISABLE_EVERY_CARRIER_OPERATION = 0x00;
        cadesplugin.ENABLE_CARRIER_OPEN_ENUM = 0x01;
        cadesplugin.ENABLE_CARRIER_CREATE = 0x02;
        cadesplugin.ENABLE_ANY_OPERATION = 0x03;

        cadesplugin.CADESCOM_PRODUCT_CSP = 0;
        cadesplugin.CADESCOM_PRODUCT_OCSP = 1;
        cadesplugin.CADESCOM_PRODUCT_TSP = 2;

        cadesplugin.MEDIA_TYPE_REGISTRY = 0x00000001;
        cadesplugin.MEDIA_TYPE_HDIMAGE = 0x00000002;
        cadesplugin.MEDIA_TYPE_CLOUD = 0x00000004;
        cadesplugin.MEDIA_TYPE_SCARD = 0x00000008;

        cadesplugin.XCN_CRYPT_STRING_BASE64HEADER = 0;
        cadesplugin.AT_KEYEXCHANGE = 1;
        cadesplugin.AT_SIGNATURE = 2;
    }

    function async_spawn(generatorFunc) {
        function continuer(verb, arg) {
            var result;
            try {
                result = generator[verb](arg);
            } catch (err) {
                return Promise.reject(err);
            }
            if (result.done) {
                return result.value;
            } else {
                return Promise.resolve(result.value).then(onFulfilled, onRejected);
            }
        }
        var generator = generatorFunc(Array.prototype.slice.call(arguments, 1));
        var onFulfilled = continuer.bind(continuer, "next");
        var onRejected = continuer.bind(continuer, "throw");
        return onFulfilled();
    }

    function isIE() {
        // var retVal = (("Microsoft Internet Explorer" == navigator.appName) || // IE < 11
        //     navigator.userAgent.match(/Trident\/./i)); // IE 11
        return (browserSpecs.name === 'IE' || browserSpecs.name === 'MSIE');
    }

    function isIOS() {
        return (navigator.userAgent.match(/ipod/i) ||
            navigator.userAgent.match(/ipad/i) ||
            navigator.userAgent.match(/iphone/i));
    }

    function isNativeMessageSupported() {
        // Р’ IE СЂР°Р±РѕС‚Р°РµРј С‡РµСЂРµР· NPAPI
        if (isIE())
            return false;
        // Р’ Edge СЂР°Р±РѕС‚Р°РµРј С‡РµСЂРµР· NativeMessage
        if (browserSpecs.name === 'Edg') {
            return true;
        }
        if (browserSpecs.name === 'YaBrowser') {
            isYandex = true;
            return true;
        }
        // Р’ Chrome, Firefox, Safari Рё Opera СЂР°Р±РѕС‚Р°РµРј С‡РµСЂРµР· Р°СЃРёРЅС…СЂРѕРЅРЅСѓСЋ РІРµСЂСЃРёСЋ РІ Р·Р°РІРёСЃРёРјРѕСЃС‚Рё РѕС‚ РІРµСЂСЃРёРё
        if (browserSpecs.name === 'Opera') {
            isOpera = true;
            if (browserSpecs.version >= 33) {
                return true;
            }
            else {
                return false;
            }
        }
        if (browserSpecs.name === 'Firefox') {
            isFireFox = true;
            if (browserSpecs.version >= 52) {
                return true;
            }
            else {
                return false;
            }
        }
        if (browserSpecs.name === 'Chrome') {
            if (browserSpecs.version >= 42) {
                return true;
            }
            else {
                return false;
            }
        }
        //Р’ РЎР°С„Р°СЂРё РЅР°С‡РёРЅР°СЏ СЃ 12 РІРµСЂСЃРёРё РЅРµС‚ NPAPI
        if (browserSpecs.name === 'Safari') {
            isSafari = true;
            if (browserSpecs.version >= 12) {
                return true;
            } else {
                return false;
            }
        }
    }

    // Р¤СѓРЅРєС†РёСЏ Р°РєС‚РёРІР°С†РёРё РѕР±СЉРµРєС‚РѕРІ РљСЂРёРїС‚РѕРџСЂРѕ Р­Р¦Рџ Browser plug-in
    function CreateObject(name) {
        if (isIOS()) {
            // РќР° iOS РґР»СЏ СЃРѕР·РґР°РЅРёСЏ РѕР±СЉРµРєС‚РѕРІ РёСЃРїРѕР»СЊР·СѓРµС‚СЃСЏ С„СѓРЅРєС†РёСЏ
            // call_ru_cryptopro_npcades_10_native_bridge, РѕРїСЂРµРґРµР»РµРЅРЅР°СЏ РІ IOS_npcades_supp.js
            return call_ru_cryptopro_npcades_10_native_bridge("CreateObject", [name]);
        }
        if (isIE()) {
            // Р’ Internet Explorer СЃРѕР·РґР°СЋС‚СЃСЏ COM-РѕР±СЉРµРєС‚С‹
            if (name.match(/X509Enrollment/i)) {
                try {
                    // РћР±СЉРµРєС‚С‹ CertEnroll РїСЂРѕР±СѓРµРј СЃРѕР·РґР°РІР°С‚СЊ С‡РµСЂРµР· РЅР°С€Сѓ С„Р°Р±СЂРёРєСѓ,
                    // РµСЃР»Рё РЅРµ РїРѕР»СѓС‡РёР»РѕСЃСЊ С‚Рѕ С‡РµСЂРµР· CX509EnrollmentWebClassFactory
                    var objCertEnrollClassFactory = document.getElementById("webClassFactory");
                    return objCertEnrollClassFactory.CreateObject(name);
                }
                catch (e) {
                    try {
                        var objWebClassFactory = document.getElementById("certEnrollClassFactory");
                        return objWebClassFactory.CreateObject(name);
                    }
                    catch (err) {
                        throw ("Р”Р»СЏ СЃРѕР·РґР°РЅРёСЏ РѕР±СЊРµРєС‚РѕРІ X509Enrollment СЃР»РµРґСѓРµС‚ РЅР°СЃС‚СЂРѕРёС‚СЊ РІРµР±-СѓР·РµР» РЅР° РёСЃРїРѕР»СЊР·РѕРІР°РЅРёРµ РїСЂРѕРІРµСЂРєРё РїРѕРґР»РёРЅРЅРѕСЃС‚Рё РїРѕ РїСЂРѕС‚РѕРєРѕР»Сѓ HTTPS");
                    }
                }
            }
            // РћР±СЉРµРєС‚С‹ CAPICOM Рё CAdESCOM СЃРѕР·РґР°СЋС‚СЃСЏ С‡РµСЂРµР· CAdESCOM.WebClassFactory
            try {
                var objWebClassFactory = document.getElementById("webClassFactory");
                return objWebClassFactory.CreateObject(name);
            }
            catch (e) {
                // Р”Р»СЏ РІРµСЂСЃРёР№ РїР»Р°РіРёРЅР° РЅРёР¶Рµ 2.0.12538
                return new ActiveXObject(name);
            }
        }
        // СЃРѕР·РґР°СЋС‚СЃСЏ РѕР±СЉРµРєС‚С‹ NPAPI
        return pluginObject.CreateObject(name);
    }

    function decimalToHexString(number) {
        if (number < 0) {
            number = 0xFFFFFFFF + number + 1;
        }

        return number.toString(16).toUpperCase();
    }

    function GetMessageFromException(e) {
        var err = e.message;
        if (!err) {
            err = e;
        } else if (e.number) {
            err += " (0x" + decimalToHexString(e.number) + ")";
        }
        return err;
    }

    function getLastError(exception) {
        if (isNativeMessageSupported() || isIE() || isIOS()) {
            return GetMessageFromException(exception);
        }

        try {
            return pluginObject.getLastError();
        } catch (e) {
            return GetMessageFromException(exception);
        }
    }

    // Р¤СѓРЅРєС†РёСЏ РґР»СЏ СѓРґР°Р»РµРЅРёСЏ СЃРѕР·РґР°РЅРЅС‹С… РѕР±СЉРµРєС‚РѕРІ
    function ReleasePluginObjects() {
        return cpcsp_chrome_nmcades.ReleasePluginObjects();
    }

    // Р¤СѓРЅРєС†РёСЏ Р°РєС‚РёРІР°С†РёРё Р°СЃРёРЅС…СЂРѕРЅРЅС‹С… РѕР±СЉРµРєС‚РѕРІ РљСЂРёРїС‚РѕРџСЂРѕ Р­Р¦Рџ Browser plug-in
    function CreateObjectAsync(name) {
        return pluginObject.CreateObjectAsync(name);
    }

    //Р¤СѓРЅРєС†РёРё РґР»СЏ IOS
    var ru_cryptopro_npcades_10_native_bridge = {
        callbacksCount: 1,
        callbacks: {},

        // Automatically called by native layer when a result is available
        resultForCallback: function resultForCallback(callbackId, resultArray) {
            var callback = ru_cryptopro_npcades_10_native_bridge.callbacks[callbackId];
            if (!callback) return;
            callback.apply(null, resultArray);
        },

        // Use this in javascript to request native objective-c code
        // functionName : string (I think the name is explicit :p)
        // args : array of arguments
        // callback : function with n-arguments that is going to be called when the native code returned
        call: function call(functionName, args, callback) {
            var hasCallback = callback && typeof callback === "function";
            var callbackId = hasCallback ? ru_cryptopro_npcades_10_native_bridge.callbacksCount++ : 0;

            if (hasCallback)
                ru_cryptopro_npcades_10_native_bridge.callbacks[callbackId] = callback;

            var iframe = document.createElement("IFRAME");
            var arrObjs = new Array("_CPNP_handle");
            try {
                iframe.setAttribute("src", "cpnp-js-call:" + functionName + ":" + callbackId + ":" + encodeURIComponent(JSON.stringify(args, arrObjs)));
            } catch (e) {
                alert(e);
            }
            document.documentElement.appendChild(iframe);
            iframe.parentNode.removeChild(iframe);
            iframe = null;
        }
    };

    function call_ru_cryptopro_npcades_10_native_bridge(functionName, array) {
        var tmpobj;
        var ex;
        ru_cryptopro_npcades_10_native_bridge.call(functionName, array, function (e, response) {
            ex = e;
            var str = 'tmpobj=' + response;
            eval(str);
            if (typeof (tmpobj) === "string") {
                tmpobj = tmpobj.replace(/\\\n/gm, "\n");
                tmpobj = tmpobj.replace(/\\\r/gm, "\r");
            }
        });
        if (ex)
            throw ex;
        return tmpobj;
    }

    function show_firefox_missing_extension_dialog() {
        if (!window.cadesplugin_skip_extension_install) {
            var ovr = document.createElement('div');
            ovr.id = "cadesplugin_ovr";
            ovr.style = "visibility: hidden; position: fixed; left: 0px; top: 0px; width:100%; height:100%; background-color: rgba(0,0,0,0.7)";
            ovr.innerHTML = "<div id='cadesplugin_ovr_item' style='position:relative; width:400px; margin:100px auto; background-color:#fff; border:2px solid #000; padding:10px; text-align:center; opacity: 1; z-index: 1500'>" +
                "<button id='cadesplugin_close_install' style='float: right; font-size: 10px; background: transparent; border: 1; margin: -5px'>X</button>" +
                "<p>Р”Р»СЏ СЂР°Р±РѕС‚С‹ РљСЂРёРїС‚РѕРџСЂРѕ Р­Р¦Рџ Browser plugin РЅР° РґР°РЅРЅРѕРј СЃР°Р№С‚Рµ РЅРµРѕР±С…РѕРґРёРјРѕ СЂР°СЃС€РёСЂРµРЅРёРµ РґР»СЏ Р±СЂР°СѓР·РµСЂР°. РЈР±РµРґРёС‚РµСЃСЊ, С‡С‚Рѕ РѕРЅРѕ Сѓ Р’Р°СЃ РІРєР»СЋС‡РµРЅРѕ РёР»Рё СѓСЃС‚Р°РЅРѕРІРёС‚Рµ РµРіРѕ." +
                "<p><a href='https://www.cryptopro.ru/sites/default/files/products/cades/extensions/firefox_cryptopro_extension_latest.xpi'>РЎРєР°С‡Р°С‚СЊ СЂР°СЃС€РёСЂРµРЅРёРµ</a></p>" +
                "</div>";
            document.getElementsByTagName("Body")[0].appendChild(ovr);
            document.getElementById("cadesplugin_close_install").addEventListener('click', function () {
                plugin_loaded_error("РџР»Р°РіРёРЅ РЅРµРґРѕСЃС‚СѓРїРµРЅ");
                document.getElementById("cadesplugin_ovr").style.visibility = 'hidden';
            });

            ovr.addEventListener('click', function () {
                plugin_loaded_error("РџР»Р°РіРёРЅ РЅРµРґРѕСЃС‚СѓРїРµРЅ");
                document.getElementById("cadesplugin_ovr").style.visibility = 'hidden';
            });
            ovr.style.visibility = "visible";
        }
    }
    function firefox_or_safari_nmcades_onload() {
        cpcsp_chrome_nmcades.check_chrome_plugin(plugin_loaded, plugin_loaded_error);
    }

    function nmcades_api_onload() {
        window.postMessage("cadesplugin_echo_request", "*");
        window.addEventListener("message", function (event) {
            if (typeof (event.data) !== "string" || !event.data.match("cadesplugin_loaded"))
                return;
            if (cadesplugin_loaded_event_recieved)
                return;
            if (isFireFox || isSafari) {
                // Р”Р»СЏ Firefox, РЎР°С„Р°СЂРё РІРјРµСЃС‚Рµ СЃ СЃРѕРѕР±С‰РµРЅРёРµРј cadesplugin_loaded РїСЂРёР»РµС‚Р°РµС‚ url РґР»СЏ Р·Р°РіСЂСѓР·РєРё nmcades_plugin_api.js
                var url = event.data.substring(event.data.indexOf("url:") + 4);
                if (!url.match("^moz-extension://[a-zA-Z0-9-]+/nmcades_plugin_api.js$")
                    && !url.match("^safari-extension://[a-zA-Z0-9-]+/[a-zA-Z0-9]+/nmcades_plugin_api.js$")) {
                    cpcsp_console_log(cadesplugin.LOG_LEVEL_ERROR, "Bad url \"" + url + "\" for load CryptoPro Extension for CAdES Browser plug-in");
                    plugin_loaded_error();
                    return;
                }
                var fileref = document.createElement('script');
                fileref.setAttribute("type", "text/javascript");
                fileref.setAttribute("src", url);
                fileref.onerror = plugin_loaded_error;
                fileref.onload = firefox_or_safari_nmcades_onload;
                document.getElementsByTagName("head")[0].appendChild(fileref);
            } else {
                cpcsp_chrome_nmcades.check_chrome_plugin(plugin_loaded, plugin_loaded_error);
            }
            cadesplugin_loaded_event_recieved = true;
        }, false);
    }

    //Р—Р°РіСЂСѓР¶Р°РµРј СЂР°СЃС€РёСЂРµРЅРёСЏ РґР»СЏ Chrome, Opera, YaBrowser, FireFox, Edge, Safari
    function load_extension() {
        if (isFireFox || isSafari) {
            // РІС‹Р·С‹РІР°РµРј callback СЂСѓРєР°РјРё С‚.Рє. РЅР°Рј РЅСѓР¶РЅРѕ СѓР·РЅР°С‚СЊ ID СЂР°СЃС€РёСЂРµРЅРёСЏ. РћРЅ СѓРЅРёРєР°Р»СЊРЅС‹Р№ РґР»СЏ Р±СЂР°СѓР·РµСЂР°.
            nmcades_api_onload();
        } else {
            // РІ Р°СЃРёРЅС…СЂРѕРЅРЅРѕРј РІР°СЂРёР°РЅС‚Рµ РґР»СЏ Yandex Рё Opera РїРѕРґРєР»СЋС‡Р°РµРј СЂР°СЃС€РёСЂРµРЅРёРµ РёР· Opera store.
            if (isOpera || isYandex) {
                var fileref = document.createElement('script');
                fileref.setAttribute("type", "text/javascript");
                fileref.setAttribute("src", "chrome-extension://epebfcehmdedogndhlcacafjaacknbcm/nmcades_plugin_api.js");
                fileref.onerror = plugin_loaded_error;
                fileref.onload = nmcades_api_onload;
                document.getElementsByTagName("head")[0].appendChild(fileref);
            } else {
                // РґР»СЏ Chrome, Chromium, Chromium Edge СЂР°СЃС€РёСЂРµРЅРёРµ РёР· Chrome store
                var fileref = document.createElement('script');
                fileref.setAttribute("type", "text/javascript");
                fileref.setAttribute("src", "chrome-extension://iifchhfnnmpdbibifmljnfjhpififfog/nmcades_plugin_api.js");
                fileref.onerror = plugin_loaded_error;
                fileref.onload = nmcades_api_onload;
                document.getElementsByTagName("head")[0].appendChild(fileref);
            }
        }
    }

    //Р—Р°РіСЂСѓР¶Р°РµРј РїР»Р°РіРёРЅ РґР»СЏ NPAPI
    function load_npapi_plugin() {
        var elem = document.createElement('object');
        elem.setAttribute("id", "cadesplugin_object");
        elem.setAttribute("type", "application/x-cades");
        elem.setAttribute("style", "visibility: hidden");
        document.getElementsByTagName("body")[0].appendChild(elem);
        pluginObject = document.getElementById("cadesplugin_object");
        if (isIE()) {
            var elem1 = document.createElement('object');
            elem1.setAttribute("id", "certEnrollClassFactory");
            elem1.setAttribute("classid", "clsid:884e2049-217d-11da-b2a4-000e7bbb2b09");
            elem1.setAttribute("style", "visibility: hidden");
            document.getElementsByTagName("body")[0].appendChild(elem1);
            var elem2 = document.createElement('object');
            elem2.setAttribute("id", "webClassFactory");
            elem2.setAttribute("classid", "clsid:B04C8637-10BD-484E-B0DA-B8A039F60024");
            elem2.setAttribute("style", "visibility: hidden");
            document.getElementsByTagName("body")[0].appendChild(elem2);
        }
    }

    //РћС‚РїСЂР°РІР»СЏРµРј СЃРѕР±С‹С‚РёРµ С‡С‚Рѕ РІСЃРµ РѕРє.
    function plugin_loaded() {
        plugin_resolved = 1;
        if (canPromise) {
            plugin_resolve();
        } else {
            window.postMessage("cadesplugin_loaded", "*");
        }
    }

    //РћС‚РїСЂР°РІР»СЏРµРј СЃРѕР±С‹С‚РёРµ С‡С‚Рѕ СЃР»РѕРјР°Р»РёСЃСЊ.
    function plugin_loaded_error(msg) {
        if (typeof (msg) === 'undefined' || typeof (msg) === 'object')
            msg = "РџР»Р°РіРёРЅ РЅРµРґРѕСЃС‚СѓРїРµРЅ";
        plugin_resolved = 1;
        if (canPromise) {
            plugin_reject(msg);
        } else {
            window.postMessage("cadesplugin_load_error", "*");
        }
    }

    //РїСЂРѕРІРµСЂСЏРµРј С‡С‚Рѕ Сѓ РЅР°СЃ С…РѕС‚СЊ РєР°РєРѕРµ С‚Рѕ СЃРѕР±С‹С‚РёРµ СѓС€Р»Рѕ, Рё РµСЃР»Рё РЅРµ СѓС…РѕРґРёР»Рѕ РєРёРґР°РµРј РµС‰Рµ СЂР°Р· РѕС€РёР±РєСѓ
    function check_load_timeout() {
        if (plugin_resolved === 1)
            return;
        if (isFireFox) {
            show_firefox_missing_extension_dialog();
        }
        plugin_resolved = 1;
        if (canPromise) {
            plugin_reject("РСЃС‚РµРєР»Рѕ РІСЂРµРјСЏ РѕР¶РёРґР°РЅРёСЏ Р·Р°РіСЂСѓР·РєРё РїР»Р°РіРёРЅР°");
        } else {
            window.postMessage("cadesplugin_load_error", "*");
        }

    }

    //Р’СЃРїРѕРјРѕРіР°С‚РµР»СЊРЅР°СЏ С„СѓРЅРєС†РёСЏ РґР»СЏ NPAPI
    function createPromise(arg) {
        return new Promise(arg);
    }

    function check_npapi_plugin() {
        try {
            var oAbout = CreateObject("CAdESCOM.About");
            plugin_loaded();
        }
        catch (err) {
            document.getElementById("cadesplugin_object").style.display = 'none';
            // РћР±СЉРµРєС‚ СЃРѕР·РґР°С‚СЊ РЅРµ СѓРґР°Р»РѕСЃСЊ, РїСЂРѕРІРµСЂРёРј, СѓСЃС‚Р°РЅРѕРІР»РµРЅ Р»Рё
            // РІРѕРѕР±С‰Рµ РїР»Р°РіРёРЅ. РўР°РєР°СЏ РІРѕР·РјРѕР¶РЅРѕСЃС‚СЊ РµСЃС‚СЊ РЅРµ РІРѕ РІСЃРµС… Р±СЂР°СѓР·РµСЂР°С…
            var mimetype = navigator.mimeTypes["application/x-cades"];
            if (mimetype) {
                var plugin = mimetype.enabledPlugin;
                if (plugin) {
                    plugin_loaded_error("РџР»Р°РіРёРЅ Р·Р°РіСЂСѓР¶РµРЅ, РЅРѕ РЅРµ СЃРѕР·РґР°СЋС‚СЃСЏ РѕР±СЊРµРєС‚С‹");
                } else {
                    plugin_loaded_error("РћС€РёР±РєР° РїСЂРё Р·Р°РіСЂСѓР·РєРµ РїР»Р°РіРёРЅР°");
                }
            } else {
                plugin_loaded_error("РџР»Р°РіРёРЅ РЅРµРґРѕСЃС‚СѓРїРµРЅ");
            }
        }
    }

    //РџСЂРѕРІРµСЂСЏРµРј СЂР°Р±РѕС‚Р°РµС‚ Р»Рё РїР»Р°РіРёРЅ
    function check_plugin_working() {
        var div = document.createElement("div");
        div.innerHTML = "<!--[if lt IE 9]><i></i><![endif]-->";
        var isIeLessThan9 = (div.getElementsByTagName("i").length === 1);
        if (isIeLessThan9) {
            plugin_loaded_error("Internet Explorer РІРµСЂСЃРёРё 8 Рё РЅРёР¶Рµ РЅРµ РїРѕРґРґРµСЂР¶РёРІР°РµС‚СЃСЏ");
            return;
        }

        if (isNativeMessageSupported()) {
            load_extension();
        } else if (!canPromise) {
            window.addEventListener("message", function (event) {
                if (event.data !== "cadesplugin_echo_request")
                    return;
                load_npapi_plugin();
                check_npapi_plugin();
            },
                false);
        } else {
            if (document.readyState === "complete") {
                load_npapi_plugin();
                check_npapi_plugin();
            } else {
                window.addEventListener("load", function (event) {
                    load_npapi_plugin();
                    check_npapi_plugin();
                }, false);
            }
        }
    }

    function set_pluginObject(obj) {
        pluginObject = obj;
    }

    function is_capilite_enabled() {
        if ((typeof (cadesplugin.EnableInternalCSP) !== 'undefined') && cadesplugin.EnableInternalCSP)
            return true;
        return false;
    };

    //Export
    cadesplugin.JSModuleVersion = "2.3.1";
    cadesplugin.async_spawn = async_spawn;
    cadesplugin.set = set_pluginObject;
    cadesplugin.set_log_level = set_log_level;
    cadesplugin.getLastError = getLastError;
    cadesplugin.is_capilite_enabled = is_capilite_enabled;

    if (isNativeMessageSupported()) {
        cadesplugin.CreateObjectAsync = CreateObjectAsync;
        cadesplugin.ReleasePluginObjects = ReleasePluginObjects;
    }

    if (!isNativeMessageSupported()) {
        cadesplugin.CreateObject = CreateObject;
    }

    if (window.cadesplugin_load_timeout) {
        setTimeout(check_load_timeout, window.cadesplugin_load_timeout);
    }
    else {
        setTimeout(check_load_timeout, 20000);
    }

    set_constantValues();

    cadesplugin.current_log_level = cadesplugin.LOG_LEVEL_ERROR;
    window.cadesplugin = cadesplugin;
    check_plugin_working();
};
module.exports = exports['default'];