using System;

namespace Melior.InterviewQuestion.Types;

public sealed record MakePaymentRequest(string CreditorAccountNumber, string DebtorAccountNumber, decimal Amount, DateTime PaymentDate, PaymentScheme PaymentScheme);
