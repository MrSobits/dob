﻿namespace Bars.Gkh.Overhaul.Tat.PriorityParams.Impl
{
    using System;
    using Bars.Gkh.Enums;
    using Entities;

    public class EnergyPassportPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public string Id 
        {
            get { return "EnergyPassport"; }
        }

        public Type EnumType
        {
            get
            {
                return typeof(TypePresence);
            }
        }

        public string Name
        {
            get { return "Наличие энергетического паспорта"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Qualit; }
        }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            return obj.RealityObject.EnergyPassport;
        }
    }
}
