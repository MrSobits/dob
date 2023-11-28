﻿namespace Bars.Gkh.Utils.EntityExtensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;

    using Fasterflect;

    public static class EntityExtension
    {
        /// <summary>
        /// Скопировать сущность
        /// </summary>
        /// <param name="entity"><see cref="PersistentObject"/></param>
        /// <param name="isNewEntity">Обнулить идентификатор</param>
        public static T PersistentObjectClone<T>(this T entity, bool isNewEntity = false)
            where T : PersistentObject, new()
        {
            ArgumentChecker.NotNull(entity, "entity");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty);

            var newEntity = new T();

            foreach (var property in properties)
            {
                property.SetValue(newEntity, property.GetValue(entity));
            }

            if (isNewEntity)
            {
                newEntity.Id = 0;
            }

            return newEntity;
        }

        /// <summary>
        /// Скопировать сущность
        /// </summary>
        /// <param name="entity"><see cref="BaseEntity"/></param>
        /// <param name="isNewEntity">Обнулить идентификатор и атрибуты редактирования</param>
        public static T BaseEntityClone<T>(this T entity, bool isNewEntity = false)
            where T : BaseEntity, new()
        {
            ArgumentChecker.NotNull(entity, "entity");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty);

            var newEntity = new T();

            foreach (var property in properties)
            {
                property.SetValue(newEntity, property.GetValue(entity));
            }

            if (isNewEntity)
            {
                newEntity.Id = 0;
                newEntity.ObjectCreateDate = DateTime.MinValue;
                newEntity.ObjectEditDate = DateTime.MinValue;
                newEntity.ObjectVersion = 0;
            }

            return newEntity;
        }
    }
}