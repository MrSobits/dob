Ext.define('B4.model.smev.SMEVChangePremisesState', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVChangePremisesState'
    },
    fields: [
        { name: 'Inspector' },
        { name: 'CalcDate' },
        { name: 'Municipality' },
        { name: 'ReqId' },
        { name: 'ChangePremisesType' },
        { name: 'RealityObject' },
        { name: 'Room' },
        { name: 'CadastralNumber' },
        { name: 'DeclarantType' },
        { name: 'DeclarantName' },
        { name: 'DeclarantAddress' },
        { name: 'Department' },
        { name: 'Area' },
        { name: 'City' },
        { name: 'AnswerFile' },
        { name: 'Street' },
        { name: 'House' },
        { name: 'Block' },
        { name: 'RoomType' },
        { name: 'Appointment' },
        { name: 'ActNumber' },
        { name: 'ActName' },
        { name: 'ActDate' },
        { name: 'OldPremisesType' },
        { name: 'NewPremisesType' },
        { name: 'ConditionTransfer' },
        { name: 'ResponsibleName' },
        { name: 'ResponsiblePost' },
        { name: 'ResponsibleDate' },
        { name: 'Answer' },
        { name: 'RequestState' },
        { name: 'MessageId' },
        { name: 'ChangePremisesTypeValue' }
    ]
});