namespace Bars.GkhGji.Interceptors
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckPeriodServiceInterceptor : EmptyDomainInterceptor<ActCheckPeriod>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            // в момент создания берем переданное время начала проверки и время окончания
            // И формируем Дату начала и Дату окончания путем сопоставления с датой проверки DateCheck
            // То есть к дате проверки добавляем время начала получаем - Дату начала
            // К дате проверки добавляем время окончания и получаем Дату окончания
            var date = entity.DateCheck.ToDateTime();

            var dateStart = DateTime.Now;
            DateTime.TryParse(entity.TimeStart, out dateStart);

            var dateEnd = DateTime.Now;
            DateTime.TryParse(entity.TimeEnd, out dateEnd);

            entity.DateStart = new DateTime(
                date.Year, date.Month, date.Day, dateStart.Hour, dateStart.Minute, 0);
            entity.DateEnd = new DateTime(date.Year, date.Month, date.Day, dateEnd.Hour, dateEnd.Minute, 0);

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            // в момент создания берем переданное время начала проверки и время окончания
            // И формируем Дату начала и Дату окончания путем сопоставления с датой проверки DateCheck
            // То есть к дате проверки добавляем время начала получаем - Дату начала
            // К дате проверки добавляем время окончания и получаем Дату окончания
            var date = entity.DateCheck.ToDateTime();

            var dateStart = DateTime.Now;
            DateTime.TryParse(entity.TimeStart, out dateStart);

            var dateEnd = DateTime.Now;
            DateTime.TryParse(entity.TimeEnd, out dateEnd);

            entity.DateStart = new DateTime(date.Year, date.Month, date.Day, dateStart.Hour, dateStart.Minute, 0);
            entity.DateEnd = new DateTime(date.Year, date.Month, date.Day, dateEnd.Hour, dateEnd.Minute, 0);

            return this.Success();
        }
    }
}