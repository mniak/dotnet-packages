using Bogus;
using Example.Client.Infrastructure.Compression;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Client.Features
{
    public class MainWorker : IMainWorker
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;

        public MainWorker(ILogger<MainWorker> logger, HttpClient httpClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task Work(CancellationToken stoppingToken)
        {
            var messageHandler = new GzipMessageHandler().WithNewInnerHandler();
            using var httpClient2 = new HttpClient(messageHandler);

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Press <Enter> to start");
                Console.ReadLine();
                Console.Clear();

                //var largePayload = ProduceLargePayload();
                var payload = new
                {
                    negotiationIdentifier = "TRB0000000000000000000002851608840564211",
                    registrarDocumentNumber = "29011780000157",
                    negotiatingParticipantDocumentNumber = "17351180000159",
                    operationIdentifier = "2020122400001381111",
                    negotiationType = "GravameCessao",
                    operationDueDate = "2020-12-24",
                    totalLimitAmountOrBalanceDue = 6.00,
                    guaranteeAmount = 6.00,
                    managedByRegistrar = true,
                    divisionRule = "Percentage",
                    unconditionalAcceptanceOfOperation = true,
                    acceptanceOfReservedReceivableUnit = false,
                    inclusionDate = "2020-12-24T17:09:26-03:00",
                    operationStatus = "Active",
                    sourceOperationIdentifiers = new[]{
                        "2020122400001380001",
                        "2020122400001380002"
                    },
                    receivables = new[] {
                        new {
                                holderBaseDocumentNumber= "001.010.100-11",
                                accountHolderDocumentNumber= "222.333.444-50",
                                accountDocumentNumber= "123.456.789-10",
                                BankAccountType= "CheckingAccount",
                                BankAccountNumber= "0118",
                                accountBankCode= "88887",
                                AgencyNumber= "0887",
                                paymentAccountNumber= "000012-2",
                                priority= 2,
                                recipientDocumentNumber= "111.111.111-22",
                                holderDocumentNumber= "12345678910",
                                paymentArrangementCode= 25,
                                forecastedSettlementDate= "2020-12-31",
                                negotiatedAmount= 33.333,
                                constituted= true
                            }
                        }
                };

                var content = JsonContent.Create(payload, null);
                //content.Headers.Add("x-api-key", "ODRiMzBlODAtOTA2Ny00Yzc3LWI0Y2MtNjA0NTJhMGFhNjJjMDhlN2NkNjYtODE1ZC00MzE1LTllYTgtM2Y0ZTViM2I5ZTA2MTU3ZjI5YjgtMWFmNi00OTEyLTg2N2YtM2MyNjhhMTRhMzhm");
                //await httpClient.PutAsync("https://splitintsandbox.braspag.com.br/receivables-api/negotiations/2E796F4A46DF41B", content);
                content.Headers.Add("x-api-key", "194c19de-b4c4-47b0-910a-b1f5aeb9fd63");
                var response = await httpClient2.PutAsync("http://localhost:5001/receivables-api/negotiations/2E796F4A46DF41B", content);
            }
        }

        private static IEnumerable<string> ProduceLargePayload()
        {
            var faker = new Faker();
            for (int i = 0; i < 10; i++)
            {
                yield return faker.Lorem.Sentence(5);
            }
        }
    }
}
