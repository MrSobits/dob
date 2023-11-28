namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ProtocolDefinitionInterceptor : ProtocolDefinitionInterceptor<ProtocolDefinition>
    {
        //Все методы переопределеять и добавлять в Generic
    }

    // Generic класс для определения протокола чтобы было лучше расширят ьсущности без дублирования кода через subclass
    public class ProtocolDefinitionInterceptor<T> : EmptyDomainInterceptor<T>
        where T : ProtocolDefinition
    {
    }
}
