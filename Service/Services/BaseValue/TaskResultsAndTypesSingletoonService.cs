using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Interfaces;

namespace Service.Services.BaseValue
{
    public sealed class TaskResultsAndTypesSingleton
    {
        private readonly IRepository _repository;
        public int ApplyToContract { get; set; }
        public int Follow { get; set; }
        public int Reference { get; set; }
        public int Appointment { get; set; }
        public int ApplyToVisitTheLand { get; set; }
        public int SendToBlackList { get; set; }
        public int Cancel { get; set; }
        public int ConfirmReference { get; set; }
        public int ConfirmAppointment { get; set; }
        public int ConfirmApplyToVisitTheLand { get; set; }
        public int ConfirmCancel { get; set; }
        public int RequestReference { get; set; }
        public int RequestAppointment { get; set; }
        public int RequestApplyToVisitTheLand { get; set; }
        public int RequestCancel { get; set; }
        public int VoiceMail { get; set; }
        public int IncomSms { get; set; }
        public int IncomingCall { get; set; }
        public int CaseClose { get; set; }
        public int OutgoingCall { get; set; }
        public int EndCall { get; set; }
        public int MissCall { get; set; }
        public int OutSms { get; set; }
        public int AnswerCall { get; set; }
        public int AnswerSms { get; set; }
        public int CancleContract { get; set; }
        private readonly List<string> _requiredBaseValues = new()
        {
            "applyToContract",
            "applyToContract",
            "follow",
            "reference",
            "appointment",
            "applyToVisitTheLand",
            "requestCancel",
            "requestReference",
            "requestAppointment",
            "requestApplyToVisitTheLand",
            "cancel",
            "confirmAppointment",
            "confirmReference",
            "confirmApplyToVisitTheLand",
            "confirmCancel",
            "sendToBlackList",
            "voiceMail",
            "incomSms",
            "incomingCall",
            "outgoingCall",
            "endCall",
            "missCall",
            "outSms",
            "cancleContract",
            "caseClose",
            "answerCall",
            "answerSms"
        };
        TaskResultsAndTypesSingleton(IRepository repository)
        {
            _repository = repository;

            var temp =   _repository.GetQuery<Domain.Entities.BaseValue>()
                .Select(x=>new{x.UniqueName,x.Id})
                .Where(x =>
                    _requiredBaseValues.Contains(x.UniqueName)
                ).ToList();

             
            foreach (var baseValue in temp)
            {
                foreach (var propertyInfo in this.GetType().GetProperties())
                {
                    if (!propertyInfo.Name.Equals(baseValue.UniqueName, StringComparison.OrdinalIgnoreCase)) continue;
                    propertyInfo.SetValue(this,baseValue.Id);
                    break;
                }
            }
        }
        private static readonly object Lock = new object();
        private static TaskResultsAndTypesSingleton _instance = null;

        public static TaskResultsAndTypesSingleton Instance(IRepository repository)
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    _instance ??= new TaskResultsAndTypesSingleton(repository);
                }
            }

            return _instance;
        }
    }
}



