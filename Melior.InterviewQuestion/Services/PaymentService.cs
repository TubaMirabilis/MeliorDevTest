using System;
using System.Configuration;
using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Services;

public class PaymentService : IPaymentService
{
    private readonly IAccountDataStore _store;
    public PaymentService()
    {
        _store = ConfigurationManager.AppSettings["DataStoreType"] switch
        {
            "Backup" => new BackupAccountDataStore(),
            _ => new AccountDataStore()
        };
    }

    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
        var account = _store.GetAccount(request.DebtorAccountNumber);
        ArgumentNullException.ThrowIfNull(account);
        return request.PaymentScheme switch
        {
            PaymentScheme.Bacs => new MakePaymentResult(HandleBacsPaymentScheme(account, request)),
            PaymentScheme.FasterPayments => new MakePaymentResult(HandleFasterPaymentsPaymentScheme(account, request)),
            PaymentScheme.Chaps => new MakePaymentResult(HandleChapsPaymentScheme(account, request)),
            _ => new MakePaymentResult(false)
        };
    }
    private bool HandleBacsPaymentScheme(Account account, MakePaymentRequest request)
    {
        if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
        {
            return false;
        }
        return HandleSuccessfulPayment(account, request);
    }
    private bool HandleChapsPaymentScheme(Account account, MakePaymentRequest request)
    {
        if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
        {
            return false;
        }
        if (account.Status != AccountStatus.Live)
        {
            return false;
        }
        return HandleSuccessfulPayment(account, request);
    }
    private bool HandleFasterPaymentsPaymentScheme(Account account, MakePaymentRequest request)
    {
        if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
        {
            return false;
        }
        if (account.Balance < request.Amount)
        {
            return false;
        }
        return HandleSuccessfulPayment(account, request);
    }
    private bool HandleSuccessfulPayment(Account account, MakePaymentRequest request)
    {
        account.Balance -= request.Amount;
        _store.UpdateAccount(account);
        return true;
    }
}
