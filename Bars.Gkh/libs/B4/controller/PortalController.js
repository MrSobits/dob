Ext.define('B4.controller.PortalController', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.mixins.LayoutControllerLoader',
        'B4.mixins.MaskBody',
        'B4.mixins.Context'
    ],

    refs: [
        {
            ref: 'mainMenu',
            selector: '#mainMenu'
        },
        {
            ref: 'b4TabPanel',
            selector: '#contentPanel'
        }
    ],
    views: ['Portal', 'instructions.Window'],
    stores: [
        'MenuItemStore',
        'SearchIndex',
        'desktop.ActiveOperator',
        'News'
    ],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    containerSelector: '#contentPanel',

    allowLoadMask: false,
    deployViewKeys: {
        'default': 'defaultDeploy'
    },

    /**
     * @private
     * Метод по умолчанию для добавления компонент на рабочий стол.
     * Добавляет компоненту в виде новой вкладки.
     */
    defaultDeploy: function (controller, view) {
        var container = this.getB4TabPanel(),
            viewSelector = '#' + view.getId();

        if (!container.down(viewSelector)) {
            container.add(view);
        }
        container.setActiveTab(view);
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Widget.Faq', applyTo: '[wtype=faq]', selector: 'portalpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                },
                {
                    name: 'Widget.News', applyTo: '[wtype=news]', selector: 'portalpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                },
                {
                    name: 'Widget.ActiveOperator', applyTo: '[wtype=activeOperator]', selector: 'portalpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                },
                {
                    name: 'B4.Security.AccessRights', applyTo: '#permissionButton', selector: 'portalpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                }
            ]
        }
    ],

    init: function () {
        var actions = {
            // Временное решение для работы старой версии routing
            'b4portal': {
                afterrender: function() {
                    var token = Ext.History.getToken();
                    if (token && token.indexOf("B4.controller") == 0) {
                        Ext.History.add('');
                    }
                }
            },
            '#contentPanel': {
                tabchange: function(tabPanel, newPanel) {
                    if (!newPanel.ctxKey) {
                        Ext.History.add(newPanel.token || '');
                    }
                }
            },
            // Конец - Временное решение для работы старой версии routing

            'menufirstlevelitem': {
                tap: this.onMenuItemClick
            },

            'iconablemenu': {
                itemtap: this.onMenuItemClick
            },

            '#searchList': {
                itemtap: this.onMenuItemClick
            },

            '#profileBtn': {
                click: this.onProfile
            },

            '#logoutBtn': {
                click: this.onLogout
            },

            '#logoutBt': {
                click: this.onLogout
            },

            '#allNewsBtn': {
                click: this.onAllNews
            },

            '#tableRefreshBtn': {
                click: this.refreshTableTask
            },

            '#col-2 #allInspectionsBtn': {
                click: this.onAllDisposal
            },

            '#col-3 #allInspectionsBtn': {
                click: this.onAllPrescription
            },

            'component': {
                opendetailcard: this.openDocumentGji
            },

            '#helpBtn': {
                click: this.onHelpBtnClick
            },

            '#goToInstructionsBtn': {
                click: this.goToInstructionsBtnClick
        }
    };

        this.control(actions);

        this.callParent(arguments);
    },

    onLaunch: function () {
        // jQuery
        Ext.Loader.loadScript(rootUrl + 'libs/jQuery/jquery-1.9.1.min.js');
        Gkh.signalR.start();
        var viewPortal = this.getView('Portal');
        if (viewPortal)
            viewPortal.create();
    },

    onMenuItemClick: function (record, icmenu) {
        var moduleScript = record.get('moduleScript');

        if (moduleScript.indexOf('B4.controller') > -1) {
            if (Ext.History.getToken() == moduleScript) {
                return;
            }
            this.loadController(record.get('moduleScript'), null, null, function () { icmenu.enableItem(record.id); });
        } else {
            Ext.History.add(moduleScript);
        }
    },

    selectHomeView: function () {
        var container = Ext.ComponentQuery.query(this.containerSelector)[0];
        container.setActiveTab(0);
    },

    onLogout: function () {
        Gkh.signalR.stop();
        window.location = B4.Url.action('LogOut', 'Login');
    },

    onProfile: function () {
        Ext.History.add('profilesettingadministration');
        //this.loadController('B4.controller.administration.ProfileSetting');
    },

    onAllNews: function () {
        this.loadController('B4.controller.Public.News');
    },

    onHelpBtnClick: function () {
        var url = B4.Url.action('/Instruction/GetMainInstruction');
        window.open(url, '_blank');
    },
    
    goToInstructionsBtnClick: function () {
        var me = this;
        me.getB4TabPanel();
        if (!me.helpWindow) {
            me.helpWindow = Ext.create('B4.view.instructions.Window', {
                renderTo: B4.getBody().getEl()
            });
        }
        this.helpWindow.show();
    }
});