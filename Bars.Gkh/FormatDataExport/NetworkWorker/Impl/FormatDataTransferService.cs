namespace Bars.Gkh.FormatDataExport.NetworkWorker.Impl
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Logging;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Responses;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Сервис взаимодействия с API РИС ЖКХ
    /// </summary>
    public class FormatDataTransferService : IFormatDataTransferService
    {
        private string RemoteAddress { get; }

        private readonly string defaultAuthToken;
        private string authToken;

        private string AuthToken
        {
            get
            {
                if (string.IsNullOrEmpty(this.authToken) && string.IsNullOrEmpty(this.defaultAuthToken))
                {
                    throw new Exception("Не задан токен аутентификации");
                }
                return this.authToken ?? this.defaultAuthToken;
            }
        }

        public ILogManager LogManager { get; set; }

        public FormatDataTransferService(IWindsorContainer container, IGkhUserManager userManager)
        {
            this.RemoteAddress = container.GetGkhConfig<AdministrationConfig>()?
                .FormatDataExport
                .FormatDataExportGeneral
                .TransferServiceAddress ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(this.RemoteAddress) && !this.RemoteAddress.EndsWith("/"))
            {
                this.RemoteAddress += "/";
            }

            this.defaultAuthToken = this.GetToken(userManager.GetActiveOperator());
        }

        private string GetToken(Operator gkhOperator)
        {
            return !string.IsNullOrWhiteSpace(gkhOperator.RisToken)
                ? $"Token {gkhOperator.RisToken}"
                : null;
        }

        public bool SetToken(Operator gkhOperator)
        {
            this.authToken = this.GetToken(gkhOperator);
            return !string.IsNullOrEmpty(this.authToken);
        }

        /// <inheritdoc />
        public IDataResult GetStatus(long id)
        {
            using (var client = new WebClient())
            {
                client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                client.UseDefaultCredentials = false;
                client.Headers.Add("Authorization", this.AuthToken);

                try
                {
                    var response = client.DownloadString(new Uri(new Uri(this.RemoteAddress), $"import/status/{id}"));

                    return new BaseDataResult(JsonConvert.DeserializeObject<StatusSuccess>(response));
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка получения статуса", e);
                }
            }
        }

        /// <inheritdoc />
        public IDataResult UploadFile(string filePath, CancellationToken cancellationToken)
        {
            var uri = new Uri(this.RemoteAddress);

            this.LogManager.Debug($"Передача файла '{filePath}' на сервер {uri.Host}:{uri.Port}");

            var fileName = Path.GetFileName(filePath);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            using (var fileStreamContent = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
            using (var name = new StringContent(fileName, Encoding.UTF8))
            using (var checksum = new StringContent(string.Empty))
            {
                fileStreamContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=\"file\"; filename=\"{fileName}\"");
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                name.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data; name=\"name\"");

                checksum.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data; name=\"checksum\"");

                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(this.AuthToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("*/*");

                formData.Add(name);
                formData.Add(checksum);
                formData.Add(fileStreamContent);

                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(new Uri(uri, "storage/upload/"), formData, cancellationToken).Result;
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка загрузки файла", e);
                }

                if (response != null)
                {
                    var responseValue = string.Empty;
                    var responseStream = response.Content.ReadAsStreamAsync().Result;
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseValue = reader.ReadToEnd();
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        this.LogManager.Debug("Передача файла завершена успешно");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.Debug(responseValue);
                        }

                        return new BaseDataResult(JsonConvert.DeserializeObject<UploadSuccess>(responseValue));
                    }
                    else
                    {
                        this.LogManager.Debug($"Передача файла завершена c ошибкой. {response}");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.Debug(responseValue);
                        }

                        return new BaseDataResult { Success = false, Data = JsonConvert.DeserializeObject<Error>(responseValue) } ;
                    }
                }
            }

            return BaseDataResult.Error("Нет ответа от сервера");
        }

        /// <inheritdoc />
        public IDataResult StartImport(long id, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(this.AuthToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("*/*");

                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(new Uri(new Uri(this.RemoteAddress), $"import/data/{id}"), formData, cancellationToken).Result;
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка запуска импорта", e);
                }

                if (response != null)
                {
                    var responseValue = string.Empty;
                    var responseStream = response.Content.ReadAsStreamAsync().Result;
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseValue = reader.ReadToEnd();
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        this.LogManager.Debug("Удаленный импорт успешно запущен");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.Debug(responseValue);
                        }

                        return new BaseDataResult(JsonConvert.DeserializeObject<DataSuccess>(responseValue));
                    }
                    else
                    {
                        this.LogManager.Debug($"Ошибка при постановке задачи удаленного импорта. {response}");
                        if (!responseValue.IsEmpty())
                        {
                            this.LogManager.Debug(responseValue);
                        }

                        return new BaseDataResult { Success = false, Data = JsonConvert.DeserializeObject<Error>(responseValue) };
                    }
                }
            }

            return BaseDataResult.Error("Нет ответа от сервера");
        }

        /// <inheritdoc />
        public IDataResult GetFile(long fileId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(this.AuthToken);

                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(new Uri(new Uri(this.RemoteAddress), $"storage/download/{fileId}")).Result;
                }
                catch (Exception e)
                {
                    return this.ReturnError("Ошибка получения файла", e);
                }

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileName = response.Content.Headers?.ContentDisposition?.FileName.Trim('"');
                        fileName = string.IsNullOrEmpty(fileName)
                            ? Path.GetTempFileName()
                            : Path.Combine(Path.GetTempPath(), fileName);

                        using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                        {
                            response.Content.CopyToAsync(fs).Wait();
                        }

                        return new BaseDataResult(fileName);
                    }
                    else
                    {
                        var responseValue = string.Empty;
                        var responseStream = response.Content.ReadAsStreamAsync().Result;
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }

                        return new BaseDataResult { Success = false, Data = JsonConvert.DeserializeObject<Error>(responseValue) };
                    }
                }
            }

            return BaseDataResult.Error("Нет ответа от сервера");
        }

        private IDataResult ReturnError(string message, Exception exception)
        {
            var errorMessage = $"{message}|{this.GetInnerException(exception).Message}";
            this.LogManager.Error(errorMessage, exception);
            return new BaseDataResult
            {
                Success = false,
                Message = errorMessage,
                Data = exception
            };
        }

        private Exception GetInnerException(Exception exception, int level = 0)
        {
            if (exception.InnerException != null && level < 10)
            {
                return this.GetInnerException(exception.InnerException, ++level);
            }
            else
            {
                return exception;
            }
        }
    }
}