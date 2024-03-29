Ext.define('B4.view.wizard.WizardWindow', {
    extend: 'Ext.window.Window',
    iconCls:'icon_wand',
    modal: true,
    closable: false,
    width:700,
    height:400,
    minWidth:300,
    minHeight: 200,
    bodyMask: null,
    layout:'card',
    defaults:{
        border:false,
        hideMode:'offsets'
    },
    title: 'Мастер выполнения операций',

    //результат выполнения мастера
    result: undefined,

    stepsFrames: [],
    // virtual. Получить конфигурацию шагов
    getStepsFrames: function() {
        return me.stepsFrames;
    },

    initComponent: function () {
        var me = this;
        me.buildActions();

        me.addEvents('wizardComplete');

        var config = {
            items: me.buildStepsFrames()
        };

        me.buttons = [
            me.actPrev,
            me.actNext,
            '-',
            me.actClose
        ];

        Ext.applyIf(me, Ext.applyIf(me.initialConfig, config));

        var currentStepId = me.initialStepId || me.initialConfig.currentStepId || 'start';

        // Установка стартовой страницы
        me.on('afterrender',
            function () {
                me.setCurrentStep(currentStepId);
            },
            me);

        me.callParent(arguments);
    },

    fireWizardComplete: function(){
        this.fireEvent('wizardComplete');
    },

    // private. Получить номер страницы шага по id
    findStepFrameIndexById:function (stepId) {
        var i = -1;

        Ext.each(this.stepsFrames,
            function (step, idx) {
                if (step.stepId === stepId) {
                    i = idx;

                    // если нашли нужный stepFrame выходим из loop, т.к. нас интересует первый
                    return false;
                }
            }
        );

        return i;
    },

    // private. Перейти на шаг с указанным id
    setCurrentStep: function (stepId) {
        var me = this,
            idx = me.findStepFrameIndexById(stepId);

        if (idx >= 0) {
            me.currentStep = me.stepsFrames[idx];

            if (me.currentStep.init() !== false) {
                me.actPrev.setDisabled(!me.currentStep.allowBackward());
                me.actNext.setDisabled(!me.currentStep.allowForward());

                me.getLayout().setActiveItem(idx);
            }
        }
    },

    buildActions: function () {
        var me = this;
        me.actClose = new Ext.Action({
            itemId: 'wizardCloseBtn',
            text: 'Закрыть',
            iconCls:'icon_cross',
            tooltip: 'Закрыть форму',
            iconAlign:'left',
            handler: me.doClose,
            scope: me
        });

        me.actPrev = new Ext.Action({
            text: 'Назад',
            iconCls:'icon_arrow-180',
            tooltip: 'Назад',
            iconAlign:'left',
            disabled:true,
            handler: function () {
                me.currentStep.doBackward();
            },
            scope: me
        });

        me.actNext = new Ext.Action({
            text: 'Далее',
            iconCls:'icon_arrow',
            tooltip: 'Далее',
            iconAlign:'left',
            disabled:true,
            handler: function () {
                me.currentStep.doForward();
            },
            scope: me
        });
    },

    buildStepsFrames: function () {
        var me = this;

        me.stepsFrames = [];

        Ext.each(me.getStepsFrames(), function (step) {
            me.stepsFrames.push(step);
        });

        // Вешаем обработчик selectionchange
        Ext.each(me.stepsFrames,
            function (stepObj) {
                stepObj.on('selectionchange',
                    function (step) {
                        me.actPrev.setDisabled(!step.allowBackward());
                        me.actNext.setDisabled(!step.allowForward());
                    },
                    me);
            },
            me);

        return me.stepsFrames;
    },

    mask: function (msg) {
        var me = this;
        if (Ext.isEmpty(msg)) {
            msg = 'Пожалуйста, подождите';
        }

        if (!Ext.isObject(msg)) {
            msg = { msg: msg };
        }

        me.unmask();

        me.bodyMask = Ext.create('Ext.LoadMask', me, msg);
        me.bodyMask.show();

        return me.bodyMask;
    },

    unmask: function () {
        var me = this;
        if (!Ext.isEmpty(me.bodyMask)) {
            try {
                me.bodyMask.hide();
                me.bodyMask.destroy();
                me.bodyMask = null;
            } catch (e) {

            }
        }
    }
});