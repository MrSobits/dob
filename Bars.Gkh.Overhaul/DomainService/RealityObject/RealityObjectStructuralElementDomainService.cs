namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;
    using Gkh.Entities.CommonEstateObject;

    public class RealityObjectStructuralElementDomainService : BaseDomainService<RealityObjectStructuralElement>
    {
        private IDomainService<StructuralElementGroupAttribute> _atrService;
        private IDomainService<RealityObjectStructuralElementAttributeValue> _valuesService;

        public IDomainService<StructuralElementGroupAttribute> AtrService
        {
            get
            {
                return _atrService ?? (_atrService = Container.Resolve<IDomainService<StructuralElementGroupAttribute>>());
            }
        }

        public IDomainService<RealityObjectStructuralElementAttributeValue> ValuesService
        {
            get
            {
                return _valuesService ?? (_valuesService = Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>());
            }
        }

        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<RealityObjectStructuralElement>();

            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();
                    UpdateInternal(value);
                    values.Add(value);

                    SaveValues(record.NonObjectProperties["Values"], value);
                }
            });

            return new BaseDataResult(values);
        }

        public override IDataResult Save(BaseParams baseParams)
        {
            var values = new List<RealityObjectStructuralElement>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();
                    SaveInternal(value);
                    values.Add(value);

                    SaveValues(record.NonObjectProperties["Values"], value);
                }
            });

            return new SaveDataResult(values);
        }

        private void SaveValues(object vals, RealityObjectStructuralElement value)
        {
            var atrValues = vals as List<object>;
            if (atrValues != null)
            {
                foreach (var atrValue in atrValues)
                {
                    var val = atrValue as DynamicDictionary;

                    if (val == null) 
                        continue;

                    var id = val.GetAs<long>("Id");

                    if (id == 0)
                    {
                        ValuesService.Save(new RealityObjectStructuralElementAttributeValue
                        {
                            Object = value,
                            Attribute = AtrService.Load(val.GetAs<long>("Attribute")),
                            Value = val.Get("Value", string.Empty)
                        });
                    }
                    else
                    {
                        var obj = ValuesService.Get(id);

                        obj.Value = val.Get("Value", string.Empty);

                        ValuesService.Update(obj);
                    }
                }
            }
        }
    }
}