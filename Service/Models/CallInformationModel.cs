using Domain.Entities;

namespace Service.Models
{
    public class SignalCommunicationInformationModel
    {
        public SignalCommunicationInformationModel(string signalId)
        {
            SignalId = signalId;
        }
        public string SignalId { get; set; }
        public int CustomerId { get; set; }
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public int CommunicationId { get; set; }
        public string StatusTitle { get; set; }
        public string CustomerConnectedNumber { get; set; }
        public string UniqueNumber { get; set; }
        public string OperatorQueueNumber { get; set; }
        public Task Task { get; set; }
        public Communication Communication {  get;set; }
    }
}