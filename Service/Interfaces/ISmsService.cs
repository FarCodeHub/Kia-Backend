using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.Models;
using Service.Services.SmsService;
using SmsIrRestful;

namespace Service.Interfaces
{
    public interface ISmsService
    {
        IRepository Repository { get; set; }

        SentSMSLog2[] Send(string message, IList<string> numbers, string headLineNumber);
        SentMessage[] GetSendedMessagesAsync(string startDate, string endDate, Pagination pagination);
        ReceivedMessages[] GetReceivedMessages(string startDate, string endDate, Pagination pagination);
        bool SendQuickMessage(string number, SmsService.QuickMessageType quickMessageType, SmsService.IQuickMessage quickMessage);
        List<UltraFastParameters> QuickMessagePreparer(SmsService.QuickMessageType quickMessageType, SmsService.IQuickMessage quickMessage);
        Task<List<SignalCommunicationInformationModel>> CheckReceivedMessages();
        Task<List<ReceivedMessages>> GetReceivedMessages();
    }
}