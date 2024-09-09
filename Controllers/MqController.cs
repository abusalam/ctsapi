using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;
using CTS_BE.BAL.Interfaces;
using CTS_BE.Events;
using CTS_BE.Controllers.Pension;

namespace CTS_BE.Controllers
{
    [Route("api/v1/mq")]
    public class MqController : ApiBaseController
    {
        private readonly IMqService _mqService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        static bool IsQueueWorkerRunning = false;
        public MqController(
            IMqService mqService,
            IClaimService claimService
        ) : base(claimService)
        {
            _mqService = mqService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [HttpPost("message/{queueName}")]
        [Tags("Message Queue: Worker")]
        [OpenApi]
        public Task<string> SendMessage(string contents, string queueName)
        {

            string response = "";
            try {
                int messagesToSend = 0;
                try{
                    messagesToSend = Int32.Parse(contents);
                }
                catch(FormatException) {
                    Console.WriteLine("Trying to send single messsage");
                }
                if(messagesToSend > 0) {
                    for(int i = 0; i < messagesToSend; i++) {
                         _mqService.Despatch(queueName, $"Bill {i+1}");
                    }
                    response = $"Dispatched {messagesToSend} messages";
                } else {
                    response = _mqService.Despatch(queueName, contents);
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"MQ-C-Error: {ex.Message}");
                response = $"Error: {ex.Message}";
            }
            return Task.FromResult(response);
        }
    
        [HttpGet("message/{queueName}")]
        [Tags("Message Queue: Worker")]
        [OpenApi]
        public Task<string> ReceiveSingleMessage(string queueName)
        {

            string response = "";
            try {
                response = _mqService.ReceiveSingle(queueName);
            }
            catch (Exception ex) {
                Console.WriteLine($"MQ-C-Error: {ex.Message}");
                response = $"Error: {ex.Message}";

            }
            return Task.FromResult(response);
        }

        [HttpGet("start-worker/{queueName}")]
        [Tags("Message Queue: Worker")]
        [OpenApi]
        public Task<JsonAPIResponse<string>> StartMqComsumer(string queueName)
        {
            
            JsonAPIResponse<string> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Worker started sucessfully!",
            };
            try {
                if(!IsQueueWorkerRunning) {
                    IsQueueWorkerRunning = true;
                    _mqService.RecceiveHandler += QueueWorker;
                }
                string consumerTag = _mqService.SetupConsumer(
                    queueName,
                    _cancellationTokenSource.Token
                );
                response.Result = consumerTag;
            }
            catch (Exception ex) {
                Console.WriteLine($"MQ-C-Error: {ex.Message}");
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"Error: {ex.Message}";

            }
            return Task.FromResult(response);
        }

        [HttpGet("stop-worker/{consumerTag}")]
        [Tags("Message Queue: Worker")]
        [OpenApi]
        public Task<string> StopMqComsumer(string consumerTag)
        {
            string response = "";
            try {
                // _cancellationTokenSource.Cancel();
                _mqService.RecceiveHandler -= QueueWorker;
                response = _mqService.CancelConsumer(consumerTag);
            }
            catch (Exception ex) {
                Console.WriteLine($"MQ-C-Error: {ex.Message}");
                response = $"Error: {ex.Message}";
            }
            return Task.FromResult(response);
        }

        private void QueueWorker(object? channel, MqDeliverEventArgs msgData)
        {
            Thread.Sleep(2000);
            _mqService.DeliveryAck(msgData.DeliveryTag);
            Console.WriteLine($" [{msgData.DeliveryTag}] Delivered: {msgData.Body}");
        }
    }
}