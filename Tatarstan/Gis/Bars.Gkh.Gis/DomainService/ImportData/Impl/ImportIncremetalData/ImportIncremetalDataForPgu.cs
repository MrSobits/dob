namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using B4;
    using B4.Modules.FileStorage;

    using BilConnection;

    using DataResult;
    using Entities.Register.LoadedFileRegister;
    using Enum;
    using Ionic.Zip;
    using Npgsql;
    using Utils;
    

    /// <summary>
    /// Загрузка инкрементальных данных для ПГМУ РТ
    /// </summary>
    public class ImportIncremetalDataForPgu : BaseImportDataHandler
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ImportIncremetalDataForPgu()
        {
            _importLog = new StringBuilder();
        }

        private readonly StringBuilder _importLog;
        
        /// <summary>
        /// Файловый менеджер
        /// </summary>
        public IFileManager FileManager { get; set; }


        /// <summary>
        /// Файловый менеджер
        /// </summary>
        public IBilConnectionService BilConnectionService { get; set; }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="loadedFiles">Загруженные файлы с данными</param>
        /// <returns>Результат импорта</returns>
        public override IEnumerable<ImportDataResult> ImportData(IEnumerable<LoadedFileRegister> loadedFiles)
        {
            return loadedFiles.Select(ImportData);
        }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="loadedFile">Загруженный файл с данными</param>
        /// <returns>Результат импорта</returns>
        public override ImportDataResult ImportData(LoadedFileRegister loadedFile)
        {
            var loadStatus = TypeStatus.InProgress;
            var infoDescript = new InfoDescript();
            _importLog.Clear();
            _importLog.AppendLine("Дата загрузки: " + DateTime.Now);
            _importLog.AppendLine("Тип загрузки: 'Загрузка инкрементальных данных в ПГМУ РТ'");
            _importLog.AppendLine("Наименование файла: " + loadedFile.File.FullName);
            try
            {
                //проверяем расширение файла
                if (!loadedFile.File.Extention.ToLower().Contains("zip"))
                    throw new Exception("Архив не прошел валидацию! Aрхив должен быть в формате zip");

                //обрабатываем архив
                using (var zipFile = ZipFile.Read(FileManager.GetFile(loadedFile.File)))
                {
                    //файлы в архиве
                    var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".txt")).ToArray();

                    if (zipEntries.Count() != 8)
                        throw new Exception(
                            "Архив не прошел валидацию! В архиве должно быть 8 файлов! Кол-во файлов в архиве: " +
                                zipEntries.Count());

                    using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
                    {
                        //Обрабатываем "Файл информационного описания"
                        var entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("InfoDescript"));
                        if (entry != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                entry.Extract(ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                AnalyzeInfoDescriptFile(
                                    new StreamReader(ms, Encoding.GetEncoding(1251)),
                                    sqlExecutor,
                                    loadedFile,
                                    out infoDescript);
                            }
                        }
                        else
                        {
                            throw new Exception("Архив не прошел валидацию! Не найден заголовочный файл!");
                        }



                        //Обрабатываем "Характеристики жилого фонда"
                        entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("CharacterGilFond"));
                        if (entry != null)
                        {
                            using (var stream = (Stream) entry.OpenReader())
                            {
                                ImportCharacterGilFondFile(stream, sqlExecutor, infoDescript.ErcCode, infoDescript.CalculationDate);
                            }
                        }

                        //Обрабатываем "Начисления и расходы по услугам"
                        entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("ChargExpenseServ"));
                        if (entry != null)
                        {
                            using (var stream = (Stream) entry.OpenReader())
                            {
                                ImportChargExpenseServFile(stream, sqlExecutor, infoDescript.ErcCode, infoDescript.CalculationDate);
                            }
                        }

                        //Обрабатываем "Показания счетчиков"
                        entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("Counters"));
                        if (entry != null)
                        {
                            using (var stream = (Stream) entry.OpenReader())
                            {
                                ImportCountersFile(stream, sqlExecutor, infoDescript.ErcCode, infoDescript.CalculationDate);
                            }
                        }

                        //Обрабатываем "Платежные реквизиты"
                        entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("PaymentDetails"));
                        if (entry != null)
                        {
                            using (var stream = (Stream) entry.OpenReader())
                            {
                                ImportPaymentDetailsFile(stream, sqlExecutor, infoDescript.ErcCode, infoDescript.CalculationDate);
                            }
                        }


                        //Обрабатываем "Оплаты"
                        entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("Payment.txt"));
                        if (entry != null)
                        {
                            using (var stream = (Stream) entry.OpenReader())
                            {
                                ImportPaymentFile(stream, sqlExecutor, infoDescript.ErcCode, infoDescript.CalculationDate);
                            }
                        }


                        //Обрабатываем "Информация органов социальной защиты"
                        entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("InfoSocProtection"));
                        if (entry != null)
                        {
                            using (var stream = (Stream) entry.OpenReader())
                            {
                                ImportInfoSocProtectionFile(stream, sqlExecutor, infoDescript.ErcCode, infoDescript.CalculationDate);
                            }
                        }

                        //Обрабатываем "Дополнительная информация"
                        entry = zipEntries.FirstOrDefault(x => x.FileName.Contains("AdditionalInfo"));
                        if (entry != null)
                        {
                            using (var stream = (Stream) entry.OpenReader())
                            {
                                ImportAdditionalInfoFile(stream, sqlExecutor, infoDescript.ErcCode, infoDescript.CalculationDate);
                            }
                        }

                        try
                        {
                            //смотрим, является ли загружаемый месяц более новым
                            var sqlQuery = String.Format(
                                " SELECT COUNT(*) AS count " +
                                    " FROM public.saldo_date " +
                                    " WHERE erc_code = {0} " +
                                    " AND saldo_date < '{1}'::DATE" +
                                    " AND active = 1 ",
                                infoDescript.ErcCode,
                                infoDescript.CalculationDate.ToShortDateString()

                                );
                            if (sqlExecutor.ExecuteScalar<int>(sqlQuery) > 0)
                            {
                                //отправляем уведомление на ПГМУ РТ о появлении новых начислений
                                new NotificationPgmu().SendNotification(
                                    infoDescript.ErcCode,
                                    infoDescript.ErcName.Replace('"', '\''),
                                    infoDescript.CalculationDate);
                                _importLog.AppendLine("Уведомление отправлено.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _importLog.AppendLine($"Уведомление не отправлено. Текст ошибки:{ex.Message}");
                        }

                        try
                        {
                            //подсчитываем кол-во ЛС и вставляем в таблицу
                            var sqlQuery =
                                $@"WITH accounts_states AS (
                                    SELECT 
                                    SUM(CASE WHEN account_state = 1 THEN 1 else 0 END) AS opened_accounts_count,
                                    SUM(CASE WHEN account_state = 2 THEN 1 else 0 END) AS closed_accounts_count,
                                    SUM(CASE WHEN account_state = 3 THEN 1 else 0 END) AS undefined_accounts_count,
                                    COUNT(*) AS total_count
                                    FROM parameters_{infoDescript.ErcCode}_{infoDescript.CalculationDate.ToString("yyyyMM")})
                                    UPDATE erc SET 
                                    accounts_count = s.total_count,
                                    opened_accounts_count = s.opened_accounts_count,
                                    closed_accounts_count = s.closed_accounts_count,
                                    undefined_accounts_count = s.undefined_accounts_count 
                                    FROM accounts_states s
                                    WHERE erc_code = {infoDescript.ErcCode};";

                            sqlExecutor.ExecuteSql(sqlQuery);
                            _importLog.AppendLine("Кол-во ЛС подсчитано и вставлено в таблицу.");
                        }
                        catch (Exception ex)
                        {
                            _importLog.AppendLine($"Ошибка при подсчете кол-ва ЛС! {ex.Message}");
                        }

                        //Добавление записи в saldo_date
                        InsertIntoSaldoDate(sqlExecutor, infoDescript.CalculationDate, infoDescript.ErcCode);
                    }
                }
                loadStatus = TypeStatus.Done;
                _importLog.AppendLine("Данные успешно загружены!");
                
            }
            catch (NpgsqlException ex)
            {
                //пишем в протокол загрузки
                var error = String.Format(
                    "Ошибка при загрузке! Архив не прошел валидацию! {0}Текст ошибки:'{1}'.{0}Местоположение некорректных данных:'{2}'",
                    Environment.NewLine,
                    ex.BaseMessage,
                    ex.Where);

                _importLog.AppendLine(error);

                //в случае ошибки удаляем все таблицы по этому ЕРЦ по загружаемому месяцу
                using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
                {
                    //делаем расчетный месяц не активным
                    var sqlQuery = String.Format(
                        " UPDATE public.saldo_date " +
                        " SET active = 0 " +
                        " WHERE saldo_date = '{0}'::DATE" +
                        " AND erc_code =  {1}",
                        infoDescript.CalculationDate.ToShortDateString(),
                        infoDescript.ErcCode);
                    sqlExecutor.ExecuteSql(sqlQuery);

                    var inheritTableNamePostfix = String.Format("{0}_{1}", infoDescript.ErcCode, infoDescript.CalculationDate.ToString("yyyyMM"));
                    sqlQuery = String.Format("DROP TABLE IF EXISTS additionalinfo_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS billinfo_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS charge_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS counters_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS parameters_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS payments_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS sz_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                }
                loadStatus = TypeStatus.ProcessingError;
                return new ImportDataResult(false, "Ошибка при загрузке!", loadedFile.Id);

            }
            catch (Exception ex)
            {
                _importLog.AppendLine("Ошибка при загрузке: " + ex.Message);

                //в случае ошибки удаляем все таблицы по этому ЕРЦ по загружаемому месяцу
                using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
                {
                    //делаем расчетный месяц не активным
                    var sqlQuery = String.Format(
                        " UPDATE public.saldo_date " +
                        " SET active = 0 " +
                        " WHERE saldo_date = '{0}'::DATE" +
                        " AND erc_code =  {1}",
                        infoDescript.CalculationDate.ToShortDateString(),
                        infoDescript.ErcCode);
                    sqlExecutor.ExecuteSql(sqlQuery);

                    var inheritTableNamePostfix = String.Format("{0}_{1}", infoDescript.ErcCode, infoDescript.CalculationDate.ToString("yyyyMM"));
                    sqlQuery = String.Format("DROP TABLE IF EXISTS additionalinfo_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS billinfo_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS charge_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS counters_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS parameters_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS payments_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery = String.Format("DROP TABLE IF EXISTS sz_{0}", inheritTableNamePostfix);
                    sqlExecutor.ExecuteSql(sqlQuery);
                }
                loadStatus = TypeStatus.ProcessingError;
                return new ImportDataResult(false, "Ошибка при загрузке!", loadedFile.Id);
            }
            finally
            {
                _importLog.AppendLine(DateTime.Now.ToString());
                //сохраняем в файл протокол-загрузки
                var logFileName = String.Format("ПРОТОКОЛ_ЗАГРУЗКИ_{0}_{1}", loadedFile.File.Name, loadedFile.Id);
                var resultString = _importLog.ToString();
                var bytes = Encoding.GetEncoding(1251).GetBytes(resultString);

                //сохраняем лог-файл
                loadedFile.Log = Container.Resolve<IFileManager>().SaveFile(logFileName,"txt", bytes);
                loadedFile.TypeStatus = loadStatus;
                Container.Resolve<IDomainService<LoadedFileRegister>>().Update(loadedFile);


                using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
                {
                    //ставим статус загрузки 
                    sqlExecutor.ExecuteSql(
                        String.Format("UPDATE public.sys_imports_register SET import_result = {0} WHERE id = {1} ",
                            (int)loadStatus, infoDescript.LoadId));
                }
            }

             return new ImportDataResult(true, "Успешно загружено!", loadedFile.Id);
        }
        
        /// <summary>
        /// Обрабатываем "Файл информационного описания"
        /// InfoDescript.txt
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="sqlExecutor"></param>
        /// <param name="loadedFile"></param>
        /// <param name="infoDescript"></param>
        private void AnalyzeInfoDescriptFile(TextReader streamReader, SqlExecutor.SqlExecutor sqlExecutor, LoadedFileRegister loadedFile, out InfoDescript infoDescript)
        {
            infoDescript = new InfoDescript();

            var line = streamReader.ReadLine();
            if (line == null)
            {
                throw new Exception("Архив не прошел валидацию! Не заполнено информационное описание!");
            }

            //разделитель полей 
            var fields = line.Split('|');

            if (fields.Count() != 13)
            {
                throw new Exception("Архив не прошел валидацию! Информационное описание не соответствует формату!");
            }

            //Считываем версию формата
            var formatVersion = fields[0];

            //Наименование организации-отправителя 
            infoDescript.ErcName = fields[1].Trim();

            //Подразделение организации-отправителя 
            var senderOrganizationUnitName = fields[2];

            //ИНН организации-отправителя
            var senderInn = fields[3];

            //КПП организации-отправителя
            var senderKpp = fields[4];

            //Код расчетного центра
            infoDescript.ErcCode = Convert.ToInt64(fields[5]);

            //№ файла
            var fileNumber = fields[6];

            //Дата файла
            var unloadDate = fields[7];

            //Телефон отправителя
            var senderPhone = fields[8];

            //Ф.И.О. отправителя
            var senderFio = fields[9];

            //Месяц и год начислений
            infoDescript.CalculationDate = Convert.ToDateTime(fields[10]);

            //Количество выгруженных лицевых счетов
            var unloadedAccountsCount = fields[11];

            //Дата начала работы системы
            var systemStartDate = fields[12];

            _importLog.AppendFormat("Версия формата: {0}{1}", formatVersion, Environment.NewLine);
            _importLog.AppendFormat("Код расчетного центра: {0}{1}", infoDescript.ErcCode, Environment.NewLine);
            _importLog.AppendFormat("Наименование организации-отправителя: '{0}'{1}", infoDescript.ErcName, Environment.NewLine);
            _importLog.AppendFormat("ИНН: '{0}', КПП: '{1}'{2}", senderInn, senderKpp, Environment.NewLine);
            _importLog.AppendFormat("Месяц и год начислений: {0}{1}", infoDescript.CalculationDate.ToShortDateString(), Environment.NewLine);
            _importLog.AppendLine();

            //проверяем формат выгрузки
            if (!Regex.IsMatch(formatVersion, "^1.1"))
            {
                throw new Exception(
                        " Архив не прошел валидацию! Неактуальная версия формата загрузки! " +
                        " Необходимо обновить программное обеспечение и повторно выгрузить файл.");
            }
            
            //проверяем код ЕРЦ на наличие в справочнике
            var sqlQuery = "SELECT COUNT(*) AS count FROM public.erc WHERE erc_code = " + infoDescript.ErcCode;
            if (sqlExecutor.ExecuteScalar<int>(sqlQuery) == 0)
            {
                throw new Exception(" Архив не прошел валидацию! Не найден соответствующий код ЕРЦ. Код ЕРЦ из файла: " + infoDescript.ErcCode);
            }

            sqlQuery = String.Format(
                " INSERT INTO public.sys_imports_register( " +
                " calculation_date, " +
                " load_date, " +
                " unload_date, " +
                " format_version, " +
                " filename, " +
                " file_number, " +
                " unloaded_accounts_count, " +
                " gis_loaded_file_register_id, " +
                " import_result)" +
                " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, {8})" +
                " RETURNING id ",
                infoDescript.CalculationDate,
                DateTime.Now,
                unloadDate,
                formatVersion,
                loadedFile.File.Name,
                fileNumber,
                unloadedAccountsCount,
                loadedFile.Id,
                (int)TypeStatus.InProgress
                );

            
            //уникальный код загрузки
            infoDescript.LoadId = sqlExecutor.ExecuteScalar<int>(sqlQuery);

            sqlQuery = String.Format(
                " INSERT INTO public.sys_data_sender_register( " +
                " sys_imports_register_id, " +
                " erc_code, " +
                " sender_organization_name, " +
                " sender_organization_unit_name, " +
                " sender_inn, " +
                " sender_kpp, " +
                " sender_phone, " +
                " sender_fio)" +
                " VALUES ({0}, {1}, '{2}', '{3}','{4}', '{5}', '{6}', '{7}')",
                infoDescript.LoadId,
                infoDescript.ErcCode,
                infoDescript.ErcName,
                senderOrganizationUnitName,
                senderInn,
                senderKpp,
                senderPhone,
                senderFio
                );
            sqlExecutor.ExecuteSql(sqlQuery);


            //сначала записываем в БД информацию о попытке загрузки, только потом проверяем
            if (infoDescript.CalculationDate == default(DateTime))
            {
                throw new Exception(
                    " Архив не прошел валидацию! " +
                    " В информационном описании некорректно указан месяц и год начислений. Значение из файла: " + infoDescript.CalculationDate);
            }

            //проставляем загружаемый расчетный месяц
            loadedFile.CalculationDate = infoDescript.CalculationDate;
            Container.Resolve<IDomainService<LoadedFileRegister>>().Update(loadedFile);

            /*
             * не выполняем данную проверку теперь
            sqlQuery = String.Format(
               " SELECT COUNT(*) AS count " +
               " FROM public.saldo_date " +
               " WHERE erc_code = {0} " +
               " AND saldo_date = '{1}'::DATE" +
               " AND active = 1 ",
               infoDescript.ErcCode,
               infoDescript.CalculationDate.ToShortDateString()

               );
            if (sqlExecutor.ExecuteScalar<int>(sqlQuery) > 0)
            {
                throw new Exception(" Архив не прошел валидацию! Нельзя перегружать данные за активный расчетный месяц!");
            }
            */

            sqlQuery = String.Format(
                " UPDATE public.erc SET " +
                "  dat_sys_start = '{0}' " +
                " WHERE erc_code = {1} ",
                systemStartDate,
                infoDescript.ErcCode
                );
            sqlExecutor.ExecuteSql(sqlQuery);
        }

        /// <summary>
        /// Обрабатываем "Характеристики жилого фонда"
        /// CharacterGilFondFile
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void ImportCharacterGilFondFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, long ercCode, DateTime calculationDate)
        {
            const string parentTableName = "public.parameters";
            var inheritTableName = String.Format("{0}_{1}_{2}", parentTableName, ercCode, calculationDate.ToString("yyyyMM"));

            var sqlQuery = String.Format("DROP TABLE IF EXISTS {0}", inheritTableName);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {0} " +
                " (LIKE {1} INCLUDING ALL, " +
                " CHECK (erc_code = {2} AND dat_month = '{3}') " +
                " ) " +
                " INHERITS({1}) " +
                " WITH (OIDS=TRUE)",
                inheritTableName,
                parentTableName,
                ercCode,
                calculationDate.ToShortDateString());
            sqlExecutor.ExecuteSql(sqlQuery);
            
            sqlQuery = String.Format(
                " COPY {0} " +
                " FROM stdin " +
                " WITH DELIMITER AS '|' " +
                " NULL AS '' " +
                " ENCODING 'WIN-1251'",
                inheritTableName);

            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream);
            }
            catch (NpgsqlException ex)
            {
                var errorType = "Данные в секции 'Характеристики жилого фонда' не соответствуют формату!";

                //расшифровываем ошибку
                switch (ex.Code)
                {
                    case "23505":
                        errorType = "В секции 'Характеристики жилого фонда' имеются дубли!";
                        break;
                    case "22505":
                    case "22P04":
                        errorType = "В секции 'Характеристики жилого фонда' имеются некорректные символы!";
                        break;
                    case "22001":
                        errorType = "В секции 'Характеристики жилого фонда' имеются строковые значения превышающие разрешенную длину!";
                        break;
                    case "22003":
                        errorType = "В секции 'Характеристики жилого фонда' имеются численные значения превышающие разрешенную размерность!";
                        break;
                }
                
                throw new Exception(String.Format(
                    "Архив не прошел валидацию! {1}{0}" +
                    "Текст ошибки: '{2}'.{0}" +
                    "Расшифровка ошибки: '{3}'.{0}" +
                    "Номер строки в файле с некорректными данными: '{4}'.{0}" +
                    "Местоположение некорректных данных:'{5}'",
                    Environment.NewLine,
                    errorType,
                    ex.BaseMessage,
                    ex.Detail,
                    ex.Line,
                    ex.Where));
            }
        }

        /// <summary>
        /// Обрабатываем "Начисления и расходы по услугам"
        /// ChargExpenseServ
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void ImportChargExpenseServFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, long ercCode, DateTime calculationDate)
        {
            const string parentTableName = "public.charge";
            var inheritTableName = String.Format("{0}_{1}_{2}", parentTableName, ercCode, calculationDate.ToString("yyyyMM"));

            var sqlQuery = String.Format("DROP TABLE IF EXISTS {0}", inheritTableName);
            sqlExecutor.ExecuteSql(sqlQuery);


            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {0} " +
                " (LIKE {1} INCLUDING ALL, " +
                " CHECK (erc_code = {2} AND dat_month = '{3}') " +
                " ) " +
                " INHERITS({1}) " +
                " WITH (OIDS=TRUE)",
                inheritTableName,
                parentTableName,
                ercCode,
                calculationDate.ToShortDateString());
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " COPY {0} " +
                " FROM stdin " +
                " WITH DELIMITER AS '|' " +
                " NULL AS '' " +
                " ENCODING 'WIN-1251'",
                inheritTableName);

            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream);
            }
            catch (NpgsqlException ex)
            {
                var errorType = "Данные в секции 'Начисления и расходы по услугам' не соответствуют формату!";

                //расшифровываем ошибку
                switch (ex.Code)
                {
                    case "23505":
                        errorType = "В секции 'Начисления и расходы по услугам' имеются дубли!";
                        break;
                    case "22505":
                    case "22P04":
                        errorType = "В секции 'Начисления и расходы по услугам' имеются некорректные символы!";
                        break;
                    case "22001":
                        errorType = "В секции 'Начисления и расходы по услугам' имеются строковые значения превышающие разрешенную длину!";
                        break;
                    case "22003":
                        errorType = "В секции 'Начисления и расходы по услугам' имеются численные значения превышающие разрешенную размерность!";
                        break;
                }

                throw new Exception(String.Format(
                    "Архив не прошел валидацию! {1}{0}" +
                    "Текст ошибки: '{2}'.{0}" +
                    "Расшифровка ошибки: '{3}'.{0}" +
                    "Номер строки в файле с некорректными данными: '{4}'.{0}" +
                    "Местоположение некорректных данных:'{5}'",
                    Environment.NewLine,
                    errorType,
                    ex.BaseMessage,
                    ex.Detail,
                    ex.Line,
                    ex.Where));
            }
        }

        /// <summary>
        /// Обрабатываем "Показания счетчиков"
        /// Counters
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void ImportCountersFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, long ercCode, DateTime calculationDate)
        {
            const string parentTableName = "public.counters";
            var inheritTableName = String.Format("{0}_{1}_{2}", parentTableName, ercCode, calculationDate.ToString("yyyyMM"));

            var sqlQuery = String.Format("DROP TABLE IF EXISTS {0}", inheritTableName);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {0} " +
                " (LIKE {1} INCLUDING ALL, " +
                " CHECK (erc_code = {2} AND dat_month = '{3}') " +
                " ) " +
                " INHERITS({1}) " +
                " WITH (OIDS=TRUE)",
                inheritTableName,
                parentTableName,
                ercCode,
                calculationDate.ToShortDateString());
            sqlExecutor.ExecuteSql(sqlQuery);

            //создаем партицию для счетчиков в схеме веб-сервисов
            //веб-сервисы в нее будут класть данные
            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {2}_{0}" +
                " (LIKE {2}  INCLUDING ALL, " +
                " CHECK (dat_month = '{1}') " +
                " ) " +
                " INHERITS({2}) " +
                " WITH (OIDS=TRUE)",
                calculationDate.ToString("yyyyMM"),
                calculationDate.ToShortDateString(),
                "webfon.counters_ord");
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " COPY {0} " +
                " FROM stdin " +
                " WITH DELIMITER AS '|' " +
                " NULL AS '' " +
                " ENCODING 'WIN-1251'",
                inheritTableName);

            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream);
            }
            catch (NpgsqlException ex)
            {
                var errorType = "Данные в секции 'Показания счетчиков' не соответствуют формату!";

                //расшифровываем ошибку
                switch (ex.Code)
                {
                    case "23505":
                        errorType = "В секции 'Показания счетчиков' имеются дубли!";
                        break;
                    case "22505":
                    case "22P04":
                        errorType = "В секции 'Показания счетчиков' имеются некорректные символы!";
                        break;
                    case "22001":
                        errorType = "В секции 'Показания счетчиков' имеются строковые значения превышающие разрешенную длину!";
                        break;
                    case "22003":
                        errorType = "В секции 'Показания счетчиков' имеются численные значения превышающие разрешенную размерность!";
                        break;
                }

                throw new Exception(String.Format(
                     "Архив не прошел валидацию! {1}{0}" +
                     "Текст ошибки: '{2}'.{0}" +
                     "Расшифровка ошибки: '{3}'.{0}" +
                     "Номер строки в файле с некорректными данными: '{4}'.{0}" +
                     "Местоположение некорректных данных:'{5}'",
                     Environment.NewLine,
                     errorType,
                     ex.BaseMessage,
                     ex.Detail,
                     ex.Line,
                     ex.Where));
            }
        }

        /// <summary>
        /// Обрабатываем "Платежные реквизиты"
        /// PaymentDetails
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void ImportPaymentDetailsFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, long ercCode, DateTime calculationDate)
        {
            const string parentTableName = "public.billinfo";
            var inheritTableName = String.Format("{0}_{1}_{2}", parentTableName, ercCode, calculationDate.ToString("yyyyMM"));

            var sqlQuery = String.Format("DROP TABLE IF EXISTS {0}", inheritTableName);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {0} " +
                " (LIKE {1} INCLUDING ALL, " +
                " CHECK (erc_code = {2} AND dat_month = '{3}') " +
                " ) " +
                " INHERITS({1}) " +
                " WITH (OIDS=TRUE)",
                inheritTableName,
                parentTableName,
                ercCode,
                calculationDate.ToShortDateString());
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " COPY {0} " +
                " FROM stdin " +
                " WITH DELIMITER AS '|' " +
                " NULL AS '' " +
                " ENCODING 'WIN-1251'",
                inheritTableName);

            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream);
            }
            catch (NpgsqlException ex)
            {
                var errorType = "Данные в секции 'Платежные реквизиты' не соответствуют формату!";

                //расшифровываем ошибку
                switch (ex.Code)
                {
                    case "23505":
                        errorType = "В секции 'Платежные реквизиты' имеются дубли!";
                        break;
                    case "22505":
                    case "22P04":
                        errorType = "В секции 'Платежные реквизиты' имеются некорректные символы!";
                        break;
                    case "22001":
                        errorType = "В секции 'Платежные реквизиты' имеются строковые значения превышающие разрешенную длину!";
                        break;
                    case "22003":
                        errorType = "В секции 'Платежные реквизиты' имеются численные значения превышающие разрешенную размерность!";
                        break;
                }

                throw new Exception(String.Format(
                    "Архив не прошел валидацию! {1}{0}" +
                    "Текст ошибки: '{2}'.{0}" +
                    "Расшифровка ошибки: '{3}'.{0}" +
                    "Номер строки в файле с некорректными данными: '{4}'.{0}" +
                    "Местоположение некорректных данных:'{5}'",
                    Environment.NewLine,
                    errorType,
                    ex.BaseMessage,
                    ex.Detail,
                    ex.Line,
                    ex.Where));
            }

        }


        /// <summary>
        /// Обрабатываем "Оплаты"
        /// Payment
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void ImportPaymentFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, long ercCode, DateTime calculationDate)
        {
            const string parentTableName = "public.payments";
            var inheritTableName = String.Format("{0}_{1}_{2}", parentTableName, ercCode, calculationDate.ToString("yyyyMM"));

            var sqlQuery = String.Format("DROP TABLE IF EXISTS {0}", inheritTableName);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {0} " +
                " (LIKE {1} INCLUDING ALL, " +
                " CHECK (erc_code = {2} AND dat_month = '{3}') " +
                " ) " +
                " INHERITS({1}) " +
                " WITH (OIDS=TRUE)",
                inheritTableName,
                parentTableName,
                ercCode,
                calculationDate.ToShortDateString());
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " COPY {0} " +
                " FROM stdin " +
                " WITH DELIMITER AS '|' " +
                " NULL AS '' " +
                " ENCODING 'WIN-1251'",
                inheritTableName);

            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream);
            }
            catch (NpgsqlException ex)
            {
                var errorType = "Данные в секции 'Оплаты' не соответствуют формату!";

                //расшифровываем ошибку
                switch (ex.Code)
                {
                    case "23505":
                        errorType = "В секции 'Оплаты' имеются дубли!";
                        break;
                    case "22505":
                    case "22P04":
                        errorType = "В секции 'Оплаты' имеются некорректные символы!";
                        break;
                    case "22001":
                        errorType = "В секции 'Оплаты' имеются строковые значения превышающие разрешенную длину!";
                        break;
                    case "22003":
                        errorType = "В секции 'Оплаты' имеются численные значения превышающие разрешенную размерность!";
                        break;
                }

                throw new Exception(String.Format(
                    "Архив не прошел валидацию! {1}{0}" +
                    "Текст ошибки: '{2}'.{0}" +
                    "Расшифровка ошибки: '{3}'.{0}" +
                    "Номер строки в файле с некорректными данными: '{4}'.{0}" +
                    "Местоположение некорректных данных:'{5}'",
                    Environment.NewLine,
                    errorType,
                    ex.BaseMessage,
                    ex.Detail,
                    ex.Line,
                    ex.Where));
            }
        }


        /// <summary>
        /// Обрабатываем "Информация органов социальной защиты"
        /// InfoSocProtection
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void ImportInfoSocProtectionFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, long ercCode, DateTime calculationDate)
        {
            const string parentTableName = "public.sz";
            var inheritTableName = String.Format("{0}_{1}_{2}", parentTableName, ercCode, calculationDate.ToString("yyyyMM"));

            var sqlQuery = String.Format("DROP TABLE IF EXISTS {0}", inheritTableName);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {0} " +
                " (LIKE {1} INCLUDING ALL, " +
                " CHECK (erc_code = {2} AND dat_month = '{3}') " +
                " ) " +
                " INHERITS({1}) " +
                " WITH (OIDS=TRUE)",
                inheritTableName,
                parentTableName,
                ercCode,
                calculationDate.ToShortDateString());
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " COPY {0} " +
                " FROM stdin " +
                " WITH DELIMITER AS '|' " +
                " NULL AS '' " +
                " ENCODING 'WIN-1251'",
                inheritTableName);


            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream);
            }
            catch (NpgsqlException ex)
            {
                var errorType = "Данные в секции 'Информация органов социальной защиты' не соответствуют формату!";

                //расшифровываем ошибку
                switch (ex.Code)
                {
                    case "23505":
                        errorType = "В секции 'Информация органов социальной защиты' имеются дубли!";
                        break;
                    case "22505":
                    case "22P04":
                        errorType = "В секции 'Информация органов социальной защиты' имеются некорректные символы!";
                        break;
                    case "22001":
                        errorType = "В секции 'Информация органов социальной защиты' имеются строковые значения превышающие разрешенную длину!";
                        break;
                    case "22003":
                        errorType = "В секции 'Информация органов социальной защиты' имеются численные значения превышающие разрешенную размерность!";
                        break;
                }

                throw new Exception(String.Format(
                    "Архив не прошел валидацию! {1}{0}" +
                    "Текст ошибки: '{2}'.{0}" +
                    "Расшифровка ошибки: '{3}'.{0}" +
                    "Номер строки в файле с некорректными данными: '{4}'.{0}" +
                    "Местоположение некорректных данных:'{5}'",
                    Environment.NewLine,
                    errorType,
                    ex.BaseMessage,
                    ex.Detail,
                    ex.Line,
                    ex.Where));
            }
            
        }

        /// <summary>
        /// Обрабатываем "Дополнительная информация"
        /// AdditionalInfo
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void ImportAdditionalInfoFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, long ercCode, DateTime calculationDate)
        {
            const string parentTableName = "public.additionalinfo";
            var inheritTableName = String.Format("{0}_{1}_{2}", parentTableName, ercCode, calculationDate.ToString("yyyyMM"));

            var sqlQuery = String.Format("DROP TABLE IF EXISTS {0}", inheritTableName);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " CREATE TABLE IF NOT EXISTS {0} " +
                " (LIKE {1} INCLUDING ALL, " +
                " CHECK (erc_code = {2} AND dat_month = '{3}') " +
                " ) " +
                " INHERITS({1}) " +
                " WITH (OIDS=TRUE)",
                inheritTableName,
                parentTableName,
                ercCode,
                calculationDate.ToShortDateString());
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = String.Format(
                " COPY {0} " +
                " FROM stdin " +
                " WITH DELIMITER AS '|' " +
                " NULL AS '' " +
                " ENCODING 'WIN-1251'",
                inheritTableName);


            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream);
            }
            catch (NpgsqlException ex)
            {
                var errorType = "Данные в секции 'Дополнительная информация' не соответствуют формату!";

                //расшифровываем ошибку
                switch (ex.Code)
                {
                    case "23505":
                        errorType = "В секции 'Дополнительная информация' имеются дубли!";
                        break;
                    case "22505":
                    case "22P04":
                        errorType = "В секции 'Дополнительная информация' имеются некорректные символы!";
                        break;
                    case "22001":
                        errorType = "В секции 'Дополнительная информация' имеются строковые значения превышающие разрешенную длину!";
                        break;
                    case "22003":
                        errorType = "В секции 'Дополнительная информация' имеются численные значения превышающие разрешенную размерность!";
                        break;
                }

                throw new Exception(String.Format(
                     "Архив не прошел валидацию! {1}{0}" +
                     "Текст ошибки: '{2}'.{0}" +
                     "Расшифровка ошибки: '{3}'.{0}" +
                     "Номер строки в файле с некорректными данными: '{4}'.{0}" +
                     "Местоположение некорректных данных:'{5}'",
                     Environment.NewLine,
                     errorType,
                     ex.BaseMessage,
                     ex.Detail,
                     ex.Line,
                     ex.Where));
            }
        }

        /// <summary>
        /// Добавление записи в saldo_date
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void InsertIntoSaldoDate(SqlExecutor.SqlExecutor sqlExecutor, DateTime calculationDate, long ercCode)
        {
            var sqlQuery = String.Format(
                " SELECT COALESCE(CAST(MAX(saldo_date) AS DATE), '1900-1-1') " +
                " FROM public.saldo_date " +
                " WHERE active = 1 " +
                " AND erc_code = {0}", 
                ercCode);

            //если загружается месяц, который раньше последнего загруженного
            if (sqlExecutor.ExecuteScalar<DateTime>(sqlQuery) > calculationDate)
            {
                //вставляем запись, но делаем ее НЕ активной
                sqlQuery = String.Format(
                    "INSERT INTO public.saldo_date(erc_code,saldo_month,saldo_year,saldo_date,active) " +
                    " VALUES ({0},{1},{2},'{3}',{4})",
                    ercCode,
                    calculationDate.Month,
                    calculationDate.Year,
                    calculationDate.ToShortDateString(),
                    0);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            else
            {
                //делаем все записи НЕактивными, и вставляем новую запись активную
                sqlQuery = String.Format(
                    " UPDATE public.saldo_date " +
                    " SET active = 0 " +
                    " WHERE erc_code = {0} ",
                    ercCode);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = String.Format(
                    "INSERT INTO public.saldo_date(erc_code,saldo_month,saldo_year,saldo_date,active) " +
                    " VALUES ({0},{1},{2},'{3}',{4})",
                    ercCode, 
                    calculationDate.Month, 
                    calculationDate.Year, 
                    calculationDate.ToShortDateString(), 1);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Вспомогательная сущность для загрузки
        /// </summary>
        private class InfoDescript
        {
            /// <summary>
            /// Уникальный код загрузки
            /// </summary>
            public int LoadId { get; set; }

            /// <summary>
            /// Код ЕРЦ
            /// </summary>
            public long ErcCode { get; set; }

            /// <summary>
            /// Наименование ЕРЦ
            /// </summary>
            public string ErcName { get; set; }

            /// <summary>
            /// Расчетный месяц за который грузятся данные
            /// </summary>
            public DateTime CalculationDate { get; set; }
        }
        
    }
}
