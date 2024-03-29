﻿namespace Bars.Gkh.Domain
{
    using System;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    public static class WindsorExtensions
    {
        /// <summary>
        /// Выполнить действие в транзакции
        /// </summary>
        /// <param name="container"></param>
        /// <param name="action"></param>
        public static void InTransaction(this IWindsorContainer container, Action action)
        {
            if (action == null)
            {
                return;
            }

            using (var tr = container.Resolve<IDataTransaction>())
            {
                using (container.Using(tr))
                {
                    try
                    {
                        action();
                        tr.Commit();
                    }
                    catch(Exception e)
                    {
                        tr.Rollback();

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Замена transient-имплементации в контейнере
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TFirstImpl"></typeparam>
        /// <typeparam name="TSecondImpl"></typeparam>
        /// <param name="container"></param>
        /// <param name="name"></param>
        public static void ReplaceTransient<TInterface, TFirstImpl, TSecondImpl>(this IWindsorContainer container, string name = null)
            where TInterface: class
            where TFirstImpl: class, TInterface
            where TSecondImpl: class, TInterface
        {
            var registration = Component.For<TInterface>().ImplementedBy<TSecondImpl>().LifestyleTransient();

            if (!name.IsEmpty())
            {
                registration.Named(name);
            }

            container.ReplaceComponent<TInterface>(typeof (TFirstImpl), registration);
        }
    }
}